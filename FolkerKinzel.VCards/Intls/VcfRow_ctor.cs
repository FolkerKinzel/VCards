using FolkerKinzel.VCards.Intls.Deserializers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Text;

namespace FolkerKinzel.VCards.Intls
{
    internal sealed partial class VcfRow
    {
        /// <summary>
        /// Parsed eine Datenzeile der VCF-Datei.
        /// </summary>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>.</param>
        /// <returns><see cref="VcfRow"/>-Objekt</returns>
        internal static VcfRow? Parse(VCardDeserializationInfo info)
        {
            // vCardRow:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part
            string vCardRow = info.Builder.ToString();


            // vCardRowParts:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
            int separatorIndex = GetValueSeparatorIndex(vCardRow);


            if (separatorIndex > -1)
            {
                string keySection;
                string? value = null;

                keySection = vCardRow.Substring(0, separatorIndex);

                int valueStart = separatorIndex + 1;

                if (valueStart < vCardRow.Length)
                {
                    value = vCardRow.Substring(valueStart);
                }

                if (keySection.Length != 0)
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
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="vCardRowParts">vCard-Zeile, gesplittet in Key- und Value-Bereich</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>-Objekt.</param>
        private VcfRow(string keySection, string? value, VCardDeserializationInfo info)
        {
            this.Value = string.IsNullOrEmpty(value) ? null : value;

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
            this.Key = keyPartSpan.Slice(startOfKey).ToString();

            if (groupSeparatorIndex > 0)
            {
                this.Group = keySection.Substring(0, groupSeparatorIndex);
            }
#endif

            if (parameterSeparatorIndex != -1 && parameterSeparatorIndex < keySection.Length - 1)
            {
                string parameters = keySection.Substring(parameterSeparatorIndex + 1);
                this.Parameters = new ParameterSection(this.Key, GetParameters(parameters, info), info);
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


        private static List<Tuple<string, string>> GetParameters(string parameterSection, VCardDeserializationInfo info)
        {
            var parameterTuples = new List<Tuple<string, string>>();

            List<string> parameters = SplitParameters(parameterSection);

            foreach (string parameter in parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter))
                {
                    continue;
                }


                string[] splitArr = parameter.Split(info.EqualSign, 2, StringSplitOptions.RemoveEmptyEntries);

                if (splitArr.Length == 2)
                {
                    parameterTuples.Add(new Tuple<string, string>(splitArr[0].ToUpperInvariant(), splitArr[1]));
                }
                else
                {
                    // in vCard 2.1. kann direkt das Value angegeben werden, z.B. Note;Quoted-Printable;UTF-8:Text des Kommentars
                    parameterTuples.Add(new Tuple<string, string>(ParseAttributeKeyFromValue(splitArr[0]), splitArr[0]));
                }

            }//foreach

            return parameterTuples;
        }


        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        private static List<string> SplitParameters(string parameterSection)
        {
            bool isInDoubleQuotes = false;
            int nextStringStartIndex = 0;

            var splittedParameterSection = new List<string>();

            int i;

            for (i = 0; i < parameterSection.Length; i++)
            {
                char c = parameterSection[i];

                if (c == '"')
                {
                    isInDoubleQuotes = !isInDoubleQuotes;
                }
                else if (c == ';' && !isInDoubleQuotes)
                {
                    AddSubstring();
                    nextStringStartIndex = i + 1;
                }
            }//for

            AddSubstring();
            return splittedParameterSection;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            void AddSubstring()
            {
                string subString = parameterSection.Substring(nextStringStartIndex, i - nextStringStartIndex);

                if (!string.IsNullOrWhiteSpace(subString))
                {
                    splittedParameterSection.Add(subString);
                }
            }
        }
    }
}
