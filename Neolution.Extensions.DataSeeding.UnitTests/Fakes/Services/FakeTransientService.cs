namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.Services
{
    /// <summary>
    /// Fake transient service implementation.
    /// </summary>
    public class FakeTransientService : IFakeTransientService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeTransientService"/> class.
        /// </summary>
        public FakeTransientService()
        {
            this.ServiceId = System.Guid.NewGuid().ToString();
        }

        /// <inheritdoc />
        public string ServiceId { get; }
    }
}
