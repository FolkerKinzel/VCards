using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates the information about the parameters of a vCard property.</summary>
/// <threadsafety static="true" instance="false" />
public sealed partial class ParameterSection
{
    /// <summary>
    /// Singleton that is only used to ensure that the property 
    /// <see cref="ParameterSerializer.ParaSection"/> is never <c>null</c>
    /// - even in unit tests.
    /// </summary>
    internal static readonly ParameterSection Empty = new();

    private readonly Dictionary<VCdParam, object> _propDic = [];

    [return: MaybeNull]
    private T Get<T>(VCdParam prop)
        => this._propDic.TryGetValue(prop, out object? value)
               ? (T)value
               : default;

    private void Set<T>(VCdParam prop, T value)
    {
        if (value is null || value.Equals(default(T)))
        {
            _ = _propDic.Remove(prop);
        }
        else
        {
            _propDic[prop] = value;
        }
    }

    /// <summary>
    /// Number of data entries stored in the <see cref="ParameterSection"/>
    /// instance.
    /// </summary>
    /// <remarks>Used for testing and debugging.</remarks>
    internal int Count => _propDic.Count;

    /// <summary> <c>TYPE</c>: Specifies the type of a postal delivery address. 
    /// <c>(2,3)</c></summary>
    public Adr? AddressType
    {
        get => Get<Adr?>(VCdParam.AddressType);
        set
        {
            value = (value == default(Adr)) ? null : value;
            Set(VCdParam.AddressType, value);
        }
    }

