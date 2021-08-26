using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class DigitParser
    {
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static int Parse(char c) => (int)c - 48;

    }
}
