namespace Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a scoped service that depends on other scoped services.
    /// </summary>
    public interface IFakeScopedServiceWithDependency
    {
        /// <summary>
        /// Performs an async operation that accesses scoped dependencies.
        /// </summary>
        /// <returns>A task representing the async operation.</returns>
        Task<string> PerformScopedOperationAsync();
    }
}
