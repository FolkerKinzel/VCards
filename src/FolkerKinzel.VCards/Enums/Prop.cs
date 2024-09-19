using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to access the properties of a <see cref="VCard" />
/// object.</summary>
public enum Prop
{
    /// <summary> <c>PROFILE</c>: States that the vCard is a vCard. <c>(3)</c></summary>
    Profile,

    /// <summary> <c>KIND</c>: Defines the type of entity, that this vCard represents.
    /// <c>(4)</c></summary>
    Kind,

    /// <summary>
    /// <c>CREATED</c>: Defines the date and time when the vCard was created.
    /// <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    Created,

    /// <summary> <c>REV</c>: A timestamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
    TimeStamp,

    /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
    /// unique identifier, associated with the object. <c>(2,3,4)</c></summary>
    ID,

    /// <summary> <c>CATEGORIES</c>: Lists of "tags" that can be used to describe the
    /// object represented by this vCard. <c>(3,4)</c></summary>
    Categories,

    /// <summary> <c>TZ</c>: The time zone(s) of the vCard object. <c>(2,3,4)</c></summary>
    TimeZones,

    /// <summary> <c>GEO</c>: Specifies latitudes and longitudes. <c>(2,3,4)</c></summary>
    GeoCoordinates,

    /// <summary> <c>CLASS</c>: Describes the sensitivity of the information in the
    /// vCard. <c>(3)</c></summary>
    Access,

    /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
    /// this vCard.<c>(3,4)</c></summary>
    Sources,

    /// <summary> <c>NAME</c>: Provides a textual representation of the 
    /// <see cref="Sources" /> property. <c>(3)</c></summary>
    DirectoryName,

    /// <summary> <c>MAILER</c>: Type of e-mail program used. <c>(2,3)</c></summary>
    Mailer,

    /// <summary> <c>PRODID</c>: The identifier for the product that created the vCard
    /// object. <c>(3,4)</c></summary>
    ProductID,

    /// <summary> <c>LANGUAGE</c>: Defines the default language that human-readable 
    /// text values in this vCard are assumed to be written in.
    /// <c>(4 - RFC 9554)</c></summary>
    Language,

    /// <summary> <c>FN</c>: The formatted name string associated with the vCard object.
    /// <c>(2,3,4)</c></summary>
    DisplayNames,

    /// <summary> <c>N</c>: A structured representation of the name of the person, place
    /// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
    NameViews,

    /// <summary> <c>GENDER</c>: Defines the person's gender. <c>(4)</c></summary>
    GenderViews,

    /// <summary> <c>NICKNAME</c>: One or more descriptive/familiar names for the object
    /// represented by this vCard. <c>(3,4)</c></summary>
    NickNames,

    /// <summary> <c>TITLE</c>: Specifies the job title, functional position or function
    /// of the individual, associated with the vCard object, within an organization.
    /// <c>(2,3,4)</c></summary>
    Titles,

    /// <summary> <c>ROLE</c>: The role, occupation, or business category of the vCard
    /// object within an organization. <c>(2,3,4)</c></summary>
    Roles,

    /// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
    /// associated with the vCard object. <c>(2,3,4)</c></summary>
    Organizations,

    /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
    /// <c>(2,3,4)</c></summary>
    BirthDayViews,

    /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
    /// RFC 6474)</c></summary>
    BirthPlaceViews,

    /// <summary> <c>ANNIVERSARY</c>: Defines the person's anniversary. <c>(4)</c></summary>
    AnniversaryViews,

    /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC&#160;6474)</c></summary>
    DeathDateViews,

    /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
    /// RFC&#160;6474)</c></summary>
    DeathPlaceViews,

    /// <summary> <c>ADR</c>: A structured representation of the physical delivery address
    /// for the vCard object. <c>(2,3,4)</c></summary>
    Addresses,

    /// <summary> <c>TEL</c>: Canonical number strings for a telephone numbers for telephony
    /// communication with the vCard object. <c>(2,3,4)</c></summary>
    Phones,

    /// <summary> <c>EMAIL</c>: The addresses for electronic mail communication with
    /// the vCard object. <c>(2,3,4)</c></summary>
    EMails,

    /// <summary>
    /// <c>CONTACT-URI</c>: URIs representing an email address or a location for a web form. <c>(4 - RFC&#160;8605)</c>
    /// </summary>
    ContactUris,

    /// <summary> <c>URL</c>: URLs pointing to websites that represent the person in
    /// some way. <c>(2,3,4)</c></summary>
    Urls,

    /// <summary> <c>IMPP</c>: Instant messenger handles. <c>(3,4)</c></summary>
    Messengers,

    /// <summary> <c>KEY</c>: Public encryption keys associated with the vCard object.
    /// <c>(2,3,4)</c></summary>
    Keys,

    /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4, 3 - RFC&#160;2739)</c></summary>
    CalendarAddresses,

    /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
    /// the person's calendar. <c>(4, 3 - RFC 2739)</c></summary>
    CalendarUserAddresses,

    /// <summary> <c>FBURL</c>: Defines URLs that show when the person is "free" or
    /// "busy" on their calendar. <c>(4, 3 - RFC 2739)</c></summary>
    FreeOrBusyUrls,

    /// <summary> <c>CAPURI</c>: A protocol independent location from which a calendaring and 
    /// scheduling client can communicate with a user's entire calendar. <c>(3 - RFC&#160;2739)</c></summary>
    CalendarAccessUris,

    /// <summary> <c>RELATED</c>: Other entities that the person is related to. <c>(4)</c></summary>
    Relations,

    /// <summary> <c>MEMBER</c>: Defines a member that is part of the group that this 
    /// vCard represents. The <see cref="VCard.Kind" /> property must be set to <see 
    /// cref="Kind.Group" /> in order to use this property. <c>(4)</c></summary>
    Members,

    /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
    /// which can be used to look up information on the person's co-workers. 
    /// <c>(RFC&#160;6715)</c></summary>
    OrgDirectories,

    /// <summary> <c>EXPERTISE</c>: A professional subject area, that the person has
    /// knowledge of. <c>(RFC&#160;6715)</c></summary>
    Expertises,

    /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
    /// in, but does not necessarily take part in. <c>(4 - RFC&#160;6715)</c></summary>
    Interests,

    /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
    /// in. <c>(4 - RFC&#160;6715)</c></summary>
    Hobbies,

    /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
    Languages,

    /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments, that
    /// are associated with the vCard. <c>(2,3,4)</c></summary>
    Notes,

    /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
    Xmls,

    /// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
    /// is associated with the individual to which the vCard belongs. <c>(2,3,4)</c></summary>
    Logos,

    /// <summary> <c>PHOTO</c>: Image(s) or photograph(s) of the individual associated
    /// with the vCard. <c>(2,3,4)</c></summary>
    Photos,

    /// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
    /// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
    Sounds,

    /// <summary> <c>CLIENTPIDMAP</c>: Mappings for <see cref="PropertyID" />s. It is
    /// used for synchronizing different revisions of the same vCard. <c>(4)</c></summary>
    AppIDs,

    /// <summary>
    /// <c>GRAMGENDER</c>: Defines which grammatical gender to use in salutations and other 
    /// grammatical constructs. <c>4 - RFC&#160;9554</c>
    /// </summary>
    GramGenders,

    /// <summary> <c>PRONOUNS</c>: Defines the pronouns that shall be used to refer to the entity 
    /// represented by this <see cref="VCard"/>. <c>(4 - RFC 9554)</c></summary>
    Pronouns,

    /// <summary>vCard-Properties that don't belong to the standard.</summary>
    NonStandards,
    
}