    /// <summary><c>ALTID</c>: A common identifier that indicates, that several instances of 
    /// the same property represent the same (e.g. in different languages). <c>(4)</c></summary>
    public string? AltID
    {
        get => Get<string?>(VCdParam.AltID);
        set => Set(VCdParam.AltID, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary>
    /// <c>AUTHOR</c>: Identifies the author of the associated <see cref="VCardProperty.Value"/>. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <value>An absolute <see cref="Uri"/> or <c>null</c>.</value>
    /// <remarks>
    /// This parameter MAY be set on any <see cref="VCardProperty"/> where conveying authorship is desired. 
    /// It identifies the author as an absolute <see cref="Uri"/>. As an alternative or in addition to this parameter, 
    /// the <see cref="AuthorName"/> parameter allows naming an author as a free-text value.
    /// </remarks>
    /// <exception cref="ArgumentException">The value is a relative <see cref="Uri"/>.</exception>
    public Uri? Author
    {
        get => Get<Uri?>(VCdParam.Author);
        set
        {
            if (!value?.IsAbsoluteUri ?? false)
            {
                throw new ArgumentException(string.Format(Res.RelativeUri, value));
            }

            Set(VCdParam.Author, value);
        }
    }

    /// <summary>
    /// <c>AUTHOR-NAME</c>: Names the author of the associated <see cref="VCardProperty.Value"/>. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <value>Author's name as a free-text value</value>
    /// <remarks>
    /// This parameter MAY be set on any <see cref="VCardProperty"/> where conveying authorship is desired. 
    /// As an alternative or in addition to this parameter, the <see cref="Author"/> parameter allows identifying
    /// an author by <see cref="Uri"/>.
    /// </remarks>
    public string? AuthorName
    {
        get => Get<string?>(VCdParam.AuthorName);
        set => Set(VCdParam.AuthorName, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary>
    /// <c>CREATED</c>: Defines the date and time when a <see cref="VCardProperty"/> was created. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// This parameter MAY be set on any <see cref="VCardProperty"/> to define the point in time when the property was created. 
    /// Generally, updating a <see cref="VCardProperty"/> value SHOULD NOT change the creation timestamp.
    /// </remarks>
    public DateTimeOffset? Created
    {
        get => Get<DateTimeOffset?>(VCdParam.Created);
        set => Set(VCdParam.Created, value);
    }

    /// <summary><c>CALSCALE</c>: The calendar system in which a date or 
    /// date-time value is expressed. <c>(4)</c></summary>
    /// <value>
    /// <para>
    /// A <see cref="string"/> that specifies the calendar system. If none
    /// is specified, returns <see cref="VCard.DefaultCalendar"/>.
    /// </para>
    /// <para>If you want to specify a <c>CALSCALE</c> parameter in the VCF file, pass a 
    /// <see cref="string"/> to this property, otherwise assign
    /// <c>null</c> to ensure that no <c>CALSCALE</c> parameter will be written.
    /// </para>
    /// </value>
    [AllowNull]
    public string Calendar
    {
        get => Get<string?>(VCdParam.Calendar) ?? VCard.DefaultCalendar;

        set => Set<string?>(VCdParam.Calendar,
                            string.IsNullOrWhiteSpace(value)
                             ? null
                             : value.Trim());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string? GetCalendar() => Get<string?>(VCdParam.Calendar);

    /// <summary> <c>CHARSET</c>: Indicates the character set that was used for the
    /// property. <c>(2)</c></summary>
    public string? CharSet
    {
        get => Get<string?>(VCdParam.CharSet);
        set => Set(VCdParam.CharSet, value);
    }

    /// <summary>
    /// <c>JSCOMPS</c>: Defines the order and separators for the elements of a structured property value. <c>(4 - RFC&#160;9555)</c>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value facilitates converting name and address components between JSContact and vCard. It preserves the order of the components 
    /// of the JSContact property and contains the verbatim values of separator components. (See RFC&#160;9555 for details.)
    /// </para>
    /// <para>
    /// This parameter can be used with the <see cref="VCardProperty"/> instances in <see cref="VCard.NameViews"/> and <see cref="VCard.Addresses"/>.
    /// </para>
    /// </remarks>
    public string? ComponentOrder
    {
        get => Get<string?>(VCdParam.ComponentOrder);
        set => Set(VCdParam.ComponentOrder, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary><c>VALUE</c>: Indicates where the actual content of the property 
    /// is located. <c>(2)</c></summary>
    public Loc ContentLocation
    {
        get => Get<Loc>(VCdParam.ContentLocation);
        set
        {
            Set(VCdParam.ContentLocation, value);

            if (value != Loc.Inline)
            {
                DataType = Data.Uri;
            }
            else if (DataType == Data.Uri)
            {
                DataType = null;
            }
        }
    }

    /// <summary> <c>CONTEXT</c>: Indicates the context of the data. <c>(3)</c></summary>
    /// <value><c>VCARD</c> or <c>LDAP</c>.</value>
    /// <remarks>Is used in the <c>SOURCE</c> property of vCard&#160;3.0.</remarks>
    public string? Context
    {
        get => Get<string?>(VCdParam.Context);
        set => Set<string?>(VCdParam.Context, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }

    /// <summary>
    /// <c>CC</c>: ISO&#160;3166 two-character country code. <c>(4 - RFC&#160;8605)</c>
    /// </summary>
    public string? CountryCode
    {
        get => Get<string?>(VCdParam.CountryCode);
        set => Set<string?>(VCdParam.CountryCode, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary><c>VALUE</c>: Indicates which of the data types predefined by the vCard
    /// standard the content of the vCard property corresponds to. <c>(3,4)</c></summary>
    public Data? DataType
    {
        get => Get<Data?>(VCdParam.DataType);
        set => Set(VCdParam.DataType, value);
    }

    /// <summary>
    /// <c>DERIVED</c>: Specifies that the value of the associated <see cref="VCardProperty"/> is derived from some other 
    /// <see cref="VCardProperty"/> values in the same <see cref="VCard"/>. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property parameter SHOULD be specified on a <see cref="VCardProperty"/> if the property value is derived 
    /// from some other 
    /// properties in the same <see cref="VCard"/>. When present with a value of <c>true</c>, clients MUST NOT update 
    /// the <see cref="VCardProperty"/>.
    /// </para>
    /// <para>
    /// As an example, an implementation may derive the value of the FN property from the name components of the N property.
    /// It indicates this fact by setting the DERIVED parameter on the FN property to "true".
    /// </para>
    /// </remarks>
    public bool Derived
    {
        get => Get<bool>(VCdParam.Derived);
        set => Set(VCdParam.Derived, value);
    }

    /// <summary> <c>TYPE</c>: Describes the type of an e-mail address. <c>(2,3)</c></summary>
    /// <value>Use only the constants defined in the <see cref="VCards.Enums.EMail" /> class!</value>
    public string? EMailType
    {
        get => Get<string?>(VCdParam.EMailType);
        set => Set<string?>(VCdParam.EMailType, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }

    /// <summary><c>ENCODING</c>: Indicates the encoding of the VCardProperty. <c>(2,3)</c></summary>
    /// <value>
    /// <para>
    /// The value is set automatically - only for <see cref="NonStandardProperty"/> objects 
    /// this has to be done manually. 
    /// </para>
    /// <para>
    /// In vCard&#160;3.0 only <see cref="Enc.Base64" /> is permitted. 
    /// </para>
    /// </value>
    public Enc? Encoding
    {
        get => Get<Enc?>(VCdParam.Encoding);
        set => Set(VCdParam.Encoding, value);
    }

    /// <summary> <c>LEVEL</c>: A person's level of expertise. <c>(4 - RFC&#160;6715)</c></summary>
    /// <remarks> Used for the property <see cref="VCard.Expertises">VCard.Expertises</see>.
    /// </remarks>
    public Expertise? Expertise
    {
        get => Get<Expertise?>(VCdParam.Expertise);
        set => Set(VCdParam.Expertise, value);
    }

    /// <summary> <c>GEO</c>: Geographical position. <c>(4)</c></summary>
    /// <remarks>
    /// <para>
    /// This parameter is only written if it is attached to an
    /// <see cref="AddressProperty" /> object. 
    /// </para>
    /// <note type="tip">
    /// To preserve this information when serializing
    /// vCard&#160;2.1 or vCard&#160;3.0 make a copy of it in the
    /// <see cref="VCard.GeoCoordinates"/> property and connect the 
    /// <see cref="GeoProperty"/> with the <see cref="AddressProperty"/>
    /// using a <see cref="VCardProperty.Group"/> identifier.
    /// </note>
    /// </remarks>
    public GeoCoordinate? GeoPosition
    {
        get => Get<VCards.GeoCoordinate?>(VCdParam.GeoPosition);
        set => Set(VCdParam.GeoPosition, value);
    }

    /// <summary><c>INDEX</c>: 1-based index of a property in a multi-valued property.
    /// <c>(4 - RFC&#160;6715)</c>
    /// </summary>
    public int? Index
    {
        get => Get<int?>(VCdParam.Index);

        set
        {
            if (value.HasValue && value < 1)
            {
                value = 1;
            }

            Set(VCdParam.Index, value);
        }
    }

    /// <summary><c>TYPE</c>: Description of an instant messenger address. <c>(3 - RFC&#160;4770)</c>
    /// </summary>
    public Impp? InstantMessengerType
    {
        get => Get<Impp?>(VCdParam.InstantMessengerType);
        set => Set(VCdParam.InstantMessengerType, value);
    }

    /// <summary> <c>LEVEL</c>: Degree of interest of a person in a thing. <c>(4 - RFC&#160;6715)</c>
    /// </summary>
    /// <remarks>Used for the properties <see cref="VCard.Hobbies">VCard.Hobbies</see>
    /// and <see cref="VCard.Interests">VCard.Interests</see>.</remarks>
    public Interest? Interest
    {
        get => Get<Interest?>(VCdParam.Interest);
        set => Set(VCdParam.Interest, value);
    }

    /// <summary>
    /// <c>JSPTR</c>: This parameter is used with the instances in <see cref="VCard.JSContactProps"/>. Its value
    /// points to the JSContact property whose value is stored in the <see cref="VCardProperty"/>. <c>(4 - RFC&#160;9555)</c>
    /// </summary>
    /// <value>A valid JSON pointer as defined in RFC&#160;6901.</value>
    public string? JSContactPointer
    {
        get => Get<string?>(VCdParam.JSContactPointer);
        set => Set(VCdParam.JSContactPointer, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary><c>LABEL</c>: Represents the actual text that should be put on the mailing 
    /// label, when delivering a physical package to the person/object associated with the 
    /// <see cref="VCard"/>. <c>([2],[3],4)</c>
    /// </summary>
    /// <remarks> 
    /// <para>In the vCard standards 2.1 and 3.0, <c>ADR</c> and <c>LABEL</c> are separate 
    /// vCard properties. Only as of vCard&#160;4.0 they are permanently linked to one another. 
    /// When saving a vCard&#160;2.1 and vCard&#160;3.0, the content of this property is 
    /// automatically inserted into the vCard as a separate <c>LABEL</c> property. It's
    /// recommended in this case to assign a <see cref="VCardProperty.Group"/> identifier to 
    /// the <see cref="AddressProperty"/>: The <c>LABEL</c> property will get the same
    /// automatically.
    /// </para>
    /// <note type="warning">
    /// <para>
    /// When loading a vCard&#160;2.1 or 3.0, the library tries to create the link between 
    /// <c>LABEL</c> and <c>ADR</c> based on matching <see cref="VCardProperty.Group"/> identifiers
    /// and parameters of both properties. A text 
    /// comparison of the content of <c>ADR</c> and <c>LABEL</c> does not take place for 
    /// performance reasons.
    /// </para>
    /// <para>
    /// Although the correct assignment is usually successful in this way, this cannot be 
    /// guaranteed for every scenario. If the assignment of <c>ADR</c> and <c>LABEL</c>
    /// plays an important role for the application, the application should check this in 
    /// the case of vCard&#160;2.1 or vCard&#160;3.0 by text comparison or create its own labels 
    /// from the <see cref="Address"/> data, e.g., using the <see 
    /// cref="AddressProperty.AttachLabel()" /> method.
    /// </para>
    /// </note>
    /// </remarks>
    /// <seealso cref="AddressProperty.AttachLabel" />
    public string? Label
    {
        get => Get<string?>(VCdParam.Label);
        set
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value;
            Set(VCdParam.Label, value);
        }
    }

    /// <summary><c>LANGUAGE</c>: Language of the <see cref="VCardProperty.Value"/> of
    /// the <see cref="VCardProperty"/>. <c>(2,3,4)</c></summary>
    /// <value>An IETF Language Tag as defined in Section 2 of RFC&#160;5646.</value>
    /// <example><code language="none">de-DE</code></example>
    public string? Language
    {
        get => Get<string?>(VCdParam.Language);
        set => Set<string?>(VCdParam.Language,
                            string.IsNullOrWhiteSpace(value) ? null
                                                             : value.Trim());
    }

    /// <summary>MEDIATYPE : Specifies the MIME type for the data to which a 
    /// URI refers. <c>(4)</c></summary>
    /// <value>Internet Media Type (&quot;MIME type&quot;) according to RFC&#160;2046.</value>
    /// <example><code language="none">text/plain</code></example>
    public string? MediaType
    {
        get => Get<string?>(VCdParam.MediaType);
        set => Set<string?>(VCdParam.MediaType, value);
    }

    /// <summary>Non-standard attributes. <c>(2,3,4)</c></summary>
    /// <remarks>
    /// <para>In order to write non-standardized attributes into a VCF file, the 
    /// <see cref="Opts.WriteNonStandardParameters">VcfOptions.WriteNonStandardParameters</see> 
    /// flag must be set explicitly when serializing the <see cref="VCard" /> object.
    /// </para>
    /// <para>
    /// Don't use this property for <c>X-SERVICE-TYPE</c>. Use <see cref="ServiceType"/> instead.
    /// </para>
    /// </remarks>
    public IEnumerable<KeyValuePair<string, string>>? NonStandard
    {
        get => Get<IEnumerable<KeyValuePair<string, string>>?>(VCdParam.NonStandard);
        set => Set(VCdParam.NonStandard, value);
    }

    /// <summary>
    /// <c>PHONETIC</c>: Defines how to pronounce the value of another property in the same vCard. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <value>The parameter value indicates the phonetic system.</value>
    /// <remarks>
    /// This property parameter indicates that the value of its property contains the phonetic representation of another 
    /// same-named property in the same vCard. Exemplary uses are defining how to pronounce Japanese names and romanizing 
    /// Mandarin or Cantonese names and address components.
    /// </remarks>
    public Phonetic? Phonetic
    {
        get => Get<Phonetic?>(VCdParam.Phonetic);
        set => Set(VCdParam.Phonetic, value);
    }

    /// <summary><c>TYPE</c>: Describes a phone number. <c>(2,3,4)</c></summary>
    public Tel? PhoneType
    {
        get => Get<Tel?>(VCdParam.PhoneType);
        set
        {
            value = (value == default(Tel)) ? null : value;
            Set(VCdParam.PhoneType, value);
        }
    }

    /// <summary><c>PREF</c> or <c>TYPE=PREF</c>: Expresses preference for a property.
    /// <c>(2,3,4)</c></summary>
    /// <value>A value between 1 and 100. 1 means most preferred.</value>
    /// <remarks>
    /// A differentiated preference can only be stated beginning with vCard&#160;4.0. In 
    /// vCard&#160;2.1 and vCard&#160;3.0 only the most preferred property is marked with 
    /// <c>PREF</c>. As the preferred property the one is selected with the lowest 
    /// numerical value for <see cref="Preference" />.
    /// </remarks>
    public int Preference
    {
        get => _propDic.TryGetValue(VCdParam.Preference, out object? value) ? (int)value : PREF_MAX_VALUE;

        set
        {
            if (value < PREF_MIN_VALUE)
            {
                value = PREF_MIN_VALUE;
            }
            else if (value > PREF_MAX_VALUE)
            {
                value = PREF_MAX_VALUE;
            }

            // discussion: A value greater than 100 (PREF_MAX_VALUE) is discarded:
            Set(VCdParam.Preference, value == PREF_MAX_VALUE ? default : value);
        }
    }

    /// <summary> <c>TYPE</c>: Indicates, wether a <see cref="VCardProperty" /> is related
    /// to an individual's work place or to an individual's personal life. <c>(2,3,4)</c></summary>
    public PCl? PropertyClass
    {
        get => Get<PCl?>(VCdParam.PropertyClass);
        set
        {
            value = (value == default(PCl)) ? null : value;
            Set(VCdParam.PropertyClass, value);
        }
    }

    /// <summary>
    /// <c>PROP-ID</c>: Identifies a property among all its siblings of the same property name. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <value>
    /// A valid value must be a <see cref="string"/> of 1 and a maximum of 255 characters in size, and it MUST only 
    /// contain the ASCII alphanumeric characters ("A-Za-z0-9"), hyphen (-), and underscore ("_"). 
    /// </value>
    /// <remarks>
    /// <para>
    /// (This is the property identification mechanism newly introduced with RFC 9554. The vCard 4.0 property identification
    /// is <see cref="PropertyIDs"/>.)
    /// </para>
    /// <para>
    /// This parameter uniquely identifies a <see cref="VCardProperty"/> among all of its siblings with the same name within a vCard. 
    /// The identifier's only purpose is to uniquely identify siblings; its value has no other meaning. If an application
    /// makes use of <see cref="PropertyID"/>, it SHOULD 
    /// assign a unique identifier to each sibling <see cref="VCardProperty"/> of the same name within their embedding component. The same 
    /// identifier MAY be used for properties of a different name, and it MAY also be assigned to a same-named property
    /// that is not a sibling.
    /// </para>
    /// <para>
    /// Resolving duplicate identifier conflicts is specific to the application. Similarly, handling properties where 
    /// some but not all siblings have a <see cref="PropertyID"/> assigned is application-specific.
    /// </para>
    /// </remarks>
    public string? PropertyID
    {
        get => Get<string?>(VCdParam.PropertyID);
        set => Set(VCdParam.PropertyID, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary> <c>PID</c>: Gets the <see cref="PropertyID" />s used 
    /// to identify the <see cref="VCardProperty" />. <c>(4)</c></summary>
    /// <remarks>
    /// The value of this property can change when calling the methods of
    /// the <see cref="Syncs.SyncOperation"/> object provided by the <see cref="VCard.Sync"/>
    /// property.
    /// </remarks>
    /// <seealso cref="VCard.Sync"/>
    /// <seealso cref="Syncs.SyncOperation"/>
    /// <seealso cref="PropertyID"/>
    public IEnumerable<PropertyID>? PropertyIDs
    {
        get => Get<IEnumerable<PropertyID>?>(VCdParam.PropertyIDs);
        internal set => Set(VCdParam.PropertyIDs, value);
    }

    /// <summary> <c>TYPE</c>: Specifies the type of relationship with 
    /// a person. <c>(4)</c></summary>
    /// <remarks> Used in the <see cref="VCard.Relations">VCard.Relations</see> 
    /// property (<c>RELATED</c>).</remarks>
    public Rel? RelationType
    {
        get => Get<Rel?>(VCdParam.RelationType);
        set
        {
            value = (value == default(Rel)) ? null : value;
            Set(VCdParam.RelationType, value);
        }
    }

    /// <summary>
    /// <c>SCRIPT</c>: Defines the script that a <see cref="VCardProperty.Value"/> is written in. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// This parameter allows defining a script for a <see cref="VCardProperty.Value"/> without also defining a language as 
    /// the <see cref="Language"/> parameter would. The value MUST be a script subtag as 
    /// defined in Section 2.2.3 of RFC 5646. The <see cref="Script"/> parameter is often used in combination 
    /// with the <see cref="Phonetic"/> parameter.
    /// </remarks>
    public string? Script
    {
        get => Get<string?>(VCdParam.Script);
        set => Set(VCdParam.Script, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary>
    /// <c>SERVICE-TYPE</c>: Defines the online service name associated with a messaging or
    /// social media profile. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// This parameter MAY be specified on a <see cref="VCard.Messengers"/> or 
    /// a <see cref="VCard.SocialMediaProfiles"/> property to name the online service associated 
    /// with that property value. 
    /// Its value is case-sensitive.
    /// </remarks>
    /// <seealso cref="VCard.Messengers"/>
    /// <seealso cref="VCard.SocialMediaProfiles"/>
    public string? ServiceType
    {
        get => Get<string?>(VCdParam.ServiceType);
        set => Set(VCdParam.ServiceType, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary><c>SORT-AS</c>: Determines the sort order. <c>([3],4)</c></summary>
    /// <value><see cref="string"/>s (case-sensitive!). Maximum as many as fields of the compound property.</value>
    /// <example>
    /// <code language="none">
    /// FN:Rene van der Harten N;SORT-AS="Harten,Rene":van der Harten;Rene,J.;Sir;R.D.O.N.
    /// </code>
    /// </example>
    /// <remarks>When serializing a file as vCard&#160;3.0, a separate <c>SORT-STRING</c>-property,
    /// which contains the first <see cref="string" />, is automatically inserted into
    /// the vCard.</remarks>
    public IEnumerable<string>? SortAs
    {
        get => Get<IEnumerable<string>?>(VCdParam.SortAs)?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim());
        set => Set(VCdParam.SortAs, value);
    }

    /// <summary><c>TZ</c>: Time zone <c>(4)</c></summary>
    /// <remarks> 
    /// <para>
    /// This parameter is only written if it is attached to an 
    /// <see cref="AddressProperty" /> object.
    /// </para>
    /// <note type="tip">
    /// To preserve this information when serializing
    /// vCard&#160;2.1 or vCard&#160;3.0 make a copy of it in the
    /// <see cref="VCard.TimeZones"/> property and connect the 
    /// <see cref="TimeZoneProperty"/> with the <see cref="AddressProperty"/>
    /// using a <see cref="VCardProperty.Group"/> identifier.
    /// </note>
    /// </remarks>
    public TimeZoneID? TimeZone
    {
        get => Get<TimeZoneID?>(VCdParam.TimeZone);
        set => Set(VCdParam.TimeZone, value);
    }

    /// <summary>
    /// <c>USERNAME</c>: Defines a username such as the user of a messaging or 
    /// social media service. <c>(4 - RFC&#160;9554)</c>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This parameter MAY be specified on a <see cref="VCard.Messengers"/> or a <see cref="VCard.SocialMediaProfiles"/> 
    /// property to name the user with that property value. Its value is case-sensitive.
    /// </para>
    /// <para>
    /// The value of the <see cref="VCard.Messengers"/> or the <see cref="VCard.SocialMediaProfiles"/> 
    /// property MUST be a URI.
    /// </para>
    /// </remarks>
    public string? UserName
    {
        get => Get<string?>(VCdParam.UserName);
        set => Set(VCdParam.UserName, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }
}
