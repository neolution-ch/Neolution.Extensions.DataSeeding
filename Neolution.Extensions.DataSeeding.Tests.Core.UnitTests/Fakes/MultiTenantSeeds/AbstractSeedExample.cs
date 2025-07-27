namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes.MultiTenantSeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Example seed that inherits from the abstract Seed class instead of implementing ISeed directly.
    /// This tests the scenario where users prefer to use the abstract Seed base class.
    /// </summary>
    public class AbstractSeedExample : Seed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AbstractSeedExample> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSeedExample"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AbstractSeedExample(ILogger<AbstractSeedExample> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public override async Task SeedAsync()
        {
            this.logger.LogInformation("AbstractSeedExample.SeedAsync() called");

            // Example of using the protected SeedAsync<T>() helper method
            await SeedAsync<TenantsSeed>().ConfigureAwait(false);

            this.logger.LogInformation("AbstractSeedExample.SeedAsync() completed");
        }
    }
}
