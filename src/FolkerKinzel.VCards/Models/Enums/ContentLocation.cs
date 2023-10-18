namespace FolkerKinzel.VCards.Models.Enums;

    /// <summary>Named constants to indicate in vCard 2.1 where the content of a vCard
    /// property is located.</summary>
public enum ContentLocation
{
    /// <summary> <c>INLINE</c>: The location of the property value is inline with the
    /// property. (Default)</summary>
    Inline,

    /// <summary> <c>Content-ID</c>, <c>CID</c>: In the case of the vCard being transported
    /// within a MIME email message, the property value can be specified as being located
    /// in a separate MIME entity with the “Content-ID” value, or “CID” for short. In
    /// this case, the property value is the Content-ID for the MIME entity containing
    /// the property value.</summary>
    ContentID,

    /// <summary> <c>URL</c>: The property value is specified as being located out on
    /// the network within some Internet resource. In this case, the property value
    /// is the Uniform Resource Locator for the Internet resource containing the property
    /// value.</summary>
    Url
}
