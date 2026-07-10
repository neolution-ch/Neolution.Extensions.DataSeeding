namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides seeding methods for the automatically registered seed components.
    /// </summary>
    /// <seealso cref="ISeeder" />
    public interface ISeeder
    {
        /// <summary>
        /// Seeds all registered seed components in dependency order.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SeedAsync();
    }
}
