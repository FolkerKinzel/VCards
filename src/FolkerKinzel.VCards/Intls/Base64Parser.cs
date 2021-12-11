using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls
{
    [Obsolete("This class will be removed with DataUrl.", false)]
    internal static class Base64Parser
    {
        ///// <summary>
        ///// Converts <paramref name="data"/> to a Base64 <see cref="string"/> and uses optionally the
        ///// Base64Url format (RFC 4648 § 5).
        ///// </summary>
        ///// <param name="data">The data to convert.</param>
        ///// <param name="useBase64UrlFormat"><c>true</c> to use the Base64Url format.</param>
        ///// <returns><paramref name="data"/> converted to Base64 or - optionally - Base64Url.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        //internal static string ToBase64String(byte[] data, bool useBase64UrlFormat)
        //{
        //    string base64 = Convert.ToBase64String(data, Base64FormattingOptions.None);

        //    if(useBase64UrlFormat)
        //    {
        //        return ConvertToBase64Url(base64);
        //    }

        //    return base64;
        //}

        

        /// <summary>
        /// Parses a Base64 <see cref="string"/> as byte array - even
        /// if it is in the Base64Url format (RFC 4648 § 5).
        /// </summary>
        /// <param name="base64">A Base64 or Base64Url <see cref="string"/>.</param>
        /// <returns>A byte array containing the data that was encoded in <paramref name="base64"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="base64"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="base64"/> has an invalid format.</exception>
        internal static byte[] ParseBase64(string base64)
        {
            //if(base64 == null)
            //{
            //    throw new ArgumentNullException(nameof(base64));
            //}

            Debug.Assert(base64 != null);

            base64 = ConvertBase64UrlToBase64(base64);
            base64 = HandleBase64WithoutPadding(base64);

            return Convert.FromBase64String(base64);
        }

        private static string ConvertBase64UrlToBase64(string base64)
        {
            base64 = base64.Replace('-', '+');
            return base64.Replace('_', '/');
        }

        //private static string ConvertToBase64Url(string base64)
        //{
        //    base64 = base64.Replace('+', '-');
        //    base64 = base64.Replace('/', '_');
        //    return base64.TrimEnd('=');
        //}

        private static string HandleBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return base64;
        }

    }
}
