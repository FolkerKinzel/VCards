using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>
/// Erweiterungsmethoden, die die Arbeit mit <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>-Objekten erleichtern.
/// </summary>
public static class VCardCollectionExtension
{
    /// <summary>
    /// Gibt eine Sammlung von <see cref="VCard"/>-Objekten zurück, in der die <see cref="RelationVCardProperty"/>-Objekte durch
    /// <see cref="RelationUuidProperty"/>-Objekte ersetzt sind und in der die in den
    /// <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte als 
    /// separate Elemente angefügt sind.
    /// </summary>
    /// 
    /// <param name="vCards">Sammlung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein und <c>null</c>-Werte
    /// enthalten.</param>
    /// 
    /// <returns>
    /// Eine Sammlung von <see cref="VCard"/>-Objekten, in der die <see cref="RelationVCardProperty"/>-Objekte 
    /// durch 
    /// <see cref="RelationUuidProperty"/>-Objekte ersetzt sind und in der die in den
    /// <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte als 
    /// separate Elemente angefügt sind. (Wenn die angefügten <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, 
    /// wird ihnen von der Methode automatisch eine neue zugewiesen.)
    /// </returns>
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
    /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
    /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
    /// </note>
    /// <note type="important">
    /// Verwenden Sie diese Methode niemals, wenn Sie eine VCF-Datei als vCard 2.1 oder vCard 3.0 speichern möchten. Es droht Datenverlust.
    /// </note>
    /// <para>
    /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem 
    /// Code ist
    /// nur dann sinnvoll, wenn ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden soll und wenn dabei jede VCF-Datei nur
    /// eine einzige vCard enthalten soll. (Dieses Vorgehen ist i.d.R. nicht vorteilhaft, da es die referentielle Integrität gefährdet.)
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
    /// <exception cref="ArgumentNullException"><paramref name="vCards"/> ist <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<VCard> ReferenceVCards(this IEnumerable<VCard?> vCards)
        => VCard.Reference(vCards);



    /// <summary>
    /// Gibt eine Sammlung von <see cref="VCard"/>-Objekten zurück, in der <see cref="RelationUuidProperty"/>-Objekte durch
    /// <see cref="RelationVCardProperty"/>-Objekte ersetzt worden sind, falls sich die referenzierten <see cref="VCard"/>-Objekte
    /// in der Sammlung befinden.
    /// </summary>
    /// 
    /// <param name="vCards">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein und <c>null</c>-Werte
    /// enthalten.</param>
    /// 
    /// <returns>
    /// Eine Sammlung von <see cref="VCard"/>-Objekten, in der <see cref="RelationUuidProperty"/>-Objekte durch
    /// <see cref="RelationVCardProperty"/>-Objekte ersetzt worden sind, falls sich die referenzierten <see cref="VCard"/>-Objekte
    /// in der als Argument übergebenen Sammlung befinden.
    /// </returns>
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
    /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
    /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
    /// </note>
    /// 
    /// 
    /// <para>Die Methode wird von den Deserialisierungsmethoden von <see cref="VCard"/> automatisch aufgerufen. Die Verwendung in 
    /// eigenem Code kann z.B. nützlich sein, wenn <see cref="VCard"/>-Objekte aus verschiedenen Quellen in einer gemeinsamen Liste 
    /// zusammengeführt werden, um ihre Daten durchsuchbar zu machen.
    /// </para>
    /// 
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
    /// <exception cref="ArgumentNullException"><paramref name="vCards"/> ist <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<VCard> DereferenceVCards(this IEnumerable<VCard?> vCards)
        => VCard.Dereference(vCards);


