namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes.MultiTenantSeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    [DependsOn(typeof(TenantsSeed))]
    public class TenantsSettingsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TenantsSettingsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantsSettingsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TenantsSettingsSeed(ILogger<TenantsSettingsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("Seed() called");
            return Task.CompletedTask;
        }
    }
}
