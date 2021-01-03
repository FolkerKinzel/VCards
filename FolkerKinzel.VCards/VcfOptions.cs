using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;

namespace FolkerKinzel.VCards
{
    /// <summary>
    /// Benannte Konstanten, um Optionen für das Schreiben von VCF-Dateien zu bestimmen. Die Konstanten sind kombinierbar.
    /// </summary>
    /// <remarks>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.VcfOptionsExtension"/>.</note>
    /// <para>Die Flags <see cref="WriteWabExtensions"/>,
    /// <see cref="WriteXExtensions"/>, <see cref="WriteEvolutionExtensions"/> und <see cref="WriteKAddressbookExtensions"/>
    /// steuern die automatische Verwendung von Non-Standard-Properties. Auch wenn diese Flags gesetzt sind,
    /// werden die Non-Standard-Properties nur dann automatisch erzeugt, wenn der gewählte vCard-Standard kein standardisiertes
    /// Äquivalent ermöglicht.</para>
    /// </remarks>
    [Flags]
    public enum VcfOptions
    {
        /// <summary>
        /// Alle Flags sind gesetzt.
        /// </summary>
        All = -1,

        /// <summary>
        /// Alle Flags sind ausgeschaltet.
        /// </summary>
        None = 0,

        /// <summary>
        /// Standardeinstellung (entspricht <see cref="WriteGroups"/> | <see cref="WriteRfc6474Extensions"/>
        /// | <see cref="WriteRfc6715Extensions"/> | <see cref="WriteImppExtension"/> | <see cref="WriteXExtensions"/>)
        /// </summary>
        Default = WriteGroups | WriteRfc6474Extensions | WriteRfc6715Extensions | WriteImppExtension | WriteXExtensions,

        /// <summary>
        /// Flag setzen, um Property-Gruppenbezeichner zu schreiben.
        /// </summary>
        WriteGroups = 1,


        /// <summary>
        /// Flag setzen, um auch leere Properties in die vCard zu schreiben.
        /// </summary>
        WriteEmptyProperties = 1 << 1,


        /// <summary>
        /// Flag setzen, um in vCard 2.1 und vCard 3.0 eingebettete
        /// <c>AGENT</c>-vCards zusätzlich an die Haupt-vCard anzuhängen.
        /// </summary>
        IncludeAgentAsSeparateVCard = 1 << 2,


        /// <summary>
        /// Flag setzen, um Non-Standard-Parameter der Eigenschaft <see cref="ParameterSection.NonStandardParameters">ParameterSection.NonStandardParameters</see>
        /// zu schreiben.
        /// </summary>
        WriteNonStandardParameters = 1 << 3,


        /// <summary>
        /// Flag setzen, um <see cref="NonStandardProperty"/>-Objekte zu serialisieren, die in der Eigenschaft <see cref="VCard.NonStandardProperties">VCard.NonStandardProperties</see>
        /// gespeichert sind.
        /// </summary>
        WriteNonStandardProperties = 1 << 4,

        /// <summary>
        /// Flag setzen, um in vCard 4.0 die Erweiterungen aus RFC 6474 zu schreiben (<c>BIRTHPLACE</c>, <c>DEATHPLACE</c>, <c>DEATHDATE</c>).
        /// </summary>
        WriteRfc6474Extensions = 1 << 5,

        /// <summary>
        /// Flag setzen, um in vCard 4.0 die Erweiterungen aus RFC 6715 zu schreiben (<c>EXPERTISE</c>, <c>HOBBY</c>, <c>INTEREST</c>, <c>ORG-DIRECTORY</c>).
        /// </summary>
        WriteRfc6715Extensions = 1 << 6,

        /// <summary>
        /// Flag setzen, um in vCard 3.0 die Erweiterung <c>IMPP</c> aus RFC 4770 zu schreiben.
        /// </summary>
        WriteImppExtension = 1 << 7,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: 
        /// <c>X-AIM</c>, <c>X-GADUGADU</c>, <c>X-GOOGLE-TALK</c>, <c>X-GTALK</c>, <c>X-ICQ</c>, <c>X-JABBER</c>, <c>X-MSN</c>, <c>X-SKYPE</c>, <c>X-TWITTER</c>,
        /// <c>X-YAHOO</c>, <c>X-MS-IMADDRESS</c>, <c>X-GENDER</c>, <c>X-ANNIVERSARY</c> und <c>X-SPOUSE</c>.
        /// </summary>
        WriteXExtensions = 1 << 8,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: <c>X-EVOLUTION-ANNIVERSARY</c>, <c>X-EVOLUTION-SPOUSE</c>.
        /// </summary>
        WriteEvolutionExtensions = 1 << 9,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben:
        /// <c>X-KADDRESSBOOK-X-IMAddress</c>, <c>X-KADDRESSBOOK-X-Anniversary</c>, <c>X-KADDRESSBOOK-X-SpouseName</c>.
        /// </summary>
        WriteKAddressbookExtensions = 1 << 10,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben:
        /// <c>X-WAB-GENDER</c>, <c>X-WAB-WEDDING-ANNIVERSARY</c>, <c>X-WAB-SPOUSE-NAME</c>.
        /// </summary>
        WriteWabExtensions = 1 << 11
    }
}
