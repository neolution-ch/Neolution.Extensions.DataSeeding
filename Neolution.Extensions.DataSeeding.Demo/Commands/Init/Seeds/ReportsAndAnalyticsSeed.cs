namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for comprehensive reporting and analytics
    /// This demonstrates a complex dependency structure with multiple paths
    /// </summary>
    public class ReportsAndAnalyticsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ReportsAndAnalyticsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsAndAnalyticsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ReportsAndAnalyticsSeed(ILogger<ReportsAndAnalyticsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(AuditLogSeed),
            typeof(NotificationTemplatesSeed),
            typeof(UserPermissionsSeed),
            typeof(DatabaseConfigurationSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding reports, dashboards, and analytics configuration");
            await Task.Delay(200).ConfigureAwait(false);
        }
    }
}
