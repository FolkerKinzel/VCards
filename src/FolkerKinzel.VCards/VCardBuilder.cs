using FolkerKinzel.VCards.BuilderParts;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>
/// Provides a fluent API for building and editing <see cref="VCard"/> objects.
/// </summary>
/// <remarks>
/// The properties of the <see cref="VCardBuilder"/> class have the same names as those
/// of the <see cref="VCard"/> class. Each of these properties gets a struct that provides
/// methods to edit the 
/// corresponding <see cref="VCard"/> property. Each of these methods return the 
/// <see cref="VCardBuilder"/> instance so that the calls can be chained.
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
public sealed class VCardBuilder
{
    private VCardBuilder(VCard vCard) => VCard = vCard;

    /// <summary>
    /// Returns a <see cref="VCardBuilder"/> that creates a new <see cref="VCard"/>
    /// object.
    /// </summary>
    /// <param name="setID"><c>true</c> to set the <see cref="VCard.ID"/> property
    /// of the newly created <see cref="VCard"/> object automatically to a new 
    /// <see cref="Guid"/>, otherwise <c>false</c>.</param>
    /// <returns>The <see cref="VCardBuilder"/> that creates a new <see cref="VCard"/>.</returns>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class. (See <see cref="VCard.RegisterApp(Uri?)"/>.)</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VCardBuilder Create(bool setID = true)
        => new(new VCard(setID));

    /// <summary>
    /// Returns a <see cref="VCardBuilder"/> that edits the <see cref="VCard"/>
    /// object that is passed to the method as argument.
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/> object to edit.</param>
    /// <returns>The <see cref="VCardBuilder"/> that that edits the <paramref name="vCard"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="vCard"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VCardBuilder Create(VCard vCard)
        => new(vCard ?? throw new ArgumentNullException(nameof(vCard)));

    internal static IEnumerable<TSource?> Add<TSource>(TSource prop,
                                                       IEnumerable<TSource?>? coll,
                                                       Action<ParameterSection>? parameters)
        where TSource : VCardProperty, IEnumerable<TSource>
    {
        parameters?.Invoke(prop.Parameters);

        coll = coll is null ? prop
                            : coll.Concat(prop);
        return coll;
    }

    ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// Gets the <see cref="FolkerKinzel.VCards.VCard"/> instance the <see cref="VCardBuilder"/> works
    /// with.
    /// </summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public VCard VCard { get; }

    /// <summary> <c>CLASS</c>: Describes the sensitivity of the information in the
    /// <see cref="VCards.VCard"/>. <c>(3)</c></summary>
    public AccessBuilder Access => new(this);

    /// <summary> <c>ADR</c>: A structured representation of the physical delivery address
    /// for the vCard object. <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public AddressBuilder Addresses => new(this);

    /// <summary> <c>ANNIVERSARY</c>: Defines the person's anniversary. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public DateAndOrTimeBuilder AnniversaryViews => new(this, Prop.AnniversaryViews);

    /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
    /// <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public DateAndOrTimeBuilder BirthDayViews => new(this, Prop.BirthDayViews);

    /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public TextViewBuilder BirthPlaceViews => new(this, Prop.BirthPlaceViews);

    /// <summary> <c>CAPURI</c>: A protocol independent location from which a calendaring and 
    /// scheduling client can communicate with a user's entire calendar. <c>(3 - RFC 2739)</c></summary>
    public TextBuilder CalendarAccessUris => new(this, Prop.CalendarAccessUris);

