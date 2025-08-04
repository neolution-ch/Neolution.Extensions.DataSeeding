namespace Neolution.Extensions.DataSeeding.Tests.ScopedServices.UnitTests
{
    using System.Threading.Tasks;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Test seed for workflow testing.
    /// </summary>
    public class WorkflowTestSeed : ISeed
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
