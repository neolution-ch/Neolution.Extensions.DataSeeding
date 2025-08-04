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
        /// Tests that seeding operations work correctly with scoped services without ObjectDisposedException.
        /// This verifies the fix for the UserManager issue in console applications.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task SeedingOperations_WorkCorrectlyWithScopedServices()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(ScopedServiceLifetimeTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert - This should work without throwing ObjectDisposedException
            await seeder.SeedAsync();

            // Verify we can still resolve scoped services in a new scope (this should work)
            using var scope = serviceProvider.CreateScope();
            var scopedService = scope.ServiceProvider.GetRequiredService<IFakeScopedService>();
            scopedService.ShouldNotBeNull();
            scopedService.ServiceId.ShouldNotBeEmpty();
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