    /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4, 3 - RFC 2739)</c></summary>
    public TextBuilder CalendarAddresses => new(this, Prop.CalendarAddresses);

    /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
    /// the person's calendar. <c>(4, 3 - RFC 2739)</c></summary>
    public TextBuilder CalendarUserAddresses => new(this, Prop.CalendarUserAddresses);

    /// <summary> <c>CATEGORIES</c>: Lists of "tags" that can be used to describe the
    /// object represented by this vCard. <c>(3,4)</c></summary>
    public StringCollectionBuilder Categories => new(this, Prop.Categories);

    /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public DateAndOrTimeBuilder DeathDateViews => new(this, Prop.DeathDateViews);

    /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public TextViewBuilder DeathPlaceViews => new(this, Prop.DeathPlaceViews);

    /// <summary> <c>NAME</c>: Provides a textual representation of the 
    /// <see cref="Sources" /> property. <c>(3)</c></summary>
    public TextSingletonBuilder DirectoryName => new(this, Prop.DirectoryName);

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
    public TextBuilder DisplayNames => new(this, Prop.DisplayNames);

    /// <summary> <c>EMAIL</c>: The addresses for electronic mail communication with
    /// the vCard object. <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public TextBuilder EMails => new(this, Prop.EMails);

    /// <summary> <c>EXPERTISE</c>: A professional subject area that the person has
    /// knowledge of. <c>(RFC 6715)</c></summary>
    /// <remarks>Define the level of expertise in the parameter 
    /// <see cref="ParameterSection.Expertise" />!</remarks>
    public TextBuilder Expertises => new(this, Prop.Expertises);

    /// <summary> <c>FBURL</c>: Defines URLs that show when the person is "free" or
    /// "busy" on their calendar. <c>(4, 3 - RFC 2739)</c></summary>
    /// <remarks>
    /// If several <see cref="TextProperty" /> objects are assigned, the standard 
    /// property is determined by the value of <see cref="ParameterSection.Preference" />. 
    /// URLs of the type <c>FTP</c> [RFC1738] or <c>HTTP</c> [RFC2616] refer to an
    /// iCalendar object [RFC5545], which represents a snapshot of the next weeks or 
    /// months with data for the busy time of the subject the <see cref="VCards.VCard"/> 
    /// represents. If the iCalendar object is a file, its file extension should 
    /// be ".ifb".
    /// </remarks>
    public TextBuilder FreeOrBusyUrls => new(this, Prop.FreeOrBusyUrls);

    /// <summary> <c>GENDER</c>: Defines the person's gender. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public GenderBuilder GenderViews => new(this);

    /// <summary> <c>GEO</c>: Specifies latitudes and longitudes. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.GeoPosition"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public GeoBuilder GeoCoordinates => new(this);

    /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
    /// in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest with the parameter 
    /// <see cref="ParameterSection.Interest" />.</remarks>
    public TextBuilder Hobbies => new(this, Prop.Hobbies);

    /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
    /// unique identifier corresponding to the entity associated with the vCard. <c>(2,3,4)</c>
    /// </summary>
    /// <value>Although the standard allows any strings for identification, the library
    /// only supports UUIDs.</value>
    /// <remarks>As a default setting each newly created <see cref="VCard"/> gets an <see cref="ID"/>
    /// automatically.</remarks>
    public IDBuilder ID => new(this);

    /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
    /// in, but does not necessarily take part in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest in the parameter 
    /// <see cref="ParameterSection.Interest" />!</remarks>
    public TextBuilder Interests => new(this, Prop.Interests);

    /// <summary> <c>KEY</c>: Public encryption keys associated with the vCard object.
    /// <c>(2,3,4)</c></summary>
    /// <value>It may point to an external URL, may be plain text, or may be embedded
    /// in the VCF file as a Base64 encoded block of text.</value>
    public DataBuilder Keys => new(this, Prop.Keys);

    /// <summary> <c>KIND</c>: Defines the type of entity, that this vCard represents.
    /// <c>(4)</c></summary>
    public KindBuilder Kind => new(this);

    /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
    public TextBuilder Languages => new(this, Prop.Languages);

    /// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
    /// is associated with the individual to which the <see cref="VCard"/> belongs. 
    /// <c>(2,3,4)</c></summary>
    public DataBuilder Logos => new(this, Prop.Logos);

    /// <summary> <c>MAILER</c>: Name of the e-mail program. <c>(2,3)</c></summary>
    public TextSingletonBuilder Mailer => new(this, Prop.Mailer);

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
    public RelationBuilder Members => new(this, Prop.Members);

    /// <summary> <c>IMPP</c>: Instant messenger handles. <c>(3,4)</c></summary>
    /// <remarks>
    /// <see cref="TextProperty.Value" /> should specify a URI for instant messaging 
    /// and presence protocol communications with the object the <see cref="VCards.VCard"/> 
    /// represents. If the URI can be used for voice and/or video, the 
    /// <see cref="VCard.Phones" /> property SHOULD be used in addition to this 
    /// property.</remarks>
    public TextBuilder Messengers => new(this, Prop.Messengers);

    /// <summary> <c>N</c>: A structured representation of the name of the person, place
    /// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public NameBuilder NameViews => new(this);

    /// <summary> <c>NICKNAME</c>: One or more descriptive/familiar names for the object
    /// represented by this vCard. <c>(3,4)</c></summary>
    public StringCollectionBuilder NickNames => new(this, Prop.NickNames);

    /// <summary>vCard-Properties that don't belong to the standard.</summary>
    /// <remarks>
    /// <para>
    /// <see cref="NonStandards" /> contains all vCard properties that could not 
    /// be evaluated, when parsing the vCard. To serialize the content of 
    /// <see cref="NonStandards" /> into a VCF file, the flag 
    /// <see cref="Opts.WriteNonStandardProperties"/> has to be set. 
    /// </para>
    /// <para>
    /// Some <see cref="NonStandardProperty" /> objects are automatically added to the 
    /// VCF file, if there is no standard equivalent for it. You can control this behavior
    /// with <see cref="Opts" />. It is therefore not recommended to assign
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
    public NonStandardBuilder NonStandards => new(this);

    /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments that
    /// are associated with the vCard. <c>(2,3,4)</c></summary>
    public TextBuilder Notes => new(this, Prop.Notes);

    /// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
    /// associated with the vCard object. <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public OrgBuilder Organizations => new(this);

    /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
    /// which can be used to look up information on the person's co-workers. <c>(RFC
    /// 6715)</c></summary>
    public TextBuilder OrgDirectories => new(this, Prop.OrgDirectories);

    /// <summary> <c>TEL</c>: Canonical number strings for a telephone numbers for 
    /// telephony communication with the vCard object. <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public TextBuilder Phones => new(this, Prop.Phones);

    /// <summary> <c>PHOTO</c>: Image(s) or photograph(s) of the individual associated
    /// with the vCard. <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public DataBuilder Photos => new(this, Prop.Photos);

    /// <summary> <c>PRODID</c>: The identifier for the product that created the vCard
    /// object. <c>(3,4)</c></summary>
    /// <value>The name should be unique worldwide. It should therefore conform to the
    /// specification for Formal Public Identifiers [ISO 9070] or Universal Resource
    /// Names in RFC 3406.</value>
    public TextSingletonBuilder ProductID => new(this, Prop.ProductID);

    /// <summary> <c>PROFILE</c>: States that the <see cref="VCards.VCard"/> is a vCard. <c>(3)</c></summary>
    public ProfileBuilder Profile => new(this);

    /// <summary> <c>RELATED</c>: Other entities that the person or organization is 
    /// related to. <c>(4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public RelationBuilder Relations => new(this, Prop.Relations);

    /// <summary> <c>ROLE</c>: The role, occupation, or business category of the vCard
    /// object within an organization. <c>(2,3,4)</c></summary>
    public TextBuilder Roles => new(this, Prop.Roles);

    /// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
    /// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
    public DataBuilder Sounds => new(this, Prop.Sounds);

    /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
    /// this vCard. <c>(3,4)</c></summary>
    /// <remarks>vCard&#160;3.0 allows only one instance of this property.</remarks>
    public TextBuilder Sources => new(this, Prop.Sources);

    /// <summary> <c>REV</c>: A time stamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// With <see cref="Opts.Default"/> the flag <see cref="Opts.UpdateTimeStamp"/> is set. So 
    /// this property is normally updated automatically when serializing VCF.
    /// </remarks>
    public TimeStampBuilder TimeStamp => new(this);

    /// <summary> <c>TZ</c>: The time zone(s) of the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.TimeZone"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public TimeZoneBuilder TimeZones => new(this);

    /// <summary> <c>TITLE</c>: Specifies the job title, functional position or function
    /// of the individual, associated with the vCard object, within an organization.
    /// <c>(2,3,4)</c></summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public TextBuilder Titles => new(this, Prop.Titles);

    /// <summary> <c>URL</c>: URLs, pointing to websites that represent the person in
    /// some way. <c>(2,3,4)</c></summary>
    public TextBuilder Urls => new(this, Prop.Urls);

    /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
    public XmlBuilder Xmls => new(this);
}

