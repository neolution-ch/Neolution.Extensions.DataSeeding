namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// API Gateway seed that manages external communication
    /// Creates another layer in the dependency hierarchy
    /// </summary>
    public class ApiGatewaySeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ApiGatewaySeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiGatewaySeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ApiGatewaySeed(ILogger<ApiGatewaySeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(SystemConfigurationSeed),
            typeof(SecurityPoliciesSeed),
            typeof(UserPermissionsSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding API gateway: rate limiting, authentication, routing rules");
            await Task.Delay(180).ConfigureAwait(false);
        }
    }
}
