namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiTenantSeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services;

    /// <summary>
    /// A seed that tests scoped service dependencies similar to UserManager scenario.
    /// </summary>
    public class ScopedServiceWithDependencySeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ScopedServiceWithDependencySeed> logger;

        /// <summary>
        /// The scoped service with dependency
        /// </summary>
        private readonly IFakeScopedServiceWithDependency scopedServiceWithDependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedServiceWithDependencySeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="scopedServiceWithDependency">The scoped service with dependency.</param>
        public ScopedServiceWithDependencySeed(
            ILogger<ScopedServiceWithDependencySeed> logger,
            IFakeScopedServiceWithDependency scopedServiceWithDependency)
        {
            this.logger = logger;
            this.scopedServiceWithDependency = scopedServiceWithDependency;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Starting scoped service with dependency seed");

            // This should trigger ObjectDisposedException if scope is disposed
            var result = await this.scopedServiceWithDependency.PerformScopedOperationAsync();

            this.logger.LogInformation("Scoped operation result: {Result}", result);
        }
    }
}
