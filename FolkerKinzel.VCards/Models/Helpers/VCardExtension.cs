using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit <see cref="VCard"/>-Objekten erleichtern.
    /// </summary>
    public static class VCardExtension
    {
        /// <summary>
        /// Ersetzt <see cref="RelationVCardProperty"/>-Objekte durch <see cref="RelationUuidProperty"/>-Objekte und fügt die 
        /// referenzierten <see cref="VCard"/>-Objekte als Elemente an <paramref name="vCardList"/> an.
        /// </summary>
        /// 
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>
        /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem Code kann z.B. 
        /// nützlich sein,
        /// wenn ein einzelnes <see cref="VCard"/>-Objekt aus einer Sammlung von <see cref="VCard"/>-Objekten als separate VCF-Datei gespeichert werden soll.
        /// </para>
        /// 
        /// <para>
        /// Auch durch mehrfachen Aufruf der Methode werden keine 
        /// Doubletten (im Sinne der mehrfachen Einfügung desselben <see cref="VCard"/>- oder <see cref="RelationUuidProperty"/>-Objekts) erzeugt.
        /// </para>
        /// 
        /// <para>Wenn die angefügten <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, wird ihnen von der Methode
        /// automatisch eine neue zugewiesen.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ReferenceVCards(this List<VCard?> vCardList)
            => VCard.Reference(vCardList);



        /// <summary>
        /// Ersetzt <see cref="RelationUuidProperty"/>-Objekte durch
        /// <see cref="RelationVCardProperty"/>-Objekte - sofern sich die referenzierten <see cref="VCard"/>-Objekte
        /// in <paramref name="vCardList"/> befinden.
        /// </summary>
        /// 
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>Die Methode wird von den Deserialisierungsmethoden von <see cref="VCard"/> automatisch aufgerufen. Die Verwendung in 
        /// eigenem Code kann z.B. nützlich sein, wenn <see cref="VCard"/>-Objekte aus verschiedenen Quellen in einer gemeinsamen Liste 
        /// zusammengeführt werden, um ihre Daten durchsuchbar zu machen.
        /// </para>
        /// 
        /// <para>Die Methode entfernt keine Elemente aus <paramref name="vCardList"/> und erzeugt 
        /// auch bei mehrfachem Aufruf keine Doubletten (<see cref="RelationVCardProperty"/>-Objekte, die dasselbe <see cref="VCard"/>-Objekt 
        /// enthalten).
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void DereferenceVCards(this List<VCard?> vCardList)
            => VCard.Dereference(vCardList);


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
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void SerializeVCards(this List<VCard?> vCardList,
                                           Stream stream,
                                           VCdVersion version = VCdVersion.V3_0,
                                           VcfOptions options = VcfOptions.Default,
                                           bool leaveStreamOpen = false)
            => VCard.Serialize(stream, vCardList, version, options, leaveStreamOpen);
    }
}
