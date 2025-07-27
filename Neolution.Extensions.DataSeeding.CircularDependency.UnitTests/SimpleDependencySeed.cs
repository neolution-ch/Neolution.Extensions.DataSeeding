namespace Neolution.Extensions.DataSeeding.CircularDependency.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Example seed showing the new simplified syntax for single dependency
    /// </summary>
    public class SimpleDependencySeed : ISeed
    {
        /// <summary>
        /// Gets the single seed type this seed depends on.
        /// </summary>
        public Type? DependsOnType => typeof(AnotherSeed);

        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <returns>A task representing the async operation.</returns>
        public Task SeedAsync()
        {
            // Your seeding logic here
            return Task.CompletedTask;
        }
    }
}
