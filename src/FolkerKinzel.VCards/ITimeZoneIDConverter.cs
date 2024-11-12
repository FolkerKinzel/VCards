using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

/// <summary>Defines a method that converts a <see cref="string" />, which represents
/// a named time zone (IANA time zone ID), into a corresponding UTC offset.</summary>
/// <remarks>
/// <para>
/// The vCard&#160;4.0 standard recommends specifying time zones as IANA time zone names. - 
/// Older standards (vCard&#160;2.1, vCard&#160;3.0), however, prefer UTC offsets.
/// </para>
/// <para>
/// It is possible to convert IANA time zone names to UTC offsets, but it's not possible 
/// to convert UTC offsets to IANA time zone names because there is no unique assignment. 
/// Therefore it's recommended to initialize <see cref="TimeZoneID" /> objects with IANA 
/// time zone names and then to convert these to UTC offsets using an implementation of 
/// <see cref="ITimeZoneIDConverter" /> when serializing a vCard&#160;2.1 or a vCard&#160;3.0. 
/// </para>
/// <para>
/// Since .NET cannot do the conversion itself, this interface enables a 3rd-party 
/// library to be integrated. (Since .NET&#160;6.0 exists a .NET solution with the ICU
/// library but this needs a separate dependency or additional configuration.)
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
    /// The <see cref="TryConvertToUtcOffset(string, out TimeSpan)" /> method should 
    /// at least be able to convert IANA time zone IDs into UTC offsets. (You could 
    /// also convert POSIX time zone identifiers, for example.) It makes sense to use a 
    /// 3rd-party NuGet package for the task, as .NET is currently not able to do this 
    /// itself. 
    /// </para>
    /// <para>
    /// It is not necessary for the method to convert <see cref="string" />s that already 
    /// represent a UTC offset, since the library <see cref="VCards" /> 
    /// already does this itself.
    /// </para>
    /// </note>
    /// </remarks>
    bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset);
}
