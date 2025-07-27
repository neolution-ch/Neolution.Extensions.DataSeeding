namespace Neolution.Extensions.DataSeeding.MultiDependency.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Test seed with multiple dependencies.
    /// </summary>
    public class ComplexSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ComplexSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ComplexSeed(ILogger<ComplexSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[] { typeof(FoundationSeedA), typeof(FoundationSeedB) };

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("ComplexSeed executed (depends on FoundationSeedA and FoundationSeedB)");
            return Task.CompletedTask;
        }
    }
}
