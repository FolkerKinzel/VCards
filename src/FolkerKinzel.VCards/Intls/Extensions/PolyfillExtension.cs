#if NET40
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class PolyfillExtension
    {

        internal static bool IsDecimalDigit(this char c) => 47u < c && 58u > c;

        internal static int ParseDecimalDigit(this char digit)
           => digit.IsDecimalDigit()
               ? (int)digit - 48
               : throw new ArgumentOutOfRangeException(nameof(digit));


        internal static bool TryParseDecimalDigit(this char digit, [NotNullWhen(true)] out int? value)
        {
            if (digit.IsDecimalDigit())
            {
                value = (int)digit - 48;
                return true;
            }

            value = null;
            return false;
        }


        internal static string[] Split(this string s, string? separator, int count, StringSplitOptions options = System.StringSplitOptions.None)
            => s.Split(separator is null ? null : new string[] { separator }, count, options);

        internal static string[] Split(this string s, string? separator, StringSplitOptions options = System.StringSplitOptions.None)
            => s.Split(separator is null ? null : new string[] { separator }, options);

        internal static bool Contains(this string s, string value, StringComparison comparisonType)
            => s.IndexOf(value, comparisonType) != -1;


        internal static string Replace(this string s, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (comparisonType == StringComparison.Ordinal)
            {
                return s.Replace(oldValue, newValue);
            }

            if (s is null)
            {
                throw new NullReferenceException();
            }

            if (oldValue is null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                throw new ArgumentException(string.Format("{0} is an empty String.", nameof(oldValue)), nameof(oldValue));
            }

            if (s.Length < oldValue.Length)
            {
                return s;
            }

            if (newValue is null)
            {
                newValue = string.Empty;
            }

            var builder = new StringBuilder(newValue.Length > oldValue.Length ? s.Length + s.Length / 2 : s.Length);
            _ = builder.Append(s);

            int idx = s.Length - 1;
            while (idx > -1)
            {
                idx = s.LastIndexOf(oldValue, idx - 1, comparisonType);
                _ = builder.Remove(idx, oldValue.Length).Insert(idx, newValue);
                --idx;
            }

            return builder.ToString();
        }

    }
}

#endif
