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
    internal static VcfRow? Parse(in ReadOnlyMemory<char> vcfRow, VcfDeserializationInfo info)
    {
        // vcfRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part

        // vcfRow parts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueSeparatorIndex = GetValueSeparatorIndex(vcfRow.Span);

        return valueSeparatorIndex > 0 ? new VcfRow(in vcfRow, valueSeparatorIndex, info)
                                       : null;

        // Attribute-values may contain :;, in vCard 4.0 if they are
        // enclosed in double quotes!
        static int GetValueSeparatorIndex(ReadOnlySpan<char> vCardRow)
        {
            bool isInDoubleQuotes = false;

            for (int i = 0; i < vCardRow.Length; i++)
            {
                char c = vCardRow[i];

                if (c == '"')
                {
                    isInDoubleQuotes = !isInDoubleQuotes;
                }
                else if (c == ':' && !isInDoubleQuotes)
                {
                    return i;
                }
            }//for

            return -1;
        }
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
            this.Value = this.Value.AsSpan().UnMask(version);
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
                this.Value.AsSpan(),
                TextEncodingConverter.GetEncoding(this.Parameters.CharSet)); // null-check not needed
        }

        _quotedPrintableDecoded = true; // not twice!
    }
}
