using System.Collections.Generic;
using System.Linq;

namespace Rappers.Baseline.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool DoesNotContain<T>(this IEnumerable<T> source, T value)
        {
            return !source.Contains(value);
        }
    }
}
