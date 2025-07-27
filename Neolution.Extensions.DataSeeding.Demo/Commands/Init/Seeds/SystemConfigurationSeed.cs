namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for basic system configuration that must run after database setup
    /// Extended to demonstrate dependency chaining
    /// </summary>
    public class SystemConfigurationSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SystemConfigurationSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SystemConfigurationSeed(ILogger<SystemConfigurationSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(DatabaseConfigurationSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding system configuration: API settings, cache policies, security rules, timezone, locale");
            await Task.Delay(100).ConfigureAwait(false);
        }
    }
}
