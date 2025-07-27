namespace Neolution.Extensions.DataSeeding.CircularDependency.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Circular dependency test seed B that depends on A.
    /// </summary>
    public class CircularDependencyB : ISeed
    {
        /// <summary>
        /// Gets the types this seed depends on.
        /// </summary>
        public Type[] DependsOnTypes { get; } = { typeof(CircularDependencyC) };

        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SeedAsync()
        {
            // This seed intentionally creates a circular dependency for testing
            return Task.CompletedTask;
        }
    }
}
