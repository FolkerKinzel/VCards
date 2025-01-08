using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal static class StringSerializer
{
    internal static void Prepare(string? value, VCardProperty prop, VCdVersion version)
    {
        if (prop.IsEmpty || prop.Parameters.DataType == Data.Uri)
        {
            // Valid URIs consist of ASCII characters and don't include
            // line breaks.
            return;
        }

        if (version == VCdVersion.V2_1 && value.NeedsToBeQpEncoded())
        {
            prop.Parameters.Encoding = Enc.QuotedPrintable;
            prop.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal static void AppendVcf(StringBuilder builder, string? value, ParameterSection parameters, VCdVersion version)
    {
        _ = version == VCdVersion.V2_1
                    ? parameters.Encoding == Enc.QuotedPrintable
                        ? builder.AppendQuotedPrintable(value.AsSpan(), builder.Length)
                        : builder.Append(value)
                    // URIs are not masked according to the "Verifier notes" in
                    // https://www.rfc-editor.org/errata/eid3845
                    // It says that "the ABNF does not support escaping for URIs."
                    : parameters.DataType == Data.Uri
                        ? builder.Append(value)
                        : builder.AppendValueMasked(value, version);
    }
}