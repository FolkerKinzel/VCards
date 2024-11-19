using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

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
    #region Remove with version 8.0.1

    [Obsolete("Use SpokenLanguages instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public IEnumerable<TextProperty?>? Languages
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    [Obsolete("Use Updated instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public TimeStampProperty? TimeStamp
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    [Obsolete("Use ContactID instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ContactIDProperty? ID
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    #endregion

    private readonly Dictionary<Prop, object> _propDic = [];

    [return: MaybeNull]
    internal T Get<T>(Prop prop) where T : class?
        => this._propDic.TryGetValue(prop, out object? value)
                    ? (T)value
                    : default;


    internal void Set(Prop prop, object? value)
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



    /// <summary>
    /// Gets this instance as <see cref="IEnumerable{T}"/> that allows to iterate over the stored
    /// properties.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> that allows to iterate over the stored
    /// properties.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Each <see cref="KeyValuePair{TKey, TValue}.Value"/> is either a <see cref="VCardProperty"/>
    /// or an <see cref="IEnumerable{T}">IEnumerable&lt;VCardProperty?&gt;</see>.
    /// </note>
    /// </remarks>
    internal IEnumerable<KeyValuePair<Prop, object>> Properties => this._propDic;

    /// <summary>
    /// Gets all the <see cref="Entities"/> stored in the <see cref="VCard" /> 
    /// instance grouped by the values of their <see cref="VCardProperty.Group"/>
    /// properties.
    /// </summary>
    /// <remarks>
    /// Group names are case-insenitive. Ungrouped entities are in the group with
    /// the <see cref="IGrouping{TKey, TElement}.Key"/>&#160;<c>null</c>.
    /// </remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\ExtensionMethodExample.cs"/>
    /// </example>
    public IEnumerable<Group> Groups => Entities.GroupBy(static x => x.Value.Group, StringComparer.OrdinalIgnoreCase);

    /// <summary>Gets all the data stored in the <see cref="VCard" /> 
    /// instance merged into a single <see cref="IEnumerable{T}"/>.</summary>
    public IEnumerable<Entity> Entities
    {
        get
        {
            foreach (KeyValuePair<Prop, object> item in _propDic)
            {
                if (item.Value is VCardProperty prop)
                {
                    yield return new KeyValuePair<Prop, VCardProperty>(item.Key, prop);
                }
                else
                {
                    var coll = (IEnumerable<VCardProperty?>)item.Value;

                    foreach (VCardProperty? p in coll)
                    {
                        if (p is not null)
                        {
                            yield return new KeyValuePair<Prop, VCardProperty>(item.Key, p);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// <c>X-ABLabel</c>: An Apple AddressBook (AB) extension to give other vCard properties a label.
    /// </summary>
    /// <remarks>
    /// The <see cref="TextProperty"/> instances stored in this property must have the same <see cref="VCardProperty.Group"/> 
    /// name as the <see cref="VCardProperty"/> instances they label.
    /// </remarks>
    public IEnumerable<TextProperty?>? ABLabels
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.ABLabels);
        set => Set(Prop.ABLabels, value);
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
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? AnniversaryViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.AnniversaryViews);
        set => Set(Prop.AnniversaryViews, value);
    }

    /// <summary> <c>CLIENTPIDMAP</c>: Gets the identifiers of the vCard clients
    /// that edited the vCard. <c>(4)</c></summary>
    /// <remarks>
    /// The value of this property can change when calling the methods of the
    /// <see cref="VCards.SyncOperation"/> object provided by the <see cref="Sync"/>
    /// property.
    /// </remarks>
    /// <seealso cref="Sync"/>
    /// <seealso cref="VCards.SyncOperation"/>
    /// <seealso cref="AppID"/>
    public IEnumerable<AppIDProperty>? AppIDs
    {
        get => Get<IEnumerable<AppIDProperty>?>(Prop.AppIDs);
        internal set => Set(Prop.AppIDs, value);
    }

    /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
    /// <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? BirthDayViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.BirthDayViews);
        set => Set(Prop.BirthDayViews, value);
    }

    /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
    /// RFC&#160;6474)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public IEnumerable<TextProperty?>? BirthPlaceViews
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.BirthPlaceViews);
        set => Set(Prop.BirthPlaceViews, value);
    }

    /// <summary> <c>CAPURI</c>: A protocol independent location from which a calendaring and 
    /// scheduling client can communicate with a user's entire calendar. <c>(3 - RFC&#160;2739)</c></summary>
    public IEnumerable<TextProperty?>? CalendarAccessUris
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.CalendarAccessUris);
        set => Set(Prop.CalendarAccessUris, value);
    }

    /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4, 3 - RFC&#160;2739)</c></summary>
    public IEnumerable<TextProperty?>? CalendarAddresses
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.CalendarAddresses);
        set => Set(Prop.CalendarAddresses, value);
    }

    /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
    /// the person's calendar. <c>(4, 3 - RFC&#160;2739)</c></summary>
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

    /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
    /// unique identifier corresponding to the entity associated with the vCard. <c>(2,3,4)</c>
    /// </summary>
    /// <remarks>As a default setting each newly created <see cref="VCard"/> gets an <see cref="ContactID"/>
    /// automatically.</remarks>
    public ContactIDProperty? ContactID
    {
        get => Get<ContactIDProperty?>(Prop.ContactID);
        set => Set(Prop.ContactID, value);
    }

    /// <summary>
    /// <c>CONTACT-URI</c>: URIs representing an email address or a location for a web form. <c>(4 - RFC&#160;8605)</c>
    /// </summary>
    /// <remarks>
    /// If the property contains more than one <see cref="TextProperty"/> instance, the
    /// <see cref="ParameterSection.Preference"/> property must be set.
    /// </remarks>
    public IEnumerable<TextProperty?>? ContactUris
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.ContactUris);
        set => Set(Prop.ContactUris, value);
    }

    /// <summary>
    /// <c>CREATED</c>: Defines the date and time when the vCard was created.
    /// <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the timestamp when the vCard was created. Copying the vCard across systems does 
    /// not count as a new creation nor a new revision. Instead, the timestamp value typically 
    /// stays unchanged for the existence of the vCard.
    /// </para>
    /// <para>
    /// Use the <see cref="Updated"/> property to specify the last revision.
    /// </para>
    /// </remarks>
    public TimeStampProperty? Created
    {
        get => Get<TimeStampProperty?>(Prop.Created);
        set => Set(Prop.Created, value);
    }

    

    /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC&#160;6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
    public IEnumerable<DateAndOrTimeProperty?>? DeathDateViews
    {
        get => Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop.DeathDateViews);
        set => Set(Prop.DeathDateViews, value);
    }

    /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
    /// RFC&#160;6474)</c></summary>
    /// <remarks>Multiple instances are only allowed if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
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
    /// The name representations stored in this property are typically intended to be 
    /// presented to the users of the application. 
    /// </remarks>
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

    /// <summary> <c>EXPERTISE</c>: A professional subject area that the person has
    /// knowledge of. <c>(RFC 6715)</c></summary>
    /// <remarks>Define the level of expertise in the parameter 
    /// <see cref="ParameterSection.Expertise" />!</remarks>
    public IEnumerable<TextProperty?>? Expertises
    {
        get => Get<IEnumerable<TextProperty>>(Prop.Expertises);
        set => Set(Prop.Expertises, value);
    }

    /// <summary> <c>FBURL</c>: Defines URLs that show when the person is "free" or
    /// "busy" on their calendar. <c>(4, 3 - RFC&#160;2739)</c></summary>
    /// <remarks>
    /// If several <see cref="TextProperty" /> objects are assigned, the standard 
    /// property is determined by the value of <see cref="ParameterSection.Preference" />. 
    /// URLs of the type <c>FTP</c> [RFC&#160;1738] or <c>HTTP</c> [RFC&#160;2616] refer to an
    /// iCalendar object [RFC&#160;5545], which represents a snapshot of the next weeks or 
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
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
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

    /// <summary>
    /// <c>GRAMGENDER</c>: Defines which grammatical gender to use in salutations and other 
    /// grammatical constructs. <c>4 - RFC&#160;9554</c>
    /// </summary>
    /// <remarks>
    /// This property defines the grammatical gender that the contact prefers to be addressed 
    /// by or referred to as in written or spoken form. For example, the German language 
    /// distinguishes by grammatical gender in salutations such as "Sehr geehrte" (feminine) 
    /// and "Sehr geehrter" (masculine). Multiple occurrences of this property MUST be 
    /// distinguished by the LANGUAGE parameter.
    /// </remarks>
    public IEnumerable<GramProperty?>? GramGenders
    {
        get => Get<IEnumerable<GramProperty?>?>(Prop.GramGenders);
        set => Set(Prop.GramGenders, value);
    }

    /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
    /// in. <c>(4 - RFC&#160;6715)</c></summary>
    /// <remarks> Define the level of interest with the parameter 
    /// <see cref="ParameterSection.Interest" />.</remarks>
    public IEnumerable<TextProperty?>? Hobbies
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Hobbies);
        set => Set(Prop.Hobbies, value);
    }

    /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
    /// in, but does not necessarily take part in. <c>(4 - RFC&#160;6715)</c></summary>
    /// <remarks> Define the level of interest in the parameter 
    /// <see cref="ParameterSection.Interest" />!</remarks>
    public IEnumerable<TextProperty?>? Interests
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Interests);
        set => Set(Prop.Interests, value);
    }

    /// <summary>
    /// <c>JSPROP</c>: JSContact properties that cannot be converted to standardized
    /// vCard properties. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    public IEnumerable<TextProperty?>? JSContactProps
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.JSContactProps);
        set => Set(Prop.JSContactProps, value);
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

    /// <summary> <c>KIND</c>: Defines the type of entity that this vCard represents.
    /// <c>(4)</c></summary>
    public KindProperty? Kind
    {
        get => Get<KindProperty?>(Prop.Kind);
        set => Set(Prop.Kind, value);
    }

    /// <summary> <c>LANGUAGE</c>: Defines the default language that human-readable 
    /// text values in this <see cref="VCard"/> are assumed to be written in.
    /// <c>(4 - RFC 9554)</c></summary>
    /// <remarks>
    /// <para>
    /// This property defines the language that property values of type <c>TEXT</c> are assumed 
    /// to be written in for this <see cref="VCard"/>. If a <see cref="VCardProperty"/> includes the 
    /// <see cref="ParameterSection.Language"/> parameter, 
    /// then the parameter value has higher precedence than the <see cref="Language"/> property value.
    /// </para>
    /// <para>
    /// The <see cref="ParameterSection.Language"/> parameter MUST NOT be assigned to this property.
    /// </para>
    /// </remarks>
    /// <value>RFC 5646 language tag.</value>
    public TextProperty? Language
    {
        get => Get<TextProperty?>(Prop.Language);
        set => Set(Prop.Language, value);
    }

    /// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
    /// is associated with the individual to which the <see cref="VCard"/> belongs. 
    /// <c>(2,3,4)</c></summary>
    public IEnumerable<DataProperty?>? Logos
    {
        get => Get<IEnumerable<DataProperty?>?>(Prop.Logos);
        set => Set(Prop.Logos, value);
    }

    /// <summary> <c>MAILER</c>: Name of the e-mail program. <c>(2,3)</c></summary>
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

    /// <summary> <c>IMPP</c>: Instant messenger handles. <c>(3,4)</c></summary>
    /// <remarks>
    /// <see cref="TextProperty.Value" /> should specify a URI for instant messaging 
    /// and presence protocol communications with the object the <see cref="VCard"/> 
    /// represents. If the URI can be used for voice and/or video, the 
    /// <see cref="VCard.Phones" /> property SHOULD be used in addition to this 
    /// property.</remarks>
    public IEnumerable<TextProperty?>? Messengers
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Messengers);
        set => Set(Prop.Messengers, value);
    }

    /// <summary> <c>N</c>: A structured representation of the name of the person, place
    /// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
    /// <remarks>Multiple instances are only allowed in vCard&#160;4.0, and only if all of them
    /// have the same <see cref="ParameterSection.AltID" /> parameter. This can,
    /// e.g., be useful if the property is displayed in different languages.</remarks>
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
    /// <see cref="NonStandards" /> contains all vCard properties that could not 
    /// be evaluated, when parsing the vCard. To serialize the content of 
    /// <see cref="NonStandards" /> into a VCF file, the flag 
    /// <see cref="VcfOpts.WriteNonStandardProperties"/> has to be set. 
    /// </para>
    /// <para>
    /// Some <see cref="NonStandardProperty" /> objects are automatically added to the 
    /// VCF file, if there is no standard equivalent for it. You can control this behavior
    /// with <see cref="VcfOpts" />. It is therefore not recommended to assign
    /// <see cref="NonStandardProperty" /> objects with these 
    /// <see cref="NonStandardProperty.Key"/>s to this property.
    /// </para>
    /// <para>
    /// These vCard properties are the following: 
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Key</term>
    /// <term>Property of the <see cref="VCard"/> class</term>
    /// </listheader>
    /// <item>
    /// <term><c>X-ABLabel</c></term>
    /// <term><see cref="ABLabels"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-AIM</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-ANNIVERSARY</c></term>
    /// <term><see cref="AnniversaryViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-EVOLUTION-SPOUSE</c></term>
    /// <term><see cref="Relations"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-EVOLUTION-ANNIVERSARY</c></term>
    /// <term><see cref="AnniversaryViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-GADUGADU</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-GENDER</c></term>
    /// <term><see cref="GenderViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-GOOGLE-TALK</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-GROUPWISE</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-GTALK</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-ICQ</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-JABBER</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-KADDRESSBOOK-X-ANNIVERSARY</c></term>
    /// <term><see cref="AnniversaryViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-KADDRESSBOOK-X-IMADDRESS</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-KADDRESSBOOK-X-SPOUSENAME</c></term>
    /// <term><see cref="Relations"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-MS-IMADDRESS</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-MSN</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-SKYPE</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-SKYPE-USERNAME</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-SOCIALPROFILE</c></term>
    /// <term><see cref="SocialMediaProfiles"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-SPOUSE</c></term>
    /// <term><see cref="Relations"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-TWITTER</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-WAB-GENDER</c></term>
    /// <term><see cref="GenderViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-WAB-WEDDING_ANNIVERSARY</c></term>
    /// <term><see cref="AnniversaryViews"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-WAB-SPOUSE_NAME</c></term>
    /// <term><see cref="Relations"/></term>
    /// </item>
    /// <item>
    /// <term><c>X-YAHOO</c></term>
    /// <term><see cref="Messengers"/></term>
    /// </item>
    /// </list>
    /// </remarks>
    public IEnumerable<NonStandardProperty?>? NonStandards
    {
        get => Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards);
        set => Set(Prop.NonStandards, value);
    }

    /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments that
    /// are associated with the vCard. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Notes
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Notes);
        set => Set(Prop.Notes, value);
    }

    /// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
    /// associated with the vCard object. <c>(2,3,4)</c></summary>
    public IEnumerable<OrgProperty?>? Organizations
    {
        get => Get<IEnumerable<OrgProperty?>?>(Prop.Organizations);
        set => Set(Prop.Organizations, value);
    }

    /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
    /// which can be used to look up information on the person's co-workers. 
    /// <c>(RFC&#160;6715)</c></summary>
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
    public TextProperty? ProductID
    {
        get => Get<TextProperty?>(Prop.ProductID);
        set => Set(Prop.ProductID, value);
    }

    /// <summary> <c>PROFILE</c>: States that the <see cref="VCard"/> is a vCard. <c>(3)</c></summary>
    public ProfileProperty? Profile
    {
        get => Get<ProfileProperty?>(Prop.Profile);
        set => Set(Prop.Profile, value);
    }

    /// <summary> <c>PRONOUNS</c>: Defines the pronouns that shall be used to refer to the entity 
    /// represented by this <see cref="VCard"/>. <c>(4 - RFC 9554)</c></summary>
    /// <remarks>
    /// This property contains the pronouns that the contact chooses to use for themselves. 
    /// The value is free-form text. These pronouns shall be used when addressing or referring to the 
    /// contact. Multiple occurrences of this property MAY define pronouns for multiple languages, 
    /// preferences, and contexts. Multiple pronouns in the same language SHOULD use the 
    /// <see cref="ParameterSection.Preference"/> parameter; 
    /// otherwise, the order of preference is implementation-specific.
    /// </remarks>
    public IEnumerable<TextProperty?>? Pronouns
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Pronouns);
        set => Set(Prop.Pronouns, value);
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

    /// <summary> <c>SOCIALPROFILE</c>: Specifies the URI or username for social media 
    /// profiles associated with the object the <see cref="VCard"/> represents. <c>(4 - RFC 9554)</c></summary>
    /// <value>A single URI or TEXT value. The default value type is URI.</value>
    /// <remarks>
    /// <para>
    /// In addition to the typical use of this property with URI values, it also allows setting usernames for social 
    /// media services as free-text TEXT values, in which case the <see cref="ParameterSection.ServiceType"/> parameter
    /// MUST be provided.
    /// </para>
    /// <para>
    /// The <see cref="ParameterSection.ServiceType"/> parameter MAY be assigned if the value type is URI.
    /// </para>
    /// </remarks>
    public IEnumerable<TextProperty?>? SocialMediaProfiles
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.SocialMediaProfiles);
        set => Set(Prop.SocialMediaProfiles, value);
    }

    /// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
    /// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
    public IEnumerable<DataProperty?>? Sounds
    {
        get => Get<IEnumerable<DataProperty?>?>(Prop.Sounds);
        set => Set(Prop.Sounds, value);
    }

    /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
    /// this vCard. <c>(3,4)</c></summary>
    /// <remarks>vCard&#160;3.0 allows only one instance of this property.</remarks>
    public IEnumerable<TextProperty?>? Sources
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Sources);
        set => Set(Prop.Sources, value);
    }

    /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
    public IEnumerable<TextProperty?>? SpokenLanguages
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.SpokenLanguages);
        set => Set(Prop.SpokenLanguages, value);
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

    /// <summary> <c>REV</c>: A time stamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// With <see cref="VcfOpts.Default"/> the flag <see cref="VcfOpts.UpdateTimeStamp"/> is set. So 
    /// this property is normally updated automatically when serializing VCF.
    /// </remarks>
    public TimeStampProperty? Updated
    {
        get => Get<TimeStampProperty?>(Prop.Updated);
        set => Set(Prop.Updated, value);
    }

    /// <summary> <c>URL</c>: URLs pointing to websites that represent the person in
    /// some way. <c>(2,3,4)</c></summary>
    public IEnumerable<TextProperty?>? Urls
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Urls);
        set => Set(Prop.Urls, value);
    }

    /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
    /// <value>
    /// The <see cref="TextProperty"/> instances may contain text that is valid XML. The root element
    /// must be explicitly assigned to an XML namespace (xmlns attribute). This namespace
    /// must not be the VCARD 4.0 namespace <c>urn:ietf:params:xml:ns:vcard-4.0</c>!
    /// </value>
    public IEnumerable<TextProperty?>? Xmls
    {
        get => Get<IEnumerable<TextProperty?>?>(Prop.Xmls);
        set => Set(Prop.Xmls, value);
    }

    /// <summary>
    /// Replaces <see cref="RelationProperty"/> instances in <see cref="Members"/>
    /// that contain text with <see cref="RelationProperty"/> instances that contain <see cref="VCard"/>s.
    /// </summary>
    /// <remarks>
    /// RFC 6350 allows only URIs as value for <c>MEMBER</c>. Values that can't be preserved as URI will be saved in 
    /// <see cref="VCard"/>s and the <see cref="VCard.ContactID"/> Guids of these <see cref="VCard"/> instances will be 
    /// the content of <c>MEMBER</c> after <see cref="VCard.ReferenceIntl(List{VCard})"/>.
    /// </remarks>
    internal void NormalizeMembers()
    {
        if (Members is null)
        {
            return;
        }

        // Members that contain Relation.Empty can be ignored because
        // an empty string is an empty URI too
        RelationProperty[] members = Members.Items().ToArray();
        Members = members;
        Span<RelationProperty> span = members.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            RelationProperty prop = span[i];

            if (prop.Value.ContactID?.String is string text)
            {
                Debug.Assert(!prop.IsEmpty);
                Debug.Assert(!Uri.TryCreate(text, UriKind.Absolute, out _));

                var relProp = new RelationProperty(Relation.Create(new VCard(setContactID: true, setCreated: false)
                    {
                        DisplayNames = new TextProperty(text)
                    }), prop.Group);
                relProp.Parameters.RelationType = prop.Parameters.RelationType;
                span[i] = relProp;
            }
        }
    }
}
