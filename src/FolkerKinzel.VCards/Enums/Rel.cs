using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to describe the type of relationship with a person
/// or organization. The constants can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the <see
/// cref="RelExtension" /> class. 
/// </note>
/// </remarks>
[Flags]
public enum Rel
{
    // CAUTION: If the enum is expanded, EnumValueCollector must be adjusted!

    /// <summary> <c>contact</c>: Someone you know how to get in touch with. Often symmetric.
    /// </summary>
    Contact = 1,

    /// <summary> <c>acquaintance</c>: Someone who you have exchanged greetings and
    /// not much (if any) more — maybe a short conversation or two. Often symmetric.</summary>
    Acquaintance = 1 << 1,

    /// <summary> <c>friend</c>: Someone you are a friend to. A compatriot, buddy, home(boy|girl)
    /// that you know. Often symmetric.</summary>
    Friend = 1 << 2,

    /// <summary> <c>met</c>: Someone who you have actually met in person. Symmetric.</summary>
    Met = 1 << 3,

    /// <summary> <c>co-worker</c>: Someone a person works with, or works at the same
    /// organization as. Symmetric. Usually transitive.</summary>
    CoWorker = 1 << 4,

    /// <summary> <c>colleague</c>: Someone in the same field of study/activity. Symmetric.
    /// Often transitive.</summary>
    Colleague = 1 << 5,

    /// <summary> <c>co-resident</c>: Someone you share a street address with. Symmetric
    /// and transitive.</summary>
    CoResident = 1 << 6,

    /// <summary> <c>neighbor</c>: Someone who lives nearby, perhaps only at an adjacent
    /// street address or doorway. Symmetric. Often transitive.</summary>
    Neighbor = 1 << 7,

    /// <summary> <c>child</c>: A person's genetic offspring, or someone that a person
    /// has adopted and takes care of. Inverse is parent.</summary>
    Child = 1 << 8,

    /// <summary> <c>parent</c>: Elternteil: Das Gegenteil von <see cref="Child" />.
    /// </summary>
    Parent = 1 << 9,

    /// <summary> <c>sibling</c>: Someone a person shares a parent with. Symmetric.
    /// Usually transitive.</summary>
    Sibling = 1 << 10,

    /// <summary> <c>spouse</c>: Someone you are married to. Symmetric. Not transitive.</summary>
    Spouse = 1 << 11,

    /// <summary> <c>kin</c>: A relative, someone you consider part of your extended
    /// family. Symmetric and typically transitive.</summary>
    Kin = 1 << 12,

    /// <summary> <c>muse</c>: Someone who brings you inspiration. No inverse.</summary>
    Muse = 1 << 13,

    /// <summary> <c>crush</c>: Someone you have a crush on. No inverse.</summary>
    Crush = 1 << 14,

    /// <summary> <c>date</c>: Someone you are dating. Symmetric. Not transitive.</summary>
    Date = 1 << 15,

    /// <summary> <c>sweetheart</c>: Someone with whom you are intimate and at least
    /// somewhat committed, typically exclusively. Symmetric. Not transitive.</summary>
    Sweetheart = 1 << 16,

    /// <summary> <c>me</c>: A link to yourself at a different URL. Exclusive of all
    /// other XFN values. Required symmetric. There is an implicit "me" relation from
    /// the contents of a directory to the directory itself.</summary>
    Me = 1 << 17,

    /// <summary> <c>agent</c>: An entity who may sometimes act on behalf of the entity
    /// associated with the vCard.</summary>
    Agent = 1 << 18,

    /// <summary> <c>emergency</c>: Indicates an emergency contact.</summary>
    Emergency = 1 << 19
}
