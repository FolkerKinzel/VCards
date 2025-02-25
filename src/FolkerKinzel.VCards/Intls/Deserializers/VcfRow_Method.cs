namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>Parses a data row of the VCF file.</summary>
    /// <param name="vCardRow">The data row of the VCF file</param>
    /// <param name="info">Additional data used for parsing.</param>
    /// <returns>A <see cref="VcfRow"/> object that represents the parsed
    /// <paramref name="vCardRow"/> or <c>null</c> if <paramref name="vCardRow"/>
    /// is invalid.</returns>
    internal static VcfRow? Parse(in ReadOnlyMemory<char> vCardRow, VcfDeserializationInfo info)
    {
        // vcfRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part

        // vcfRow parts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueSeparatorIndex = GetValueSeparatorIndex(vCardRow.Span);

        return valueSeparatorIndex > 0 ? new VcfRow(in vCardRow, valueSeparatorIndex, info)
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
}
