namespace Neolution.Extensions.DataSeeding.ScopedServices.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;
    using FakeScopedService = Neolution.Extensions.DataSeeding.ScopedServices.UnitTests.Fakes.Services.FakeScopedService;
    using FakeScopedServiceWithDependency = Neolution.Extensions.DataSeeding.ScopedServices.UnitTests.Fakes.Services.FakeScopedServiceWithDependency;
    using IFakeScopedService = Neolution.Extensions.DataSeeding.ScopedServices.UnitTests.Fakes.Services.IFakeScopedService;
    using IFakeScopedServiceWithDependency = Neolution.Extensions.DataSeeding.ScopedServices.UnitTests.Fakes.Services.IFakeScopedServiceWithDependency;

    /// <summary>
    /// Component tests that verify the complete seeding workflow with various service lifetimes.
    /// These tests ensure the entire seeding pipeline works correctly with scoped dependencies.
    /// </summary>
    public class SeedingWorkflowComponentTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedingWorkflowComponentTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public SeedingWorkflowComponentTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests the complete workflow: registration → dependency analysis → execution with scoped services.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task CompleteWorkflow_WithScopedServices_ExecutesSuccessfully()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(SeedingWorkflowComponentTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - This tests the complete workflow:
            // 1. Service registration (AddDataSeeding)
            // 2. Dependency analysis (UseServiceProvider in Seeding)
            // 3. Seed execution (SeedAsync in Seeder)
            var seeder = serviceProvider.GetRequiredService<ISeeder>();
            await seeder.SeedAsync();

            // If we reach here, the entire workflow succeeded
            seeder.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests that seeds are resolved with correct concrete types during execution.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task SeedExecution_ResolvesSeedsWithCorrectConcreteTypes()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(SeedingWorkflowComponentTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Verify that specific seeds can be resolved by their concrete types
            var iSeedImplementations = serviceProvider.GetServices<ISeed>();
            var concreteSeedTypes = iSeedImplementations.Select(s => s.GetType()).ToList();

            // Assert - Look for WorkflowTestSeed
            var workflowTestSeedType = concreteSeedTypes.FirstOrDefault(t => t.Name == "WorkflowTestSeed");
            workflowTestSeedType.ShouldNotBeNull();

            // Execute seeding to verify everything works
            await seeder.SeedAsync();
        }

        /// <summary>
        /// Tests that service registration includes both interface and concrete type registrations.
        /// </summary>
        [Fact]
        public void ServiceRegistration_IncludesBothInterfaceAndConcreteTypes()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(SeedingWorkflowComponentTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - Seeds should be registered as both ISeed and their concrete types
            var workflowTestSeedType = typeof(WorkflowTestSeed);

            var workflowSeedAsInterface = serviceProvider.GetServices<ISeed>()
                .FirstOrDefault(s => s.GetType() == workflowTestSeedType);
            var workflowSeedAsConcrete = serviceProvider.GetService(workflowTestSeedType!);

            workflowSeedAsInterface.ShouldNotBeNull();
            workflowSeedAsConcrete.ShouldNotBeNull();

            // Seeds are transient, so instances will be different, but both registrations should work
            workflowSeedAsInterface.GetType().ShouldBe(workflowTestSeedType);
            workflowSeedAsConcrete.GetType().ShouldBe(workflowTestSeedType);
        }

        /// <summary>
        /// Tests that dependency analysis can handle seeds with complex dependency chains.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task DependencyAnalysis_HandlesComplexDependencyChains()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(SeedingWorkflowComponentTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act - This should work even with seeds that have multiple scoped dependencies
            var seeder = serviceProvider.GetRequiredService<ISeeder>();
            await seeder.SeedAsync();

            // Assert - If we get here, dependency analysis and execution both succeeded
            seeder.ShouldNotBeNull();
        }

        /// <summary>
        /// Creates the service collection with comprehensive service registrations.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Register services with different lifetimes
            services.AddScoped<IFakeScopedService, FakeScopedService>();
            services.AddSingleton<IFakeSingletonService, FakeSingletonService>();
            services.AddTransient<IFakeTransientService, FakeTransientService>();
            services.AddScoped<IFakeScopedServiceWithDependency, FakeScopedServiceWithDependency>();

            return services;
        }
    }
}
