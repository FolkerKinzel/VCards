using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit <see cref="VCard"/>-Objekten erleichtern.
    /// </summary>
    public static class VCardExtension
    {
        /// <summary>
        /// Ersetzt bei den in <paramref name="vCardList"/> gespeicherten <see cref="VCard"/>-Objekten die <see cref="RelationVCardProperty"/>-Objekte 
        /// in den Eigenschaften <see cref="VCard.Members"/> und
        /// <see cref="VCard.Relations"/> durch <see cref="RelationUuidProperty"/>-Objekte und fügt die in den ersetzten <see cref="RelationVCardProperty"/>-Objekten
        /// gespeicherten <see cref="VCard"/>-Objekte als separate Items an <paramref name="vCardList"/> an, falls sie nicht schon in der Liste enthalten waren. Falls
        /// die zu referenzierenden <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, wird ihnen dabei automatisch
        /// eine zugewiesen.
        /// </summary>
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void SetVCardReferences(this List<VCard?> vCardList)
            => VCard.SetReferences(vCardList);



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
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void DereferenceVCards(List<VCard?> vCardList)
            => VCard.Dereference(vCardList);


        /// <summary>
        /// Speichert eine Liste von <see cref="VCard"/>-Objekten in eine gemeinsame VCF-Datei.
        /// </summary>
        /// 
        /// <param name="vCardList">Die zu speichernden <see cref="VCard"/>-Objekte. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten. Wenn die Auflistung kein <see cref="VCard"/>-Objekt enthält, wird keine Datei geschrieben.</param>
        /// <param name="fileName">Der Dateipfad. Wenn die Datei existiert, wird sie überschrieben.</param>
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
        /// vCards, wenn <paramref name="vCardList"/> nur ein <see cref="VCard"/>-Objekt enthielt - nämlich dann,
        /// wenn dieses <see cref="VCard"/>-Objekt in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert hat.</para>
        /// <para>
        /// Die Methode ruft <see cref="VCard.SetReferences(List{VCard?})"/> auf. Falls Sie nach Ausführung der Methode lesend auf den 
        /// Inhalt von <paramref name="vCardList"/> zugreifen möchten, sollten Sie <paramref name="vCardList"/> an die Methode 
        /// <see cref="VCard.Dereference(List{VCard})"/> übergeben oder auf <paramref name="vCardList"/> die Erweiterungsmethode 
        /// <see cref="VCardExtension.DereferenceVCards(List{VCard})"/> aus dem Namespace 
        /// <see cref="FolkerKinzel.VCards.Models.Helpers">FolkerKinzel.VCards.Models.Helpers</see>
        /// aufrufen.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> oder <paramref name="vCardList"/>
        /// ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void SaveVCards(
            this List<VCard?> vCardList,
            string fileName,
            VCdVersion version = VCdVersion.V3_0,
            VcfOptions options = VcfOptions.Default)
            => VCard.Save(fileName, vCardList, version, options);


        
        /// <summary>
        /// Serialisiert eine Liste von <see cref="VCard"/>-Objekten in einen <see cref="Stream"/>.
        /// </summary>
        /// 
        /// <param name="vCardList">Die zu serialisierenden <see cref="VCard"/>-Objekte.</param>
        /// <param name="stream">Ein <see cref="Stream"/>, in den die serialisierten <see cref="VCard"/>-Objekte geschrieben werden. <paramref name="stream"/>
        /// wird von der Methode geschlossen.</param>
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
        /// 
        /// <para>Die Methode serialisiert möglicherweise auch dann mehrere
        /// vCards, wenn <paramref name="vCardList"/> nur ein <see cref="VCard"/>-Objekt enthielt - nämlich dann,
        /// wenn dieses <see cref="VCard"/>-Objekt in den Properties <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> 
        /// weitere <see cref="VCard"/>-Objekte referenziert hat.
        /// </para>
        /// <para>
        /// Die Methode ruft <see cref="VCard.SetReferences(List{VCard?})"/> auf. Falls Sie nach Ausführung der Methode lesend auf den 
        /// Inhalt von <paramref name="vCardList"/> zugreifen möchten, sollten Sie <paramref name="vCardList"/> an die Methode 
        /// <see cref="VCard.Dereference(List{VCard})"/> übergeben oder auf <paramref name="vCardList"/> die Erweiterungsmethode 
        /// <see cref="VCardExtension.DereferenceVCards(List{VCard})"/> aus dem Namespace 
        /// <see cref="FolkerKinzel.VCards.Models.Helpers">FolkerKinzel.VCards.Models.Helpers</see>
        /// aufrufen.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> oder <paramref name="vCardList"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void SerializeVCards(this List<VCard?> vCardList,
                                           Stream stream,
                                           VCdVersion version,
                                           VcfOptions options = VcfOptions.Default)
            => VCard.Serialize(stream, vCardList, version, options);
    }
}
