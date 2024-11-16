using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal static class StringDeserializer
{
    internal static string? Deserialize(VcfRow vcfRow, VCdVersion version)
    {
        string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMaskValue(version);

        return val.Length == 0 ? null : val;
    }
}


