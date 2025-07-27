namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Seed for database configuration that runs first
    /// </summary>
    public class DatabaseConfigurationSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<DatabaseConfigurationSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfigurationSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DatabaseConfigurationSeed(ILogger<DatabaseConfigurationSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(DatabaseConfigurationSeed)} - Setting up database configuration");
            return Task.CompletedTask;
        }
    }
}
