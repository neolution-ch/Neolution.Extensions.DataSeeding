namespace Neolution.Extensions.DataSeeding.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests to ensure proper service lifetime handling during seeding operations.
    /// These tests prevent regressions related to scoped service injection.
    /// </summary>
    public class ScopedServiceLifetimeTests
    {
        /// <summary>
        /// The test output helper.
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedServiceLifetimeTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public ScopedServiceLifetimeTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests that seeding operations don't leak scoped services outside their scope.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task SeedingOperations_DoNotLeakScopedServices()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(ScopedServiceLifetimeTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act
            await seeder.SeedAsync();

            // Assert - After seeding, we should not be able to access scoped services from root provider
            Should.Throw<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<IFakeScopedService>());
        }

        /// <summary>
        /// Tests that dependency analysis does not cache scoped service instances.
        /// </summary>
        [Fact]
        public void DependencyAnalysis_DoesNotCacheScopedServiceInstances()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(ScopedServiceLifetimeTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Act - Just creating the seeder should trigger dependency analysis
            serviceProvider.GetRequiredService<ISeeder>();

            // Assert - We should still be able to create new scoped services
            using var scope = serviceProvider.CreateScope();
            var scopedService = scope.ServiceProvider.GetRequiredService<IFakeScopedService>();
            scopedService.ShouldNotBeNull();
            scopedService.ServiceId.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests that the root cause error (cannot resolve scoped service from root) does not occur.
        /// </summary>
        [Fact]
        public void SeedRegistration_DoesNotTriggerScopedServiceRootResolutionError()
        {
            // Arrange & Act - This should not throw the root cause error:
            // "Cannot resolve 'IEnumerable<ISeed>' from root provider because it requires scoped service"
            var services = this.CreateServiceCollection();

            Should.NotThrow(() =>
            {
                services.AddDataSeeding(typeof(ScopedServiceLifetimeTests).Assembly);
                var serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetRequiredService<ISeeder>();
            });
        }

        /// <summary>
        /// Creates the service collection with all required dependencies.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Register services with different lifetimes
            services.AddSingleton<IFakeSingletonService, FakeSingletonService>();
            services.AddScoped<IFakeScopedService, FakeScopedService>();
            services.AddTransient<IFakeTransientService, FakeTransientService>();
            services.AddScoped<IFakeScopedServiceWithDependency, FakeScopedServiceWithDependency>();

            return services;
        }
    }
}
