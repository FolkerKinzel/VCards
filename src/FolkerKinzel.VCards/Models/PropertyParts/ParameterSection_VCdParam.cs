using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    private enum VCdParam
    {
        /// <summary> <c>CONTEXT</c>: Indicates the context of the data. <c>(3)</c></summary>
        Context,

        /// <summary> <c>TYPE</c>: Indicates, wether a <see cref="VCardProperty" /> is related
        /// to an individual's work place or to an individual's personal life. <c>(2,3,4)</c></summary>
        PropertyClass,

        /// <summary> <c>TYPE</c>: Specifies the type of relationship with 
        /// a person. <c>(4)</c></summary>
        RelationType,

        /// <summary> <c>TYPE</c>: Specifies the type of a postal delivery address. 
        /// <c>(2,3)</c></summary>
        AddressType,

        /// <summary> <c>TYPE</c>: Describes the type of an e-mail address. <c>(2,3)</c></summary>
        EMailType,

        /// <summary><c>TYPE</c>: Describes a telephone number. <c>(2,3,4)</c></summary>
        PhoneType,

        /// <summary> <c>LEVEL</c>: A person's level of expertise. <c>(4 - RFC&#160;6715)</c></summary>
        Expertise,

        /// <summary>
        /// <c>SERVICE-TYPE</c>: Defines the online service name associated with a messaging or
        /// social media profile. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        ServiceType,

        /// <summary>
        /// <c>USERNAME</c>: Defines a username such as the user of a messaging or 
        /// social media service. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        UserName,

        /// <summary><c>TYPE</c>: Description of an instant messenger address. <c>(3 - RFC&#160;4770)</c>
        /// </summary>
        InstantMessengerType,

        /// <summary> <c>LEVEL</c>: Degree of interest of a person in a thing. <c>(4 - RFC&#160;6715)</c>
        /// </summary>
        Interest,

        /// <summary><c>LABEL</c>: Represents the actual text that should be put on the mailing 
        /// label, when delivering a physical package to the person/object associated with the 
        /// <see cref="VCard"/>. <c>([2],[3],4)</c>
        /// </summary>
        Label,

        /// <summary><c>PREF</c> or <c>TYPE=PREF</c>: Expresses preference for a property.
        /// <c>(2,3,4)</c></summary>
        Preference,

        /// <summary> <c>CHARSET</c>: Indicates the character set that was used for the
        /// property. <c>(2)</c></summary>
        CharSet,

        /// <summary><c>ENCODING</c>: Indicates the encoding of the VCardProperty. <c>(2,3)</c></summary>
        Encoding,

        /// <summary><c>LANGUAGE</c>: Language of the <see cref="VCardProperty.Value"/> of
        /// the <see cref="VCardProperty"/>. <c>(2,3,4)</c></summary>
        Language,

        /// <summary>
        /// <c>PHONETIC</c>: Defines how to pronounce the value of another property in the same vCard. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        Phonetic,

        /// <summary>
        /// <c>SCRIPT</c>: Defines the script that a <see cref="VCardProperty.Value"/> is written in. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        Script,

        /// <summary><c>VALUE</c>: Indicates which of the data types predefined by the vCard
        /// standard the content of the vCard property corresponds to. <c>(3,4)</c></summary>
        DataType,

        /// <summary><c>VALUE</c>: Indicates where the actual content of the property 
        /// is located. <c>(2)</c></summary>
        ContentLocation,

        /// <summary>MEDIATYPE : Specifies the MIME type for the data to which a 
        /// URI refers. <c>(4)</c></summary>
        MediaType,

        /// <summary> <c>GEO</c>: Geographical position. <c>(4)</c></summary>
        GeoPosition,

        /// <summary><c>TZ</c>: Time zone <c>(4)</c></summary>
        TimeZone,

        /// <summary><c>CALSCALE</c>: It is used to define the calendar system in which 
        /// a date or date-time value is expressed. <c>(4)</c></summary>
        Calendar,

        /// <summary><c>SORT-AS</c>: Determines the sort order. <c>([3],4)</c></summary>
        SortAs,

        /// <summary>Non-standard attributes. <c>(2,3,4)</c></summary>
        NonStandard,

        /// <summary>
        /// <c>PROP-ID</c>: Identifies a property among all its siblings of the same property name. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        PropertyID,

        /// <summary> <c>PID</c>: <see cref="PropertyID" />s to identify the 
        /// <see cref="VCardProperty" />. <c>(4)</c></summary>
        PropertyIDs,

        /// <summary><c>ALTID</c>: A common identifier that indicates, that several instances of 
        /// the same property represent the same (e.g. in different languages). <c>(4)</c></summary>
        AltID,

        /// <summary><c>INDEX</c>: 1-based index of a property if several instances of the same 
        /// property are allowed. <c>(4 - RFC&#160;6715)</c>
        /// </summary>
        Index,

        /// <summary>
        /// <c>CC</c>: ISO&#160;3166 two-character country code. <c>(4 - RFC&#160;8605)</c>
        /// </summary>
        CountryCode,

        /// <summary>
        /// <c>AUTHOR</c>: Identifies the author of the associated <see cref="VCardProperty.Value"/>. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        Author,

        /// <summary>
        /// <c>AUTHOR-NAME</c>: Names the author of the associated <see cref="VCardProperty.Value"/>. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        AuthorName,

        /// <summary>
        /// <c>CREATED</c>: Defines the date and time when a <see cref="VCardProperty"/> was created. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        Created,

        /// <summary>
        /// <c>DERIVED</c>: Specifies that the value of the associated <see cref="VCardProperty"/> is derived from some other 
        /// <see cref="VCardProperty"/> values in the same <see cref="VCard"/>. <c>(4 - RFC&#160;9554)</c>
        /// </summary>
        Derived,

        /// <summary>
        /// <c>JSCOMPS</c>: Defines the order and separators for the elements of a structured property value. <c>(4 - RFC&#160;9555)</c>
        /// </summary>
        ComponentOrder,

        /// <summary>
        /// <c>JSPTR</c>: This parameter is used with the instances in <see cref="VCard.JSContactProps"/>. Its value
        /// points to the JSContact property whose value is stored in the <see cref="VCardProperty"/>. <c>(4 - RFC&#160;9555)</c>
        /// </summary>
        JSContactPointer,
    }
}
