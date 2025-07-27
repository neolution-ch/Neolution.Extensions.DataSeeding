namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates user profiles and personalization settings
    /// Depends on UsersSeed to have users available for profile creation
    /// </summary>
    public class UserProfilesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserProfilesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UserProfilesSeed(ILogger<UserProfilesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(UsersSeed),
        };

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating user profiles: bio, avatar, contact information...");
            await Task.Delay(200);
            this.logger.LogInformation("Setting up user preferences: theme, language, notifications...");
            await Task.Delay(150);
            this.logger.LogInformation("Configuring user dashboard layouts and widgets...");
            await Task.Delay(100);
            this.logger.LogInformation("User profiles and personalization settings created successfully");
        }
    }
}
