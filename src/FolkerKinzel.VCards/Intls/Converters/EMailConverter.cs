using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class EMailConverter
{
    internal static string ToString(ReadOnlySpan<char> value)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return value.Equals(EMail.SMTP, comp) ? EMail.SMTP
             : value.Equals(EMail.X400, comp) ? EMail.X400
             : value.ToString();
    }
}
