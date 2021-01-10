
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Lädt eine vcf-Datei.
        /// </summary>
        /// <param name="fileName">Absoluter oder relativer Pfad zu einer vcf-Datei.</param>
        /// <param name="textEncoding">Die zum Einlesen der Datei zu verwendende Textenkodierung oder <c>null</c>, um die Datei mit der 
        /// standardgerechten Enkodierung <see cref="Encoding.UTF8"/> einzulesen.</param>
        /// <returns>Ein Collection geparster <see cref="VCard"/>-Objekte, die den Inhalt der vcf-Datei darstellen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
        public static List<VCard> Load(string fileName, Encoding? textEncoding = null)
        {
            try
            {
                using var reader = new StreamReader(fileName, textEncoding ?? Encoding.UTF8, true);
                return DoParse(reader);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (NotSupportedException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (PathTooLongException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
        }


        /// <summary>
        /// Parst einen <see cref="string"/>, der vCard-Daten enthält.
        /// </summary>
        /// <param name="content">Ein <see cref="string"/>, der vCard-Daten enthält.</param>
        /// <returns>Eine Collection geparster <see cref="VCard"/>-Objekte, die den Inhalt von <paramref name="content"/> darstellen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> ist <c>null</c>.</exception>
        public static List<VCard> Parse(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            using var reader = new StringReader(content);
            return DoParse(reader);
        }


        // Achtung: Version 2.1. unterstützt solche eingebetteten VCards in der AGENT-Property:
        //
        // AGENT:
        // BEGIN:VCARD
        // VERSION:2.1
        // N:Friday;Fred
        // TEL; WORK;VOICE:+1-213-555-1234
        // TEL;WORK;FAX:+1-213-555-5678
        // END:VCARD
        private static List<VCard> DoParse(TextReader reader)
        {
            DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoParse) + "(TextReader)");

            var info = new VCardDeserializationInfo();
            var vCardList = new List<VCard>();

            while (GetVCard(reader, info, VCdVersion.V2_1, out VCard? vCard))
            {
                if (vCard != null)
                {
                    vCardList.Add(vCard);
                }
            }

            SetReferences(vCardList);

            return vCardList;
        }



        private static VCard? ParseNestedVcard(string? content, VCardDeserializationInfo info, VCdVersion versionHint)
        {
            // Version 2.1 ist unmaskiert:
            content = versionHint == VCdVersion.V2_1 ? content : info.Builder.Clear().Append(content).UnMask(versionHint).ToString();

            using var reader = new StringReader(content);
            _ = GetVCard(reader, info, versionHint, out VCard? vCard);

            return vCard;
        }


        private static bool GetVCard(TextReader reader, VCardDeserializationInfo info, VCdVersion versionHint, out VCard? vCard)
        {
            // Die TextReader.ReadLine()-Methode normalisiert die Zeilenwechselzeichen!

            Debug.WriteLine("");

            string s;
            //var builder = info.Builder;

            vCard = null;

            do //findet den Anfang der vCard
            {
                s = reader.ReadLine();
                if (s == null)
                {
                    return false; //Dateiende
                }

                Debug.WriteLine(s);

            } while (!_vCardBegin.IsMatch(s));


            Queue<VcfRow>? vcfRows = ParseVcfRows();

            if (vcfRows == null)
            {
                return false; //Dateiende
            }

            try
            {
                vCard = new VCard(vcfRows, info, versionHint);

                Debug.WriteLine("");
                Debug.WriteLine("", "Parsed " + nameof(VCard));
                Debug.WriteLine("");
                Debug.WriteLine(vCard);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return true; //Datei noch nicht zu Ende


            //////////////////////////////////////////////////////////////////

            Queue<VcfRow>? ParseVcfRows()
            {

                _ = info.Builder.Clear(); // nötig, wenn die vcf-Datei mehrere vCards enthält

                var parsedVcfRows = new Queue<VcfRow>();

                bool isFirstLine = true;
                bool isVcard_2_1 = true;

                do
                {
                    s = reader.ReadLine();
                    if (s == null)
                    {
                        return null;  // Dateiende: Sollte END:VCARD fehlen, wird die vCard nicht gelesen.
                    }

                    Debug.WriteLine(s);

                    if (isFirstLine && s.Length != 0)
                    {
                        isFirstLine = false;
                        DetectVCardVersion();
                    }

                    if (isVcard_2_1 && s.Length != 0 && s[s.Length - 1] == '=')  // QuotedPrintable Soft-Linebreak (Dies kann kein "BEGIN:VCARD" und kein "END:VCARD" sein.)
                    {
                        ConcatQuotedPrintableSoftLineBreak();
                        continue;
                    }
                    else if (s.Length != 0 && char.IsWhiteSpace(s[0])) //vCard-Wrapping (Dies kann kein "BEGIN:VCARD" und kein "END:VCARD" sein.)
                    {
                        Debug.WriteLine("  == vCard Line-Wrapping detected ==");
                        ConcatVcardWrapping();
                        continue;
                    }
                    else
                    {
                        if (info.Builder.Length != 0)
                        {
                            if (_vCardBegin.IsMatch(s)) //eingebettete VCard 2.1. AGENT-vCard:
                            {
                                Debug.WriteLine("  == Embedded VCARD 2.1 vCard detected ==");

                                ConcatNestedVcard();
                                _ = AddVcfRow();
                            }
                            else
                            {
                                // s stellt den Beginn einer neuen VcfRow dar. Deshalb enthält
                                // builder bereits eine vollständige VcfRow, die erzeugt werden muss,
                                // bevor s in builder geladen werden kann:
                                _ = AddVcfRow();
                            }
                        }
                        _ = info.Builder.Append(s);
                    }

                } while (!_vCardEnd.IsMatch(s));

                return parsedVcfRows;

                ///////////////////////////////////////////////

                void DetectVCardVersion()
                {
                    if (s.StartsWith("VERSION", StringComparison.OrdinalIgnoreCase))
                    {
                        s = s.TrimEnd(null);
                        if (!s.EndsWith("2.1", StringComparison.Ordinal))
                        {
                            isVcard_2_1 = false;

                            Debug.WriteLine("  == No vCard 2.1 detected ==");
                        }
                    }
                }


                void ConcatQuotedPrintableSoftLineBreak()
                {
                    bool isBase64 = false;

                    if (info.Builder.Length != 0)
                    {
                        isBase64 = AddVcfRow();
                    }

                    if (isBase64)
                    {
                        return;
                    }

                    Debug.WriteLine("  == QuotedPrintable Soft-Linebreak detected ==");
                    _ = info.Builder.Append(s);
                    var vcfRow = VcfRow.Parse(info);

                    if (vcfRow?.Parameters.Encoding == VCdEncoding.QuotedPrintable)
                    {
                        while (s.Length == 0 || s[s.Length - 1] == '=')
                        {
                            s = reader.ReadLine();
                            if (s == null)
                            {
                                return;
                            }

                            Debug.WriteLine(s);
                            if (s.Length == 0)
                            {
                                continue;
                            }

                            _ = info.Builder.Append(Environment.NewLine);
                            _ = info.Builder.Append(s);
                        }
                    }
                }


                ///////////////////////////////////////////////////

                void ConcatVcardWrapping()
                {
                    int insertPosition = info.Builder.Length;
                    _ = info.Builder.Append(s);

                    if (!isVcard_2_1)
                    {
                        _ = info.Builder.Remove(insertPosition, 1); //automatisch eingefügtes Leerzeichen wieder entfernen
                    }
                }


                ///////////////////////////////////////////////////////

                void ConcatNestedVcard()
                {
                    _ = info.Builder.Append(s);
                    do
                    {
                        s = reader.ReadLine();
                        if (s == null)
                        {
                            return;
                        }

                        Debug.WriteLine(s);
                        if (s.Length == 0)
                        {
                            continue;
                        }

                        _ = info.Builder.Append(VCard.NewLine);
                        _ = info.Builder.Append(s);
                    }
                    while (!_vCardEnd.IsMatch(s));

                    s = string.Empty; //damit die äußere Schleife nicht endet
                }


                ////////////////////////////////////////////////

                bool AddVcfRow()
                {
                    var vcfRow = VcfRow.Parse(info);

                    bool vCard2_1Base64Detected = isVcard_2_1 && ConcatVcard2_1Base64();

                    if (vcfRow != null) // null, wenn nicht lesbar
                    {
                        parsedVcfRows.Enqueue(vcfRow);
                    }

                    _ = info.Builder.Clear();

                    return vCard2_1Base64Detected;

                    ///////////////////////////////////////////////////////////////

                    bool ConcatVcard2_1Base64()
                    {
                        if (vcfRow is null)
                        {
                            return false;
                        }

                        if (vcfRow.Parameters.Encoding == VCdEncoding.Base64)
                        {
                            Debug.WriteLine("  == vCard 2.1 Base64 detected ==");

                            _ = info.Builder.Clear();
                            _ = info.Builder.Append(vcfRow.Value);

                            while (s.Length != 0)
                            {
                                _ = info.Builder.Append(s);
                                s = reader.ReadLine();
                                if (s == null)
                                {
                                    return true;
                                }
                                Debug.WriteLine(s);
                                if (s.StartsWith("END:", true, CultureInfo.InvariantCulture))
                                {
                                    break;
                                }
                            }

                            vcfRow.SetValue(info.Builder.Replace(" ", "").ToString());
                            return true;
                        }

                        return false;
                    }
                }
            }
        }


        /// <summary>
        /// Ersetzt die <see cref="RelationUuidProperty"/>-Objekte jeder in 
        /// <paramref name="vCardList"/> enthaltenen <see cref="VCard"/> durch 
        /// <see cref="RelationVCardProperty"/>-Objekte, die die <see cref="VCard"/>s enthalten,
        /// die durch die <see cref="Guid"/>s der <see cref="RelationUuidProperty"/>-Objekte 
        /// referenziert wurden. Das ist nur möglich, wenn sich diese <see cref="VCard"/>s in
        /// <paramref name="vCardList"/> befinden.
        /// </summary>
        /// <param name="vCardList">Eine Liste mit <see cref="VCard"/>-Objekten.</param>
        private static void SetReferences(List<VCard> vCardList)
        {
            Debug.Assert(vCardList != null);

            foreach (VCard? vcard in vCardList)
            {
                Debug.Assert(vcard != null);
                SetRelationReferences((List<RelationProperty?>?)vcard.Relations);
                SetRelationReferences((List<RelationProperty?>?)vcard.Members);
            }

            void SetRelationReferences(List<RelationProperty?>? relations)
            {
                if (relations is null)
                {
                    return;
                }

                RelationUuidProperty[] guidProps = relations
                    .Select(x => x as RelationUuidProperty)
                    .Where(x => x != null && !x.IsEmpty)
                    .ToArray()!;

                if (guidProps == null)
                {
                    return;
                }

                foreach (RelationUuidProperty? guidProp in guidProps)
                {
                    VCard referencedVCard =
                        vCardList.FirstOrDefault(v => v.UniqueIdentifier?.Value == guidProp.Value);

                    if (referencedVCard != null)
                    {
                        var vcardProp = new RelationVCardProperty(
                                            referencedVCard,
                                            propertyGroup: guidProp.Group);
                        vcardProp.Parameters.Assign(guidProp.Parameters);

                        _ = relations.Remove(guidProp);
                    }
                }

            }
        }

    }
}
