namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Sets up the database schema and initial configuration
    /// This seed runs first and has no dependencies
    /// </summary>
    public class DatabaseMigrationSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<DatabaseMigrationSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DatabaseMigrationSeed(ILogger<DatabaseMigrationSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Setting up database schema: creating tables for users, content, categories, menus...");
            await Task.Delay(200);
            this.logger.LogInformation("Database migration completed successfully");
        }
    }
}
