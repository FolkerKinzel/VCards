namespace FolkerKinzel.VCards.Intls.Enums;

internal enum AdrProp
{
    /// <summary>The post office box. (2,3,4)</summary>
    PostOfficeBox,

    /// <summary>The extended address (e.g., apartment or suite number). (2,3,4)</summary>
    ExtendedAddress,

    /// <summary>The street address. (2,3,4)</summary>
    Street,

    /// <summary>The locality (e.g., city). (2,3,4)</summary>
    Locality,

    /// <summary>The region (e.g., state or province). (2,3,4)</summary>
    Region,

    /// <summary>The postal code. (2,3,4)</summary>
    PostalCode,

    /// <summary>The country name (full name). (2,3,4)</summary>
    Country,

    /// <summary> The room, suite number, or identifier. (4 - RFC 9554)</summary>
    Room,

    /// <summary> The extension designation such as the apartment number, unit, or box number. (4 - RFC 9554)</summary>
    Apartment,

    /// <summary> The floor or level the address is located on. (4 - RFC 9554)</summary>
    Floor,

    /// <summary> The street number, e.g., "123". This value is not restricted to numeric values and can include
    /// any value such as number ranges ("112-10"), grid style ("39.2 RD"), alphanumerics ("N6W23001"), or 
    /// fractionals ("123 1/2"). (4 - RFC 9554)</summary>
    StreetNumber,

    /// <summary> The street name. (4 - RFC 9554)</summary>
    StreetName,

    /// <summary> The building, tower, or condominium the address is located in. (4 - RFC 9554)</summary>
    Building,

    /// <summary> The block name or number. (4 - RFC 9554)</summary>
    Block,


    /// <summary> The subdistrict, ward, or other subunit of a district. (4 - RFC 9554)</summary>
    SubDistrict,

    /// <summary> The district name. (4 - RFC 9554)</summary>
    District,

    /// <summary> The publicly known prominent feature that can substitute the street name and number,
    /// e.g., "White House" or "Taj Mahal". (4 - RFC 9554)</summary>
    Landmark,

    /// <summary> The cardinal direction or quadrant, e.g., "north". (4 - RFC 9554)</summary>
    Direction

}
