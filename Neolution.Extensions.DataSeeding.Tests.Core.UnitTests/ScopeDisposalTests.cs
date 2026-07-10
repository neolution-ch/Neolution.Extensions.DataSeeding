namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests that demonstrate the ObjectDisposedException issue and verify the fix.
    /// </summary>
    public class ScopeDisposalTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeDisposalTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public ScopeDisposalTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests that scoped services accessed after scope disposal throw ObjectDisposedException.
        /// This demonstrates the problem we fixed.
        /// </summary>
        [Fact]
        public void ScopedService_AccessedAfterScopeDisposal_ThrowsObjectDisposedException()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            IFakeScopedServiceWithDependency scopedService;

            // Create and dispose a scope (simulating the old problematic approach)
            using (var scope = serviceProvider.CreateScope())
            {
                scopedService = scope.ServiceProvider.GetRequiredService<IFakeScopedServiceWithDependency>();
            } // Scope is disposed here

            // Act & Assert - This should throw because the scope is disposed
            var exception = Should.Throw<System.ObjectDisposedException>(async () =>
            {
                await scopedService.PerformScopedOperationAsync();
            });

            exception.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests that the current seeding implementation works correctly with scoped services.
        /// This verifies our fix is working.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task CurrentSeedingImplementation_WithScopedServices_WorksCorrectly()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(ScopeDisposalTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - This should work without throwing
            var seeder = serviceProvider.GetRequiredService<ISeeder>();
            await seeder.SeedAsync();

            // If we get here, the scoped services worked correctly
            seeder.ShouldNotBeNull();
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Register all fake services that seeds might need
            services.AddSingleton<IFakeSingletonService, FakeSingletonService>();
            services.AddScoped<IFakeScopedService, FakeScopedService>();
            services.AddTransient<IFakeTransientService, FakeTransientService>();
            services.AddScoped<IFakeScopedServiceWithDependency, FakeScopedServiceWithDependency>();

            return services;
        }
    }
}
