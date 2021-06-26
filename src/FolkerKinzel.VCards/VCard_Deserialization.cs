using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Lädt eine VCF-Datei.
        /// </summary>
        /// 
        /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
        /// <param name="textEncoding">Die zum Einlesen der Datei zu verwendende Textenkodierung oder <c>null</c>, um die Datei mit der 
        /// standardgerechten Enkodierung <see cref="Encoding.UTF8"/> einzulesen.</param>
        /// 
        /// <returns>Eine Auflistung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
        /// 
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
        public static List<VCard> LoadVcf(string fileName, Encoding? textEncoding = null)
        {
            using StreamReader reader = InitializeStreamReader(fileName, textEncoding);
            return DoParseVcf(reader);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use LoadVcf instead.", true)]
        public static List<VCard> Load(string fileName, Encoding? textEncoding = null)
            => LoadVcf(fileName, textEncoding);


        /// <summary>
        /// Parst einen <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.
        /// </summary>
        /// <param name="content">Ein <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.</param>
        /// <returns>Eine Sammlung geparster <see cref="VCard"/>-Objekte, die den Inhalt von <paramref name="content"/> darstellen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> ist <c>null</c>.</exception>
        public static List<VCard> ParseVcf(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            using var reader = new StringReader(content);
            return DoParseVcf(reader);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ParseVcf instead.", true)]
        public static List<VCard> Parse(string content) => ParseVcf(content);



        /// <summary>
        /// Deserialisiert eine VCF-Datei mit einem <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">Ein <see cref="TextReader"/>.</param>
        /// <returns>Eine Sammlung von <see cref="VCard"/>-Objekten, die den Inhalt der deserialisierten VCF-Datei darstellen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static List<VCard> DeserializeVcf(TextReader reader)
            => DoParseVcf(reader ?? throw new ArgumentNullException(nameof(reader)));


        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use DeserializeVcf instead.", true)]
        public static List<VCard> Deserialize(TextReader reader) => DeserializeVcf(reader);


        private static List<VCard> DoParseVcf(TextReader reader, VCdVersion versionHint = VCdVersion.V2_1)
        {
            Debug.Assert(reader != null);
            DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoParseVcf) + "(TextReader)");

            var info = new VcfDeserializationInfo();
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

                    if (info.Builder.Capacity > VcfDeserializationInfo.MAX_STRINGBUILDER_CAPACITY)
                    {
                        info.Builder.Clear().Capacity = VcfDeserializationInfo.INITIAL_STRINGBUILDER_CAPACITY;
                    }
                }
            } while (!vcfReader.EOF);

            VCard.Dereference(vCardList);
            return vCardList;
        }



        private static VCard? ParseNestedVcard(string? content, VcfDeserializationInfo info, VCdVersion versionHint)
        {
            // Version 2.1 ist unmaskiert:
            content = versionHint == VCdVersion.V2_1
                ? content
                : content.UnMask(info.Builder, versionHint);

            using var reader = new StringReader(content ?? string.Empty);

            List<VCard>? list = DoParseVcf(reader, versionHint);

            return list.FirstOrDefault();
        }


        [ExcludeFromCodeCoverage]
        private static StreamReader InitializeStreamReader(string fileName, Encoding? textEncoding)
        {
            try
            {
                return new StreamReader(fileName, textEncoding ?? Encoding.UTF8, true);
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

    }
}
