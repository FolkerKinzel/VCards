namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Benannte Konstanten, um in vCard 3.0 anzugeben, welche Geheimhaltungsstufe für die vCard gilt.
/// </summary>
public enum Access
{
    /// <summary>
    /// Der Inhalt der vCard ist öffentlich zugänglich.
    /// </summary>
    Public,

    /// <summary>
    /// Der Inhalt der vCard ist für persönliche Bekannte zugänglich.
    /// </summary>
    Private,

    /// <summary>
    /// Der Inhalt der vCard ist vertraulich.
    /// </summary>
    Confidential
}
