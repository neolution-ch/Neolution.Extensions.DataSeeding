namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Enterprise data warehouse seed that represents the highest complexity tier
    /// This seed demonstrates maximum dependency complexity with multiple convergent paths
    /// </summary>
    public class DataWarehouseSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<DataWarehouseSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWarehouseSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DataWarehouseSeed(ILogger<DataWarehouseSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(IntegrationPlatformSeed),
            typeof(ReportsAndAnalyticsSeed),
            typeof(AuditLogSeed),
            typeof(WorkflowEngineSeed),
            typeof(BusinessRulesSeed),
            typeof(SecurityPoliciesSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding data warehouse: ETL jobs, data marts, OLAP cubes, data lineage, fact tables");
            await Task.Delay(400).ConfigureAwait(false);
        }
    }
}
