namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

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

    /// <summary>
    /// A scoped service that depends on other scoped services, similar to UserManager.
    /// </summary>
    public class FakeScopedServiceWithDependency : IFakeScopedServiceWithDependency
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeScopedServiceWithDependency"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public FakeScopedServiceWithDependency(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Performs an async operation that accesses scoped dependencies.
        /// </summary>
        /// <returns>A task representing the async operation.</returns>
        public async Task<string> PerformScopedOperationAsync()
        {
            // This simulates UserManager.CreateAsync accessing other scoped services
            var scopedService = this.serviceProvider.GetRequiredService<IFakeScopedService>();
            await Task.Delay(1); // Simulate async work
            return $"Operation completed with {scopedService.ServiceId}";
        }
    }
}
