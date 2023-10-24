using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>Named constants to describe the type of a phone number. The constants can be 
/// combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the <see
/// cref="PhoneTypesExtension" /> class. 
/// </note>
/// </remarks>
[Flags]
public enum PhoneTypes
{
    // CAUTION: If the enum is expanded, PhoneTypesConverter and
    // EnumValueCollector must be adjusted!

    /// <summary> <c>VOICE</c>: Indicates a voice telephone number. (Default). <c>(2,3,4)</c></summary>
    Voice = 1,

    /// <summary> <c>FAX</c>: Indicates a facsimile telephone number. <c>(2,3,4)</c></summary>
    Fax = 1 << 1,

    /// <summary> <c>MSG</c>: Indicates that the telephone number has voice messaging
    /// support. <c>(2,3)</c></summary>
    Msg = 1 << 2,

    /// <summary> <c>CELL</c>: Indicates a cellular or mobile telephone number. <c>(2,3,4)</c></summary>
    Cell = 1 << 3,

    /// <summary> <c>PAGER</c>: Indicates a paging device telephone number. <c>(2,3,4)</c></summary>
    Pager = 1 << 4,

    /// <summary> <c>BBS</c>: Indicates a bulletin board system telephone number. <c>(2,3)</c></summary>
    BBS = 1 << 5,

    /// <summary> <c>MODEM</c>: Indicates a MODEM connected telephone number. <c>(2,3)</c></summary>
    Modem = 1 << 6,

    /// <summary> <c>CAR</c>: Indicates a car-phone telephone number.<c>(2,3)</c></summary>
    Car = 1 << 7,

    /// <summary> <c>ISDN</c>: Indicates an ISDN service telephone number. <c>(2,3)</c></summary>
    ISDN = 1 << 8,

    /// <summary> <c>VIDEO</c>: Indicates a video conferencing telephone number. <c>(2,3,4)</c></summary>
    Video = 1 << 9,

    /// <summary> <c>PCS</c>: Indicates a personal communication services telephone
    /// number. <c>(3)</c></summary>
    PCS = 1 << 10, // nur vCard 3.0

    /// <summary> <c>TEXTPHONE</c>: Indicates a telecommunication device for people
    /// with hearing or speech difficulties. <c>(4)</c></summary>
    TextPhone = 1 << 11,

    /// <summary> <c>TEXT</c>: Indicates that the telephone number supports text messages
    /// (SMS). <c>(4)</c></summary>
    Text = 1 << 12
}
