using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Utilities
{
    /// <summary>
    /// Class containing all core extension methods in project
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Clamps value between min and max (inclusive)
        /// </summary>
        /// <typeparam name="T">Type of value being clamped</typeparam>
        /// <param name="value">Value to clamp</param>
        /// <param name="min">Minimum bound for value, inclusive</param>
        /// <param name="max">Maximum bound for value, inclusive</param>
        /// <returns>Clamped value</returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            else if (value.CompareTo(max) > 0)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
    }
}
