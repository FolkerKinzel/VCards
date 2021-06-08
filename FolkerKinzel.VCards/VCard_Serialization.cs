using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        #region static Methods

        /// <summary>
        /// Speichert eine Liste von <see cref="VCard"/>-Objekten in eine gemeinsame VCF-Datei.
        /// </summary>
        /// 
        /// <param name="vCardList">Die zu speichernden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten. Wenn die Auflistung kein <see cref="VCard"/>-Objekt enthält, wird keine Datei geschrieben.</param>
        /// <param name="fileName">Der Dateipfad. Wenn die Datei existiert, wird sie überschrieben.</param>
        /// <param name="version">Die vCard-Version der zu speichernden VCF-Datei.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>Die Methode serialisiert möglicherweise mehr
        /// vCards, als sich ursprünglich Elemente in <paramref name="vCardList"/> befanden. Dies geschieht, wenn eine VCF-Datei als
        /// vCard 4.0 gespeichert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// Diese <see cref="VCard"/>-Objekte werden von der Methode an <paramref name="vCardList"/> angefügt.
        /// </para>
        /// 
        /// <para>
        /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// <para>
        /// Wenn eine VCF-Datei als vCard 4.0 gespeichert wird, ruft die Methode <see cref="VCard.Dereference(List{VCard?})"/> auf bevor sie erfolgreich
        /// zurückkehrt. Im Fall, dass die Methode eine Ausnahme wirft, ist dies nicht garantiert.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> oder <paramref name="vCardList"/>
        /// ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
        public static void Save(
            string fileName,
            List<
#nullable disable
                VCard
#nullable restore
                > vCardList,
            VCdVersion version = DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default)
        {
            if (vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            // verhindert, dass eine leere Datei geschrieben wird
            if (!vCardList.Any(x => x != null))
            {
                //File.Delete(fileName);
                return;
            }

            using FileStream stream = InitializeFileStream(fileName);
            Serialize(stream, vCardList, version, options);
        }


        /// <summary>
        /// Serialisiert eine Liste von <see cref="VCard"/>-Objekten unter Verwendung des VCF-Formats in einen <see cref="Stream"/>.
        /// </summary>
        /// 
        /// <param name="vCardList">Die zu serialisierenden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// <param name="stream">Ein <see cref="Stream"/>, in den die serialisierten <see cref="VCard"/>-Objekte geschrieben werden.</param>
        /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
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
        /// <para>Die Methode serialisiert möglicherweise mehr
        /// vCards, als sich ursprünglich Elemente in <paramref name="vCardList"/> befanden. Dies geschieht, wenn eine
        /// vCard 4.0 serialisiert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// Diese <see cref="VCard"/>-Objekte werden von der Methode an <paramref name="vCardList"/> angefügt.
        /// </para>
        /// 
        /// <para>Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// <para>
        /// Wenn eine vCard 4.0 serialisiert wird, ruft die Methode <see cref="VCard.Dereference(List{VCard?})"/> auf bevor sie erfolgreich
        /// zurückkehrt. Im Fall, dass die Methode eine Ausnahme wirft, ist dies nicht garantiert.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> oder <paramref name="vCardList"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
        public static void Serialize(Stream stream,
                                     List<
#nullable disable
                                         VCard
#nullable restore
                                         > vCardList,
                                     VCdVersion version = DEFAULT_VERSION,
                                     VcfOptions options = VcfOptions.Default,
                                     bool leaveStreamOpen = false)
        {
            DebugWriter.WriteMethodHeader($"{nameof(VCard)}.{nameof(Serialize)}({nameof(TextWriter)}, List<{nameof(VCard)}>, {nameof(VCdVersion)}, {nameof(VcfOptions)}");

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                if (!leaveStreamOpen)
                {
                    stream.Close();
                }

                throw new ArgumentException(Res.StreamNotWritable, nameof(stream));
            }

            if (vCardList is null)
            {
                if (!leaveStreamOpen)
                {
                    stream.Close();
                }
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
                Reference(vCardList);
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


        /// <summary>
        /// Serialisiert <paramref name="vCardList"/> als einen <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.
        /// </summary>
        /// 
        /// <param name="vCardList">Die zu serialisierenden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <returns><paramref name="vCardList"/>, serialisiert als <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.</returns>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>Die Methode serialisiert möglicherweise mehr
        /// vCards, als sich ursprünglich Elemente in <paramref name="vCardList"/> befanden. Dies geschieht, wenn eine
        /// vCard 4.0 serialisiert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// Diese <see cref="VCard"/>-Objekte werden von der Methode an <paramref name="vCardList"/> angefügt.
        /// </para>
        /// 
        /// <para>Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// <para>
        /// Wenn eine vCard 4.0 serialisiert wird, ruft die Methode <see cref="VCard.Dereference(List{VCard?})"/> auf bevor sie erfolgreich
        /// zurückkehrt. Im Fall, dass die Methode eine Ausnahme wirft, ist dies nicht garantiert.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="OutOfMemoryException">Es ist nicht genug Speicher vorhanden.</exception>
        public static string ToVcfString(List<
#nullable disable
            VCard
#nullable restore
            > vCardList, VCdVersion version = VCard.DEFAULT_VERSION, VcfOptions options = VcfOptions.Default)
        {
            if (vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            using var stream = new MemoryStream();

            VCard.Serialize(stream, vCardList, version, options, leaveStreamOpen: true);
            stream.Position = 0;

            using var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }



        #endregion

        #region Instance Methods



        /// <summary>
        /// Speichert die <see cref="VCard"/>-Instanz als VCF-Datei.
        /// </summary>
        /// 
        /// <param name="fileName">Der Dateipfad. Wenn die Datei existiert, wird sie überschrieben.</param>
        /// <param name="version">Die vCard-Version der zu speichernden VCF-Datei.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <remarks>
        /// 
        /// <para>Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn eine VCF-Datei als
        /// vCard 4.0 gespeichert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// </para>
        /// 
        /// <para>
        /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// <para>
        /// Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Save(
            string fileName,
            VCdVersion version = DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default) => VCard.Save(fileName, new List<VCard> { this }, version, options);


        /// <summary>
        /// Serialisiert die <see cref="VCard"/>-Instanz unter Verwendung des VCF-Formats in einen <see cref="Stream"/>.
        /// </summary>
        /// 
        /// <param name="stream">Ein <see cref="Stream"/>, in den das serialisierte <see cref="VCard"/>-Objekt geschrieben wird.</param>
        /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// <param name="leaveStreamOpen">Mit <c>true</c> wird bewirkt, dass die Methode <paramref name="stream"/> nicht schließt. Der Standardwert
        /// ist <c>false</c>.</param>
        /// 
        /// <remarks>
        /// 
        /// <para>Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn das <see cref="VCard"/>-Objekt als
        /// vCard 4.0 serialisiert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// </para>
        /// 
        /// <para>
        /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// <para>
        /// Wenn eine vCard 4.0 serialisiert wird, ruft die Methode <see cref="VCard.Dereference(List{VCard?})"/> auf bevor sie erfolgreich
        /// zurückkehrt. Im Fall, dass die Methode eine Ausnahme wirft, ist dies nicht garantiert.
        /// </para>
        /// 
        /// <para>
        /// Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Serialize(Stream stream,
                              VCdVersion version = DEFAULT_VERSION,
                              VcfOptions options = VcfOptions.Default,
                              bool leaveStreamOpen = false)

            => VCard.Serialize(stream, new List<VCard> { this }, version, options, leaveStreamOpen);


        /// <summary>
        /// Serialisiert die <see cref="VCard"/>-Instanz als einen <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.
        /// </summary>
        /// 
        /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// 
        /// <returns>Die <see cref="VCard"/>, serialisiert als <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.</returns>
        /// 
        /// <remarks>
        /// 
        /// <para>Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn das <see cref="VCard"/>-Objekt als
        /// vCard 4.0 serialisiert wird und sich 
        /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts
        /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden. 
        /// </para>
        /// 
        /// <para>
        /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
        /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> des <see cref="VCard"/>-Objekts ein 
        /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
        /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
        /// </para>
        /// 
        /// 
        /// <para>
        /// Wenn eine vCard 4.0 serialisiert wird, ruft die Methode <see cref="VCard.Dereference(List{VCard?})"/> auf bevor sie erfolgreich
        /// zurückkehrt. Im Fall, dass die Methode eine Ausnahme wirft, ist dies nicht garantiert.
        /// </para>
        /// 
        /// <para>
        /// Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException"><paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
        /// <exception cref="OutOfMemoryException">Es ist nicht genug Speicher vorhanden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public string ToVcfString(VCdVersion version = DEFAULT_VERSION, VcfOptions options = VcfOptions.Default)
            => VCard.ToVcfString(new List<VCard> { this }, version, options);



        #endregion


        [ExcludeFromCodeCoverage]
        private static FileStream InitializeFileStream(string fileName)
        {
            try
            {
                return new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
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
