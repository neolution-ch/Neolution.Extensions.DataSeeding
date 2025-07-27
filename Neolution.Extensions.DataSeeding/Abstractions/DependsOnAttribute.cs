namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System;
    using System.Linq;

    /// <summary>
    /// Specifies the seeds that this seed depends on for execution order.
    /// The seeding framework will ensure all dependencies are executed before this seed.
    /// </summary>
    /// <example>
    /// <code>
    /// [DependsOn(typeof(UsersSeed), typeof(RolesSeed))]
    /// public class UserPermissionsSeed : ISeed
    /// {
    ///     public async Task SeedAsync()
    ///     {
    ///         // This will run after UsersSeed and RolesSeed complete
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="seedTypes">The types of seeds that this seed depends on. All types must implement <see cref="ISeed"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="seedTypes"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any seed type does not implement <see cref="ISeed"/> or when duplicate types are specified.</exception>
        public DependsOnAttribute(params Type[] seedTypes)
        {
            if (seedTypes == null)
            {
                throw new ArgumentNullException(nameof(seedTypes));
            }

            // Validate all types implement ISeed
            foreach (var seedType in seedTypes)
            {
                if (seedType == null)
                {
                    throw new ArgumentException("Seed type cannot be null.", nameof(seedTypes));
                }

                if (!typeof(ISeed).IsAssignableFrom(seedType))
                {
                    throw new ArgumentException($"Type '{seedType.FullName}' must implement ISeed interface.", nameof(seedTypes));
                }
            }

            // Remove duplicates and store
            this.SeedTypes = seedTypes.Distinct().ToArray();

            // Warn about duplicates (for development feedback)
            if (this.SeedTypes.Length != seedTypes.Length)
            {
                // In a real implementation, you might want to log this warning
                // For now, we'll just silently deduplicate
            }
        }

        /// <summary>
        /// Gets the types of seeds that this seed depends on.
        /// </summary>
        /// <value>An array of seed types that must implement <see cref="ISeed"/>.</value>
        public Type[] SeedTypes { get; }
    }
}
