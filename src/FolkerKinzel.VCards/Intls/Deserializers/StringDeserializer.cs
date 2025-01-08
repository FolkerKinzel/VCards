using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal static class StringDeserializer
{
    internal static string Deserialize(VcfRow vcfRow, VCdVersion version)
    {
        return vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMaskValue(version);
    }
}


