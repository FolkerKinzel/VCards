namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    private enum VCdParam
    {
    /// <summary> <c>CONTEXT</c>: Indicates the context of the data, e.g. <c>VCARD</c>
    /// or <c>LDAP</c>. <c>(3)</c></summary>
        Context,

    /// <summary> <c>TYPE</c>: Indicates, wether a <see cref="VCardProperty" /> is related
    /// to an individual's work place or to an individual's personal life. <c>(2,3,4)</c></summary>
        PropertyClass,

    /// <summary> <c>TYPE</c>: Bestimmt in einer <see cref="RelationProperty" /> (<c>RELATED</c>)
    /// die Art der Beziehung zu einer Person. <c>(4)</c></summary>
        RelationType,

    /// <summary> <c>TYPE</c>: Specifies the type of a postal delivery address. <c>(2,3)</c></summary>
        AddressType,

    /// <summary> <c>TYPE</c>: Describes the type of an email. <c>(2,3)</c></summary>
        EmailType,

    /// <summary />
        TelephoneType,

    /// <summary />
        ExpertiseLevel,

    /// <summary />
        InstantMessengerType,

    /// <summary />
        InterestLevel,

    /// <summary />
        Label,

    /// <summary />
        Preference,

    /// <summary> <c>CHARSET</c>: Indicates the character set that was used for the
    /// property. <c>(2)</c></summary>
        CharSet,

    /// <summary />
        Encoding,

    /// <summary />
        Language,

    /// <summary />
        DataType,

    /// <summary />
        ContentLocation,

    /// <summary />
        MediaType,

    /// <summary> <c>GEO</c>: Geographical position. <c>(4)</c></summary>
        GeoPosition,

    /// <summary />
        TimeZone,

    /// <summary />
        Calendar,

    /// <summary />
        SortAs,

    /// <summary />
        NonStandard,

    /// <summary />
        PropertyIDs,

    /// <summary />
        AltID,

    /// <summary />
        Index
    }
}
