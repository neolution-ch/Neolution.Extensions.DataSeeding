namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services;
    using Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Basic Tests.
    /// </summary>
    public class BasicTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public BasicTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Asserts that the seeder runs without any exceptions. The seeded seeds should log to console.
        /// </summary>
        [Fact]
        public void SeedsCanBeSeededWithLoggingTest()
        {
            // Assign
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(BasicTests).Assembly);
            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();
            var dataInitializer = serviceProvider.GetRequiredService<DataInitializerFake>();

            // Act
            var dataInitializerRun = dataInitializer.Run();

            // Assert
            dataInitializerRun.ShouldBeTrue();
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Register fake services with different lifetimes to test scoped service injection
            // Use Tests.Common implementations to match what seeds expect
            services.AddSingleton<IFakeSingletonService, Tests.Common.Fakes.Services.FakeSingletonService>();
            services.AddScoped<IFakeScopedService, Tests.Common.Fakes.Services.FakeScopedService>();
            services.AddTransient<IFakeTransientService, Tests.Common.Fakes.Services.FakeTransientService>();

            // Register the scoped service with dependency to test UserManager-like scenarios
            services.AddScoped<IFakeScopedServiceWithDependency, Tests.Common.Fakes.Services.FakeScopedServiceWithDependency>();

            return services;
        }
    }
}
