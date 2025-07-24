namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A data seed component.
    /// </summary>
    public interface ISeed
    {
        /// <summary>
        /// Gets the priority of this seed. Lower number means higher priority. Default is 1.
        /// NOTE: This property is deprecated and will be removed in a future version.
        /// Use DependsOn instead for proper dependency management.
        /// </summary>
        [Obsolete("Use DependsOn property instead. Priority-based ordering will be removed in favor of dependency-based ordering.")]
        public int Priority => 1;

        /// <summary>
        /// Gets the seed type this seed depends on.
        /// NOTE: This property is deprecated in favor of DependsOnTypes for multiple dependencies.
        /// </summary>
        [Obsolete("Use DependsOnTypes property instead for multiple dependency support.")]
        public Type? DependsOn => null;

        /// <summary>
        /// Gets the seed types this seed depends on. These seeds will be executed before this seed.
        /// </summary>
        public Type[] DependsOnTypes => Array.Empty<Type>();

        /// <summary>
        /// The data to seed.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SeedAsync();
    }
}
