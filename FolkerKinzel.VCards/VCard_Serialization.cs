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
    public partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {

        #region static Methods
        /// <summary>
        /// Speichert eine Liste von <see cref="VCard"/>-Objekten in eine gemeinsame VCF-Datei.
        /// </summary>
        /// <param name="fileName">Der Dateipfad.</param>
        /// <param name="vcards">Die zu speichernden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten. Wenn die Auflistung kein <see cref="VCard"/>-Objekt enthält, wird keine Datei geschrieben.</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> oder <paramref name="vcards"/>
        /// ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
        /// <remarks>Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!</remarks>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Save(
            string fileName,
            List<VCard?> vcards,
            VCdVersion version = VCdVersion.V3_0,
            VcfOptions options = VcfOptions.Default)
        {
            if (vcards is null) throw new ArgumentNullException(nameof(vcards));

            if (!vcards.Any(x => x != null)) return;

            try
            {
                // UTF-8 muss ohne BOM geschrieben werden, da sonst nicht lesbar
                // (vCard 2.1 kann UTF-8 verwenden, da nur ASCII-Zeichen geschrieben werden)
                using var writer = new StreamWriter(fileName, false, new UTF8Encoding(false));
                Serialize(writer, vcards, version, options);
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
        /// Serialisiert eine Liste von <see cref="VCard"/>-Objekten mit einem <see cref="TextWriter"/>.
        /// </summary>
        /// <remarks>Die Methode serialisiert möglicherweise auch dann mehrere
        /// vCards, wenn <paramref name="vcards"/> nur ein <see cref="VCard"/>-Objekt enthielt, nämlich dann,
        /// wenn dieses <see cref="VCard"/>-Objekt in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere VCards refenziert hat.</remarks>
        /// <param name="writer">Ein <see cref="TextWriter"/>, mit dem die serialisierten <see cref="VCard"/>-Objekte geschrieben werden.</param>
        /// <param name="vcards">Die zu serialisierenden <see cref="VCard"/>-Objekte.</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// <remarks>Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="vcards"/> ist <c>null</c>.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException">Die Ressourcen von <paramref name="writer"/> sind bereits freigegeben.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Serialize(TextWriter writer, List<VCard?> vcards, VCdVersion version, VcfOptions options = VcfOptions.Default)
        {
            DebugWriter.WriteMethodHeader($"{nameof(VCard)}.{nameof(Serialize)}({nameof(TextWriter)}, List<{nameof(VCard)}>, {nameof(VCdVersion)}, {nameof(VcfOptions)}");

            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (vcards is null) throw new ArgumentNullException(nameof(vcards));

            Dereference(vcards, version, options);

            var serializer = VcfSerializer.GetSerializer(writer, version, options);

            foreach (var vCard in vcards)
            {
                if (vCard is null) continue;

                vCard.Version = version;

                serializer.Serialize(vCard);
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Schreibt die <see cref="VCard"/> auf einen Datenträger.
        /// </summary>
        /// <param name="fileName">Der Dateipfad.</param>
        /// <param name="version">Die vCard-Version, in die die Datei serialisiert wird.</param>
        /// <param name="options">Optionen für das Schreiben der VCF-Datei. Die Flags können
        /// kombiniert werden.</param>
        /// <note type="tip">Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.</note>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Save(
            string fileName,
            VCdVersion version = VCdVersion.V3_0,
            VcfOptions options = VcfOptions.Default)
        {
            VCard.Save(fileName, new List<VCard?> { this }, version, options);
        }


        /// <summary>
        /// Serialisiert die <see cref="VCard"/> mit einem <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">Ein <see cref="TextWriter"/>, mit dem die <see cref="VCard"/> serialisiert wird.</param>
        /// <param name="version">Die vCard-Version, in die serialisiert wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// <note type="tip">Wenn mehrere <see cref="VCard"/>-Objekte zu serialisieren sind, empfiehlt 
        /// sich aus Performancegründen die Verwendung der statischen Methoden der Klasse <see cref="VCard"/>.</note>
        /// <remarks>Die <see cref="VCard"/> - serialisiert als <see cref="string"/>. Der von der Methode 
        /// zurückgegebene <see cref="string"/> kann auch mehrere serialisierte
        /// vCards enthalten, nämlich dann, wenn in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere VCards refenziert wurden.</remarks>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Serialize(TextWriter writer, VCdVersion version, VcfOptions options = VcfOptions.Default)
        {
            VCard.Serialize(writer, new List<VCard?> { this }, version, options);
        }


        /// <summary>
        /// Serialisiert die <see cref="VCard"/> als VCF-<see cref="String"/>. (Ideal für Testzwecke.)
        /// </summary>
        /// <param name="version">Die vCard-Version, in die serialisiert wird.</param>
        /// <param name="options">Optionen für das Serialisieren. Die Flags können
        /// kombiniert werden.</param>
        /// <returns>Die <see cref="VCard"/>, serialisiert als VCF-<see cref="String"/>.</returns>
        public string ToVcfString(VCdVersion version, VcfOptions options = VcfOptions.Default)
        {
            // kein Inlining, da schon VCard.Serialize ge-inlined ist und die Methode in Tests
            // häufig aufgerufen wird

            using var stringWriter = new StringWriter();
            VCard.Serialize(stringWriter, new List<VCard?> { this }, version, options);
            return stringWriter.ToString();
        }

        #endregion


        #region private

        private static void Dereference(List<VCard?> vcdList, VCdVersion version, VcfOptions options)
        {
            Debug.Assert(vcdList != null);

            foreach (var vcard in vcdList)
            {
                if (vcard is null) continue;

                DereferenceMembers(vcard);
                DereferenceRelations(vcard);
            }


            void DereferenceMembers(VCard vcard)
            {
                if (version < VCdVersion.V4_0) return;

                var vcdProps = vcard.Members?
                                .Select(x => x as RelationVCardProperty)
                                .Where(x => x != null && !x.IsEmpty)
                                .ToArray();

                if (vcdProps is null || vcdProps.Length == 0) return;

                foreach (var vcdProp in vcdProps)
                {
                    Debug.Assert(vcdProp != null);
                    Debug.Assert(vcdProp.VCard != null);

                    if (!vcdList.Contains(vcdProp.VCard))
                    {
                        vcdList.Add(vcdProp.VCard);
                    }

                    if (vcdProp.VCard.UniqueIdentifier is null)
                    {
                        vcdProp.VCard.UniqueIdentifier = new UuidProperty();
                    }
                }
            }

            void DereferenceRelations(VCard vcard)
            {
                var vcdProps = vcard.Relations?
                                .Select(x => x as RelationVCardProperty)
                                .Where(x => x != null && !x.IsEmpty)
                                .ToArray();

                if (vcdProps is null || vcdProps.Length == 0) return;

                foreach (var vcdProp in vcdProps)
                {
                    Debug.Assert(vcdProp != null);
                    Debug.Assert(vcdProp.VCard != null);

                    if (!vcdList.Contains(vcdProp.VCard))
                    {
                        if (version < VCdVersion.V4_0
                           && vcdProp.Parameters.RelationType.IsSet(RelationTypes.Agent)
                           && !options.IsSet(VcfOptions.IncludeAgentAsSeparateVCard))
                        {
                            continue;
                        }

                        vcdList.Add(vcdProp.VCard);
                    }

                    if (vcdProp.VCard.UniqueIdentifier is null)
                    {
                        vcdProp.VCard.UniqueIdentifier = new UuidProperty();
                    }
                }
            }

        }


        #endregion

    }
}
