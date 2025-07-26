namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services
{
    /// <summary>
    /// Fake singleton service implementation.
    /// </summary>
    public class FakeSingletonService : IFakeSingletonService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeSingletonService"/> class.
        /// </summary>
        public FakeSingletonService()
        {
            this.ServiceId = System.Guid.NewGuid().ToString();
        }

        /// <inheritdoc />
        public string ServiceId { get; }
    }
}
