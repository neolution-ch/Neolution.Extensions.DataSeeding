namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Configures site-wide settings like theme, language, SEO options
    /// Depends on MenusSeed to configure theme settings for menus
    /// </summary>
    [DependsOn(typeof(MenusSeed))]
    public class SettingsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SettingsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SettingsSeed(ILogger<SettingsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Configuring site theme to 'Professional Dark'...");
            await Task.Delay(150);

            this.logger.LogInformation("Setting default language to 'English', timezone to 'UTC'...");
            await Task.Delay(100);

            this.logger.LogInformation("Configuring SEO settings: meta titles, descriptions, keywords...");
            await Task.Delay(120);

            this.logger.LogInformation("Site-wide settings configuration completed");
        }
    }
}
