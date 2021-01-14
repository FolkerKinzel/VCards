using FolkerKinzel.VCards.Intls.Deserializers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Text;
using System.Collections;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal sealed partial class VcfRow
    {
        /// <summary>
        /// Parsed eine Datenzeile der VCF-Datei.
        /// </summary>
        /// <param name="vCardRow">Die Datenzeile der vCard als <see cref="string"/>.</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>.</param>
        /// <returns><see cref="VcfRow"/>-Objekt</returns>
        internal static VcfRow? Parse(string vCardRow, VCardDeserializationInfo info)
        {
            // vCardRow:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part


            // vCardRowParts:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
            int separatorIndex = GetValueSeparatorIndex(vCardRow);


            if (separatorIndex == -1)
            {
                return null;
            }
            else
            {
                string keySection;
                string? value = null;

                keySection = vCardRow.Substring(0, separatorIndex);

                int valueStart = separatorIndex + 1;

                if (valueStart < vCardRow.Length)
                {
                    value = vCardRow.Substring(valueStart);
                }

                if (keySection.Length == 0)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return new VcfRow(keySection, value, info);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="vCardRowParts">vCard-Zeile, gesplittet in Key- und Value-Bereich</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>-Objekt.</param>
        private VcfRow(string keySection, string? value, VCardDeserializationInfo info)
        {
            this.Info = info;
            this.Value = value;

            // keySectionParts:
            // group.KEY | ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue

            int parameterSeparatorIndex;
            int groupSeparatorIndex = -1;
#if NET40
            string[] keySectionParts = keySection.Split(info.Semicolon, 2, StringSplitOptions.RemoveEmptyEntries);
            parameterSeparatorIndex = keySectionParts[0].Length == keySection.Length ? -1 : keySectionParts[0].Length;

            for (int i = 0; i < parameterSeparatorIndex; i++)
            {
                if(keySectionParts[0][i] == '.')
                {
                    groupSeparatorIndex = i;
                }
            }
#else
            parameterSeparatorIndex = keySection.IndexOf(';');


            // TODO: Überarbeiten wenn keySection ein ReadOnlySpan<char> ist:
            ReadOnlySpan<char> keyPartSpan = keySection.AsSpan(0, parameterSeparatorIndex == -1 ? keySection.Length : parameterSeparatorIndex);
            groupSeparatorIndex = keyPartSpan.IndexOf('.');
#endif

            // keyParts:
            // group | key
            int startOfKey = groupSeparatorIndex + 1;

#if NET40

            if (groupSeparatorIndex > 0)
            {
                this.Group = keySection.Substring(0, groupSeparatorIndex);

                this.Key = keySection.Substring(startOfKey,
                                                parameterSeparatorIndex == -1
                                                    ? keySection.Length - startOfKey
                                                    : parameterSeparatorIndex - startOfKey).ToUpperInvariant();
            }
            else
            {
                this.Key = keySection.Substring(0, parameterSeparatorIndex == -1 ? keySection.Length : parameterSeparatorIndex).ToUpperInvariant();
            }
#else
            Span<char> keySpan = stackalloc char[keyPartSpan.Length - startOfKey];
            _ = startOfKey > 0 ? keyPartSpan.Slice(startOfKey).ToUpperInvariant(keySpan) : keyPartSpan.ToUpperInvariant(keySpan);

            this.Key = keySpan.ToString();

            if (groupSeparatorIndex > 0)
            {
                this.Group = keySection.Substring(0, groupSeparatorIndex);
            }
#endif

            if (parameterSeparatorIndex != -1 && parameterSeparatorIndex < keySection.Length - 1)
            {
                string parameterSection = keySection.Substring(parameterSeparatorIndex + 1);
                this.Parameters = new ParameterSection(this.Key, GetParameters(parameterSection), info);
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


        private static List<KeyValuePair<string, string>> GetParameters(string parameterSection)
        {
            int splitIndex;
            string parameter;
            int parameterStartIndex = 0;
            var parameterTuples = new List<KeyValuePair<string, string>>();

            while (-1 != (splitIndex = GetNextParameterSplitIndex(parameterStartIndex, parameterSection)))
            {
                // key=value;key="value,value,va;lue";key="val;ue" wird zu
                // key=value | key="value,value,va;lue" | key="val;ue"
                parameter = parameterSection.Substring(parameterStartIndex, splitIndex - parameterStartIndex);
                parameterStartIndex = splitIndex + 1;

                if (string.IsNullOrWhiteSpace(parameter))
                {
                    continue;
                }

                SplitParameterKeyAndValue(parameterTuples, parameter);
            }

            int length = parameterSection.Length - parameterStartIndex;

            if (length > 0)
            {
                parameter = parameterSection.Substring(parameterStartIndex, parameterSection.Length - parameterStartIndex);

                if (!string.IsNullOrWhiteSpace(parameter))
                {
                    SplitParameterKeyAndValue(parameterTuples, parameter);
                }
            }


            return parameterTuples;


            ////////////////////////////////////////////////////////////////////

            // key=value;key="value,value,va;lue";key="val;ue" wird zu
            // key=value | key="value,value,va;lue" | key="val;ue"
            static int GetNextParameterSplitIndex(int parameterStartIndex, string parameterSection)
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


            static void SplitParameterKeyAndValue(List<KeyValuePair<string, string>> parameterTuples, string parameter)
            {
                int splitIndex = parameter.IndexOf('=');

                if (splitIndex == -1)
                {
                    // in vCard 2.1. kann direkt das Value angegeben werden, z.B. Note;Quoted-Printable;UTF-8:Text des Kommentars
                    parameterTuples.Add(new KeyValuePair<string, string>(ParseAttributeKeyFromValue(parameter), parameter));
                }
                else
                {
                    parameterTuples.Add(
                        new KeyValuePair<string, string>(
                            parameter.Substring(0, splitIndex).ToUpperInvariant(),
                            parameter.Substring(splitIndex + 1, parameter.Length - (splitIndex + 1))));
                }

                //string[] splitArr = parameter.Split(info.EqualSign, 2, StringSplitOptions.RemoveEmptyEntries);

                //if (splitArr.Length == 2)
                //{
                //    parameterTuples.Add(new Tuple<string, string>(splitArr[0].ToUpperInvariant(), splitArr[1]));
                //}
                //else
                //{
                //    // in vCard 2.1. kann direkt das Value angegeben werden, z.B. Note;Quoted-Printable;UTF-8:Text des Kommentars
                //    parameterTuples.Add(new Tuple<string, string>(ParseAttributeKeyFromValue(splitArr[0]), splitArr[0]));
                //}
            }
        }




    }
}
