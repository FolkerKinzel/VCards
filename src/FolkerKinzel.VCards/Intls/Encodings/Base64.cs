namespace FolkerKinzel.VCards.Intls.Encodings;

internal static class Base64
{
    /// <summary>
    /// Parses a Base64 <see cref="string"/> as byte array - even
    /// if it is in the Base64Url format (RFC 4648 § 5).
    /// </summary>
    /// <param name="base64">A Base64 or Base64Url <see cref="string"/>.</param>
    /// <returns>A byte array containing the data that was encoded in <paramref name="base64"/> or <c>null</c>
    /// if the decoding fails.</returns>
    internal static byte[]? Decode(string? base64)
    {
        if (base64 == null)
        {
            return null;
        }

        base64 = ConvertBase64UrlToBase64(base64);
        base64 = HandleBase64WithoutPadding(base64);

        try
        {
            return Convert.FromBase64String(base64);
        }
        catch { return null; }
    }


    //internal static string Encode(byte[] data)
    //{
    //    Debug.Assert(data != null);
    //    return Convert.ToBase64String(data, Base64FormattingOptions.None);
    //}

    private static string ConvertBase64UrlToBase64(string base64) => base64.Replace('-', '+').Replace('_', '/');


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
