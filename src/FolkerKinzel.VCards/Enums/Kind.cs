namespace FolkerKinzel.VCards.Enums;

/// <summary>Named Constants to specify the type of object the vCard represents.</summary>
public enum Kind
{
    /// <summary> <c>INDIVIDUAL</c>: The vCard represents a single person or entity.
    /// (Default) <c>4</c></summary>
    Individual,

    /// <summary> <c>GROUP</c>: The vCard represents a group of persons or entities.
    /// <c>4</c></summary>
    Group,

    /// <summary> <c>ORGANIZATION</c>: The vCard represents an organization. <c>4</c></summary>
    Organization,

    /// <summary> <c>LOCATION</c>: The vCard represents a named geographical place.
    /// <c>4</c></summary>
    Location,

    /// <summary> <c>APPLICATION</c>: The vCard represents a software application such
    /// as a server, an online service (e.g., a chat room), or an automated software
    /// bot. <c>(4 - RFC 6473)</c></summary>
    Application
}
