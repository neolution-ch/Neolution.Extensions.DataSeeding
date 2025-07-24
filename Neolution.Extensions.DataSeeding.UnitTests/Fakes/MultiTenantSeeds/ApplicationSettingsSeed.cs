namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiTenantSeeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services;

    /// <inheritdoc />
    public class ApplicationSettingsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ApplicationSettingsSeed> logger;

        /// <summary>
        /// The singleton service
        /// </summary>
        private readonly IFakeSingletonService singletonService;

        /// <summary>
        /// The scoped service
        /// </summary>
        private readonly IFakeScopedService scopedService;

        /// <summary>
        /// The transient service
        /// </summary>
        private readonly IFakeTransientService transientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="singletonService">The singleton service.</param>
        /// <param name="scopedService">The scoped service.</param>
        /// <param name="transientService">The transient service.</param>
        public ApplicationSettingsSeed(
            ILogger<ApplicationSettingsSeed> logger,
            IFakeSingletonService singletonService,
            IFakeScopedService scopedService,
            IFakeTransientService transientService)
        {
            this.logger = logger;
            this.singletonService = singletonService;
            this.scopedService = scopedService;
            this.transientService = transientService;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation(
                "Seed() called with Singleton: {SingletonId}, Scoped: {ScopedId}, Transient: {TransientId}",
                this.singletonService.ServiceId,
                this.scopedService.ServiceId,
                this.transientService.ServiceId);
            return Task.CompletedTask;
        }
    }
}
