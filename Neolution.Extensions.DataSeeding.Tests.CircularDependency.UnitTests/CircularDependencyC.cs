namespace Neolution.Extensions.DataSeeding.Tests.CircularDependency.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Circular dependency test seed C that depends on A, forming a 3-node cycle: A → B → C → A.
    /// </summary>
    [DependsOn(typeof(CircularDependencyA))]
    public class CircularDependencyC : ISeed
    {
        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SeedAsync()
        {
            // This seed intentionally creates a 3-node circular dependency for testing
            return Task.CompletedTask;
        }
    }
}
