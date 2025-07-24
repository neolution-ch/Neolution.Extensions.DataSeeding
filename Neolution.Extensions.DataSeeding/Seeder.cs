namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
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
            foreach (var seed in sortedSeeds)
            {
                this.logger.LogTrace("Executing seed: {SeedTypeName}", seed.GetType().Name);
                await seed.SeedAsync().ConfigureAwait(false);
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
        /// Logs the wrap tree in a pretty format.
        /// </summary>
        /// <param name="wrap">The wrap.</param>
        /// <param name="last">if set to <c>true</c> it's the last wrap.</param>
        /// <param name="indent">The indent.</param>
        private void LogWrapTree(Wrap wrap, bool last, string indent = "")
        {
            var seedTypeName = wrap.SeedType?.Name ?? string.Empty;

            // Shorten the seed type name if it was suffixed with "Seed"
            const string suffix = "Seed";
            if (seedTypeName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                seedTypeName = seedTypeName[..^suffix.Length];
            }

            this.logger.LogDebug("{Indent}+- {SeedTypeName}", indent, seedTypeName);
            indent += last ? "   " : "|  ";

            for (var i = 0; i < wrap.Wrapped.Count; i++)
            {
                this.LogWrapTree(wrap.Wrapped[i], i == wrap.Wrapped.Count - 1, indent);
            }
        }
    }
}
