namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Workflow engine seed that provides business process automation
    /// Demonstrates diamond dependency pattern by depending on overlapping components
    /// </summary>
    public class WorkflowEngineSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WorkflowEngineSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowEngineSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public WorkflowEngineSeed(ILogger<WorkflowEngineSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(UserPermissionsSeed),
            typeof(NotificationTemplatesSeed),
            typeof(SystemConfigurationSeed),
            typeof(BusinessRulesSeed),
        };

        /// <inheritdoc />
        public Type DependsOnType => null;

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Seeding workflow engine: process definitions, approval flows, state machines");
            await Task.Delay(250).ConfigureAwait(false);
        }
    }
}
