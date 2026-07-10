namespace Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services
{
    /// <summary>
    /// Fake scoped service interface.
    /// </summary>
    public interface IFakeScopedService
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        string ServiceId { get; }
    }
}
