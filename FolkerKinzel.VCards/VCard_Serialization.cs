using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        #region static Methods
        /// <summary>
        /// Speichert eine Liste von <see cref="VCard"/>-Objekten in eine gemeinsame VCF-Datei.
        /// </summary>
        /// 
        /// <param name="fileName">Der Dateipfad. Wenn die Datei existiert, wird sie überschrieben.</param>
        /// <param name="vCardList">Die zu speichernden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten. Wenn die Auflistung kein <see cref="VCard"/>-Objekt enthält, wird keine Datei geschrieben. Die Methode kann
        /// Anzahl und Reihenfolge der Elemente in <paramref name="vCardList"/> ändern!</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// <para>Die Methode serialisiert möglicherweise auch dann mehrere
        /// vCards, wenn <paramref name="vCardList"/> nur ein <see cref="VCard"/>-Objekt enthält - nämlich dann,
        /// wenn dieses <see cref="VCard"/>-Objekt in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert.</para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> oder <paramref name="vCardList"/>
        /// ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
        public static void Save(
            string fileName,
            List<VCard?> vCardList,
            VCdVersion version = VCdVersion.V3_0,
            VcfOptions options = VcfOptions.Default)
        {
            if (vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            // verhindert, dass eine leere Datei geschrieben wird
            if (!vCardList.Any(x => x != null))
            {
                return;
            }

            try
            {
                using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                Serialize(stream, vCardList, version, options);
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
        /// Serialisiert eine Liste von <see cref="VCard"/>-Objekten in einen <see cref="Stream"/>.
        /// </summary>
        /// 
        /// <param name="stream">Ein <see cref="Stream"/>, in den die serialisierten <see cref="VCard"/>-Objekte geschrieben werden.</param>
        /// <param name="vCardList">Die zu serialisierenden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten. Die Methode kann
        /// Anzahl und Reihenfolge der Elemente in <paramref name="vCardList"/> ändern!</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// <param name="leaveStreamOpen">Mit <c>true</c> wird bewirkt, dass die Methode <paramref name="stream"/> nicht schließt. Der Standardwert
        /// ist <c>false</c>.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>Die Methode serialisiert möglicherweise auch dann mehrere
        /// vCards, wenn <paramref name="vCardList"/> nur ein <see cref="VCard"/>-Objekt enthält - nämlich dann,
        /// wenn dieses <see cref="VCard"/>-Objekt in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> oder <paramref name="vCardList"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
        public static void Serialize(Stream stream,
                                     List<VCard?> vCardList,
                                     VCdVersion version = VCdVersion.V3_0,
                                     VcfOptions options = VcfOptions.Default,
                                     bool leaveStreamOpen = false)
        {
            DebugWriter.WriteMethodHeader($"{nameof(VCard)}.{nameof(Serialize)}({nameof(TextWriter)}, List<{nameof(VCard)}>, {nameof(VCdVersion)}, {nameof(VcfOptions)}");

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }


            if (vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }


            if (version < VCdVersion.V4_0)
            {
                if (options.IsSet(VcfOptions.IncludeAgentAsSeparateVCard))
                {
                    for (int i = 0; i < vCardList.Count; i++)
                    {
                        VCard? vCard = vCardList[i];

                        if (vCard?.Relations is null)
                        {
                            continue;
                        }


                        RelationVCardProperty? agent = vCard.Relations
                            .Select(x => x as RelationVCardProperty)
                            .Where(x => x != null && !x.IsEmpty && x.Parameters.RelationType.IsSet(RelationTypes.Agent))
                            .OrderBy(x => x!.Parameters.Preference)
                            .FirstOrDefault();

                        if (agent != null)
                        {
                            if (!vCardList.Contains(agent.Value))
                            {
                                vCardList.Add(agent.Value);
                            }
                        }

                    }//for
                }//if
            }
            else
            {
                SetReferences(vCardList);
            }


            // UTF-8 muss ohne BOM geschrieben werden, da sonst nicht lesbar
            // (vCard 2.1 kann UTF-8 verwenden, da nur ASCII-Zeichen geschrieben werden)
            var encoding = new UTF8Encoding(false);


            using StreamWriter? writer = leaveStreamOpen
#if NET40
                ? new StreamWriter(new Net40LeaveOpenStream(stream), encoding)
#else
                ? new StreamWriter(stream, encoding, 1024, true)
#endif
                : new StreamWriter(stream, encoding);


            var serializer = VcfSerializer.GetSerializer(writer, version, options);


            foreach (VCard? vCard in vCardList)
            {
                if (vCard is null)
                {
                    continue;
                }
                vCard.Version = version;
                serializer.Serialize(vCard);
            }

            if (version >= VCdVersion.V4_0)
            {
                Dereference(vCardList);
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Schreibt die <see cref="VCard"/> auf einen Datenträger.
        /// </summary>
        /// 
        /// <param name="fileName">Der Dateipfad.</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, ist es das 
        /// <see cref="VCard"/>-Objekt nicht. Sperren Sie den lesenden und schreibenden Zugriff auf das
        /// <see cref="VCard"/>-Objekt während der Ausführung dieser Methode!
        /// </note>
        /// <para>Die Methode serialisiert möglicherweise mehrere vCards - nämlich dann,
        /// wenn in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert sind.</para>
        /// <para>
        /// Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Save(
            string fileName,
            VCdVersion version = VCdVersion.V3_0,
            VcfOptions options = VcfOptions.Default) => VCard.Save(fileName, new List<VCard?> { this }, version, options);


        /// <summary>
        /// Serialisiert die <see cref="VCard"/> mit einem <see cref="TextWriter"/>.
        /// </summary>
        /// 
        /// <param name="stream">Der <see cref="Stream"/>, in den die <see cref="VCard"/> serialisiert wird. </param>
        /// <param name="version">Die vCard-Version, in die serialisiert wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// <param name="leaveStreamOpen">Mit <c>true</c> wird bewirkt, dass die Methode <paramref name="stream"/> nicht schließt. Der Standardwert
        /// ist <c>false</c>.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, ist es das 
        /// <see cref="VCard"/>-Objekt nicht. Sperren Sie den lesenden und schreibenden Zugriff auf das
        /// <see cref="VCard"/>-Objekt während der Ausführung dieser Methode!
        /// </note>
        ///<para>Die Methode serialisiert möglicherweise mehrere
        /// vCards - nämlich dann,
        /// wenn in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert sind.</para>
        /// <para>
        /// Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Serialize(Stream stream,
                              VCdVersion version,
                              VcfOptions options = VcfOptions.Default,
                              bool leaveStreamOpen = false)

            => VCard.Serialize(stream, new List<VCard?> { this }, version, options, leaveStreamOpen);


        /// <summary>
        /// Serialisiert die <see cref="VCard"/> als <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.
        /// </summary>
        /// 
        /// <param name="version">Die vCard-Version, in die serialisiert wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <returns>Die <see cref="VCard"/>, serialisiert als <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.</returns>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, ist es das 
        /// <see cref="VCard"/>-Objekt nicht. Sperren Sie den lesenden und schreibenden Zugriff auf das
        /// <see cref="VCard"/>-Objekt während der Ausführung dieser Methode!
        /// </note>
        /// <para>
        /// Die Methode serialisiert möglicherweise mehrere
        /// vCards - nämlich dann,
        /// wenn in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere VCards referenziert sind.
        /// </para>
        /// </remarks>
        public string ToVcfString(VCdVersion version, VcfOptions options = VcfOptions.Default)
        {
            using var stream = new MemoryStream();

            VCard.Serialize(stream, new List<VCard?> { this }, version, options);

            return Encoding.UTF8.GetString(stream.ToArray());
        }


        #endregion




    }
}
