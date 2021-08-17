using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class PolyfillExtension
    {
#if NET40 || NET461 || NETSTANDARD2_0

        public static string[] Split(this string s, string? separator, int count, StringSplitOptions options = System.StringSplitOptions.None)
            => s.Split(separator is null ? null : new string[] { separator }, count, options);

        public static string[] Split(this string s, string? separator, StringSplitOptions options = System.StringSplitOptions.None)
            => s.Split(separator is null ? null : new string[] { separator }, options);

        public static bool Contains(this string s, string value, StringComparison comparisonType)
            => s.IndexOf(value, comparisonType) != -1;


        public static string Replace(this string s, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if(comparisonType == StringComparison.Ordinal)
            {
                return s.Replace(oldValue, newValue);
            }

            if(s is null)
            {
                throw new NullReferenceException();
            }

            if(oldValue is null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if(oldValue.Length == 0)
            {
                throw new ArgumentException(string.Format("{0} is an empty String.", nameof(oldValue)), nameof(oldValue));
            }

            if(s.Length < oldValue.Length)
            {
                return s;
            }

            if(newValue is null)
            {
                newValue = string.Empty;
            }

            var builder = new StringBuilder(newValue.Length > oldValue.Length ? s.Length + s.Length / 2 : s.Length);
            _ = builder.Append(s);

            int idx = s.Length - 1;
            while(idx > -1)
            {
                idx = s.LastIndexOf(oldValue, idx - 1, comparisonType);
                _ = builder.Remove(idx, oldValue.Length).Insert(idx, newValue);
                --idx;
            }

            return builder.ToString();
        }
        

#endif

#if NET461 || NETSTANDARD2_0

        public static bool StartsWith(this ReadOnlySpan<char> span, string? value, StringComparison comparisonType)
            => span.StartsWith(value, comparisonType);

        public static bool EndsWith(this ReadOnlySpan<char> span, string? value, StringComparison comparisonType)
            => span.EndsWith(value, comparisonType);
#endif
    }
}
