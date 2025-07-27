namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Advanced integration platform seed that demonstrates deep dependency chains
    /// This seed depends on multiple high-level seeds creating a complex dependency web
    /// </summary>
    public class IntegrationPlatformSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<IntegrationPlatformSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationPlatformSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public IntegrationPlatformSeed(ILogger<IntegrationPlatformSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(ReportsAndAnalyticsSeed),
            typeof(NotificationTemplatesSeed),
            typeof(AuditLogSeed),
            typeof(WorkflowEngineSeed),
            typeof(ApiGatewaySeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding integration platform: webhooks, API endpoints, data connectors, ETL pipelines");
            await Task.Delay(300).ConfigureAwait(false);
        }
    }
}
