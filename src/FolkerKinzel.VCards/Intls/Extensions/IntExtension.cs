using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class IntExtension
    {
        /// <summary>
        /// Testet, ob <paramref name="id"/> eine Zahl zwischen 1 und 9 ist.
        /// </summary>
        /// <param name="id">Die zu überprüfende Zahl.</param>
        /// <param name="idName">Name des überprüften Parameters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> liegt nicht im erlaubten Bereich.</exception>
        internal static void ValidateID(this int id, string idName)
        {
            if (id < 1 || id > 9)
            {
                throw new ArgumentOutOfRangeException(idName, Res.PidValue);
            }
        }
    }
}
