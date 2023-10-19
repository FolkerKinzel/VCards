using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates the information about the parameters of a vCard property.</summary>
/// <threadsafety static="true" instance="false" />
public sealed partial class ParameterSection
{
    private readonly Dictionary<VCdParam, object> _propDic = new();

    [return: MaybeNull]
    private T Get<T>(VCdParam prop)
        => _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;


    private void Set<T>(VCdParam prop, T value)
    {
        if (value is null || value.Equals(default))
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
    public AddressTypes? AddressType
    {
        get => Get<AddressTypes?>(VCdParam.AddressType);
        set
        {
            value = (value == default(AddressTypes)) ? null : value;
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

    /// <summary><c>CALSCALE</c>: It is used to define the calendar system in which a date or 
    /// date-time value is expressed. <c>(4)</c></summary>
    /// <value>The only value specified is <c>GREGORIAN</c>, which stands for the Gregorian
    /// system.</value>
    public string? Calendar
    {
        get => Get<string?>(VCdParam.Calendar);
        set => Set<string?>(VCdParam.Calendar, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary> <c>CHARSET</c>: Indicates the character set that was used for the
    /// property. <c>(2)</c></summary>
    public string? CharSet
    {
        get => Get<string?>(VCdParam.CharSet);
        set => Set(VCdParam.CharSet, value);
    }

    /// <summary><c>VALUE</c>: Indicates where the actual content of the property 
    /// is located. <c>(2)</c></summary>
    public ContentLocation ContentLocation
    {
        get => Get<ContentLocation>(VCdParam.ContentLocation);
        set
        {
            Set(VCdParam.ContentLocation, value);

            if (value != ContentLocation.Inline)
            {
                DataType = VCdDataType.Uri;
            }
            else if (DataType == VCdDataType.Uri)
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

    /// <summary><c>VALUE</c>: Indicates which of the data types predefined by the vCard
    /// standard the content of the vCard property corresponds to. <c>(3,4)</c></summary>
    public VCdDataType? DataType
    {
        get => Get<VCdDataType?>(VCdParam.DataType);
        set => Set(VCdParam.DataType, value);
    }

    /// <summary> <c>TYPE</c>: Describes the type of an email. <c>(2,3)</c></summary>
    /// <value>Use only the constants defined in the <see cref="EmailType" /> class!</value>
    public string? EmailType
    {
        get => Get<string?>(VCdParam.EmailType);
        set => Set<string?>(VCdParam.EmailType, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }

    /// <summary><c>ENCODING</c>: Indicates the encoding of the VCardProperty. <c>(2,3)</c></summary>
    /// <value>
    /// <para>
    /// The value is set automatically - only for <see cref="NonStandardProperty"/> objects 
    /// this has to be done manually. 
    /// </para>
    /// <para>
    /// In vCard&#160;3.0 only <see cref="ValueEncoding.Base64" /> is permitted. 
    /// </para>
    /// </value>
    public ValueEncoding? Encoding
    {
        get => Get<ValueEncoding?>(VCdParam.Encoding);
        set => Set(VCdParam.Encoding, value);
    }

    /// <summary> <c>LEVEL</c>: A person's level of expertise. <c>(4 - RFC&#160;6715)</c></summary>
    /// <remarks> Used for the property <see cref="VCard.Expertises">VCard.Expertises</see>.
    /// </remarks>
    public ExpertiseLevel? Expertise
    {
        get => Get<ExpertiseLevel?>(VCdParam.Expertise);
        set => Set(VCdParam.Expertise, value);
    }

    /// <summary> <c>GEO</c>: Geographical position. <c>(4)</c></summary>
    /// <remarks> This parameter is only written if it is attached to an
    /// <see cref="AddressProperty" /> object. </remarks>
    public FolkerKinzel.VCards.Models.GeoCoordinate? GeoPosition
    {
        get => Get<FolkerKinzel.VCards.Models.GeoCoordinate?>(VCdParam.GeoPosition);
        set => Set(VCdParam.GeoPosition, value);
    }

    /// <summary><c>INDEX</c>: 1-based index of a property if several instances of the same 
    /// property are allowed. <c>(4 - RFC&#160;6715)</c>
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
    public ImppTypes? InstantMessengerType
    {
        get => Get<ImppTypes?>(VCdParam.InstantMessengerType);
        set => Set(VCdParam.InstantMessengerType, value);
    }

    /// <summary> <c>LEVEL</c>: Degree of interest of a person in a thing. <c>(4 - RFC&#160;6715)</c>
    /// </summary>
    /// <remarks>Used for the properties <see cref="VCard.Hobbies">VCard.Hobbies</see>
    /// and <see cref="VCard.Interests">VCard.Interests</see>.</remarks>
    public InterestLevel? Interest
    {
        get => Get<InterestLevel?>(VCdParam.Interest);
        set => Set(VCdParam.Interest, value);
    }

    /// <summary><c>LABEL</c>: Represents the actual text that should be put on the mailing 
    /// label, when delivering a physical package to the person/object associated with the 
    /// <see cref="VCard"/>. <c>([2],[3],4)</c>
    /// </summary>
    /// <remarks> 
    /// <para>In the vCard standards 2.1 and 3.0, <c>ADR</c> and <c>LABEL</c> are separate 
    /// vCard properties. Only as of vCard&#160;4.0 they are permanently linked to one another. 
    /// When saving a vCard&#160;2.1 and vCard&#160;3.0, the content of this property is 
    /// automatically inserted into the vCard as a separate <c>LABEL</c> property. 
    /// </para>
    /// <note type="warning">
    /// <para>
    /// When loading a vCard&#160;2.1 or 3.0, the library tries to create the link between 
    /// <c>LABEL</c> and <c>ADR</c> based on matching parameters of both properties. A text 
    /// comparison of the content of <c>ADR</c> and <c>LABEL</c> does not take place for 
    /// performance reasons.
    /// </para>
    /// <para>
    /// Although the correct assignment is usually successful in this way, this cannot be 
    /// guaranteed for every scenario. If the assignment of <c>ADR</c> and <c>LABEL</c>
    /// plays an important role for the application, the application should check this in 
    /// the case of vCard&#160;2.1 or vCard&#160;3.0 by text comparison or create its own labels 
    /// from the <see cref="Address"/> data, e.g., using the <see 
    /// cref="AddressProperty.AppendLabel()" /> method.
    /// </para>
    /// </note>
    /// </remarks>
    /// <seealso cref="AddressProperty.AppendLabel" />
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
    /// <remarks> In order to write non-standardized attributes into a VCF file, the 
    /// <see cref="VcfOptions.WriteNonStandardParameters">VcfOptions.WriteNonStandardParameters</see> 
    /// flag must be set explicitly when serializing the <see cref="VCard" /> object.</remarks>
    public IEnumerable<KeyValuePair<string, string>>? NonStandard
    {
        get => Get<IEnumerable<KeyValuePair<string, string>>?>(VCdParam.NonStandard);
        set => Set(VCdParam.NonStandard, value);
    }

    /// <summary><c>PREF</c> or <c>TYPE=PREF</c>: Expresses preference for a property.
    /// <c>(2,3,4)</c></summary>
    /// <value>A value between 1 and 100. 1 means most preferred.</value>
    /// <remarks>
    /// A differentiated preference can only be stated beginning with vCard 4.0. In 
    /// vCard&#160;2.1 and vCard&#160;3.0 only the most preferred property is marked with 
    /// <c>PREF</c>. As the preferred property the one is selected with the lowest 
    /// numerical value for <see cref="Preference" />.
    /// </remarks>
    public int Preference
    {
        get => _propDic.ContainsKey(VCdParam.Preference) ? (int)_propDic[VCdParam.Preference] : PREF_MAX_VALUE;

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
    public PropertyClassTypes? PropertyClass
    {
        get => Get<PropertyClassTypes?>(VCdParam.PropertyClass);
        set
        {
            value = (value == default(PropertyClassTypes)) ? null : value;
            Set(VCdParam.PropertyClass, value);
        }
    }

    /// <summary> <c>PID</c>: <see cref="PropertyID" />s to identify the 
    /// <see cref="VCardProperty" />. <c>(4)</c></summary>
    public IEnumerable<PropertyID?>? PropertyIDs
    {
        get => Get<IEnumerable<PropertyID?>?>(VCdParam.PropertyIDs);
        set => Set(VCdParam.PropertyIDs, value);
    }

    /// <summary> <c>TYPE</c>: Specifies the type of relationship with 
    /// a person. <c>(4)</c></summary>
    /// <remarks> Used in the <see cref="VCard.Relations">VCard.Relations</see> 
    /// property (<c>RELATED</c>).</remarks>
    public RelationTypes? Relation
    {
        get => Get<RelationTypes?>(VCdParam.Relation);
        set
        {
            value = (value == default(RelationTypes)) ? null : value;
            Set(VCdParam.Relation, value);
        }
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
    public IEnumerable<string?>? SortAs
    {
        get => Get<IEnumerable<string?>?>(VCdParam.SortAs);
        set => Set(VCdParam.SortAs, value);
    }

    /// <summary><c>TYPE</c>: Describes a telephone number. <c>(2,3,4)</c></summary>
    public TelTypes? TelephoneType
    {
        get => Get<TelTypes?>(VCdParam.TelephoneType);
        set
        {
            value = (value == default(TelTypes)) ? null : value;
            Set(VCdParam.TelephoneType, value);
        }
    }

    /// <summary><c>TZ</c>: Time zone <c>(4)</c></summary>
    /// <remarks> This parameter is only written if it is attached to an 
    /// <see cref="AddressProperty" /> object.</remarks>
    public TimeZoneID? TimeZone
    {
        get => Get<TimeZoneID?>(VCdParam.TimeZone);
        set => Set(VCdParam.TimeZone, value);
    }
}
