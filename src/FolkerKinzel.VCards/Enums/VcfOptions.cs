using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to specify options for writing VCF files. The constants
/// can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the 
/// <see cref="VcfOptionsExtension" /> class.
/// </note>
/// <para>
/// Die Flags <see cref="WriteWabExtensions" />, <see cref="WriteXExtensions" />,
/// <see cref="WriteEvolutionExtensions" /> und <see cref="WriteKAddressbookExtensions"
/// /> steuern die automatische Verwendung von Non-Standard-Properties. Auch wenn
/// diese Flags gesetzt sind, werden die Non-Standard-Properties nur dann automatisch
/// erzeugt, wenn der gewählte vCard-Standard kein standardisiertes Äquivalent ermöglicht.
/// 
/// The flags <see cref="WriteWabExtensions" />, <see cref="WriteXExtensions" />,  
/// <see cref="WriteEvolutionExtensions" />, and <see cref="WriteKAddressbookExtensions" /> 
/// control the automatic generation of <see cref="NonStandardProperty"/> objects. 
/// Even if these flags are set, the <see cref="NonStandardProperty"/> objects are 
/// only generated automatically, if the selected vCard standard does not allow a 
/// standardized equivalent. 
/// </para>
/// </remarks>
[Flags]
public enum VcfOptions
{
    /// <summary>All flags are set.</summary>
    All = -1,

    /// <summary>All flags are unset.</summary>
    None = 0,

    /// <summary> Default setting (corresponds to  <see cref="WriteGroups" />
    /// | <see cref="WriteRfc6474Extensions" /> | <see cref="WriteRfc6715Extensions" /> 
    /// | <see cref="WriteImppExtension" /> | <see cref="WriteXExtensions" /> 
    /// | <see cref="AllowMultipleAdrAndLabelInVCard21" /> | <see cref="UpdateTimeStamp"/>) </summary>
    Default = WriteGroups | WriteRfc6474Extensions | WriteRfc6715Extensions 
            | WriteImppExtension | WriteXExtensions 
            | AllowMultipleAdrAndLabelInVCard21 | UpdateTimeStamp,

    /// <summary>Set the flag to write property group identifiers.</summary>
    WriteGroups = 1,

    /// <summary>Set the flag to also write empty properties to the vCard.</summary>
    WriteEmptyProperties = 1 << 1,

    /// <summary>Set the flag to append in vCard&#160;2.1 and vCard&#160;3.0 embedded 
    /// <c>AGENT</c>-vCards in the VCF file to the main vCard.</summary>
    AppendAgentAsSeparateVCard = 1 << 2,

    /// <summary> Set the flag to write non-standard parameters to the VCF file.
    /// </summary>
    WriteNonStandardParameters = 1 << 3,

    /// <summary> Set the flag to write <see cref="NonStandardProperty" /> objects
    /// to the VCF file.</summary>
    WriteNonStandardProperties = 1 << 4,

    /// <summary>Set the flag to write the extensions from RFC 6474 (<c>BIRTHPLACE</c>,
    /// <c>DEATHPLACE</c>, <c>DEATHDATE</c>). (Beginning from vCard&#160;4.0.)</summary>
    WriteRfc6474Extensions = 1 << 5,

    /// <summary>Set the flag to write the extensions from RFC 6715 (<c>EXPERTISE</c>,
    /// <c>HOBBY</c>, <c>INTEREST</c>, <c>ORG-DIRECTORY</c>). 
    /// (Beginning from vCard&#160;4.0.)</summary>
    WriteRfc6715Extensions = 1 << 6,

    /// <summary>Set the flag to write the extension <c>IMPP</c> from RFC 4770 in 
    /// vCard&#160;3.0.</summary>
    WriteImppExtension = 1 << 7,

    /// <summary>Set the flag to write the following vCard&#160;properties if necessary:
    /// <c>X-AIM</c>, <c>X-GADUGADU</c>, <c>X-GOOGLE-TALK</c>, <c>X-GTALK</c>, <c>X-ICQ</c>,
    /// <c>X-JABBER</c>, <c>X-MSN</c>, <c>X-SKYPE</c>, <c>X-TWITTER</c>, <c>X-YAHOO</c>,
    /// <c>X-MS-IMADDRESS</c>, <c>X-GENDER</c>, <c>X-ANNIVERSARY</c> and <c>X-SPOUSE</c>.</summary>
    WriteXExtensions = 1 << 8,

    /// <summary>Set the flag to write the following vCard properties if necessary:
    /// <c>X-EVOLUTION-ANNIVERSARY</c>, <c>X-EVOLUTION-SPOUSE</c>.</summary>
    WriteEvolutionExtensions = 1 << 9,

    /// <summary>Set the flag to write the following vCard properties if necessary:
    /// <c>X-KADDRESSBOOK-X-IMAddress</c>, <c>X-KADDRESSBOOK-X-Anniversary</c>, 
    /// <c>X-KADDRESSBOOK-X-SpouseName</c>.</summary>
    WriteKAddressbookExtensions = 1 << 10,

    /// <summary>Set the flag to write the following vCard properties if necessary:
    /// <c>X-WAB-GENDER</c>, <c>X-WAB-WEDDING-ANNIVERSARY</c>, <c>X-WAB-SPOUSE-NAME</c>.</summary>
    WriteWabExtensions = 1 << 11,

    /// <summary>Set the flag to allow multiple "ADR" and "LABEL" properties to be written
    /// into a vCard&#160;2.1.</summary>
    AllowMultipleAdrAndLabelInVCard21 = 1 << 12,

    /// <summary>
    /// Set the flag to update the <see cref="VCard.TimeStamp"/> property automatically when
    /// serializing VCF.
    /// </summary>
    UpdateTimeStamp = 1 << 13,
}
