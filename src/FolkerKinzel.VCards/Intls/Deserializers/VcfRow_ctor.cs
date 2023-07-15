using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="vCardRow">vCard-Zeile</param>
    /// <param name="valueSeparatorIndex">Index des des Trennzeichens <c>':'</c>, das den Wert von
    /// <paramref name="vCardRow"/> vom Schlüssel- und Parameterteil trennt.</param>
    /// <param name="info">Ein <see cref="VcfDeserializationInfo"/>-Objekt.</param>
    private VcfRow(string vCardRow, int valueSeparatorIndex, VcfDeserializationInfo info)
    {
        // vCardRow:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part
        Debug.Assert(valueSeparatorIndex > 0);

        this.Info = info;

        // vCardRowParts:
        // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
        int valueStart = valueSeparatorIndex + 1;

        if (valueStart < vCardRow.Length)
        {
            this.Value = vCardRow.Substring(valueStart);
        }


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
            this.Parameters = new ParameterSection(this.Key, GetParameters(parameterSection, info.ParameterList), info);
        }
        else
        {
            this.Parameters = new ParameterSection();
        }
    }



    // Attribut-Values dürfen in vCard 4.0 :;, enthalten, wenn sie in doppelte Anführungszeichen
    // eingeschlossen sind!
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


    private static List<KeyValuePair<string, string>> GetParameters(ReadOnlySpan<char> parameterSection, List<KeyValuePair<string, string>> parameterTuples)
    {
        int splitIndex;
        ReadOnlySpan<char> parameter;
        int parameterStartIndex = 0;

        parameterTuples.Clear();

        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        while (-1 != (splitIndex = GetNextParameterSplitIndex(parameterStartIndex, parameterSection)))
        {
            int paramLength = splitIndex - parameterStartIndex;

            if (paramLength != 0)
            {
                parameter = parameterSection.Slice(parameterStartIndex, paramLength);
                parameterStartIndex = splitIndex + 1;

                if (parameter.IsWhiteSpace())
                {
                    continue;
                }

                SplitParameterKeyAndValue(parameterTuples, parameter);
            }
        }

        int length = parameterSection.Length - parameterStartIndex;

        if (length > 0)
        {
            parameter = parameterSection.Slice(parameterStartIndex, parameterSection.Length - parameterStartIndex);

            if (!parameter.IsWhiteSpace())
            {
                SplitParameterKeyAndValue(parameterTuples, parameter);
            }
        }


        return parameterTuples;


        ////////////////////////////////////////////////////////////////////

        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        static int GetNextParameterSplitIndex(int parameterStartIndex, ReadOnlySpan<char> parameterSection)
        {
            bool isInDoubleQuotes = false;

            for (int i = parameterStartIndex; i < parameterSection.Length; i++)
            {
                char c = parameterSection[i];

                if (c == '"')
                {
                    isInDoubleQuotes = !isInDoubleQuotes;
                }
                else if (c == ';' && !isInDoubleQuotes)
                {
                    return i;
                }
            }//for

            return -1;
        }


        static void SplitParameterKeyAndValue(List<KeyValuePair<string, string>> parameterTuples, ReadOnlySpan<char> parameter)
        {
            int splitIndex = parameter.IndexOf('=');

            if (splitIndex == -1)
            {
                // in vCard 2.1. kann direkt das Value angegeben werden, z.B. Note;Quoted-Printable;UTF-8:Text des Kommentars
                string parameterString = parameter.ToString();
                parameterTuples.Add(new KeyValuePair<string, string>(ParseAttributeKeyFromValue(parameterString), parameterString));
            }
            else
            {
                int valueStart = splitIndex + 1;
                int valueLength = parameter.Length - valueStart;

                if (valueLength != 0)
                {
                    parameterTuples.Add(
                            new KeyValuePair<string, string>(
                                parameter.Slice(0, splitIndex).ToString().ToUpperInvariant(),
                                parameter.Slice(valueStart, valueLength).ToString()));
                }
            }
        }
    }

}
