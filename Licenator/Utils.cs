using System.Collections.Generic;
using System.Linq;

namespace Licenator
{
    public static class Utils
    {
        public static IEnumerable<T> CombineUnique<T>(this IEnumerable<T> me, IEnumerable<T> toCombine)
        {
            return me.Concat(toCombine).Distinct();
        }
    }
}