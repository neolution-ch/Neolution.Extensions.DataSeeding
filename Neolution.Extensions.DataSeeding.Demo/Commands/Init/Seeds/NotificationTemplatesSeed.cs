namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for notification templates and configurations
    /// Depends on system configuration and tenant settings
    /// </summary>
    public class NotificationTemplatesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<NotificationTemplatesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplatesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NotificationTemplatesSeed(ILogger<NotificationTemplatesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(SystemConfigurationSeed),
            typeof(TenantsSettingsSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding notification templates (email, SMS, push)");
            await Task.Delay(120).ConfigureAwait(false);
        }
    }
}
