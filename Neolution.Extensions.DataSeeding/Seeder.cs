namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc cref="ISeeder" />
    internal sealed class Seeder : ISeeder
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<Seeder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seeder" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public Seeder(IServiceProvider serviceProvider, ILogger<Seeder> logger)
        {
            this.logger = logger;
            Seeding.Instance.UseServiceProvider(serviceProvider);
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogDebug("Resolving seed dependencies using topological sort");

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                var visualization = Internal.DependencyResolver.CreateDependencyGraphVisualization(Seeding.Instance.Seeds);
                this.logger.LogDebug("Dependency Graph Visualization:{NewLine}{Visualization}", Environment.NewLine, visualization);
            }

            var sortedSeeds = Internal.DependencyResolver.ResolveDependencies(Seeding.Instance.Seeds);

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Dependency resolution completed. Seeds will be executed in the following order:");
                for (var index = 0; index < sortedSeeds.Count; index++)
                {
                    var seed = sortedSeeds[index];
                    var dependencies = GetDependencyDescription(seed);
                    this.logger.LogDebug("{Index}.\t{SeedTypeName}{Dependencies}", index + 1, seed.GetType().Name, dependencies);
                }
            }

            this.logger.LogDebug("Start seeding...");

            // Create a scope to handle scoped dependencies and resolve fresh seeds
            using (var scope = Seeding.Instance.CreateScope())
            {
                // Resolve fresh instances of seeds within the scope to handle scoped dependencies
                foreach (var seed in sortedSeeds)
                {
                    // Get a fresh instance of the seed from the scoped service provider
                    // Use the concrete type to ensure proper resolution
                    var seedType = seed.GetType();
                    var scopedSeed = (ISeed)scope.ServiceProvider.GetRequiredService(seedType);
                    this.logger.LogTrace("Executing seed: {SeedTypeName}", scopedSeed.GetType().Name);
                    await scopedSeed.SeedAsync().ConfigureAwait(false);
                }
            }

            this.logger.LogDebug("All seeds have been seeded!");
        }

        /// <inheritdoc />
        public async Task SeedAsync<T>()
            where T : Seed
        {
            var seed = Seeding.Instance.FindSeed<T>();
            await seed.SeedAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a description of the seed's dependencies for logging purposes.
        /// </summary>
        /// <param name="seed">The seed to describe.</param>
        /// <returns>A formatted dependency description.</returns>
        private static string GetDependencyDescription(ISeed seed)
        {
            // Check dependencies in priority order
            if (seed.DependsOnTypes?.Length > 0)
            {
                return $" (depends on: {string.Join(", ", System.Linq.Enumerable.Select(seed.DependsOnTypes, t => t.Name))})";
            }

            if (seed.DependsOnType != null)
            {
                return $" (depends on: {seed.DependsOnType.Name})";
            }

            return " (no dependencies)";
        }
    }
}
