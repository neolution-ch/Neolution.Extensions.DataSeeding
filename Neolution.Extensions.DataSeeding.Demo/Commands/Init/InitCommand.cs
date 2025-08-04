namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.DotNet.Console.Abstractions;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// The data initializer.
    /// </summary>
    public class InitCommand : IDotNetConsoleCommand<InitOptions>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<InitCommand> logger;

        /// <summary>
        /// The seeder.
        /// </summary>
        private readonly ISeeder seeder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitCommand" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="seeder">The seeder.</param>
        public InitCommand(ILogger<InitCommand> logger, ISeeder seeder)
        {
            this.logger = logger;
            this.seeder = seeder;
        }

        /// <inheritdoc />
        public Task RunAsync(InitOptions options, CancellationToken cancellationToken)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return RunInternalAsync();

            async Task RunInternalAsync()
            {
                this.logger.LogInformation("Start data initializer...");

                // Automatic seeding
                await this.seeder.SeedAsync();

                this.logger.LogInformation("Data initializer finished!");
            }
        }
    }
}
