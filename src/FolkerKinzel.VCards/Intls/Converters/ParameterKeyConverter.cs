using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ParameterKeyConverter
{
    internal static string ParseParameterKey(ReadOnlySpan<char> key)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return key.Equals(ParameterSection.ParameterKey.TYPE, comp) ? ParameterSection.ParameterKey.TYPE
             : key.Equals(ParameterSection.ParameterKey.VALUE, comp) ? ParameterSection.ParameterKey.VALUE
             : key.Equals(ParameterSection.ParameterKey.PREF, comp) ? ParameterSection.ParameterKey.PREF
             : key.Equals(ParameterSection.ParameterKey.PID, comp) ? ParameterSection.ParameterKey.PID
             : key.Equals(ParameterSection.ParameterKey.ENCODING, comp) ? ParameterSection.ParameterKey.ENCODING
             : key.Equals(ParameterSection.ParameterKey.CHARSET, comp) ? ParameterSection.ParameterKey.CHARSET
             : key.Equals(ParameterSection.ParameterKey.LABEL, comp) ? ParameterSection.ParameterKey.LABEL
             : key.Equals(ParameterSection.ParameterKey.MEDIATYPE, comp) ? ParameterSection.ParameterKey.MEDIATYPE
             : key.Equals(ParameterSection.ParameterKey.LANGUAGE, comp) ? ParameterSection.ParameterKey.LANGUAGE
             : key.Equals(ParameterSection.ParameterKey.ALTID, comp) ? ParameterSection.ParameterKey.ALTID
             : key.Equals(ParameterSection.ParameterKey.SORT_AS, comp) ? ParameterSection.ParameterKey.SORT_AS
             : key.Equals(ParameterSection.ParameterKey.GEO, comp) ? ParameterSection.ParameterKey.GEO
             : key.Equals(ParameterSection.ParameterKey.TZ, comp) ? ParameterSection.ParameterKey.TZ
             : key.Equals(ParameterSection.ParameterKey.INDEX, comp) ? ParameterSection.ParameterKey.INDEX
             : key.Equals(ParameterSection.ParameterKey.LEVEL, comp) ? ParameterSection.ParameterKey.LEVEL
             : key.Equals(ParameterSection.ParameterKey.CALSCALE, comp) ? ParameterSection.ParameterKey.CALSCALE
             : key.Equals(ParameterSection.ParameterKey.CONTEXT, comp) ? ParameterSection.ParameterKey.CONTEXT
             : key.Equals(ParameterSection.ParameterKey.CC, comp) ? ParameterSection.ParameterKey.CC
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.PROP_ID, comp) ? ParameterSection.ParameterKey.Rfc9554.PROP_ID
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.CREATED, comp) ? ParameterSection.ParameterKey.Rfc9554.CREATED
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.DERIVED, comp) ? ParameterSection.ParameterKey.Rfc9554.DERIVED
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.PHONETIC, comp) ? ParameterSection.ParameterKey.Rfc9554.PHONETIC
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.SCRIPT, comp) ? ParameterSection.ParameterKey.Rfc9554.SCRIPT
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.SERVICE_TYPE, comp) ? ParameterSection.ParameterKey.Rfc9554.SERVICE_TYPE
             : key.Equals(ParameterSection.ParameterKey.NonStandard.X_SERVICE_TYPE, comp) ? ParameterSection.ParameterKey.Rfc9554.SERVICE_TYPE
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.USERNAME, comp) ? ParameterSection.ParameterKey.Rfc9554.USERNAME
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.AUTHOR, comp) ? ParameterSection.ParameterKey.Rfc9554.AUTHOR
             : key.Equals(ParameterSection.ParameterKey.Rfc9554.AUTHOR_NAME, comp) ? ParameterSection.ParameterKey.Rfc9554.AUTHOR_NAME
             : key.ToString();
    }
}
