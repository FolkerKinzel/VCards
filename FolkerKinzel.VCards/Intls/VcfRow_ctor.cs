using FolkerKinzel.VCards.Intls.Deserializers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls
{
    partial class VcfRow
    {
        /// <summary>
        /// Parsed eine Datenzeile der VCF-Datei.
        /// </summary>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>.</param>
        /// <returns><see cref="VcfRow"/>-Objekt</returns>
        internal static VcfRow? Parse(VCardDeserializationInfo info)
        {
            var builder = info.Builder;

            // vCardRow:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue:Value-Part
            string vCardRow = builder.ToString();

            // vCardRowParts:
            // group.KEY;ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue | Value-Part
            var vCardRowParts = SplitKeySectionFromValue(vCardRow);

            //VcfRow? row = vCardRowParts.Count != 0 ? new VcfRow(vCardRowParts, info) : null; // eigentlich überfüssig (siebt Leerzeilen aus)

            //if(row?.Parameters.Encoding == Models.Enums.VCdEncoding.Base64)
            //{
            //    row.Value = row.Value?.Replace(" ", "");
            //}

            //return row;

            return vCardRowParts.Count != 0 ? new VcfRow(vCardRowParts, info) : null; // eigentlich überfüssig (siebt Leerzeilen aus)

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="vCardRowParts">vCard-Zeile, gesplittet in Key- und Value-Bereich</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>-Objekt.</param>
        private VcfRow(List<string> vCardRowParts, VCardDeserializationInfo info)
        {
            Debug.Assert(vCardRowParts.Count >= 1);

            // keySectionParts:
            // group.KEY | ATTRIBUTE1=AttributeValue;ATTRIBUTE2=AttributeValue
            var keySectionParts = vCardRowParts[0].Split(info.Semicolon, 2, StringSplitOptions.RemoveEmptyEntries);


            // keyParts:
            // group | key
            var keyParts = keySectionParts[0].Split(info.Dot, 2, StringSplitOptions.RemoveEmptyEntries);

            if (keyParts.Length == 2)
            {
                this.Group = keyParts[0];
                this.Key = keyParts[1].ToUpperInvariant();
            }
            else
            {
                this.Key = keyParts[0].ToUpperInvariant();
            }


            this.Parameters = (keySectionParts.Length == 2)
                ? new ParameterSection(this.Key, GetParameters(keySectionParts[1], info), info)
                : new ParameterSection();



            this.Value = (vCardRowParts.Count == 2) ? vCardRowParts[1] : null;
            if (string.IsNullOrWhiteSpace(this.Value)) { this.Value = null; }
        }



        // Attribut-Values dürfen in vCard 4.0 :;, enthalten, wenn sie in doppelte Anführungszeichen
        // eingeschlossen sind!
        private static List<string> SplitKeySectionFromValue(string vCardRow)
        {
            bool isInDoubleQuotes = false;

            List<string> propertyParts = new List<string>();

            for (int i = 0; i < vCardRow.Length; i++)
            {
                char c = vCardRow[i];

                if (c == '"')
                {
                    isInDoubleQuotes = !isInDoubleQuotes;
                }
                else if (c == ':' && !isInDoubleQuotes)
                {
                    propertyParts.Add(vCardRow.Substring(0, i));

                    int valueStart = i + 1;

                    if (valueStart < vCardRow.Length)
                    {
                        propertyParts.Add(vCardRow.Substring(valueStart));
                    }

                    break;
                }
            }//for

            return propertyParts;
        }


        private static List<Tuple<string, string>> GetParameters(string parameterSection, VCardDeserializationInfo info)
        {
            var parameterTuples = new List<Tuple<string, string>>();

            var parameters = SplitParameters(parameterSection);

            foreach (string parameter in parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter))
                {
                    continue;
                }


                var splitArr = parameter.Split(info.EqualSign, 2, StringSplitOptions.RemoveEmptyEntries);

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

            List<string> splittedParameterSection = new List<string>();

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
