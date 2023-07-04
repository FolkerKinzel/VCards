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
        public static IEnumerable AsWeakEnumerable(this IEnumerable source)
        {
            foreach (object o in source)
            {
                yield return o;
            }
        }
    }
}
