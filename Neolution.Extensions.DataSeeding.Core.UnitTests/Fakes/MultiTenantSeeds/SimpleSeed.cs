namespace Neolution.Extensions.DataSeeding.Core.UnitTests.Fakes.MultiTenantSeeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services;

    /// <summary>
    /// A simple seed for basic testing.
    /// </summary>
    public class SimpleSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SimpleSeed> logger;

        /// <summary>
        /// The singleton service
        /// </summary>
        private readonly IFakeSingletonService singletonService;

        /// <summary>
        /// The transient service
        /// </summary>
        private readonly IFakeTransientService transientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="singletonService">The singleton service.</param>
        /// <param name="transientService">The transient service.</param>
        public SimpleSeed(
            ILogger<SimpleSeed> logger,
            IFakeSingletonService singletonService,
            IFakeTransientService transientService)
        {
            this.logger = logger;
            this.singletonService = singletonService;
            this.transientService = transientService;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => Array.Empty<Type>();

        /// <inheritdoc />
        public Type? DependsOn => null;

        /// <inheritdoc />
        public int Priority => 0;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Executing simple seed");
            await Task.CompletedTask;
        }
    }
}
