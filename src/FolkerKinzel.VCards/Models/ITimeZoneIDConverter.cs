namespace FolkerKinzel.VCards.Models;

    /// <summary>Defines a method that converts a <see cref="string" />, which represents
    /// a named time zone (IANA time zone ID), into a corresponding UTC offset.</summary>
    /// <remarks>
    /// <para>
    /// Der Standard empfiehlt für vCard 4.0, Zeitzonen als IANA Zeitzonen-IDs anzugeben.
    /// - Ältere Standards (vCard 2.1, vCard 3.0) bevorzugen hingegen UTC-Offsets.
    /// </para>
    /// <para>
    /// Es ist möglich, IANA Zeitzonennamen in UTC-Offsets umzuwandeln, aber es ist
    /// nicht möglich, UTC-Offsets in IANA-Zeitzonennamen umzuwandeln, da keine eindeutige
    /// Zuordnung besteht. Deshalb wird empfohlen <see cref="TimeZoneID" />-Objekte
    /// mit IANA-Zeitzonennamen zu initialisieren und diese dann mit einer Implementierung
    /// von <see cref="ITimeZoneIDConverter" /> in UTC-Offsets umzuwandeln, wenn eine
    /// vCard 2.1 oder eine vCard 3.0 serialisiert wird.
    /// </para>
    /// <para>
    /// Da .NET die Umwandlung bis .NET 5.0 nicht selbst vornehmen kann, ermöglicht
    /// dieses Interface eine 3rd-Party-Library einzubinden.
    /// </para>
    /// </remarks>
    /// <example>
    /// <note type="note">
    /// To make it easier to read, exception handling has been omitted in the following
    /// example.
    /// </note>
    /// <para>
    /// Example implementation for <see cref="ITimeZoneIDConverter" />:
    /// </para>
    /// <code language="cs" source="..\Examples\TimeZoneIDConverter.cs" />
    /// </example>
public interface ITimeZoneIDConverter
{
    /// <summary>Tries to convert a <see cref="string" />, which represents a time zone
    /// name (IANA time zone ID), into a UTC offset belonging to this time zone.</summary>
    /// <param name="timeZoneID">A <see cref="string" /> representing the name of a
    /// time zone.</param>
    /// <param name="utcOffset">Contains the UTC offset after the method has been successfully
    /// returned. The argument is passed uninitialized.</param>
    /// <returns> <c>true</c>, if <paramref name="timeZoneID" /> could be converted
    /// into a UTC offset, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// <note type="implement">
    /// <para>
    /// Die Methode <see cref="TryConvertToUtcOffset(string, out TimeSpan)" /> sollte
    /// mindestens in der Lage sein IANA Zeitzonen-IDs in UTC-Offsets umzuwandeln. (Sie
    /// könnte darüberhinaus z.B. auch POSIX-Zeitzonenbezeichner umwandeln.) Es bietet
    /// sich an, für die Aufgabe ein 3rd party nuget-Package einzusetzen, da .NET derzeit
    /// nicht selbst dazu in der Lage ist.
    /// </para>
    /// <para>
    /// Es ist nicht notwendig, dass die Methode <see cref="string" />s umwandelt, die
    /// bereits einen UTC-Offset darstellen, da das von <see cref="FolkerKinzel.VCards"
    /// /> bereits selbst erledigt wird.
    /// </para>
    /// </note>
    /// </remarks>
    bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset);
}
