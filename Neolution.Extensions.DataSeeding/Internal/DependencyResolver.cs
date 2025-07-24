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
            var result = new List<ISeed>();

            // Build adjacency list and in-degree count
            var graph = new Dictionary<ISeed, List<ISeed>>();
            var inDegree = new Dictionary<ISeed, int>();
            var seedLookup = seedList.ToDictionary(s => s.GetType(), s => s);

            // Initialize graph and in-degree
            foreach (var seed in seedList)
            {
                graph[seed] = new List<ISeed>();
                inDegree[seed] = 0;
            }

            // Build the graph
            foreach (var seed in seedList)
            {
                // Get dependencies from new DependsOnTypes property
                var dependencies = seed.DependsOnTypes ?? Array.Empty<Type>();

                // Fall back to legacy DependsOn property if DependsOnTypes is empty
#pragma warning disable CS0618 // Type or member is obsolete
                if (dependencies.Length == 0 && seed.DependsOn != null)
                {
                    dependencies = new[] { seed.DependsOn };
                }
#pragma warning restore CS0618 // Type or member is obsolete

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

            // Find all seeds with no dependencies (in-degree = 0)
            var queue = new Queue<ISeed>();
            foreach (var seed in seedList)
            {
                if (inDegree[seed] == 0)
                {
                    queue.Enqueue(seed);
                }
            }

            // Process seeds in topological order
            while (queue.Count > 0)
            {
                var currentSeed = queue.Dequeue();
                result.Add(currentSeed);

                // Reduce in-degree for all dependent seeds
                foreach (var dependentSeed in graph[currentSeed])
                {
                    inDegree[dependentSeed]--;
                    if (inDegree[dependentSeed] == 0)
                    {
                        queue.Enqueue(dependentSeed);
                    }
                }
            }

            // Check for circular dependencies
            if (result.Count != seedList.Count)
            {
                var unprocessedSeeds = seedList.Except(result).Select(s => s.GetType().Name);
                throw new InvalidOperationException(
                    $"Circular dependency detected in seeds: {string.Join(", ", unprocessedSeeds)}. " +
                    "Please review your seed dependencies to eliminate cycles.");
            }

            return result;
        }
    }
}
