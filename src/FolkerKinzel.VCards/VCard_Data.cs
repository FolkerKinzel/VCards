using System.ComponentModel;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates the information contained in a vCard.</summary>
/// <threadsafety static="true" instance="false" />
/// <example>
/// <note type="note">
/// For the sake of easier readability, exception handling is not used in the examples.
/// </note>
/// <para>
/// Writing, reading and converting VCF files:
/// </para>
/// <code language="cs" source="..\Examples\VCardExample.cs" />
/// <para>
/// Example implementation for <see cref="ITimeZoneIDConverter" />:
/// </para>
/// <code language="cs" source="..\Examples\TimeZoneIDConverter.cs" />
/// </example>
public sealed partial class VCard
{
    private readonly Dictionary<Prop, object> _propDic = new();

    [return: MaybeNull]
    private T Get<T>(Prop prop) where T : class?
        => _propDic.ContainsKey(prop)
        ? (T)_propDic[prop]
        : default;


    private void Set(Prop prop, object? value)
    {
        if (value is null)
        {
            _ = _propDic.Remove(prop);
        }
        else
        {
            _propDic[prop] = value;
        }
    }

    /// <summary>Indicates whether the <see cref="VCard" /> object doesn't contain
    /// any usable data.</summary>
    /// <returns> <c>true</c> if the <see cref="VCard" /> object doesn't contain
    /// any usable data, otherwise <c>false</c>.</returns>
    public bool IsEmpty() =>
        !_propDic
            .Select(x => x.Value)
            .Any(x => x switch
            {
                VCardProperty prop => !prop.IsEmpty,
                IEnumerable<VCardProperty?> numerable => numerable.Any(x => !(x?.IsEmpty ?? true)),
                _ => false
            });

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> of <see cref="string"/>s that can be used
    /// to iterate over the
    /// <see cref="VCardProperty.Group"/> identifiers of the <see cref="VCard"/>.
    /// </summary>
    /// <remarks>
    /// <note type="tip">
    /// Iterating over the <see cref="VCardProperty.Group"/> identifiers is an expensive
    /// operation. Store the results if they are needed several times.
    /// </note>
    /// <para>
    /// The method returns each <see cref="VCardProperty.Group"/> identifier only
    /// once. <c>null</c> is not a <see cref="VCardProperty.Group"/> identifier.
    /// The comparison of <see cref="VCardProperty.Group"/> identifiers is
    /// case-insensitive.
    /// </para>
    /// </remarks>
    public IEnumerable<string> GroupIDs 
        => EnumerateGroups().Distinct(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.
    /// </summary>
    /// <returns>A new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.</returns>
    public string NewGroup()
    {
        int i = -1;

        foreach (var group in GroupIDs)
        {
            if(int.TryParse(group, out int result) && result > i)
            {
                i = result;
            }
        }

        return (++i).ToString();
    }

    /// <summary> <c>VERSION</c>: Version of the vCard standard. <c>(2,3,4)</c></summary>
    public VCdVersion Version
    {
        get; private set;
    }

    /// <summary> <c>CLASS</c>: Describes the sensitivity of the information in the
    /// <see cref="VCard"/>. <c>(3)</c></summary>
    public AccessProperty? Access
    {
        get => Get<AccessProperty?>(Prop.Access);
        set => Set(Prop.Access, value);
    }

    /// <summary> <c>ADR</c>: A structured representation of the physical delivery address
    /// for the vCard object. <c>(2,3,4)</c></summary>
    public IEnumerable<AddressProperty?>? Addresses
    {
        get => Get<IEnumerable<AddressProperty?>?>(Prop.Addresses);
        set => Set(Prop.Addresses, value);
    }

    /// <summary> <c>ANNIVERSARY</c>: Defines the person's anniversary. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? AnniversaryViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.AnniversaryViews);
        set => Set(Prop.AnniversaryViews, value);
    }

