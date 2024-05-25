using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>ctor</summary>
    /// <param name="vCardRow" />
    /// <param name="valueSeparatorIndex" />
    /// <param name="info" />
    private VcfRow(in ReadOnlyMemory<char> vCardRow, int valueSeparatorIndex, VcfDeserializationInfo info)
    {
        // vCardRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part
        Debug.Assert(valueSeparatorIndex > 0);

        // vCardRowParts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueStart = valueSeparatorIndex + 1;

        this.Value = valueStart < vCardRow.Length ? vCardRow.Slice(valueStart) : default;

        // keySection:
        // group.KEY | ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue
        ReadOnlyMemory<char> keySection = vCardRow.Slice(0, valueSeparatorIndex);
        ReadOnlySpan<char> keySectionSpan = keySection.Span;

        int parameterSeparatorIndex = keySectionSpan.IndexOf(';');
        int keyPartLength = parameterSeparatorIndex == -1 ? keySection.Length : parameterSeparatorIndex;
        ReadOnlySpan<char> keyPartSpan = keySectionSpan.Slice(0, keyPartLength);
        int groupSeparatorIndex = keyPartSpan.IndexOf('.');

        // keyParts:
        // group | key
        int startOfKey = groupSeparatorIndex + 1;

        this.Key = startOfKey > 0
            ? PropertyKeyConverter.Parse(keyPartSpan.Slice(startOfKey))
            : PropertyKeyConverter.Parse(keyPartSpan);

        if (groupSeparatorIndex > 0)
        {
            this.Group = keyPartSpan.Slice(0, groupSeparatorIndex).ToString();
        }

        if (parameterSeparatorIndex != -1 && parameterSeparatorIndex < keySection.Length - 1)
        {
            ReadOnlyMemory<char> parameterSection = keySection.Slice(parameterSeparatorIndex + 1);
            this.Parameters = new ParameterSection(this.Key, in parameterSection, info);
        }
        else
        {
            this.Parameters = new ParameterSection();
        }
    }

}
