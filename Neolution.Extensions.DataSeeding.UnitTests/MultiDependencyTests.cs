namespace Neolution.Extensions.DataSeeding.UnitTests
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiDependencySeeds;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests for the new multi-dependency functionality.
    /// </summary>
    public class MultiDependencyTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDependencyTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public MultiDependencyTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests that seeds with multiple dependencies are executed in correct order.
        /// </summary>
        [Fact]
        public void SeedsWithMultipleDependenciesExecuteInCorrectOrder()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(MultiDependencyTests).Assembly);
            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();
            var dataInitializer = serviceProvider.GetRequiredService<DataInitializerFake>();

            // Act
            var result = dataInitializer.Run();

            // Assert
            result.ShouldBeTrue();
        }

        /// <summary>
        /// Tests circular dependency detection.
        /// </summary>
        [Fact]
        public void CircularDependencyThrowsException()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(CircularDependencyA).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => seeder.SeedAsync().GetAwaiter().GetResult())
                .Message.ShouldContain("Circular dependency detected");
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper).SetMinimumLevel(LogLevel.Debug));
            return services;
        }
    }
}
