namespace FolkerKinzel.VCards.Intls.Encodings;

internal static class Base64Helper
{
    internal static byte[]? GetBytesOrNull(string? base64)
    {
        try
        {
            return Base64.GetBytes(base64, 
                                   Base64ParserOptions.AcceptMissingPadding);
        }
        catch
        {
            return null;
        }
    }
}
