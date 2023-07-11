using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Kapselt die Information über die Parameter einer vCard-Property.
/// </summary>
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
    /// Anzahl der gespeicherten Parameter (zum Testen und Debuggen)
    /// </summary>
    internal int Count => _propDic.Count;


    /// <summary>
    /// <c>TYPE</c>: Beschreibt die Art einer Adresse. <c>(2,3)</c>
    /// </summary>
    public AddressTypes? AddressType
    {
        get => Get<AddressTypes?>(VCdParam.AddressType);
        set
        {
            value = (value == default(AddressTypes)) ? null : value;
            Set(VCdParam.AddressType, value);
        }
    }


    /// <summary>
    /// <c>ALTID</c>: Ein gemeinsamer Bezeichner, der zu erkennen gibt, dass mehrere Instanzen derselben Property dasselbe 
    /// darstellen (z.B. in unterschiedlichen Sprachen). <c>(4)</c>
    /// </summary>
    public string? AltID
    {
        get => Get<string?>(VCdParam.AltID);
        set => Set(VCdParam.AltID, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }


    /// <summary>
    /// <c>CALSCALE</c>: Gibt die Art des Kalenders an, der für Datumsangaben verwendet wird. <c>(4)</c>
    /// </summary>
    /// <value>Der einzige offiziell registrierte Wert ist <c>GREGORIAN</c> für den gregorianischen 
    /// Kalender.</value>
    public string? Calendar
    {
        get => Get<string?>(VCdParam.Calendar);
        set => Set<string?>(VCdParam.Calendar, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary>
    /// <c>CHARSET</c>: Gibt den Zeichensatz an, der für die Property verwendet wurde. <c>(2)</c>
    /// </summary>
    [Obsolete("Use CharSet instead.", true)]
    [CLSCompliant(false)]
    public string? Charset
    {
        get => CharSet;
        set => CharSet = value;
    }

    /// <summary>
    /// <c>CHARSET</c>: Gibt den Zeichensatz an, der für die Property verwendet wurde. <c>(2)</c>
    /// </summary>
    public string? CharSet
    {
        get => Get<string?>(VCdParam.CharSet);
        set => Set(VCdParam.CharSet, value);
    }


    /// <summary>
    /// <c>VALUE</c>: Gibt an, wo sich der eigentiche Inhalt der Property befindet. <c>(2)</c>
    /// </summary>
    public VCdContentLocation ContentLocation
    {
        get => Get<VCdContentLocation>(VCdParam.ContentLocation);
        set
        {
            Set(VCdParam.ContentLocation, value);

            if (value != VCdContentLocation.Inline)
            {
                DataType = VCdDataType.Uri;
            }
        }
    }


    /// <summary>
    /// <c>CONTEXT</c>: Gibt den Kontext der Daten an, z.B. <c>VCARD</c> oder <c>LDAP</c>. <c>(3)</c>
    /// </summary>
    /// <remarks>Kommt in der <c>SOURCE</c>-Property von vCard 3.0 zum Einsatz.</remarks>
    public string? Context
    {
        get => Get<string?>(VCdParam.Context);
        set => Set<string?>(VCdParam.Context, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }


    /// <summary>
    /// <c>VALUE</c>: Gibt an, welchem der vom vCard-Standard vordefinierten Datentypen der Inhalt
    /// der vCard-Property entspricht. <c>(3,4)</c>
    /// </summary>
    public VCdDataType? DataType
    {
        get => Get<VCdDataType?>(VCdParam.DataType);
        set => Set(VCdParam.DataType, value);
    }


    /// <summary>
    /// <c>TYPE</c>: Beschreibt die Art einer E-Mail. <c>(2,3)</c>
    /// </summary>
    /// <value>Verwenden Sie nur die Konstanten der Klasse
    /// <see cref="EmailType"/>.</value>
    public string? EmailType
    {
        get => Get<string?>(VCdParam.EmailType);
        set => Set<string?>(VCdParam.EmailType, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
    }


    /// <summary>
    /// <c>ENCODING</c>: Gibt die Encodierung der <see cref="VCardProperty"/> an. <c>(2,3)</c>
    /// </summary>
    /// <value>
    /// <para>Der Wert wird automatisch gesetzt - 
    /// lediglich bei <see cref="NonStandardProperty"/>-Objekten muss dies manuell erfolgen.</para>
    /// <para>In vCard 3.0 ist nur <see cref="VCdEncoding.Base64"/> gestattet.</para> 
    /// </value>
    public VCdEncoding? Encoding
    {
        get => Get<VCdEncoding?>(VCdParam.Encoding);
        set => Set(VCdParam.Encoding, value);
    }


    /// <summary>
    /// <c>LEVEL</c>: Grad der Sachkenntnis einer Person. (Für die Eigenschaft <see cref="VCard.Expertises">VCard.Expertises</see>.) <c>(4 - RFC 6715)</c>
    /// </summary>
    public ExpertiseLevel? ExpertiseLevel
    {
        get => Get<ExpertiseLevel?>(VCdParam.ExpertiseLevel);
        set => Set(VCdParam.ExpertiseLevel, value);
    }


    /// <summary>
    /// <c>GEO</c>: Geografische Position. <c>(4)</c>
    /// </summary>
    /// <remarks>
    /// Dieser Parameter wird nur geschrieben, wenn er an ein <see cref="AddressProperty"/>-Objekt
    /// angehängt ist.
    /// </remarks>
    public FolkerKinzel.VCards.Models.GeoCoordinate? GeoPosition
    {
        get => Get<FolkerKinzel.VCards.Models.GeoCoordinate?>(VCdParam.GeoPosition);
        set => Set(VCdParam.GeoPosition, value);
    }


    /// <summary>
    /// <c>TYPE</c>: Nähere Beschreibung einer Instant-Messenger-Adresse. <c>(3 - RFC 4770)</c>
    /// </summary>
    public ImppTypes? InstantMessengerType
    {
        get => Get<ImppTypes?>(VCdParam.InstantMessengerType);
        set => Set(VCdParam.InstantMessengerType, value);
    }

    /// <summary>
    /// <c>INDEX</c>: 1-basierter Index einer Property, wenn mehrere Instanzen derselben Property möglich sind. <c>(4 - RFC 6715)</c>
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


    /// <summary>
    /// <c>LEVEL</c>: Grad des Interesses einer Person für eine Sache. (Für die Eigenschaften 
    /// <see cref="VCard.Hobbies">VCard.Hobbies</see> und <see cref="VCard.Interests">VCard.Interests</see>.) <c>(4 - RFC 6715)</c>
    /// </summary>
    public InterestLevel? InterestLevel
    {
        get => Get<InterestLevel?>(VCdParam.InterestLevel);
        set => Set(VCdParam.InterestLevel, value);
    }


    /// <summary>
    /// <c>LABEL</c>: Gibt die formatierte Textdarstellung einer Adresse an. <c>([2],[3],4)</c>
    /// </summary>
    /// <remarks>
    /// Im vCard-Standard 2.1 und 3.0 sind <c>ADR</c> und <c>Label</c> separate vCard-Properties. Erst ab vCard 4.0 sind sie fest 
    /// miteinander verknüpft.
    /// Beim Speichern einer vCard 2.1 und vCard 3.0 wird der Inhalt dieser Eigenschaft automatisch als separate <c>LABEL</c>-Property in die 
    /// vCard eingefügt.
    /// 
    /// <note type="warning">
    /// <para>
    /// Beim Einlesen einer vCard 2.1 oder 3.0 versucht die Library, die Verküpfung zwischen <c>Label</c> und <c>ADR</c> aufgrund
    /// von Übereinstimmungen der Parameter beider Properties herzustellen. Ein Textvergleich der Inhalte von <c>ADR</c> und <c>Label</c>
    /// findet aber aus Performancegründen nicht statt.
    /// </para>
    /// <para>
    /// Obwohl die korrekte Zuordnung auf diese Weise meist gelingt, kann dies nicht für jedes Szenario garantiert werden. Wenn die 
    /// Zuordnung von <c>ADR</c> und <c>Label</c> für die Anwendung eine Rolle spielt, sollte die Anwendung diese im Falle von vCard 2.1 
    /// oder vCard 3.0 durch 
    /// Textvergleich überprüfen oder mit der Methode <see cref="AddressProperty.AppendLabel()"/> aus den Adressdaten eigene Labels erstellen.
    /// </para>
    /// </note>
    /// </remarks>
    /// <seealso cref="AddressProperty.AppendLabel"/>
    public string? Label
    {
        get => Get<string?>(VCdParam.Label);
        set
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value;
            Set(VCdParam.Label, value);
        }
    }


    /// <summary>
    /// <c>LANGUAGE</c>: Sprache der <see cref="VCardProperty"/>. <c>(2,3,4)</c>
    /// </summary>
    /// <value>Ein IETF-Language-Tag wie in Section 2 von RFC 5646 definiert.</value>
    public string? Language
    {
        get => Get<string?>(VCdParam.Language);
        set => Set<string?>(VCdParam.Language, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }


    /// <summary>
    /// <c>MEDIATYPE</c>: Gibt bei URIs den MIME-Typ der Daten an, auf den der URI verweist (z.B. <c>text/plain</c>). <c>(4)</c>
    /// </summary>
    /// <value>
    /// MIME-Typ entsprechend RFC 2046.
    /// </value>
    public string? MediaType
    {
        get => Get<string?>(VCdParam.MediaType);
        set => Set<string?>(VCdParam.MediaType, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
    }

    /// <summary>
    /// Nichtstandardisierte Attribute. <c>(2,3,4)</c>
    /// </summary>
    /// <remarks>
    /// Um nicht-standardisierte Attribute in eine vCard zu schreiben, muss beim Serialisieren des 
    /// <see cref="VCard"/>-Objekts das Flag <see cref="VcfOptions.WriteNonStandardParameters">VcfOptions.WriteNonStandardParameters</see> 
    /// explizit gesetzt werden.
    /// </remarks>
    public IEnumerable<KeyValuePair<string, string>>? NonStandardParameters
    {
        get => Get<IEnumerable<KeyValuePair<string, string>>?>(VCdParam.NonStandard);
        set => Set(VCdParam.NonStandard, value);
    }


    /// <summary>
    /// <c>PREF</c> oder <c>TYPE=PREF</c>: Drückt die Beliebtheit einer Property aus. <c>(2,3,4)</c>
    /// </summary>
    /// <value>Ein Wert zwischen 1 und 100. 1 bedeutet 
    /// am beliebtesten.</value>
    /// <remarks>Erst ab vCard 4.0 kann eine differenzierte Beliebtheit angegeben werden. In vCard 2.1 und vCard 3.0
    /// wird lediglich die beliebteste Property markiert. Als beliebteste Property wird diejenige ausgewählt,
    /// deren Zahlenwert für <see cref="Preference"/> am geringsten ist.</remarks>
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


    /// <summary>
    /// <c>TYPE</c>: Klassifiziert eine <see cref="VCardProperty"/> als dienstlich und / oder privat. <c>(2,3,4)</c>
    /// </summary>
    public PropertyClassTypes? PropertyClass
    {
        get => Get<PropertyClassTypes?>(VCdParam.PropertyClass);
        set
        {
            value = (value == default(PropertyClassTypes)) ? null : value;
            Set(VCdParam.PropertyClass, value);
        }
    }


    /// <summary>
    /// <c>PID</c>: <see cref="PropertyID"/>s zur Identifizierung der <see cref="VCardProperty"/>. <c>(4)</c>
    /// </summary>
    public IEnumerable<PropertyID?>? PropertyIDs
    {
        get => Get<IEnumerable<PropertyID?>?>(VCdParam.PropertyIDs);
        set => Set(VCdParam.PropertyIDs, value);
    }



    /// <summary>
    /// <c>TYPE</c>: Bestimmt in einer <see cref="RelationProperty"/> (<c>RELATED</c>) die Art der Beziehung zu einer Person. <c>(4)</c>
    /// </summary>
    public RelationTypes? RelationType
    {
        get => Get<RelationTypes?>(VCdParam.RelationType);
        set
        {
            value = (value == default(RelationTypes)) ? null : value;
            Set(VCdParam.RelationType, value);
        }
    }

    /// <summary>
    /// <c>SORT-AS</c>:&#160;<see cref="string"/>s (case-sensitiv!), die die Sortierreihenfolge festlegen. (Maximal so viele, wie Felder der 
    /// zusammengesetzten Property!) <c>([3],4)</c>
    /// </summary>
    /// <example>
    /// <code>
    /// FN:Rene van der Harten
    /// N;SORT-AS="Harten,Rene":van der Harten;Rene,J.;Sir;R.D.O.N.
    /// </code>
    /// </example>
    /// <remarks>
    /// In vCard 3.0 wird automatisch eine separate <c>SORT-STRING</c>-Property eingefügt, in die lediglich der erste <see cref="string"/>
    /// übernommen wird.
    /// </remarks>
    public IEnumerable<string?>? SortAs
    {
        get => Get<IEnumerable<string?>?>(VCdParam.SortAs);
        set => Set(VCdParam.SortAs, value);
    }


    /// <summary>
    /// <c>TYPE</c>: Beschreibt die Art einer Telefonnummer. <c>(2,3,4)</c>
    /// </summary>
    public TelTypes? TelephoneType
    {
        get => Get<TelTypes?>(VCdParam.TelephoneType);
        set
        {
            value = (value == default(TelTypes)) ? null : value;
            Set(VCdParam.TelephoneType, value);
        }
    }


    /// <summary>
    /// <c>TZ</c>: Zeitzone <c>(4)</c>
    /// </summary>
    /// <remarks>
    /// Dieser Parameter wird nur geschrieben, wenn er an ein <see cref="AddressProperty"/>-Objekt
    /// angehängt ist.
    /// </remarks>
    public TimeZoneID? TimeZone
    {
        get => Get<TimeZoneID?>(VCdParam.TimeZone);
        set => Set(VCdParam.TimeZone, value);
    }

}
