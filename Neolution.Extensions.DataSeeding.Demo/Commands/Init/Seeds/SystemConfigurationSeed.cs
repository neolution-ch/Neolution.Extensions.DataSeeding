namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Sets up basic system configuration and application settings
    /// Depends on database migration being completed first
    /// </summary>
    public class SystemConfigurationSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SystemConfigurationSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SystemConfigurationSeed(ILogger<SystemConfigurationSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type DependsOnType => typeof(DatabaseMigrationSeed);

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Configuring system settings: site name, timezone, locale, email settings...");
            await Task.Delay(150);
            this.logger.LogInformation("System configuration completed");
        }
    }
}
