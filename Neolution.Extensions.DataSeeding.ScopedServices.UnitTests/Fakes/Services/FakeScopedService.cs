namespace Neolution.Extensions.DataSeeding.ScopedServices.UnitTests.Fakes.Services
{
    /// <summary>
    /// Fake scoped service implementation.
    /// </summary>
    public class FakeScopedService : IFakeScopedService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeScopedService"/> class.
        /// </summary>
        public FakeScopedService()
        {
            this.ServiceId = System.Guid.NewGuid().ToString();
        }

        /// <inheritdoc />
        public string ServiceId { get; }
    }
}
