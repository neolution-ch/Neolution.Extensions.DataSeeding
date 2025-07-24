namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiDependencySeeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Test seed for circular dependency detection.
    /// </summary>
    public class CircularDependencyB : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CircularDependencyB> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependencyB"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CircularDependencyB(ILogger<CircularDependencyB> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[] { typeof(CircularDependencyA) };

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("CircularDependencyB executed");
            return Task.CompletedTask;
        }
    }
}
