namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates user roles for the CMS application
    /// Depends on system configuration being set up first
    /// </summary>
    [DependsOn(typeof(SystemConfigurationSeed))]
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
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating user roles: Administrator, Editor, Contributor, Subscriber...");
            await Task.Delay(100);
            this.logger.LogInformation("User roles created successfully");
        }
    }
}
