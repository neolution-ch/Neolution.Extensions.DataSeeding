namespace Neolution.Extensions.DataSeeding.Tests.CircularDependency.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Circular dependency test seed A that depends on B.
    /// </summary>
    [DependsOn(typeof(CircularDependencyB))]
    public class CircularDependencyA : ISeed
    {
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
