using System.Xml.Linq;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.BuilderParts;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed class VCardBuilder
{
    internal readonly VCard _vCard;

    private VCardBuilder(VCard vCard) => _vCard = vCard;

    public static VCardBuilder Create(bool setUniqueIdentifier = true)
        => new(new VCard(setUniqueIdentifier));

    public static VCardBuilder Create(VCard vCard)
        => new(vCard ?? throw new ArgumentNullException(nameof(vCard)));

    public VCard Build() => _vCard;

    ///////////////////////////////////////////////////////////////////

    /// <summary> <c>CLASS</c>: Describes the sensitivity of the information in the
    /// <see cref="VCard"/>. <c>(3)</c></summary>
    public AccessBuilder Access => new AccessBuilder(this);

    /// <summary> <c>ADR</c>: A structured representation of the physical delivery address
    /// for the vCard object. <c>(2,3,4)</c></summary>
    public AddressBuilder Addresses => new AddressBuilder(this);

    /// <summary> <c>ANNIVERSARY</c>: Defines the person's anniversary. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public DateAndOrTimeBuilder AnniversaryViews => new DateAndOrTimeBuilder(this, Prop.AnniversaryViews);

    /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
    /// <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public DateAndOrTimeBuilder BirthDayViews => new DateAndOrTimeBuilder(this, Prop.BirthDayViews);
    
    /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public TextViewBuilder BirthPlaceViews => new TextViewBuilder(this, Prop.BirthPlaceViews);

    /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4)</c></summary>
    public TextBuilder CalendarAddresses => new TextBuilder(this, Prop.CalendarAddresses);

    /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
    /// the person's calendar. <c>(4)</c></summary>
    public TextBuilder CalendarUserAddresses => new TextBuilder(this, Prop.CalendarUserAddresses);

    /// <summary> <c>CATEGORIES</c>: Lists of "tags" that can be used to describe the
    /// object represented by this vCard. <c>(3,4)</c></summary>
    public StringCollectionBuilder Categories => new StringCollectionBuilder(this, Prop.Categories);

    /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public DateAndOrTimeBuilder DeathDateViews => new DateAndOrTimeBuilder(this, Prop.DeathDateViews);

    /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
    /// RFC 6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    /// e.g. be useful, if the property is displayed in different languages.</remarks>
    public TextViewBuilder DeathPlaceViews => new TextViewBuilder(this, Prop.DeathPlaceViews);

    /// <summary> <c>NAME</c>: Provides a textual representation of the 
    /// <see cref="Sources" /> property. <c>(3)</c></summary>
    public TextSingletonBuilder DirectoryName => new TextSingletonBuilder(this, Prop.DirectoryName);

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
    public TextBuilder DisplayNames => new TextBuilder(this, Prop.DisplayNames);

    /// <summary> <c>EMAIL</c>: The addresses for electronic mail communication with
    /// the vCard object. <c>(2,3,4)</c></summary>
    public TextBuilder EMails => new TextBuilder(this, Prop.EMails);

    /// <summary> <c>EXPERTISE</c>: A professional subject area, that the person has
    /// knowledge of. <c>(RFC 6715)</c></summary>
    /// <remarks>Define the level of expertise in the parameter 
    /// <see cref="ParameterSection.Expertise" />!</remarks>
    public TextBuilder Expertises => new TextBuilder(this, Prop.Expertises);

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
    public TextBuilder FreeOrBusyUrls => new TextBuilder(this, Prop.FreeOrBusyUrls);

    /// <summary> <c>GENDER</c>: Defines the person's gender. <c>(4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if they
    /// all have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public GenderBuilder GenderViews => new GenderBuilder(this);

    /// <summary> <c>GEO</c>: Specifies latitudes and longitudes. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.GeoPosition"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public GeoBuilder GeoCoordinates => new GeoBuilder(this);

    /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
    /// in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest with the parameter 
    /// <see cref="ParameterSection.Interest" />.</remarks>
    public TextBuilder Hobbies => new TextBuilder(this, Prop.Hobbies);

    /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
    /// unique identifier corresponding to the entity associated with the vCard. <c>(2,3,4)</c>
    /// </summary>
    /// <value>Although the standard allows any strings for identification, the library
    /// only supports UUIDs.</value>
    public UuidBuilder ID => new UuidBuilder(this);

    /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
    /// in, but does not necessarily take part in. <c>(4 - RFC 6715)</c></summary>
    /// <remarks> Define the level of interest in the parameter 
    /// <see cref="ParameterSection.Interest" />!</remarks>
    public TextBuilder Interests => new TextBuilder(this, Prop.Interests);

    ///// <summary> <c>KEY</c>: Public encryption keys associated with the vCard object.
    ///// <c>(2,3,4)</c></summary>
    ///// <value>It may point to an external URL, may be plain text, or may be embedded
    ///// in the VCF file as a Base64 encoded block of text.</value>
    //public IEnumerable<DataProperty?>? Keys
    //{
    //    get => Get<IEnumerable<DataProperty?>?>(Prop.Keys);
    //    set => Set(Prop.Keys, value);
    //}

    /// <summary> <c>KIND</c>: Defines the type of entity, that this vCard represents.
    /// <c>(4)</c></summary>
    public KindBuilder Kind => new KindBuilder(this);

    /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
    public TextBuilder Languages => new TextBuilder(this, Prop.Languages);

    ///// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
    ///// is associated with the individual to which the <see cref="VCard"/> belongs. 
    ///// <c>(2,3,4)</c></summary>
    //public IEnumerable<DataProperty?>? Logos
    //{
    //    get => Get<IEnumerable<DataProperty?>?>(Prop.Logos);
    //    set => Set(Prop.Logos, value);
    //}

    /// <summary> <c>MAILER</c>: Name of the e-mail program. <c>(2,3)</c></summary>
    public TextSingletonBuilder Mailer => new TextSingletonBuilder(this, Prop.Mailer);

    ///// <summary> <c>MEMBER</c>:
    ///// Defines a member that is part of the group that this <see cref="VCard"/> represents.
    ///// The <see cref="VCard.Kind" /> property must be set to <see cref="Kind.Group" />
    ///// in order to use this property. <c>(4)</c>
    ///// </summary>
    ///// <remarks>
    ///// If the <see cref="Relation"/> property embeds a <see cref="string"/> value, it will
    ///// be converted to the <see cref="DisplayNames"/> property of a <see cref="VCard"/> 
    ///// object if it can't be converted to an absolute <see cref="Uri"/>.
    ///// </remarks>
    //public IEnumerable<RelationProperty?>? Members
    //{
    //    get => Get<IEnumerable<RelationProperty?>?>(Prop.Members);
    //    set => Set(Prop.Members, value);
    //}

    /// <summary> <c>IMPP</c>: Instant messenger handles. <c>(3,4)</c></summary>
    /// <remarks>
    /// <see cref="TextProperty.Value" /> should specify a URI for instant messaging 
    /// and presence protocol communications with the object the <see cref="VCard"/> 
    /// represents. If the URI can be used for voice and/or video, the 
    /// <see cref="VCard.Phones" /> property SHOULD be used in addition to this 
    /// property.</remarks>
    public TextBuilder Messengers => new TextBuilder(this, Prop.Messengers);

    ///// <summary> <c>N</c>: A structured representation of the name of the person, place
    ///// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
    ///// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only, if they
    ///// all have the same <see cref="ParameterSection.AltID" /> parameter. This can
    ///// e.g. be useful, if the property is displayed in different languages.</remarks>
    //public IEnumerable<NameProperty?>? NameViews
    //{
    //    get => Get<IEnumerable<NameProperty?>?>(Prop.NameViews);
    //    set => Set(Prop.NameViews, value);
    //}

    /// <summary> <c>NICKNAME</c>: One or more descriptive/familiar names for the object
    /// represented by this vCard. <c>(3,4)</c></summary>
    public StringCollectionBuilder NickNames => new StringCollectionBuilder(this, Prop.NickNames);

    ///// <summary>vCard-Properties that don't belong to the standard.</summary>
    ///// <remarks>
    ///// <para>
    ///// <see cref="NonStandards" /> contains all vCard properties that could not 
    ///// be evaluated, when parsing the vCard. To serialize the content of 
    ///// <see cref="NonStandards" /> into a VCF file, the flag 
    ///// <see cref="VcfOptions.WriteNonStandardProperties"/> has to be set. 
    ///// </para>
    ///// <para>
    ///// Some <see cref="NonStandardProperty" /> objects are automatically added to the 
    ///// VCF file, if there is no standard equivalent for it. You can control this behavior
    ///// with <see cref="VcfOptions" />. It is therefore not recommended to assign
    ///// <see cref="NonStandardProperty" /> objects with these 
    ///// <see cref="NonStandardProperty.XName"/>s to this property.
    ///// </para>
    ///// <para>
    ///// These vCard properties are the following: 
    ///// </para>
    ///// <list type="bullet">
    ///// <item>
    ///// <c>X-AIM</c>
    ///// </item>
    ///// <item>
    ///// <c>X-ANNIVERSARY</c>
    ///// </item>
    ///// <item>
    ///// <c>X-EVOLUTION-SPOUSE</c>
    ///// </item>
    ///// <item>
    ///// <c>X-EVOLUTION-ANNIVERSARY</c>
    ///// </item>
    ///// <item>
    ///// <c>X-GADUGADU</c>
    ///// </item>
    ///// <item>
    ///// <c>X-GENDER</c>
    ///// </item>
    ///// <item>
    ///// <c>X-GOOGLE-TALK</c>
    ///// </item>
    ///// <item>
    ///// <c>X-GROUPWISE</c>
    ///// </item>
    ///// <item>
    ///// <c>X-GTALK</c>
    ///// </item>
    ///// <item>
    ///// <c>X-ICQ</c>
    ///// </item>
    ///// <item>
    ///// <c>X-JABBER</c>
    ///// </item>
    ///// <item>
    ///// <c>X-KADDRESSBOOK-X-ANNIVERSARY</c>
    ///// </item>
    ///// <item>
    ///// <c>X-KADDRESSBOOK-X-IMADDRESS</c>
    ///// </item>
    ///// <item>
    ///// <c>X-KADDRESSBOOK-X-SPOUSENAME</c>
    ///// </item>
    ///// <item>
    ///// <c>X-MS-IMADDRESS</c>
    ///// </item>
    ///// <item>
    ///// <c>X-MSN</c>
    ///// </item>
    ///// <item>
    ///// <c>X-SKYPE</c>
    ///// </item>
    ///// <item>
    ///// <c>X-SKYPE-USERNAME</c>
    ///// </item>
    ///// <item>
    ///// <c>X-SPOUSE</c>
    ///// </item>
    ///// <item>
    ///// <c>X-TWITTER</c>
    ///// </item>
    ///// <item>
    ///// <c>X-WAB-GENDER</c>
    ///// </item>
    ///// <item>
    ///// <c>X-WAB-WEDDING_ANNIVERSARY</c>
    ///// </item>
    ///// <item>
    ///// <c>X-WAB-SPOUSE_NAME</c>
    ///// </item>
    ///// <item>
    ///// <c>X-YAHOO</c>
    ///// </item>
    ///// </list>
    ///// </remarks>
    //public IEnumerable<NonStandardProperty?>? NonStandards
    //{
    //    get => Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandard);
    //    set => Set(Prop.NonStandard, value);
    //}

    /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments, that
    /// are associated with the vCard. <c>(2,3,4)</c></summary>
    public TextBuilder Notes => new TextBuilder(this, Prop.Notes);

    ///// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
    ///// associated with the vCard object. <c>(2,3,4)</c></summary>
    //public IEnumerable<OrgProperty?>? Organizations
    //{
    //    get => Get<IEnumerable<OrgProperty?>?>(Prop.Organizations);
    //    set => Set(Prop.Organizations, value);
    //}

    /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
    /// which can be used to look up information on the person's co-workers. <c>(RFC
    /// 6715)</c></summary>
    public TextBuilder OrgDirectories => new TextBuilder(this, Prop.OrgDirectories);

    /// <summary> <c>TEL</c>: Canonical number strings for a telephone numbers for 
    /// telephony communication with the vCard object. <c>(2,3,4)</c></summary>
    public TextBuilder Phones => new TextBuilder(this, Prop.Phones);

    ///// <summary> <c>PHOTO</c>: Image(s) or photograph(s) of the individual associated
    ///// with the vCard. <c>(2,3,4)</c></summary>
    //public IEnumerable<DataProperty?>? Photos
    //{
    //    get => Get<IEnumerable<DataProperty>>(Prop.Photos);
    //    set => Set(Prop.Photos, value);
    //}

    /// <summary> <c>PRODID</c>: The identifier for the product that created the vCard
    /// object. <c>(3,4)</c></summary>
    /// <value>The name should be unique worldwide. It should therefore conform to the
    /// specification for Formal Public Identifiers [ISO 9070] or Universal Resource
    /// Names in RFC 3406.</value>
    public TextSingletonBuilder ProductID => new TextSingletonBuilder(this, Prop.ProductID);

    /// <summary> <c>PROFILE</c>: States that the <see cref="VCard"/> is a vCard. <c>(3)</c></summary>
    public ProfileBuilder Profile => new ProfileBuilder(this);

    ///// <summary> <c>RELATED</c>: Other entities that the person or organization is 
    ///// related to. <c>(4)</c></summary>
    //public IEnumerable<RelationProperty?>? Relations
    //{
    //    get => Get<IEnumerable<RelationProperty?>?>(Prop.Relations);
    //    set => Set(Prop.Relations, value);
    //}

    /// <summary> <c>ROLE</c>: The role, occupation, or business category of the vCard
    /// object within an organization. <c>(2,3,4)</c></summary>
    public TextBuilder Roles => new TextBuilder(this, Prop.Roles);

    ///// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
    ///// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
    //public IEnumerable<DataProperty?>? Sounds
    //{
    //    get => Get<IEnumerable<DataProperty?>?>(Prop.Sounds);
    //    set => Set(Prop.Sounds, value);
    //}

    /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
    /// this vCard. <c>(3,4)</c></summary>
    /// <remarks>vCard&#160;3.0 allows only one instance of this property.</remarks>
    public TextBuilder Sources => new TextBuilder(this, Prop.Sources);


    /// <summary> <c>REV</c>: A time stamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// With <see cref="VcfOptions.Default"/> the flag <see cref="VcfOptions.UpdateTimeStamp"/> is set. So 
    /// this property is normally updated automatically when serializing VCF.
    /// </remarks>
    public TimeStampBuilder TimeStamp => new TimeStampBuilder(this);

    /// <summary> <c>TZ</c>: The time zone(s) of the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <note type="tip">
    /// This information can be connected directly with an <see cref="AddressProperty"/> 
    /// object via its <see cref="ParameterSection.TimeZone"/> property in vCard&#160;4.0.
    /// </note>
    /// </remarks>
    public TimeZoneBuilder TimeZones => new TimeZoneBuilder(this);

    /// <summary> <c>TITLE</c>: Specifies the job title, functional position or function
    /// of the individual, associated with the vCard object, within an organization.
    /// <c>(2,3,4)</c></summary>
    public TextBuilder Titles => new TextBuilder(this, Prop.Titles);

    /// <summary> <c>URL</c>: URLs, pointing to websites that represent the person in
    /// some way. <c>(2,3,4)</c></summary>
    public TextBuilder Urls => new TextBuilder(this, Prop.Urls);

    /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
    public XmlBuilder Xmls => new XmlBuilder(this);


    ////////////////////////////////////////////////////////////////////
    public VCardBuilder AddKey() => throw new NotImplementedException();

    public VCardBuilder ClearKeys()
    {
        _vCard.Keys = null;
        return this;
    }

    //public VCardBuilder RemoveKey(DataProperty? prop)
    //{
    //    _vCard.Keys = _vCard.Keys.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveKey(Func<DataProperty, bool> predicate)
    {
        _vCard.Keys = _vCard.Keys.Remove(predicate);
        return this;
    }

    public VCardBuilder AddLogo() => throw new NotImplementedException();

    public VCardBuilder ClearLogos()
    {
        _vCard.Logos = null;
        return this;
    }

    //public VCardBuilder RemoveLogo(DataProperty? prop)
    //{
    //    _vCard.Logos = _vCard.Logos.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveLogo(Func<DataProperty, bool> predicate)
    {
        _vCard.Logos = _vCard.Logos.Remove(predicate);
        return this;
    }

    

    public VCardBuilder AddMember() => throw new NotImplementedException();

    public VCardBuilder ClearMembers()
    {
        _vCard.Members = null;
        return this;
    }

    //public VCardBuilder RemoveMember(RelationProperty? prop)
    //{
    //    _vCard.Members = _vCard.Members.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveMember(Func<RelationProperty, bool> predicate)
    {
        _vCard.Members = _vCard.Members.Remove(predicate);
        return this;
    }

    public VCardBuilder AddNameView() => throw new NotImplementedException();

    public VCardBuilder ClearNameViews()
    {
        _vCard.NameViews = null;
        return this;
    }

    //public VCardBuilder RemoveNameView(NameProperty? prop)
    //{
    //    _vCard.NameViews = _vCard.NameViews.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveNameView(Func<NameProperty, bool> predicate)
    {
        _vCard.NameViews = _vCard.NameViews.Remove(predicate);
        return this;
    }

 
    public VCardBuilder AddNonStandard() => throw new NotImplementedException();

    public VCardBuilder ClearNonStandards()
    {
        _vCard.NonStandards = null;
        return this;
    }

    //public VCardBuilder RemoveNonStandard(NonStandardProperty? prop)
    //{
    //    _vCard.NonStandards = _vCard.NonStandards.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveNonStandard(Func<NonStandardProperty, bool> predicate)
    {
        _vCard.NonStandards = _vCard.NonStandards.Remove(predicate);
        return this;
    }


    public VCardBuilder AddOrganization() => throw new NotImplementedException();

    public VCardBuilder ClearOrganizations()
    {
        _vCard.Organizations = null;
        return this;
    }

    //public VCardBuilder RemoveOrganization(OrgProperty? prop)
    //{
    //    _vCard.Organizations = _vCard.Organizations.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveOrganization(Func<OrgProperty, bool> predicate)
    {
        _vCard.Organizations = _vCard.Organizations.Remove(predicate);
        return this;
    }


    public VCardBuilder AddPhoto() => throw new NotImplementedException();

    public VCardBuilder ClearPhotos()
    {
        _vCard.Photos = null;
        return this;
    }

    //public VCardBuilder RemovePhoto(DataProperty? prop)
    //{
    //    _vCard.Photos = _vCard.Photos.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemovePhoto(Func<DataProperty, bool> predicate)
    {
        _vCard.Photos = _vCard.Photos.Remove(predicate);
        return this;
    }

    public VCardBuilder AddRelation() => throw new NotImplementedException();

    public VCardBuilder ClearRelations()
    {
        _vCard.Relations = null;
        return this;
    }

    //public VCardBuilder RemoveRelation(RelationProperty? prop)
    //{
    //    _vCard.Relations = _vCard.Relations.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveRelation(Func<RelationProperty, bool> predicate)
    {
        _vCard.Relations = _vCard.Relations.Remove(predicate);
        return this;
    }


    public VCardBuilder AddSound() => throw new NotImplementedException();

    public VCardBuilder ClearSounds()
    {
        _vCard.Sounds = null;
        return this;
    }

    //public VCardBuilder RemoveSound(DataProperty? prop)
    //{
    //    _vCard.Sounds = _vCard.Sounds.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveSound(Func<DataProperty, bool> predicate)
    {
        _vCard.Sounds = _vCard.Sounds.Remove(predicate);
        return this;
    }


    //public VCardBuilder AddXml() => throw new NotImplementedException();

    //public VCardBuilder ClearXmls()
    //{
    //    _vCard.Xmls = null;
    //    return this;
    //}

    ////public VCardBuilder RemoveXml(XmlProperty? prop)
    ////{
    ////    _vCard.Xmls = _vCard.Xmls.Remove(prop);
    ////    return this;
    ////}

    //public VCardBuilder RemoveXml(Func<XmlProperty, bool> predicate)
    //{
    //    _vCard.Xmls = _vCard.Xmls.Remove(predicate);
    //    return this;
    //}

    internal static IEnumerable<TSource?> Add<TSource>(TSource prop,
                                                       IEnumerable<TSource?>? coll,
                                                       Action<ParameterSection>? parameters,
                                                       bool pref)
        where TSource : VCardProperty, IEnumerable<TSource>
    {
        parameters?.Invoke(prop.Parameters);

        coll = coll is null ? prop
                            : pref ? prop.Concat(coll.OrderByPref(false))
                                   : coll.Concat(prop);
        if (pref)
        {
            coll.SetPreferences(false);
        }

        return coll;
    }
}

