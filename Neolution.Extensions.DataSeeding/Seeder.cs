namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Internal;

    /// <inheritdoc cref="ISeeder" />
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Resolved as a singleton by DI container")]
    internal sealed class Seeder : ISeeder
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<Seeder> logger;

        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seeder" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public Seeder(IServiceProvider serviceProvider, ILogger<Seeder> logger)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            Seeding.Instance.UseServiceProvider(serviceProvider);
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogDebug("Resolving seed dependencies using topological sort");
            var sortedSeeds = Internal.DependencyResolver.ResolveDependencies(Seeding.Instance.Seeds);

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Dependency resolution completed. Seeds will be executed in the following order:");
                for (var index = 0; index < sortedSeeds.Count; index++)
                {
                    var seed = sortedSeeds[index];
                    var dependencies = seed.DependsOnTypes?.Length > 0
                        ? $" (depends on: {string.Join(", ", seed.DependsOnTypes.Select(t => t.Name))})"
                        : " (no dependencies)";
                    this.logger.LogDebug("{Index}.\t{SeedTypeName}{Dependencies}", index + 1, seed.GetType().Name, dependencies);
                }
            }

            this.logger.LogDebug("Start seeding...");

            // Create a scope to handle scoped dependencies and resolve fresh seeds
            using (var scope = this.serviceProvider.CreateScope())
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
    }
}
