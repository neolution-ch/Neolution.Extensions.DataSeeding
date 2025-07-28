namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates content templates and workflow configurations
    /// Depends on UserRolesSeed for permission setup, used by ContentSeed and MenusSeed
    /// </summary>
    [DependsOn(typeof(UserRolesSeed))]
    public class ContentTemplatesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ContentTemplatesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTemplatesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ContentTemplatesSeed(ILogger<ContentTemplatesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating content templates: Article template, Page template, Blog post template...");
            await Task.Delay(200);
            this.logger.LogInformation("Setting up content workflows: Draft → Review → Published states...");
            await Task.Delay(150);
            this.logger.LogInformation("Configuring template permissions based on user roles...");
            await Task.Delay(100);
            this.logger.LogInformation("Content templates and workflows created successfully");
        }
    }
}
