using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>ctor</summary>
    /// <param name="vCardRow" />
    /// <param name="valueSeparatorIndex" />
    /// <param name="info" />
    private VcfRow(string vCardRow, int valueSeparatorIndex, VcfDeserializationInfo info)
    {
        // vCardRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part
        Debug.Assert(valueSeparatorIndex > 0);

        this.Info = info;

        // vCardRowParts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueStart = valueSeparatorIndex + 1;

        this.Value = valueStart < vCardRow.Length ? vCardRow.Substring(valueStart) : "";

        // keySection:
        // group.KEY | ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue
        ReadOnlySpan<char> keySection = vCardRow.AsSpan(0, valueSeparatorIndex);
        int parameterSeparatorIndex = keySection.IndexOf(';');
        int keyPartLength = parameterSeparatorIndex == -1 ? keySection.Length : parameterSeparatorIndex;
        ReadOnlySpan<char> keyPartSpan = keySection.Slice(0, keyPartLength);
        int groupSeparatorIndex = keyPartSpan.IndexOf('.');

        // keyParts:
        // group | key
        int startOfKey = groupSeparatorIndex + 1;

        this.Key = startOfKey > 0
            ? keyPartSpan.Slice(startOfKey).ToString().ToUpperInvariant()
            : keyPartSpan.ToString().ToUpperInvariant();

        if (groupSeparatorIndex > 0)
        {
            this.Group = keySection.Slice(0, groupSeparatorIndex).ToString();
        }

        if (parameterSeparatorIndex != -1 && parameterSeparatorIndex < keySection.Length - 1)
        {
            ReadOnlySpan<char> parameterSection = keySection.Slice(parameterSeparatorIndex + 1);
            this.Parameters = new ParameterSection(this.Key, parameterSection, info);
        }
        else
        {
            this.Parameters = new ParameterSection();
        }
    }

    // Attribute-values may contain :;, in vCard 4.0 if they are
    // enclosed in double quotes!
    private static int GetValueSeparatorIndex(string vCardRow)
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
