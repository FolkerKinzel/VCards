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



        private static List<VCard> DoParse(TextReader reader, VCdVersion versionHint = VCdVersion.V2_1)
        {
            DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoParse) + "(TextReader)");

            var info = new VCardDeserializationInfo();
            var vCardList = new List<VCard>();
            var vcfReader = new VcfReader(reader, info);
            var queue = new Queue<VcfRow>(DESERIALIZER_QUEUE_INITIAL_CAPACITY);

            do
            {
                foreach (VcfRow vcfRow in vcfReader)
                {
                    queue.Enqueue(vcfRow);
                }

                if (queue.Count != 0)
                {
                    var vCard = new VCard(queue, info, versionHint);
                    vCardList.Add(vCard);

                    Debug.WriteLine("");
                    Debug.WriteLine("", "Parsed " + nameof(VCard));
                    Debug.WriteLine("");
                    Debug.WriteLine(vCard);

                    queue.Clear();

                    if (info.Builder.Capacity > VCardDeserializationInfo.MAX_STRINGBUILDER_CAPACITY)
                    {
                        info.Builder.Clear().Capacity = VCardDeserializationInfo.INITIAL_STRINGBUILDER_CAPACITY;
                    }
                }
            } while (!vcfReader.EOF);

            return vCardList;
        }



        private static VCard? ParseNestedVcard(string? content, VCardDeserializationInfo info, VCdVersion versionHint)
        {
            // Version 2.1 ist unmaskiert:
            content = versionHint == VCdVersion.V2_1 ? content : info.Builder.Clear().Append(content).UnMask(versionHint).ToString();

            using var reader = new StringReader(content);

            List<VCard>? list = DoParse(reader, versionHint);

            return list.FirstOrDefault();
        }



        /// <summary>
        /// Ersetzt die <see cref="RelationUuidProperty"/>-Objekte der in 
        /// <paramref name="vCardList"/> enthaltenen <see cref="VCard"/>-Objekte durch 
        /// <see cref="RelationVCardProperty"/>-Objekte, die die <see cref="VCard"/>s enthalten,
        /// die durch die <see cref="Guid"/>s der <see cref="RelationUuidProperty"/>-Objekte 
        /// referenziert wurden. Das geschieht nur, wenn sich die referenzierten <see cref="VCard"/>-Objekte in
        /// <paramref name="vCardList"/> befinden.
        /// </summary>
        /// <param name="vCardList">Eine Liste mit <see cref="VCard"/>-Objekten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void DereferenceVCards(List<VCard> vCardList)
        {
            if(vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            foreach (VCard vcard in vCardList)
            {
                if (vcard != null)
                {
                    if (vcard.Relations != null)
                    {
                        List<RelationProperty?> relations = vcard.Relations as List<RelationProperty?> ?? vcard.Relations.ToList();
                        vcard.Relations = relations;
                        SetRelationReferences(relations, vCardList);
                    }

                    if (vcard.Members != null)
                    {
                        List<RelationProperty?> members = vcard.Members as List<RelationProperty?> ?? vcard.Members.ToList();
                        vcard.Members = members;
                        SetRelationReferences(members, vCardList);
                    }
                }
            }

            static void SetRelationReferences(List<RelationProperty?> relations, List<VCard> vCardList)
            {
                IEnumerable<RelationUuidProperty> guidProps = relations
                    .Select(x => x as RelationUuidProperty)
                    .Where(x => x != null && !x.IsEmpty).ToArray()!;

                foreach (RelationUuidProperty guidProp in guidProps)
                {
                    VCard? referencedVCard =
                        vCardList.Where(x => x.UniqueIdentifier != null).FirstOrDefault(x => x.UniqueIdentifier!.Value == guidProp.Value);

                    if (referencedVCard != null)
                    {
                        var vcardProp = new RelationVCardProperty(
                                            referencedVCard,
                                            propertyGroup: guidProp.Group);
                        vcardProp.Parameters.Assign(guidProp.Parameters);

                        _ = relations.Remove(guidProp);
                        relations.Add(vcardProp);
                    }
                }
            }
        }

    }
}
