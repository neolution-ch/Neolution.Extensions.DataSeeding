namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for audit logs and user activity tracking
    /// Depends on users, permissions, and user roles
    /// </summary>
    public class AuditLogSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AuditLogSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AuditLogSeed(ILogger<AuditLogSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(UsersSeed),
            typeof(UserPermissionsSeed),
            typeof(SystemConfigurationSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding audit log configuration and sample entries");
            await Task.Delay(150).ConfigureAwait(false);
        }
    }
}
