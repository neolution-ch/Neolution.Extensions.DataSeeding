namespace Neolution.Extensions.DataSeeding.Tests.CircularDependency.UnitTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests for circular dependency detection.
    /// </summary>
    public class CircularDependencyTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependencyTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public CircularDependencyTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests circular dependency detection.
        /// </summary>
        [Fact]
        public void CircularDependencyThrowsException()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(CircularDependencyTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => seeder.SeedAsync().GetAwaiter().GetResult())
                .Message.ShouldContain("Circular dependency detected");
        }

        /// <summary>
        /// Tests that complex 3-node circular dependency (A → B → C → A) is detected.
        /// </summary>
        [Fact]
        public void ThreeNodeCircularDependencyDetected()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(CircularDependencyTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert - should detect A → B → C → A cycle
            var exception = Should.Throw<InvalidOperationException>(() => seeder.SeedAsync().GetAwaiter().GetResult());
            exception.Message.ShouldContain("Circular dependency detected");

            // Verify that all three seeds are mentioned as part of the cycle
            exception.Message.ShouldSatisfyAllConditions(
                () => exception.Message.ShouldContain("CircularDependencyA"),
                () => exception.Message.ShouldContain("CircularDependencyB"),
                () => exception.Message.ShouldContain("CircularDependencyC"));
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Don't register any fake services for circular dependency tests
            // The circular dependency seeds should be self-contained
            return services;
        }
    }
}
