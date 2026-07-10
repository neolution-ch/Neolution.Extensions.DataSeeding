namespace Neolution.Extensions.DataSeeding.Tests.CircularDependency.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Example seed showing the new simplified syntax for single dependency
    /// </summary>
    [DependsOn(typeof(AnotherSeed))]
    public class SimpleDependencySeed : ISeed
    {
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
