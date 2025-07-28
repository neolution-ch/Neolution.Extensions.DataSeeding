namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes.MultiTenantSeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    [DependsOn(typeof(TenantsSeed))]
    public class UsersSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UsersSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UsersSeed(ILogger<UsersSeed> logger)
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