    /// <summary>
    /// Speichert eine Sammlung von <see cref="VCard"/>-Objekten in eine gemeinsame VCF-Datei.
    /// </summary>
    /// 
    /// <param name="vCards">Die zu speichernden <see cref="VCard"/>-Objekte. Die Sammlung darf leer sein und <c>null</c>-Werte
    /// enthalten. Wenn die Sammlung kein <see cref="VCard"/>-Objekt enthält, wird keine Datei geschrieben.</param>
    /// <param name="fileName">Der Dateipfad. Wenn die Datei existiert, wird sie überschrieben.</param>
    /// <param name="version">Die vCard-Version der zu speichernden VCF-Datei.</param>
    /// <param name="tzConverter">Ein Objekt, das <see cref="ITimeZoneIDConverter"/> implementiert, um beim Schreiben von vCard 2.1 oder 
    /// vCard 3.0 Zeitzonennamen aus der "IANA time zone database" in UTC-Offsets umwandeln zu können, oder <c>null</c>, um 
    /// auf eine Umwandlung zu verzichten.</param>
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
    /// 
    /// <para>Die Methode serialisiert möglicherweise mehr
    /// vCards, als die Anzahl der Elemente in der Sammlung, die an den Parameter <paramref name="vCards"/> übergeben wird.
    /// Dies geschieht, wenn eine VCF-Datei als
    /// vCard 4.0 gespeichert wird und sich 
    /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
    /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befinden.
    /// </para>
    /// 
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
    /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
    /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
    /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
    /// </para>
    /// </remarks>
    /// 
    /// <seealso cref="ITimeZoneIDConverter"/>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> oder <paramref name="vCards"/>
    /// ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
    /// <exception cref="IOException">Die Datei konnte nicht geschrieben werden.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SaveVcf(
        this IEnumerable<VCard?> vCards,
        string fileName,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default)
        => VCard.SaveVcf(fileName, vCards, version, tzConverter, options);


    /// <summary>
    /// Serialisiert eine Sammlung von <see cref="VCard"/>-Objekten unter Verwendung des VCF-Formats in einen <see cref="Stream"/>.
    /// </summary>
    /// 
    /// <param name="vCards">Die zu serialisierenden <see cref="VCard"/>-Objekte. Die Sammlung darf leer sein und <c>null</c>-Werte
    /// enthalten.</param>
    /// <param name="stream">Ein <see cref="Stream"/>, in den die serialisierten <see cref="VCard"/>-Objekte geschrieben werden.</param>
    /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
    /// <param name="tzConverter">Ein Objekt, das <see cref="ITimeZoneIDConverter"/> implementiert, um beim Schreiben von vCard 2.1 oder 
    /// vCard 3.0 Zeitzonennamen aus der "IANA time zone database" in UTC-Offsets umwandeln zu können, oder <c>null</c>, um 
    /// auf eine Umwandlung zu verzichten.</param>
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
    /// vCards, als die Anzahl der Elemente in der Sammlung, die an den Parameter <paramref name="vCards"/> übergeben wird.
    /// Dies geschieht, wenn eine
    /// vCard 4.0 serialisiert wird und sich 
    /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
    /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden.
    /// </para>
    /// 
    /// <para>Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
    /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
    /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
    /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
    /// </para>
    /// 
    /// </remarks>
    /// 
    /// <seealso cref="ITimeZoneIDConverter"/>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> oder <paramref name="vCards"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="stream"/> unterstützt keine Schreibvorgänge oder <paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
    /// <exception cref="IOException">E/A-Fehler.</exception>
    /// <exception cref="ObjectDisposedException"><paramref name="stream"/> war bereits geschlossen.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeVcf(
        this IEnumerable<VCard?> vCards,
        Stream stream,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default,
        bool leaveStreamOpen = false)
        => VCard.SerializeVcf(stream, vCards, version, tzConverter, options, leaveStreamOpen);


    /// <summary>
    /// Serialisiert <paramref name="vCards"/> als einen <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.
    /// </summary>
    /// 
    /// <param name="vCards">Die zu serialisierenden <see cref="VCard"/>-Objekte. Die Sammlung darf leer sein und <c>null</c>-Werte
    /// enthalten.</param>
    /// <param name="version">Die vCard-Version, die für die Serialisierung verwendet wird.</param>
    /// <param name="tzConverter">Ein Objekt, das <see cref="ITimeZoneIDConverter"/> implementiert, um beim Schreiben von vCard 2.1 oder 
    /// vCard 3.0 Zeitzonennamen aus der "IANA time zone database" in UTC-Offsets umwandeln zu können, oder <c>null</c>, um 
    /// auf eine Umwandlung zu verzichten.</param>
    /// <param name="options">Optionen für das Serialisieren. Die Flags können
    /// kombiniert werden.</param>
    /// 
    /// <returns><paramref name="vCards"/>, serialisiert als <see cref="string"/>, der den Inhalt einer VCF-Datei darstellt.</returns>
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
    /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
    /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
    /// </note>
    /// 
    /// <para>Die Methode serialisiert möglicherweise mehr
    /// vCards, als sich ursprünglich Elemente in <paramref name="vCards"/> befanden. Dies geschieht, wenn eine
    /// vCard 4.0 serialisiert wird und sich 
    /// in den Eigenschaften <see cref="VCard.Members"/> oder <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts
    /// weitere <see cref="VCard"/>-Objekte in Form von <see cref="RelationVCardProperty"/>-Objekten befanden.
    /// </para>
    /// 
    /// <para>Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option <see cref="VcfOptions.IncludeAgentAsSeparateVCard"/> 
    /// serialisiert wird und wenn sich in der Eigenschaft <see cref="VCard.Relations"/> eines <see cref="VCard"/>-Objekts ein 
    /// <see cref="RelationVCardProperty"/>-Objekt befindet, auf dessen <see cref="ParameterSection"/> in der Eigenschaft <see cref="ParameterSection.RelationType"/>
    /// das Flag <see cref="RelationTypes.Agent"/> gesetzt ist.
    /// </para>
    /// 
    /// </remarks>
    /// 
    ///  <seealso cref="ITimeZoneIDConverter"/>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="vCards"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="version"/> hat einen nichtdefinierten Wert.</exception>
    /// <exception cref="OutOfMemoryException">Es ist nicht genug Speicher vorhanden.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToVcfString(
        this IEnumerable<VCard?> vCards,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default)
    => VCard.ToVcfString(vCards, version, tzConverter, options);
}
