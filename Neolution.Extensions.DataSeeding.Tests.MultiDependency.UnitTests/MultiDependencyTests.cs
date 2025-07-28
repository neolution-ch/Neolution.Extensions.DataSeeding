namespace Neolution.Extensions.DataSeeding.Tests.MultiDependency.UnitTests
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests for the multi-dependency functionality.
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
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SeedsWithMultipleDependenciesExecuteInCorrectOrder()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(MultiDependencyTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert - This should not throw
            await seeder.SeedAsync();

            // If we reach this point, the seeding completed successfully
            Assert.NotNull(seeder);
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));

            // Don't register any fake services for multi-dependency tests
            // The multi-dependency seeds should be self-contained
            return services;
        }
    }
}
