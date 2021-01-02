using System;
using System.Collections.Generic;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class StringExtension
    {
        /// <summary>
        /// Splittet den Value-Teil einer vCard-Property unter Berücksichtigung der maskierten Zeichen.
        /// Kann auch auf <c>null</c> aufgerufen werden: Gibt dann eine leere Liste zurück.
        /// </summary>
        /// <param name="valueString">Der zu splittende String.</param>
        /// <param name="splitChar">Das Trennzeichen.</param>
        /// <param name="options"><see cref="StringSplitOptions"/>: Der Standardwert ist <see cref="StringSplitOptions.None"/>.</param>
        /// <returns>Den gesplitteten String als Liste oder eine leere Liste, wenn <paramref name="valueString"/>
        /// <c>null</c> war.</returns>
        internal static List<string> SplitValueString(
            this string? valueString, char splitChar, StringSplitOptions options = StringSplitOptions.None)
        {
            // wichtig: NIE ändern!
            if (valueString is null)
            {
                return new List<string>();
            }

#if NET40
            var arr = valueString.Split(new char[] { splitChar }, options);
#else
            var arr = valueString.Split(splitChar, options);
#endif

            var list = new List<string>(arr.Length);

            const string MASK = @"\";
            const string MASKED_BACKSLASH = @"\\";

            for (int i = 0; i < arr.Length; i++)
            {
                string current = arr[i];

                if (current != null &&
                    current.EndsWith(MASK, StringComparison.Ordinal) &&
                    !current.EndsWith(MASKED_BACKSLASH, StringComparison.Ordinal))
                {
                    if (++i < arr.Length)
                    {
                        string next = arr[i];
                        current += splitChar + next; // Verkettung funktioniert auch mit null
                    }
                    else
                    {
                        current += splitChar; // maskiertes Zeichen am Stringende
                    }
                }

                list.Add(current ?? string.Empty);
            }//for

            return list;
        }


        /// <summary>
        /// Gibt <c>true</c> zurück, wenn <paramref name="s"/> NON-ASCII-Zeichen oder Zeilenwechsel enthält.
        /// Benötigt keine NULL-Prüfung.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NeedsToBeQpEncoded(this string? s)
        {
            if (s is null)
            {
                return false;
            }

            if (
#if NET40
                s.Contains(Environment.NewLine)
#else
                s.Contains(Environment.NewLine, StringComparison.Ordinal)
#endif
                )
            {
                return true;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] > 126)
                {
                    return true;
                }
            }

            return false;
        }

    }
}