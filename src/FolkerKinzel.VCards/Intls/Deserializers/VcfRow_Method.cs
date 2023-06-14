using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>
    /// Parst eine Datenzeile der VCF-Datei.
    /// </summary>
    /// <param name="vCardRow">Die Datenzeile der vCard als <see cref="string"/>.</param>
    /// <param name="info">Ein <see cref="VcfDeserializationInfo"/>.</param>
    /// <returns><see cref="VcfRow"/>-Objekt</returns>
    internal static VcfRow? Parse(string vCardRow, VcfDeserializationInfo info)
    {
        // vCardRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part


        // vCardRowParts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueSeparatorIndex = GetValueSeparatorIndex(vCardRow);


        return valueSeparatorIndex > 0 ? new VcfRow(vCardRow, valueSeparatorIndex, info) : null;
    }


    /// <summary>
    /// Unmaskiert maskierten Text, der sich in <see cref="Value"/> befindet, nach den Maßgaben des
    /// verwendeten vCard-Standards und dekodiert ihn, falls er Quoted-Printable-kodiert ist.
    /// </summary>
    /// <param name="version">Die Versionsnummer des vCard-Standards.</param>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal void UnMask(VCdVersion version)
    {
        if (!_unMasked)
        {
            this.Value = this.Value.UnMask(Info.Builder, version);
            _unMasked = true; // Unmask nicht 2x

            DecodeQuotedPrintable();
        }
    }


    /// <summary>
    /// Dekodiert Quoted-Printable kodierten Text, der sich in <see cref="Value"/> befindet, wenn 
    /// <see cref="VCards.Models.PropertyParts.ParameterSection.Encoding"/>
    /// gleich <see cref="VCdEncoding.QuotedPrintable"/> ist.
    /// </summary>
    internal void DecodeQuotedPrintable()
    {
        if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable && !_quotedPrintableDecoded)
        {
            this.Value = QuotedPrintableConverter.Decode(this.Value, // null-Prüfung nicht erforderlich
                TextEncodingConverter.GetEncoding(this.Parameters.CharSet)); // null-Prüfung nicht erforderlich

            _quotedPrintableDecoded = true; // Encoding nicht 2x durchführen

        }
    }

}
