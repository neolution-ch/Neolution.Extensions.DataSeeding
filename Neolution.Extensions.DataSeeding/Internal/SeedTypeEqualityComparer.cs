namespace Neolution.Extensions.DataSeeding.Internal
{
    using System;
    using System.Collections.Generic;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Compares seeds by their type to detect duplicates.
    /// </summary>
    internal sealed class SeedTypeEqualityComparer : IEqualityComparer<ISeed>
    {
        /// <summary>
        /// Determines whether the specified seeds are equal by comparing their types.
        /// </summary>
        /// <param name="x">The first seed to compare.</param>
        /// <param name="y">The second seed to compare.</param>
        /// <returns>true if the specified seeds have the same type; otherwise, false.</returns>
        public bool Equals(ISeed? x, ISeed? y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.GetType() == y.GetType();
        }

        /// <summary>
        /// Returns a hash code for the specified seed based on its type.
        /// </summary>
        /// <param name="obj">The seed for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified seed.</returns>
        public int GetHashCode(ISeed obj)
        {
            return obj?.GetType().GetHashCode() ?? 0;
        }
    }
}