public readonly struct NonStandardBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal NonStandardBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string xName,
                            string? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop.NonStandards,
                           VCardBuilder.Add(new NonStandardProperty(xName, value, group),
                                            Builder._vCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards),
                                            parameters,
                                            pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.NonStandards, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<NonStandardProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.NonStandards, 
                           Builder._vCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards).Remove(predicate));
        return _builder!;
    }
}


public readonly struct OrgBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal OrgBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string? organizationName,
                            IEnumerable<string?>? organizationalUnits = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop.Organizations,
                           VCardBuilder.Add(new OrgProperty(organizationName, organizationalUnits, group),
                                            Builder._vCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations),
                                            parameters,
                                            pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.Organizations, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<OrgProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.Organizations,
                           Builder._vCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations).Remove(predicate));
        return _builder!;
    }
}

public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal NameBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder._vCard.Set(Prop.NameViews, 
                           VCardBuilder.Add(new NameProperty(familyNames,
                                                             givenNames,
                                                             additionalNames,
                                                             prefixes,
                                                             suffixes, group),
                           Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                           parameters,
                           false));
        return _builder!;
    }

    public VCardBuilder Add(string? familyName,
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder._vCard.Set(Prop.NameViews,
                           VCardBuilder.Add(new NameProperty(familyName,
                                                             givenName,
                                                             additionalName,
                                                             prefix,
                                                             suffix,
                                                             group),
                           Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                           parameters,
                           false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.NameViews, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<NameProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.NameViews, Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews).Remove(predicate));
        return _builder!;
    }
}

