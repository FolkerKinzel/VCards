using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Extensions
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit <see cref="List{T}">List&lt;VCard&gt;</see>-Objekten erleichtern.
    /// </summary>
    public static class VCardListExtension
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
        /// <note type="important">
        /// Verwenden Sie diese Methode niemals, wenn Sie eine VCF-Datei als vCard 2.1 oder vCard 3.0 speichern möchten!
        /// </note>
        /// <para>
        /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem 
        /// Code ist
        /// nur dann sinnvoll, wenn ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden soll und wenn dabei jede VCF-Datei nur
        /// eine einzige vCard enthalten soll. (Dieses Vorgehen ist i.d.R. nicht vorteilhaft, da es die referentielle Integrität gefährdet.)
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
        /// <example>
        /// <para>
        /// Das Beispiel demonstriert, wie ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden kann, wenn beabsichtigt ist,
        /// dass eine VCF-Datei jeweils nur eine einzige vCard enthalten soll. Das Beispiel zeigt möglicherweise auch, dass dieses Vorgehen i.d.R.
        /// nicht vorteilhaft ist, da es die referentielle Integrität gefährdet.
        /// </para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ReferenceVCards(this List<
#nullable disable
            VCard
#nullable restore
            > vCardList)
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
        /// </remarks>
        /// 
        /// <example>
        /// <para>
        /// Das Beispiel zeigt das Deserialisieren und Auswerten einer VCF-Datei, deren Inhalt auf andere VCF-Dateien verweist.
        /// </para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void DereferenceVCards(this List<
#nullable disable
            VCard
#nullable restore
            > vCardList)
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
        public static void SaveVcf(
            this List<
#nullable disable
                VCard
#nullable restore
                > vCardList,
            string fileName,
            VCdVersion version = VCard.DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default)
            => VCard.Save(fileName, vCardList, version, options);


#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Obsolete("Use SaveVcf instead!", VCardProperty.OBSOLETE_AS_ERROR)]
#pragma warning disable CS1591 // Fehlender XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public static void SaveVCards(
#pragma warning restore CS1591 // Fehlender XML-Kommentar für öffentlich sichtbaren Typ oder Element
            this List<
#nullable disable
                VCard
#nullable restore
                > vCardList,
            string fileName,
            VCdVersion version = VCard.DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default)
            => SaveVcf(vCardList, fileName, version, options);


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
        /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void SerializeVcf(this List<
#nullable disable
            VCard
#nullable restore
            > vCardList,
                                           Stream stream,
                                           VCdVersion version = VCard.DEFAULT_VERSION,
                                           VcfOptions options = VcfOptions.Default,
                                           bool leaveStreamOpen = false)
            => VCard.Serialize(stream, vCardList, version, options, leaveStreamOpen);



#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Obsolete("Use SerializeVcf instead!", VCardProperty.OBSOLETE_AS_ERROR)]
#pragma warning disable CS1591 // Fehlender XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public static void SerializeVCards(this List<
#pragma warning restore CS1591 // Fehlender XML-Kommentar für öffentlich sichtbaren Typ oder Element
#nullable disable
            VCard
#nullable restore
            > vCardList,
                                           Stream stream,
                                           VCdVersion version = VCard.DEFAULT_VERSION,
                                           VcfOptions options = VcfOptions.Default,
                                           bool leaveStreamOpen = false)
            => SerializeVcf(vCardList, stream, version, options, leaveStreamOpen);



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
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        /// <exception cref="OutOfMemoryException">Es ist nicht genug Speicher vorhanden.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string ToVcfString(this List<
#nullable disable
            VCard
#nullable restore
            > vCardList, VCdVersion version = VCard.DEFAULT_VERSION, VcfOptions options = VcfOptions.Default)
        => VCard.ToVcfString(vCardList, version, options);
    }
}
