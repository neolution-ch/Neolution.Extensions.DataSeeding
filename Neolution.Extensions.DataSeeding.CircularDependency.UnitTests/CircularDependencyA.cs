namespace Neolution.Extensions.DataSeeding.CircularDependency.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Circular dependency test seed A that depends on B.
    /// </summary>
    public class CircularDependencyA : ISeed
    {
        /// <summary>
        /// Gets the types this seed depends on.
        /// </summary>
        public Type[] DependsOnTypes { get; } = { typeof(CircularDependencyB) };

        /// <inheritdoc />
        public Type? DependsOn => null;

        /// <inheritdoc />
        public int Priority => 0;

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
