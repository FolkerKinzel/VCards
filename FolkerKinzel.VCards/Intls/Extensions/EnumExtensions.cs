using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK.VCards.Intls.Extensions
{
    internal static class EnumExtensions
    {
        internal static IEnumerable<Enum> GetFlags(this Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value)) yield return value;
            }
        }


    }
}
