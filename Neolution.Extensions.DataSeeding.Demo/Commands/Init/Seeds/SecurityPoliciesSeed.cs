namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Security policies seed that configures advanced security settings
    /// Forms part of the diamond dependency pattern with multiple convergence points
    /// </summary>
    public class SecurityPoliciesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SecurityPoliciesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityPoliciesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SecurityPoliciesSeed(ILogger<SecurityPoliciesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(SystemConfigurationSeed),
            typeof(UserRolesSeed),
            typeof(PermissionsSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding security policies: password rules, session management, encryption settings");
            await Task.Delay(140).ConfigureAwait(false);
        }
    }
}
