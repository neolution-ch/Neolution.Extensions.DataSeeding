namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates initial administrator and sample users
    /// Depends on user roles being available
    /// </summary>
    [DependsOn(typeof(UserRolesSeed))]
    public class UsersSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UsersSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UsersSeed(ILogger<UsersSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating initial users: admin@example.com, editor@example.com...");
            await Task.Delay(180);
            this.logger.LogInformation("Initial users created and assigned to roles");
        }
    }
}
