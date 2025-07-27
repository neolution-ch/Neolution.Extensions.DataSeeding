namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// The Seeding singleton.
    /// </summary>
    internal sealed class Seeding
    {
        /// <summary>
        /// The lazy singleton instantiation.
        /// </summary>
        private static readonly Lazy<Seeding> Lazy = new(() => new Seeding());

        /// <summary>
        /// The assembly that contains the seeds.
        /// </summary>
        private Assembly? seedsAssembly;

        /// <summary>
        /// The service provider.
        /// </summary>
        private IServiceProvider? serviceProvider;

        /// <summary>
        /// Prevents a default instance of the <see cref="Seeding"/> class from being created.
        /// </summary>
        private Seeding()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        internal static Seeding Instance => Lazy.Value;

        /// <summary>
        /// Gets the seeds.
        /// </summary>
        internal IReadOnlyList<ISeed> Seeds { get; private set; } = Enumerable.Empty<ISeed>().ToList();

        /// <summary>
        /// Configures the services with the internal dependency injection container and scans the specified assembly for data seeds.
        /// </summary>
        /// <param name="assembly">The assembly containing the <see cref="ISeed"/> implementations.</param>
        internal void Configure(Assembly assembly)
        {
            this.seedsAssembly = assembly;
        }

        /// <summary>
        /// Uses the <see cref="IServiceProvider"/> with the internal dependency injection container.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal void UseServiceProvider(IServiceProvider serviceProvider)
        {
            if (this.seedsAssembly is null)
            {
                throw new InvalidOperationException("Cannot find the assembly containing the seeds. Did you call the Configure() method before calling this?");
            }

            // Store the service provider for creating scopes during execution
            this.serviceProvider = serviceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<Seeding>>();

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Scan configured assembly for seeds");
                logger.LogTrace("Assembly full name: '{SeedsAssemblyFullName}'", this.seedsAssembly.FullName);
            }

            using (var tempScope = serviceProvider.CreateScope())
            {
                var allSeeds = tempScope.ServiceProvider.GetServices<ISeed>().ToList();
                this.Seeds = allSeeds.Where(seed => seed.GetType().Assembly == this.seedsAssembly).ToList();
            }

            logger.LogDebug("{SeedsCount} seeds have been found and loaded", this.Seeds.Count);
            logger.LogTrace("{SeedCount} ISeed implementations found", this.Seeds.Count);
            logger.LogDebug("Seeding instance ready");
        }

        /// <summary>
        /// Creates a new service scope for seed execution.
        /// </summary>
        /// <returns>A new service scope.</returns>
        internal IServiceScope CreateScope()
        {
            if (this.serviceProvider is null)
            {
                throw new InvalidOperationException("Service provider not configured. Call UseServiceProvider first.");
            }

            return this.serviceProvider.CreateScope();
        }
    }
}
