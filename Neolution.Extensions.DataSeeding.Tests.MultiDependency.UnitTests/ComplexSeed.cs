namespace Neolution.Extensions.DataSeeding.Tests.MultiDependency.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// A seed with complex multi-dependencies for testing.
    /// </summary>
    [DependsOn(typeof(FoundationSeedA), typeof(FoundationSeedB))]
    public class ComplexSeed : ISeed
    {
        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }
}
