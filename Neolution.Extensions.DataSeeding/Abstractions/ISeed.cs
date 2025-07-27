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
        /// Gets the seed types this seed depends on. These seeds will be executed before this seed.
        /// For single dependency, you can also use the DependsOnType property for simpler syntax.
        /// </summary>
        public Type[] DependsOnTypes => Array.Empty<Type>();

        /// <summary>
        /// Gets the single seed type this seed depends on.
        /// This provides a simpler alternative to DependsOnTypes when you only have one dependency.
        /// If both DependsOnType and DependsOnTypes are specified, DependsOnTypes takes precedence.
        /// </summary>
        public Type? DependsOnType => null;

        /// <summary>
        /// The data to seed.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SeedAsync();
    }
}
