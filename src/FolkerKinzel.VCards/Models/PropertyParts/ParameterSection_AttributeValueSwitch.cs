using System;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
#if NET8_0_OR_GREATER
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters",
        Justification = "Not localizable")]
#endif
    private static string ParseAttributeKeyFromValue(ReadOnlySpan<char> value)
    {
        Debug.Assert(!value.IsWhiteSpace());

        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        const string ENCODING_PROPERTY = ParameterSection.ParameterKey.ENCODING;
        //const string CONTEXT_PROPERTY = ParameterSection.ParameterKey.CONTEXT;
        const string CHARSET_PROPERTY = ParameterSection.ParameterKey.CHARSET;
        const string TYPE_PROPERTY = ParameterSection.ParameterKey.TYPE;
        const string LANGUAGE_PROPERTY = ParameterSection.ParameterKey.LANGUAGE;
        const string VALUE_PROPERTY = ParameterSection.ParameterKey.VALUE;

        if (value.Equals(TypeValue.PREF, comp)
         || value.Equals(TypeValue.HOME, comp) 
         || value.Equals(TypeValue.WORK, comp)
         || value.Equals(EMail.SMTP, comp)
         || value.Equals(TelConverter.PhoneTypesValue.CELL, comp)
         || value.Equals(TelConverter.PhoneTypesValue.VOICE, comp)
         || value.Equals(TelConverter.PhoneTypesValue.FAX, comp))
        {
            return TYPE_PROPERTY;
        }

        if (value.StartsWith("QUOTED".AsSpan(), comp) || value.Equals("BASE64", comp))
        {
            return ENCODING_PROPERTY;
        }

        if (value.StartsWith("UTF".AsSpan(), comp) || value.StartsWith("ISO".AsSpan(), comp))
        {
            return CHARSET_PROPERTY;
        }

        if (value.Equals(LocConverter.Values.URL, comp)
         || value.Equals(LocConverter.Values.CID, comp)
         || value.Equals(LocConverter.Values.CONTENT_ID, comp)
         || value.Equals(LocConverter.Values.INLINE, comp))
        {
            return VALUE_PROPERTY;
        }

        // Don't change the order: "UTF-8" contains '-' as well as "en-US" as well as "CONTENT-ID"
        return value.Contains('-') 
            ? LANGUAGE_PROPERTY 
            : value.Equals("8BIT", comp) 
                ? ENCODING_PROPERTY 
                : TYPE_PROPERTY;
    }
}
