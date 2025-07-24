namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiDependencySeeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Test seed for circular dependency detection.
    /// </summary>
    public class CircularDependencyA : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CircularDependencyA> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependencyA"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CircularDependencyA(ILogger<CircularDependencyA> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[] { typeof(CircularDependencyB) };

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("CircularDependencyA executed");
            return Task.CompletedTask;
        }
    }
}
