namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Business rules engine seed that provides configurable business logic
    /// Sits in the middle tier of dependencies
    /// </summary>
    public class BusinessRulesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<BusinessRulesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRulesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public BusinessRulesSeed(ILogger<BusinessRulesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(SystemConfigurationSeed),
            typeof(TenantsSettingsSeed),
            typeof(PermissionsSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding business rules: validation rules, calculation formulas, decision trees");
            await Task.Delay(160).ConfigureAwait(false);
        }
    }
}