public readonly struct DataBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal DataBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder AddFile(string filePath,
                                string? mimeType = null,
                                string? group = null,
                                Action<ParameterSection>? parameters = null,
                                bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(DataProperty.FromFile(filePath, mimeType, group),
                                                  Builder._vCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder AddBytes(byte[]? bytes,
                                 string? mimeType = MimeString.OctetStream,
                                 string? group = null,
                                 Action<ParameterSection>? parameters = null,
                                 bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(DataProperty.FromBytes(bytes, mimeType, group),
                                                  Builder._vCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder AddText(string? text,
                                string? mimeType = null,
                                string? group = null,
                                Action<ParameterSection>? parameters = null,
                                bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(DataProperty.FromText(text, mimeType, group),
                                                  Builder._vCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder AddUri(Uri? uri,
                               string? mimeType = null,
                               string? group = null,
                               Action<ParameterSection>? parameters = null,
                               bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(DataProperty.FromUri(uri, mimeType, group),
                                                  Builder._vCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<DataProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop, Builder._vCard.Get<IEnumerable<DataProperty?>?>(Prop).Remove(predicate));
        return _builder!;
    }
}


public readonly struct RelationBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal RelationBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(Guid uuid,
                            Rel? relationType,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromGuid(uuid, relationType, group),
                                                  Builder._vCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder Add(string? text,
                            Rel? relationType,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromText(text, relationType, group),
                                                  Builder._vCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<RelationProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop, Builder._vCard.Get<IEnumerable<RelationProperty?>?>(Prop).Remove(predicate));
        return _builder!;
    }
}

