namespace Neolution.Extensions.DataSeeding.Tests.MultiDependency.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Foundation seed A.
    /// </summary>
    public class FoundationSeedA : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<FoundationSeedA> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationSeedA"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FoundationSeedA(ILogger<FoundationSeedA> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => Array.Empty<Type>();

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("FoundationSeedA executed (no dependencies)");
            return Task.CompletedTask;
        }
    }
}
