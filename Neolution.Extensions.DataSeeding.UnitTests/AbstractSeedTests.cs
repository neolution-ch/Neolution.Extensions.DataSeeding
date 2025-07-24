namespace Neolution.Extensions.DataSeeding.UnitTests
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiTenantSeeds;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests for seeds that inherit from the abstract Seed class.
    /// </summary>
    public class AbstractSeedTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSeedTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public AbstractSeedTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Tests that seeds inheriting from the abstract Seed class can be registered and resolved.
        /// </summary>
        [Fact]
        public void AbstractSeedCanBeRegisteredAndResolved()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(AbstractSeedTests).Assembly);
            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - This should not throw
            var abstractSeeds = serviceProvider.GetServices<Seed>();
            abstractSeeds.ShouldNotBeNull();
            abstractSeeds.ShouldNotBeEmpty();

            // Verify we can find our specific abstract seed
            var abstractSeedExample = abstractSeeds.OfType<AbstractSeedExample>().FirstOrDefault();
            abstractSeedExample.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests that the seeder can execute seeds that inherit from the abstract Seed class.
        /// </summary>
        [Fact]
        public void SeederCanExecuteAbstractSeeds()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(AbstractSeedTests).Assembly);
            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();
            var dataInitializer = serviceProvider.GetRequiredService<DataInitializerFake>();

            // Act
            var result = dataInitializer.Run();

            // Assert
            result.ShouldBeTrue();
        }

        /// <summary>
        /// Tests that abstract seeds can be manually executed via <see cref="ISeeder.SeedAsync{T}" />.
        /// </summary>
        [Fact]
        public void AbstractSeedsCanBeManuallyExecuted()
        {
            // Arrange
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(AbstractSeedTests).Assembly);
            var serviceProvider = services.BuildServiceProvider();
            var seeder = serviceProvider.GetRequiredService<ISeeder>();

            // Act & Assert - This should not throw
            var seedTask = seeder.SeedAsync<AbstractSeedExample>();
            seedTask.ShouldNotBeNull();
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
            services.AddSingleton<IFakeSingletonService, FakeSingletonService>();
            services.AddScoped<IFakeScopedService, FakeScopedService>();
            services.AddTransient<IFakeTransientService, FakeTransientService>();

            return services;
        }
    }
}
