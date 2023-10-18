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

    /// <summary />
    internal int Count => _propDic.Count;


    /// <summary> <c>TYPE</c>: Specifies the type of a postal delivery address. <c>(2,3)</c></summary>
    public AddressTypes? AddressType
    {
        get => Get<AddressTypes?>(VCdParam.AddressType);
        set
        {
            value = (value == default(AddressTypes)) ? null : value;
            Set(VCdParam.AddressType, value);
        }
    }


    /// <summary />
    public string? AltID
    {
        get => Get<string?>(VCdParam.AltID);
        set => Set(VCdParam.AltID, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }


    /// <summary />
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


    /// <summary />
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


    /// <summary> <c>CONTEXT</c>: Indicates the context of the data, e.g. <c>VCARD</c>
    /// or <c>LDAP</c>. <c>(3)</c></summary>
    /// <remarks>Is used in the <c>SOURCE</c> property of vCard 3.0.</remarks>
    public string? Context
    {
        get => Get<string?>(VCdParam.Context);
        set => Set<string?>(VCdParam.Context, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }


    /// <summary />
    public VCdDataType? DataType
    {
        get => Get<VCdDataType?>(VCdParam.DataType);
        set => Set(VCdParam.DataType, value);
    }


    /// <summary> <c>TYPE</c>: Describes the type of an email. <c>(2,3)</c></summary>
    /// <value>Verwenden Sie nur die Konstanten der Klasse <see cref="EmailType" />.</value>
    public string? EmailType
    {
        get => Get<string?>(VCdParam.EmailType);
        set => Set<string?>(VCdParam.EmailType, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }


    /// <summary />
    /// <value>
    /// <para>
    /// Der Wert wird automatisch gesetzt - lediglich bei <see cref="NonStandardProperty"
    /// />-Objekten muss dies manuell erfolgen.
    /// </para>
    /// <para>
    /// In vCard 3.0 ist nur <see cref="ValueEncoding.Base64" /> gestattet.
    /// </para>
    /// </value>
    public ValueEncoding? Encoding
    {
        get => Get<ValueEncoding?>(VCdParam.Encoding);
        set => Set(VCdParam.Encoding, value);
    }


    /// <summary> <c>LEVEL</c>: Grad der Sachkenntnis einer Person. (Für die Eigenschaft
    /// <see cref="VCard.Expertises">VCard.Expertises</see>.) <c>(4 - RFC 6715)</c></summary>
    public ExpertiseLevel? ExpertiseLevel
    {
        get => Get<ExpertiseLevel?>(VCdParam.ExpertiseLevel);
        set => Set(VCdParam.ExpertiseLevel, value);
    }


    /// <summary> <c>GEO</c>: Geographical position. <c>(4)</c></summary>
    /// <remarks> Dieser Parameter wird nur geschrieben, wenn er an ein <see cref="AddressProperty"
    /// />-Objekt angehängt ist. </remarks>
    public FolkerKinzel.VCards.Models.GeoCoordinate? GeoPosition
    {
        get => Get<FolkerKinzel.VCards.Models.GeoCoordinate?>(VCdParam.GeoPosition);
        set => Set(VCdParam.GeoPosition, value);
    }


    /// <summary />
    public ImppTypes? InstantMessengerType
    {
        get => Get<ImppTypes?>(VCdParam.InstantMessengerType);
        set => Set(VCdParam.InstantMessengerType, value);
    }

    /// <summary />
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


    /// <summary> <c>LEVEL</c>: Grad des Interesses einer Person für eine Sache. (Für
    /// die Eigenschaften <see cref="VCard.Hobbies">VCard.Hobbies</see> und <see cref="VCard.Interests">VCard.Interests</see>.)
    /// <c>(4 - RFC 6715)</c></summary>
    public InterestLevel? InterestLevel
    {
        get => Get<InterestLevel?>(VCdParam.InterestLevel);
        set => Set(VCdParam.InterestLevel, value);
    }


    /// <summary />
    /// <remarks> Im vCard-Standard 2.1 und 3.0 sind <c>ADR</c> und <c>Label</c> separate
    /// vCard-Properties. Erst ab vCard 4.0 sind sie fest miteinander verknüpft. Beim
    /// Speichern einer vCard 2.1 und vCard 3.0 wird der Inhalt dieser Eigenschaft automatisch
    /// als separate <c>LABEL</c>-Property in die vCard eingefügt.
    /// <note type="warning">
    /// <para>
    /// Beim Einlesen einer vCard 2.1 oder 3.0 versucht die Library, die Verküpfung
    /// zwischen <c>Label</c> und <c>ADR</c> aufgrund von Übereinstimmungen der Parameter
    /// beider Properties herzustellen. Ein Textvergleich der Inhalte von <c>ADR</c>
    /// und <c>Label</c> findet aber aus Performancegründen nicht statt.
    /// </para>
    /// <para>
    /// Obwohl die korrekte Zuordnung auf diese Weise meist gelingt, kann dies nicht
    /// für jedes Szenario garantiert werden. Wenn die Zuordnung von <c>ADR</c> und
    /// <c>Label</c> für die Anwendung eine Rolle spielt, sollte die Anwendung diese
    /// im Falle von vCard 2.1 oder vCard 3.0 durch Textvergleich überprüfen oder mit
    /// der Methode <see cref="AddressProperty.AppendLabel()" /> aus den Adressdaten
    /// eigene Labels erstellen.
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


    /// <summary />
    /// <value>An IETF Language Tag as defined in Section 2 of RFC 5646.</value>
    public string? Language
    {
        get => Get<string?>(VCdParam.Language);
        set => Set<string?>(VCdParam.Language, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }


    /// <summary />
    /// <value>MIME type according to RFC 2046.</value>
    public string? MediaType
    {
        get => Get<string?>(VCdParam.MediaType);
        set => Set<string?>(VCdParam.MediaType, value);
    }

    /// <summary>Non-standard attributes. <c>(2,3,4)</c></summary>
    /// <remarks> Um nicht-standardisierte Attribute in eine vCard zu schreiben, muss
    /// beim Serialisieren des <see cref="VCard" />-Objekts das Flag <see cref="VcfOptions.WriteNonStandardParameters">VcfOptions.WriteNonStandardParameters</see>
    /// explizit gesetzt werden. </remarks>
    public IEnumerable<KeyValuePair<string, string>>? NonStandardParameters
    {
        get => Get<IEnumerable<KeyValuePair<string, string>>?>(VCdParam.NonStandard);
        set => Set(VCdParam.NonStandard, value);
    }


    /// <summary />
    /// <value>A value between 1 and 100. 1 means most preferred.</value>
    /// <remarks>Erst ab vCard 4.0 kann eine differenzierte Beliebtheit angegeben werden.
    /// In vCard 2.1 und vCard 3.0 wird lediglich die beliebteste Property markiert.
    /// Als beliebteste Property wird diejenige ausgewählt, deren Zahlenwert für <see
    /// cref="Preference" /> am geringsten ist.</remarks>
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

            // Diskussion ein Wert von 100 (PREF_MAX_VALUE) wird nun nicht mehr gespeichert:
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


    /// <summary> <c> PID </c>: <see cref="PropertyID" />s to identify the <see cref="VCardProperty"
    /// />. <c> (4) </c></summary>
    public IEnumerable<PropertyID?>? PropertyIDs
    {
        get => Get<IEnumerable<PropertyID?>?>(VCdParam.PropertyIDs);
        set => Set(VCdParam.PropertyIDs, value);
    }



    /// <summary> <c>TYPE</c>: Bestimmt in einer <see cref="RelationProperty" /> (<c>RELATED</c>)
    /// die Art der Beziehung zu einer Person. <c>(4)</c></summary>
    public RelationTypes? RelationType
    {
        get => Get<RelationTypes?>(VCdParam.RelationType);
        set
        {
            value = (value == default(RelationTypes)) ? null : value;
            Set(VCdParam.RelationType, value);
        }
    }

    /// <summary />
    /// <example>
    /// <code>
    /// FN:Rene van der Harten N;SORT-AS="Harten,Rene":van der Harten;Rene,J.;Sir;R.D.O.N.
    /// </code>
    /// </example>
    /// <remarks>When serializing a file as vCard 3.0, a separate <c>SORT-STRING</c>-property,
    /// which contains the first <see cref="string" />, is automatically inserted into
    /// the vCard.</remarks>
    public IEnumerable<string?>? SortAs
    {
        get => Get<IEnumerable<string?>?>(VCdParam.SortAs);
        set => Set(VCdParam.SortAs, value);
    }


    /// <summary />
    public TelTypes? TelephoneType
    {
        get => Get<TelTypes?>(VCdParam.TelephoneType);
        set
        {
            value = (value == default(TelTypes)) ? null : value;
            Set(VCdParam.TelephoneType, value);
        }
    }


    /// <summary />
    /// <remarks> Dieser Parameter wird nur geschrieben, wenn er an ein <see cref="AddressProperty"
    /// />-Objekt angehängt ist. </remarks>
    public TimeZoneID? TimeZone
    {
        get => Get<TimeZoneID?>(VCdParam.TimeZone);
        set => Set(VCdParam.TimeZone, value);
    }

}
