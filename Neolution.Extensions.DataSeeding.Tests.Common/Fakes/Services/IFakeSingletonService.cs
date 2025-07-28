namespace Neolution.Extensions.DataSeeding.Tests.Common.Fakes.Services
{
    /// <summary>
    /// Fake singleton service interface.
    /// </summary>
    public interface IFakeSingletonService
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        string ServiceId { get; }
    }
}
