namespace Neolution.Extensions.DataSeeding.Tests.CircularDependency.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Another seed with no dependencies
    /// </summary>
    public class AnotherSeed : ISeed
    {
        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <returns>A task representing the async operation.</returns>
        public Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }
}
