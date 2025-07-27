namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for user permissions that depends on both users and permissions
    /// </summary>
    public class UserPermissionsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserPermissionsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UserPermissionsSeed(ILogger<UserPermissionsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(UsersSeed),
            typeof(PermissionsSeed),
            typeof(UserRolesSeed),
        };

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(UserPermissionsSeed)} - Assigning permissions to users");
            return Task.CompletedTask;
        }
    }
}
