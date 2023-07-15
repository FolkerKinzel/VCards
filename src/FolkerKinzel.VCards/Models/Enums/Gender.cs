namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Benannte Konstanten, um das Geschlecht des Subjekts anzugeben, das die vCard repräsentiert.
/// </summary>
public enum Gender
{
    /// <summary>
    /// <c>U</c>: unbekannt <c>(4)</c>
    /// </summary>
    Unknown,

    /// <summary>
    /// <c>F</c>: weiblich <c>(4)</c>
    /// </summary>
    Female,

    /// <summary>
    /// <c>M</c>: männlich <c>(4)</c>
    /// </summary>
    Male,

    /// <summary>
    /// <c>N</c>: geschlechtslos oder nicht zuzuordnen <c>(4)</c>
    /// </summary>
    NonOrNotApplicable,

    /// <summary>
    /// <c>O</c>: anderes Geschlecht <c>(4)</c>
    /// </summary>
    Other
}
