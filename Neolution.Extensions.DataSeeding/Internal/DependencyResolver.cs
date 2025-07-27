namespace Neolution.Extensions.DataSeeding.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Implements topological sorting for seed dependency resolution using Kahn's algorithm.
    /// This replaces the simple tree-based approach with proper dependency graph resolution.
    /// </summary>
    internal static class DependencyResolver
    {
        /// <summary>
        /// Resolves the execution order of seeds using topological sorting.
        /// </summary>
        /// <param name="seeds">The collection of seeds to sort.</param>
        /// <returns>The seeds in execution order.</returns>
        /// <exception cref="InvalidOperationException">Thrown when circular dependencies are detected.</exception>
        public static IList<ISeed> ResolveDependencies(IEnumerable<ISeed> seeds)
        {
            var seedList = seeds.ToList();
            var seedLookup = seedList.ToDictionary(s => s.GetType(), s => s);

            var (graph, inDegree) = BuildDependencyGraph(seedList, seedLookup);
            var result = ProcessTopologicalSort(seedList, graph, inDegree);
            ValidateNoCycles(seedList, result);

            return result;
        }

        /// <summary>
        /// Creates a pretty-printed visualization of the dependency graph.
        /// </summary>
        /// <param name="seeds">The collection of seeds to visualize.</param>
        /// <returns>A formatted string showing the dependency graph structure.</returns>
        public static string CreateDependencyGraphVisualization(IEnumerable<ISeed> seeds)
        {
            var seedList = seeds.ToList();
            var seedLookup = seedList.ToDictionary(s => s.GetType(), s => s);
            var lines = new List<string>();

            lines.Add("Dependency Graph:");
            lines.Add("================");

            foreach (var seed in seedList)
            {
                var seedName = seed.GetType().Name;
                var dependencies = GetSeedDependencies(seed);

                if (dependencies.Length == 0)
                {
                    lines.Add($"◦ {seedName} (no dependencies)");
                }
                else
                {
                    lines.Add($"◦ {seedName}");
                    foreach (var dependency in dependencies)
                    {
                        var status = seedLookup.ContainsKey(dependency)
                            ? string.Empty
                            : " (not found in seed collection)";
                        lines.Add($"  └─ depends on: {dependency.Name}{status}");
                    }
                }
            }

            lines.Add(string.Empty);
            lines.Add("Execution Order (topologically sorted):");
            lines.Add("=======================================");

            try
            {
                var sortedSeeds = ResolveDependencies(seedList);
                for (int i = 0; i < sortedSeeds.Count; i++)
                {
                    lines.Add($"{i + 1}. {sortedSeeds[i].GetType().Name}");
                }
            }
            catch (InvalidOperationException ex)
            {
                lines.Add($"ERROR: {ex.Message}");
            }

            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Builds the dependency graph and calculates in-degrees for all seeds.
        /// </summary>
        /// <param name="seedList">The list of seeds to process.</param>
        /// <param name="seedLookup">Dictionary for fast seed lookup by type.</param>
        /// <returns>A tuple containing the dependency graph and in-degree counts.</returns>
        private static (SeedGraph Graph, InDegreeMap InDegree) BuildDependencyGraph(
            IList<ISeed> seedList,
            Dictionary<Type, ISeed> seedLookup)
        {
            var graph = new SeedGraph();
            var inDegree = new InDegreeMap();

            // Initialize graph and in-degree for all seeds
            foreach (var seed in seedList)
            {
                graph[seed] = new List<ISeed>();
                inDegree[seed] = 0;
            }

            // Build dependency relationships
            foreach (var seed in seedList)
            {
                var dependencies = GetSeedDependencies(seed);
                AddDependenciesToGraph(seed, dependencies, seedLookup, graph, inDegree);
            }

            return (graph, inDegree);
        }

        /// <summary>
        /// Gets the dependencies for a specific seed, supporting both new and legacy dependency properties.
        /// </summary>
        /// <param name="seed">The seed to get dependencies for.</param>
        /// <returns>Array of dependency types.</returns>
        private static Type[] GetSeedDependencies(ISeed seed)
        {
            // Priority order: DependsOnTypes > DependsOnType > DependsOn (legacy)
            if (seed.DependsOnTypes?.Length > 0)
            {
                return seed.DependsOnTypes;
            }

            if (seed.DependsOnType != null)
            {
                return new[] { seed.DependsOnType };
            }

            // Fall back to legacy DependsOn property with pragma to suppress obsolete warning
#pragma warning disable CS0618 // Type or member is obsolete
            return seed.DependsOn != null ? new[] { seed.DependsOn } : Array.Empty<Type>();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Adds dependencies to the graph and updates in-degree counts.
        /// </summary>
        /// <param name="seed">The seed that has dependencies.</param>
        /// <param name="dependencies">The types this seed depends on.</param>
        /// <param name="seedLookup">Dictionary for fast seed lookup by type.</param>
        /// <param name="graph">The dependency graph to update.</param>
        /// <param name="inDegree">The in-degree count to update.</param>
        private static void AddDependenciesToGraph(
            ISeed seed,
            Type[] dependencies,
            Dictionary<Type, ISeed> seedLookup,
            SeedGraph graph,
            InDegreeMap inDegree)
        {
            foreach (var dependencyType in dependencies)
            {
                if (seedLookup.TryGetValue(dependencyType, out var dependency))
                {
                    // dependency -> seed (dependency must execute before seed)
                    graph[dependency].Add(seed);
                    inDegree[seed]++;
                }
            }
        }

        /// <summary>
        /// Processes the topological sort using Kahn's algorithm.
        /// </summary>
        /// <param name="seedList">The list of seeds to process.</param>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="inDegree">The in-degree count for each seed.</param>
        /// <returns>The seeds sorted in execution order.</returns>
        private static List<ISeed> ProcessTopologicalSort(
            IList<ISeed> seedList,
            SeedGraph graph,
            InDegreeMap inDegree)
        {
            var result = new List<ISeed>();
            var queue = new Queue<ISeed>();

            // Find all seeds with no dependencies (in-degree = 0)
            foreach (var seed in seedList)
            {
                if (inDegree[seed] == 0)
                {
                    queue.Enqueue(seed);
                }
            }

            // Process the queue using Kahn's algorithm
            while (queue.Count > 0)
            {
                var currentSeed = queue.Dequeue();
                result.Add(currentSeed);

                // For each seed that depends on the current seed
                foreach (var dependentSeed in graph[currentSeed])
                {
                    inDegree[dependentSeed]--;
                    if (inDegree[dependentSeed] == 0)
                    {
                        queue.Enqueue(dependentSeed);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Validates that no circular dependencies exist by checking if all seeds were processed.
        /// </summary>
        /// <param name="seedList">The original list of seeds.</param>
        /// <param name="result">The processed seeds from topological sort.</param>
        /// <exception cref="InvalidOperationException">Thrown when circular dependencies are detected.</exception>
        private static void ValidateNoCycles(IList<ISeed> seedList, List<ISeed> result)
        {
            if (result.Count != seedList.Count)
            {
                var unprocessedSeeds = seedList.Except(result).Select(s => s.GetType().Name);
                throw new InvalidOperationException($"Circular dependency detected among seeds: {string.Join(", ", unprocessedSeeds)}");
            }
        }

        /// <summary>
        /// Helper class to wrap seed dependency graph to avoid nested generic types (S4017).
        /// </summary>
        private sealed class SeedGraph : Dictionary<ISeed, List<ISeed>>
        {
        }

        /// <summary>
        /// Helper class to wrap in-degree mapping to avoid nested generic types (S4017).
        /// </summary>
        private sealed class InDegreeMap : Dictionary<ISeed, int>
        {
        }
    }
}
