namespace Neolution.Extensions.DataSeeding.Tests.Core.UnitTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services;
    using Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes;
    using Neolution.Extensions.DataSeeding.Tests.Core.UnitTests.Fakes.MultiTenantSeeds;
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
        /// Tests that duplicate seed registration throws an InvalidOperationException with a helpful message.
        /// </summary>
        [Fact]
        public void DuplicateSeedRegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var services = this.CreateServiceCollection();

            services.AddDataSeeding();

            // Issue: SimpleSeed is already added because AddDataSeeding() scanned the assembly for ISeed implementations
            services.AddTransient<ISeed, SimpleSeed>();

            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - The exception should be thrown when the seeder is constructed (UseServiceProvider is called)
            var exception = Should.Throw<InvalidOperationException>(() =>
            {
                serviceProvider.GetRequiredService<DataInitializerFake>();
            });

            exception.Message.ShouldContain("Duplicate seed type(s) detected");
            exception.Message.ShouldContain("SimpleSeed");
            exception.Message.ShouldContain("This usually means the same seed class was registered more than once.");
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