    /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
    /// <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? BirthDayViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.BirthDayViews);
        set => Set(Prop.BirthDayViews, value);
    }

    /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<TextProperty?>? BirthPlaceViews
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.BirthPlaceViews);
        set => Set(Prop.BirthPlaceViews, value);
    }

    /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4)</c></summary>
    public IEnumerable<TextProperty?>? CalendarAddresses
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.CalendarAddresses);
        set => Set(Prop.CalendarAddresses, value);
    }

    /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
    /// the person's calendar. <c>(4)</c></summary>
    public IEnumerable<TextProperty?>? CalendarUserAddresses
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.CalendarUserAddresses);
        set => Set(Prop.CalendarUserAddresses, value);
    }

    /// <summary> <c>CATEGORIES</c>: Lists of "tags" that can be used to describe the
    /// object represented by this vCard. <c>(3,4)</c></summary>
    public IEnumerable<StringCollectionProperty?>? Categories
    {
        get => Get<IEnumerable<StringCollectionProperty?>?>(Prop.Categories);
        set => Set(Prop.Categories, value);
    }

    /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? DeathDateViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.DeathDateViews);
        set => Set(Prop.DeathDateViews, value);
    }

    /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<TextProperty?>? DeathPlaceViews
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.DeathPlaceViews);
        set => Set(Prop.DeathPlaceViews, value);
    }

    /// <summary> <c>NAME</c>: Provides a textual representation of the 
    /// <see cref="Sources" /> property. <c>(3)</c></summary>
    public TextProperty? DirectoryName
    {
        get => Get<TextProperty?>(Prop.DirectoryName);
        set => Set(Prop.DirectoryName, value);
    }

    /// <summary> <c>FN</c>: The formatted name string associated with the vCard object.
    /// <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <para>
    /// The name representations stored in this property are typically intended to be 
    /// presented to the users of the application. 
    /// </para>
    /// <note type="tip">
    /// You can use the <see cref="NameProperty.ToDisplayName" /> method to convert the 
    /// structured name representations that are stored in the <see cref="NameViews" /> 
    /// property to produce formatted representations readable by the users of the 
    /// application.  
    /// </note>
    /// </remarks>
    /// <seealso cref="NameProperty.ToDisplayName" />
    public IEnumerable<TextProperty?>? DisplayNames
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.DisplayNames);
        set => Set(Prop.DisplayNames, value);
    }

    /// <summary> <c>EMAIL</c>: The addresses for electronic mail communication with
    /// the vCard object. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? EMails
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.EMails);
        set => Set(Prop.EMails, value);
    }

    /// <summary> <c>EXPERTISE</c>: A professional subject area, that the person has
    /// knowledge of. <c>(RFC 6715)</c></summary>
    /// <remarks>Define the level of expertise in the parameter 
    /// <see cref="ParameterSection.Expertise" />!</remarks>
    public IEnumerable<TextProperty?>? Expertises
    {
        get => Get<IEnumerable<TextProperty>>(Prop.Expertises);
        set => Set(Prop.Expertises, value);
    }

    /// <summary> <c>FBURL</c>: Defines URLs that show when the person is "free" or
    /// "busy" on their calendar. <c>(4)</c></summary>
    /// <remarks>
    /// If several <see cref="TextProperty" /> objects are assigned, the standard 
    /// property is determined by the value of <see cref="ParameterSection.Preference" />. 
    /// URLs of the type <c>FTP</c> [RFC1738] or <c>HTTP</c> [RFC2616] refer to an
    /// iCalendar object [RFC5545], which represents a snapshot of the next weeks or 
    /// months with data for the busy time of the subject the <see cref="VCard"/> 
    /// represents. If the iCalendar object is a file, its file extension should 
    /// be ".ifb".
    /// </remarks>
    public IEnumerable<TextProperty?>? FreeOrBusyUrls
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.FreeOrBusyUrls);
        set => Set(Prop.FreeOrBusyUrls, value);
    }

    /// <summary> <c>GENDER</c>: Defines the person's gender. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<GenderProperty?>? GenderViews
    {
        get => Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews);
        set => Set(Prop.GenderViews, value);
    }

    /// <summary> <c>GEO</c>: Specifies latitudes and longitudes. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.GeoPosition"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public IEnumerable<GeoProperty?>? GeoCoordinates
    {
        get => Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates);
        set => Set(Prop.GeoCoordinates, value);
    }

    /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
    /// in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest with the parameter 
    /// <see cref="ParameterSection.Interest" />.</remarks>
    public IEnumerable<TextProperty?>? Hobbies
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Hobbies);
        set => Set(Prop.Hobbies, value);
    }

    /// <summary> <c>IMPP</c>: List of instant messenger handles. <c>(3,4)</c></summary>
    /// <remarks>
    /// <see cref="TextProperty.Value" /> should specify a URI for instant messaging 
    /// and presence protocol communications with the object the <see cref="VCard"/> 
    /// represents. If the URI can be used for voice and/or video, the 
    /// <see cref="VCard.Phones" /> property SHOULD be used in addition to this 
    /// property.</remarks>
    [Obsolete("Use InstantMessengers instead.", false)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    public IEnumerable<TextProperty?>? InstantMessengerHandles
    {
        get => InstantMessengers;
        set => InstantMessengers = value;
    }

    /// <summary> <c>IMPP</c>: List of instant messenger handles. <c>(3,4)</c></summary>
    /// <remarks>
    /// <see cref="TextProperty.Value" /> should specify a URI for instant messaging 
    /// and presence protocol communications with the object the <see cref="VCard"/> 
    /// represents. If the URI can be used for voice and/or video, the 
    /// <see cref="VCard.Phones" /> property SHOULD be used in addition to this 
    /// property.</remarks>
    public IEnumerable<TextProperty?>? InstantMessengers
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.InstantMessengers);
        set => Set(Prop.InstantMessengers, value);
    }

    /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
    /// in, but does not necessarily take part in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest in the parameter 
    /// <see cref="ParameterSection.Interest" />!</remarks>
    public IEnumerable<TextProperty?>? Interests
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Interests);
        set => Set(Prop.Interests, value);
    }

    /// <summary> <c>KEY</c>: Public encryption keys associated with the vCard object.
    /// <c>(2,3,4)</c></summary>
    /// <value>It may point to an external URL, may be plain text, or may be embedded
    /// in the VCF file as a Base64 encoded block of text.</value>
    public IEnumerable<DataProperty?>? Keys
    {
        get => Get<IEnumerable<DataProperty?>?>(Prop.Keys);
        set => Set(Prop.Keys, value);
    }

    /// <summary> <c>KIND</c>: Defines the type of entity, that this vCard represents.
    /// <c>(4)</c></summary>
    public KindProperty? Kind
    {
        get => Get<KindProperty?>(Prop.Kind);
        set => Set(Prop.Kind, value);
    }

    /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
    public IEnumerable<TextProperty?>? Languages
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Languages);
        set => Set(Prop.Languages, value);
    }

    /// <summary> <c>REV</c>: A time stamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
    public TimeStampProperty? TimeStamp
    {
        get => Get<TimeStampProperty?>(Prop.TimeStamp);
        set => Set(Prop.TimeStamp, value);
    }

    /// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
    /// is associated with the individual to which the <see cref="VCard"/> belongs. 
    /// <c>(2,3,4)</c></summary>
    public IEnumerable<DataProperty?>? Logos
    {
        get => Get<IEnumerable<DataProperty?>?>(Prop.Logos);
        set => Set(Prop.Logos, value);
    }

    /// <summary> <c>MAILER</c>: Type of e-mail program used. <c>(2,3)</c></summary>
    public TextProperty? Mailer
    {
        get => Get<TextProperty?>(Prop.Mailer);
        set => Set(Prop.Mailer, value);
    }

    /// <summary> <c>MEMBER</c>:
    /// Defines a member that is part of the group that this <see cref="VCard"/> represents.
    /// The <see cref="VCard.Kind" /> property must be set to <see cref="Kind.Group" />
    /// in order to use this property. <c>(4)</c>
    /// </summary>
    /// <remarks>
    /// If the <see cref="Relation"/> property embeds a <see cref="string"/> value, it will
    /// be converted to the <see cref="DisplayNames"/> property of a <see cref="VCard"/> 
    /// object if it can't be converted to an absolute <see cref="Uri"/>.
    /// </remarks>
    public IEnumerable<RelationProperty?>? Members
    {
        get => Get<IEnumerable<RelationProperty?>?>(Prop.Members);
        set => Set(Prop.Members, value);
    }

    /// <summary> <c>N</c>: A structured representation of the name of the person, place
    /// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public IEnumerable<NameProperty?>? NameViews
    {
        get => Get<IEnumerable<NameProperty?>?>(Prop.NameViews);
        set => Set(Prop.NameViews, value);
    }

    /// <summary> <c>NICKNAME</c>: One or more descriptive/familiar names for the object
    /// represented by this vCard. <c>(3,4)</c></summary>
    public IEnumerable<StringCollectionProperty?>? NickNames
    {
        get => Get<IEnumerable<StringCollectionProperty?>?>(Prop.NickNames);
        set => Set(Prop.NickNames, value);
    }

    /// <summary>vCard-Properties that don't belong to the standard.</summary>
    /// <remarks>
    /// <para>
    /// <see cref="NonStandard" /> contains all vCard properties that could not 
    /// be evaluated, when parsing the vCard. To serialize the content of 
    /// <see cref="NonStandard" /> into a VCF file, the flag 
    /// <see cref="VcfOptions.WriteNonStandardProperties"/> has to be set. 
    /// </para>
    /// <para>
    /// Some <see cref="NonStandardProperty" /> objects are automatically added to the 
    /// VCF file, if there is no standard equivalent for it. You can control this behavior
    /// with <see cref="VcfOptions" />. It is therefore not recommended to assign
    /// <see cref="NonStandardProperty" /> objects with these 
    /// <see cref="NonStandardProperty.XName"/>s to this property.
    /// </para>
    /// <para>
    /// These vCard properties are the following: 
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <c>X-AIM</c>
    /// </item>
    /// <item>
    /// <c>X-ANNIVERSARY</c>
    /// </item>
    /// <item>
    /// <c>X-EVOLUTION-SPOUSE</c>
    /// </item>
    /// <item>
    /// <c>X-EVOLUTION-ANNIVERSARY</c>
    /// </item>
    /// <item>
    /// <c>X-GADUGADU</c>
    /// </item>
    /// <item>
    /// <c>X-GENDER</c>
    /// </item>
    /// <item>
    /// <c>X-GOOGLE-TALK</c>
    /// </item>
    /// <item>
    /// <c>X-GROUPWISE</c>
    /// </item>
    /// <item>
    /// <c>X-GTALK</c>
    /// </item>
    /// <item>
    /// <c>X-ICQ</c>
    /// </item>
    /// <item>
    /// <c>X-JABBER</c>
    /// </item>
    /// <item>
    /// <c>X-KADDRESSBOOK-X-ANNIVERSARY</c>
    /// </item>
    /// <item>
    /// <c>X-KADDRESSBOOK-X-IMADDRESS</c>
    /// </item>
    /// <item>
    /// <c>X-KADDRESSBOOK-X-SPOUSENAME</c>
    /// </item>
    /// <item>
    /// <c>X-MS-IMADDRESS</c>
    /// </item>
    /// <item>
    /// <c>X-MSN</c>
    /// </item>
    /// <item>
    /// <c>X-SKYPE</c>
    /// </item>
    /// <item>
    /// <c>X-SKYPE-USERNAME</c>
    /// </item>
    /// <item>
    /// <c>X-SPOUSE</c>
    /// </item>
    /// <item>
    /// <c>X-TWITTER</c>
    /// </item>
    /// <item>
    /// <c>X-WAB-GENDER</c>
    /// </item>
    /// <item>
    /// <c>X-WAB-WEDDING_ANNIVERSARY</c>
    /// </item>
    /// <item>
    /// <c>X-WAB-SPOUSE_NAME</c>
    /// </item>
    /// <item>
    /// <c>X-YAHOO</c>
    /// </item>
    /// </list>
    /// </remarks>
    public IEnumerable<NonStandardProperty?>? NonStandard
    {
        get => Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandard);
        set => Set(Prop.NonStandard, value);
    }

    /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments, that
    /// are associated with the vCard. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Notes
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Notes);
        set => Set(Prop.Notes, value);
    }

    /// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
    /// associated with the vCard object. <c>(2,3,4)</c></summary>
    public IEnumerable<OrganizationProperty?>? Organizations
    {
        get => Get<IEnumerable<OrganizationProperty?>?>(Prop.Organizations);
        set => Set(Prop.Organizations, value);
    }

    /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
    /// which can be used to look up information on the person's co-workers. <c>(RFC
    /// 6715)</c></summary>
    public IEnumerable<TextProperty?>? OrgDirectories
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.OrgDirectories);
        set => Set(Prop.OrgDirectories, value);
    }

    /// <summary> <c>TEL</c>: Canonical number strings for a telephone numbers for 
    /// telephony communication with the vCard object. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Phones
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Phones);
        set => Set(Prop.Phones, value);
    }

    /// <summary> <c>PHOTO</c>: Image(s) or photograph(s) of the individual associated
    /// with the vCard. <c>(2,3,4)</c></summary>
    public IEnumerable<DataProperty?>? Photos
    {
        get => Get<IEnumerable<DataProperty>>(Prop.Photos);
        set => Set(Prop.Photos, value);
    }

    /// <summary> <c>PRODID</c>: The identifier for the product that created the vCard
    /// object. <c>(3,4)</c></summary>
    /// <value>The name should be unique worldwide. It should therefore conform to the
    /// specification for Formal Public Identifiers [ISO 9070] or Universal Resource
    /// Names in RFC 3406.</value>
    public TextProperty? ProdID
    {
        get => Get<TextProperty?>(Prop.ProdID);
        set => Set(Prop.ProdID, value);
    }

    /// <summary> <c>PROFILE</c>: States that the <see cref="VCard"/> is a vCard. <c>(3)</c></summary>
    public ProfileProperty? Profile
    {
        get => Get<ProfileProperty?>(Prop.Profile);
        set => Set(Prop.Profile, value);
    }

    /// <summary> <c>CLIENTPIDMAP</c>: Mappings for <see cref="PropertyID" />s. It is
    /// used for synchronizing different revisions of the same vCard. <c>(4)</c></summary>
    public IEnumerable<PropertyIDMappingProperty?>? PropertyIDMappings
    {
        get => Get<IEnumerable<PropertyIDMappingProperty?>?>(Prop.PropertyIDMappings);
        set => Set(Prop.PropertyIDMappings, value);
    }

    /// <summary> <c>RELATED</c>: Other entities that the person or organization is 
    /// related to. <c>(4)</c></summary>
    public IEnumerable<RelationProperty?>? Relations
    {
        get => Get<IEnumerable<RelationProperty?>?>(Prop.Relations);
        set => Set(Prop.Relations, value);
    }

    /// <summary> <c>ROLE</c>: The role, occupation, or business category of the vCard
    /// object within an organization. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Roles
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Roles);
        set => Set(Prop.Roles, value);
    }

    /// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
    /// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
    public IEnumerable<DataProperty?>? Sounds
    {
        get => Get<IEnumerable<DataProperty?>?>(Prop.Sounds);
        set => Set(Prop.Sounds, value);
    }

    /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
    /// this vCard.<c>(3,4)</c></summary>
    /// <remarks>vCard&#160;3.0 only allows one instance of this property.</remarks>
    public IEnumerable<TextProperty?>? Sources
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Sources);
        set => Set(Prop.Sources, value);
    }

    /// <summary> <c>TZ</c>: The time zone(s) of the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.TimeZone"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public IEnumerable<TimeZoneProperty?>? TimeZones
    {
        get => Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones);
        set => Set(Prop.TimeZones, value);
    }

    /// <summary> <c>TITLE</c>: Specifies the job title, functional position or function
    /// of the individual, associated with the vCard object, within an organization.
    /// <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Titles
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Titles);
        set => Set(Prop.Titles, value);
    }

    /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
    /// unique identifier, associated with the object. <c>(2,3,4)</c></summary>
    /// <value>Although the standard allows any strings for identification, the library
    /// only supports UUIDs.</value>
    public UuidProperty? UniqueIdentifier
    {
        get => Get<UuidProperty?>(Prop.UniqueIdentifier);
        set => Set(Prop.UniqueIdentifier, value);
    }

    /// <summary> <c>URL</c>: URLs, pointing to websites that represent the person in
    /// some way. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? URLs
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.URLs);
        set => Set(Prop.URLs, value);
    }

    /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
    public IEnumerable<XmlProperty?>? XmlProperties
    {
        get => Get<IEnumerable<XmlProperty?>?>(Prop.XmlProperties);
        set => Set(Prop.XmlProperties, value);
    }

    private IEnumerable<string> EnumerateGroups()
    {
        foreach (var kvp in _propDic)
        {
            if (kvp.Value is VCardProperty prop && prop.Group != null)
            {
                yield return prop.Group;
                continue;
            }

            if (kvp.Value is IEnumerable<VCardProperty?> numerable)
            {
                foreach (VCardProperty? vcProp in numerable)
                {
                    string? group = vcProp?.Group;

                    if (group != null)
                    {
                        yield return group;
                    }
                }
            }
        }
    }
}
