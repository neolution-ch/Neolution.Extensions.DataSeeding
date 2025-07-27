namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates navigation menus for the website
    /// Depends on content being available to link to
    /// </summary>
    public class MenusSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MenusSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenusSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MenusSeed(ILogger<MenusSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(ContentSeed),
            typeof(ContentTemplatesSeed),
        };

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating navigation menus: Main menu, Footer menu, User account menu...");
            await Task.Delay(100);
            this.logger.LogInformation("Navigation menus created and linked to content");
        }
    }
}
