using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>Parses a data row of the VCF file.</summary>
    /// <param name="vcfRow">The data row of the VCF file</param>
    /// <param name="info">Additional data used for parsing.</param>
    /// <returns>A <see cref="VcfRow"/> object that represents the parsed
    /// <paramref name="vcfRow"/> or <c>null</c> if <paramref name="vcfRow"/>
    /// is invalid.</returns>
    internal static VcfRow? Parse(string vcfRow, VcfDeserializationInfo info)
    {
        // vcfRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part

        // vcfRow parts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueSeparatorIndex = GetValueSeparatorIndex(vcfRow);

        return valueSeparatorIndex > 0 ? new VcfRow(vcfRow, valueSeparatorIndex, info) 
                                       : null;
    }

    /// <summary> Unmasks masked text contained in <see cref="Value" /> according to the
    /// vCard standard used, and decodes it if it is Quoted-Printable encoded.</summary>
    /// <param name="version">The <see cref="VCard.Version"/> of the <see cref="VCard"/>.</param>
    /// <remarks>If the method is called multiple times, it will still only be executed once.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void UnMask(VCdVersion version)
    {
        if (!_unMasked)
        {
            this.Value = this.Value.UnMask(Info.Builder, version);
        }

        _unMasked = true; // not twice!
        DecodeQuotedPrintable();
    }

    /// <summary> Decodes Quoted-Printable encoded text located in <see cref="Value" /> if 
    /// <see cref="ParameterSection.Encoding" /> is equal to 
    /// <see cref="Enc.QuotedPrintable" />.</summary>
    /// <remarks>If the method is called multiple times, it will still only be executed once.</remarks>
    internal void DecodeQuotedPrintable()
    {
        if (!_quotedPrintableDecoded && this.Parameters.Encoding == Enc.QuotedPrintable)
        {
            this.Value = QuotedPrintable.Decode(
                this.Value,
                TextEncodingConverter.GetEncoding(this.Parameters.CharSet)); // null-check not needed
        }

        _quotedPrintableDecoded = true; // not twice!
    }
}
