namespace Neolution.Extensions.DataSeeding.MultiDependency.UnitTests
{
    using System;
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
        public Type[] DependsOnTypes => Array.Empty<Type>();

        #pragma warning disable CS0618 // Type or member is obsolete
        /// <inheritdoc />
        public Type? DependsOn => null;

        /// <inheritdoc />
        public int Priority => 0;
        #pragma warning restore CS0618 // Type or member is obsolete

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("FoundationSeedB executed (no dependencies)");
            return Task.CompletedTask;
        }
    }
}
