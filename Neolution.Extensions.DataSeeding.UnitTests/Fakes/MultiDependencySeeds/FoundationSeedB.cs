namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiDependencySeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Foundation seed B.
    /// </summary>
    public class FoundationSeedB : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<FoundationSeedB> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationSeedB"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FoundationSeedB(ILogger<FoundationSeedB> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("FoundationSeedB executed (no dependencies)");
            return Task.CompletedTask;
        }
    }
}
