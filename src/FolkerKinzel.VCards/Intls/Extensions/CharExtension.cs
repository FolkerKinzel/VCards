using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class CharExtension
    {
        internal static bool IsDecimalDigit(this char c) => 47u < c && 58u > c;
    }
}
