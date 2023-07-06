using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Tests
{
    public static class HelperExtensions
    {
        public static IEnumerable<TSource> AsWeakEnumerable<TSource>(this IEnumerable<TSource> source) => source.EnumerateWeak().Cast<TSource>();

        private static IEnumerable EnumerateWeak(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (object o in source)
            {
                yield return o;
            }
        }
    }
}
