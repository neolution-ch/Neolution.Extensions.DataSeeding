namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes
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

    /// <summary>
    /// Fake transient service interface.
    /// </summary>
    public interface IFakeTransientService
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        string ServiceId { get; }
    }

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
