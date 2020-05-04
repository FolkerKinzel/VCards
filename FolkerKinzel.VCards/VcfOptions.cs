using FolkerKinzel.VCards.Models.PropertyParts;
using System;

namespace FolkerKinzel.VCards
{
    /// <summary>
    /// Benannte Konstanten, um Optionen für das Schreiben von VCF-Dateien zu bestimmen. Die Konstanten sind kombinierbar.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.VcfOptionsExtension"/>.</note>
    [Flags]
    public enum VcfOptions
    {
        /// <summary>
        /// Alle Options-Flags sind gesetzt.
        /// </summary>
        All = -1,

        /// <summary>
        /// Alle Options-Flags sind ausgeschaltet.
        /// </summary>
        None = 0,

        /// <summary>
        /// Standardeinstellung (entspricht WriteGroups | WriteRfc6474Extensions | WriteRfc6715Extensions | WriteImppExtension | WriteXExtensions)
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
        /// AGENT-vCards zusätzlich an die Haupt-vCard anzuhängen.
        /// </summary>
        IncludeAgentAsSeparateVCard = 1 << 2,


        /// <summary>
        /// Flag setzen, um Non-Standard-Parameter der Property <see cref="ParameterSection.NonStandardParameters"/>
        /// zu schreiben.
        /// </summary>
        WriteNonStandardParameters = 1 << 3,


        /// <summary>
        /// Flag setzen, um Non-Standard-vCard-Properties zu schreiben, die in der Eigenschaft <see cref="VCard.NonStandardProperties"/>
        /// gespeichert sind.
        /// </summary>
        WriteNonStandardProperties = 1 << 4,

        /// <summary>
        /// Flag setzen, um in vCard 4.0 die Erweiterungen aus RFC 6474 zu schreiben (BIRTHPLACE, DEATHPLACE, DEATHDATE).
        /// </summary>
        WriteRfc6474Extensions = 1 << 5,

        /// <summary>
        /// Flag setzen, um in vCard 4.0 die Erweiterungen aus RFC 6715 zu schreiben (EXPERTISE, HOBBY, INTEREST, ORG-DIRECTORY).
        /// </summary>
        WriteRfc6715Extensions = 1 << 6,

        /// <summary>
        /// Flag setzen, um in vCard 3.0 die Erweiterung IMPP aus RFC 4770 zu schreiben.
        /// </summary>
        WriteImppExtension = 1 << 7,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: X-AIM, X-GADUGADU, X-GOOGLE-TALK, X-GTALK, X-ICQ, X-JABBER, 
        /// X-MSN, X-SKYPE, X-TWITTER, X-YAHOO, X-MS-IMADDRESS, X-GENDER, X-ANNIVERSARY und X-SPOUSE.
        /// </summary>
        /// <remarks>
        /// Die Erweiterungen werden nur dann in die vCard geschrieben, wenn in der gewählten vCard-Version keine 
        /// standardgerechte Property für die entsprechenden Daten existiert. So werden z.B. die Instant-Messenger-Properties
        /// in vCard 3.0 nur dann geschrieben, wenn die in RFC 4770 definierte Erweiterung "IMPP" nicht verfügbar ist,
        /// weil das Flag <see cref="WriteImppExtension"/> nicht gesetzt ist.
        /// </remarks>
        WriteXExtensions = 1 << 8,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: X-EVOLUTION-ANNIVERSARY, X-EVOLUTION-SPOUSE.
        /// </summary>
        /// <remarks>
        /// Die Erweiterungen werden nur dann in die vCard geschrieben, wenn in der gewählten vCard-Version keine 
        /// standardgerechte Property für die entsprechenden Daten existiert.
        /// </remarks>
        WriteEvolutionExtensions = 1 << 9,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: X-KADDRESSBOOK-X-IMAddress,
        /// X-KADDRESSBOOK-X-Anniversary, X-KADDRESSBOOK-X-SpouseName.
        /// </summary>
        /// <remarks>
        /// Die Erweiterungen werden nur dann in die vCard geschrieben, wenn in der gewählten vCard-Version keine 
        /// standardgerechte Property für die entsprechenden Daten existiert. So wird z.B. X-KADDRESSBOOK-X-IMAddress
        /// in vCard 3.0 nur dann geschrieben, wenn die in RFC 4770 definierte Erweiterung "IMPP" nicht verfügbar ist,
        /// weil das Flag <see cref="WriteImppExtension"/> nicht gesetzt ist.
        /// </remarks>
        WriteKAddressbookExtensions = 1 << 10,

        /// <summary>
        /// Flag setzen, um bei Bedarf die folgenden vCard-Properties zu schreiben: X-WAB-GENDER, X-WAB-WEDDING-ANNIVERSARY
        /// und X-WAB-SPOUSE-NAME.
        /// </summary>
        /// <remarks>
        /// Die Erweiterungen werden nur dann in die vCard geschrieben, wenn in der gewählten vCard-Version keine 
        /// standardgerechte Property für die entsprechenden Daten existiert.
        /// </remarks>
        WriteWabExtensions = 1 << 11
    }
}
