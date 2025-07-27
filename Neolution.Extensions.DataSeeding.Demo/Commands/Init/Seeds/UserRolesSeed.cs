namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for user roles that depends on database configuration
    /// </summary>
    public class UserRolesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserRolesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UserRolesSeed(ILogger<UserRolesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type DependsOnType => typeof(DatabaseConfigurationSeed);

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(UserRolesSeed)} - Creating user roles (Admin, User, Guest)");
            return Task.CompletedTask;
        }
    }
}
