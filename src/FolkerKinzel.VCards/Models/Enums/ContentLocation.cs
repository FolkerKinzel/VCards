namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Benannte Konstanten, um in vCard 2.1 anzugeben, wo der Inhalt einer vCard-Property lokalisiert ist.
/// </summary>
public enum ContentLocation
{
    /// <summary>
    /// <c>INLINE</c>: Der Inhalt ist direkt in die vCard eingebettet. (Default)
    /// </summary>
    Inline,

    /// <summary>
    /// <c>Content-ID</c>, <c>CID</c>: Der Inhalt befindet sich an einer anderen Stelle in derselben Datei (z.B. Multipart-E-Mail). Auf
    /// ihn wird mit einer Content-ID (cid) verwiesen.
    /// </summary>
    ContentID,

    /// <summary>
    /// <c>URL</c>: Der Inhalt befindet sich im Internet. Auf ihn wird mit einer Url verwiesen.
    /// </summary>
    Url
}
