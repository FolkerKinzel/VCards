using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>Newline used in VCF files ("\r\n").</summary>
    public const string NewLine = "\r\n";

    internal const int MAX_BYTES_PER_LINE = 75;
    internal const string DEFAULT_CHARSET = "UTF-8";
    internal const VCdVersion DEFAULT_VERSION = VCdVersion.V3_0;


    internal static class PropKeys
    {
        /// <summary> <c>ADR</c>: A structured representation of the physical delivery address
        /// for the vCard object. <c>(2,3,4)</c></summary>
        internal const string ADR = "ADR";

        /// <summary> <c>AGENT</c>: Information about an entity who may sometimes act on behalf 
        /// of the entity associated with the vCard. Typically this is a representative, 
        /// assistant or secretary. Either a URL or an embedded vCard can be specified here.
        /// <c>(2,3,[4])</c> </summary>
        internal const string AGENT = "AGENT";

        /// <summary> <c>ANNIVERSARY</c>: Defines the person's anniversary. <c>(4)</c></summary>
        internal const string ANNIVERSARY = "ANNIVERSARY";

        /// <summary> <c>BDAY</c>: Date of birth of the individual associated with the vCard.
        /// <c>(2,3,4)</c></summary>
        internal const string BDAY = "BDAY";

        /// <summary> <c>CAPURI</c>: A protocol independent location from which a calendaring and 
        /// scheduling client can communicate with a user's entire calendar. <c>(3 - RFC 2739)</c></summary>
        internal const string CAPURI = "CAPURI";

        /// <summary> <c>CALADRURI</c>: URLs to use for sending a scheduling request to
        /// the person's calendar. <c>(4, 3 - RFC 2739)</c></summary>
        internal const string CALADRURI = "CALADRURI";

        /// <summary> <c>CALURI</c>: URLs to the person's calendar. <c>(4, 3 - RFC 2739)</c></summary>
        internal const string CALURI = "CALURI";

        /// <summary> <c>CATEGORIES</c>: Lists of "tags" that can be used to describe the
        /// object represented by this vCard. <c>(3,4)</c></summary>
        internal const string CATEGORIES = "CATEGORIES";

        /// <summary> <c>CLASS</c>: Describes the sensitivity of the information in the
        /// <see cref="VCard"/>. <c>(3)</c></summary>
        internal const string CLASS = "CLASS";

        /// <summary> <c>CLIENTPIDMAP</c>: Mappings for <see cref="PropertyID" />s. It is
        /// used for synchronizing different revisions of the same vCard. <c>(4)</c></summary>
        internal const string CLIENTPIDMAP = "CLIENTPIDMAP";

        /// <summary> <c>EMAIL</c>: The addresses for electronic mail communication with
        /// the vCard object. <c>(2,3,4)</c></summary>
        internal const string EMAIL = "EMAIL";

        /// <summary> <c>FBURL</c>: Defines URLs that show when the person is "free" or
        /// "busy" on their calendar. <c>(4, 3 - RFC 2739)</c></summary>
        internal const string FBURL = "FBURL";

        /// <summary> <c>FN</c>: The formatted name string associated with the vCard object.
        /// <c>(2,3,4)</c></summary>
        internal const string FN = "FN";

        /// <summary> <c>GENDER</c>: Defines the person's gender. <c>(4)</c></summary>
        internal const string GENDER = "GENDER";

        /// <summary> <c>GEO</c>: Specifies latitudes and longitudes. <c>(2,3,4)</c></summary>
        internal const string GEO = "GEO";

        /// <summary> <c>IMPP</c>: List of instant messenger handles. <c>(3,4)</c></summary>
        internal const string IMPP = "IMPP";

        /// <summary> <c>KEY</c>: Public encryption keys associated with the vCard object.
        /// <c>(2,3,4)</c></summary>
        internal const string KEY = "KEY";

        /// <summary> <c>KIND</c>: Defines the type of entity, that this vCard represents.
        /// <c>(4)</c></summary>
        internal const string KIND = "KIND";

        /// <summary><c>LABEL</c> Represents the actual text present on a physical address 
        /// label for addressing to the object associated with the vCard (similar to the 
        /// <c>ADR</c> property). <c>(2,3)</c></summary>
        internal const string LABEL = "LABEL";

        /// <summary> <c>LANG</c>: Defines languages that the person speaks. <c>(4)</c></summary>
        internal const string LANG = "LANG";

        /// <summary> <c>LOGO</c>: Images or graphics of the logo of the organization that
        /// is associated with the individual to which the <see cref="VCard"/> belongs. 
        /// <c>(2,3,4)</c></summary>
        internal const string LOGO = "LOGO";

        /// <summary> <c>MAILER</c>: Type of e-mail program used. <c>(2,3)</c></summary>
        internal const string MAILER = "MAILER";

        /// <summary> <c>MEMBER</c>:
        /// Defines a member that is part of the group that this <see cref="VCard"/> represents.
        /// The <see cref="VCard.Kind" /> property must be set to <see cref="Kind.Group" />
        /// in order to use this property. <c>(4)</c>
        /// </summary>
        internal const string MEMBER = "MEMBER";

        /// <summary> <c>N</c>: A structured representation of the name of the person, place
        /// or thing associated with the vCard object. <c>(2,3,4)</c></summary>
        internal const string N = "N";

        /// <summary> <c>NAME</c>: Provides a textual representation of the 
        /// <see cref="Sources" /> property. <c>(3)</c></summary>
        internal const string NAME = "NAME";

        /// <summary> <c>NICKNAME</c>: One or more descriptive/familiar names for the object
        /// represented by this vCard. <c>(3,4)</c></summary>
        internal const string NICKNAME = "NICKNAME";

        /// <summary> <c>NOTE</c>: Specifies supplemental informations or comments, that
        /// are associated with the vCard. <c>(2,3,4)</c></summary>
        internal const string NOTE = "NOTE";

        /// <summary> <c>ORG</c>: The name and optionally the unit(s) of the organization
        /// associated with the vCard object. <c>(2,3,4)</c></summary>
        internal const string ORG = "ORG";

        /// <summary> <c>PHOTO</c>: Image(s) or photograph(s) of the individual associated
        /// with the vCard. <c>(2,3,4)</c></summary>
        internal const string PHOTO = "PHOTO";

        /// <summary> <c>PRODID</c>: The identifier for the product that created the vCard
        /// object. <c>(3,4)</c></summary>
        internal const string PRODID = "PRODID";

        /// <summary> <c>PROFILE</c>: States that the <see cref="VCard"/> is a vCard. <c>(3)</c></summary>
        internal const string PROFILE = "PROFILE";

        /// <summary> <c>RELATED</c>: Other entities that the person or organization is 
        /// related to. <c>(4)</c></summary>
        internal const string RELATED = "RELATED";

        /// <summary> <c>REV</c>: A time stamp for the last time the vCard was updated. <c>(2,3,4)</c></summary>
        internal const string REV = "REV";

        /// <summary> <c>ROLE</c>: The role, occupation, or business category of the vCard
        /// object within an organization. <c>(2,3,4)</c></summary>
        internal const string ROLE = "ROLE";

        /// <summary>
        /// <c>SORT-STRING</c> <see cref="string"/> describing the sorting order of the vCard 
        /// in applications. <c>(3)</c> 
        /// </summary>
        internal const string SORT_STRING = "SORT-STRING";

        /// <summary> <c>SOUND</c>: Specifies the pronunciation of the <see cref="VCard.DisplayNames"
        /// /> property of the <see cref="VCard" />-object. <c>(2,3,4)</c></summary>
        internal const string SOUND = "SOUND";

        /// <summary> <c>SOURCE</c>: URLs that can be used to get the latest version of
        /// this vCard.<c>(3,4)</c></summary>
        internal const string SOURCE = "SOURCE";

        /// <summary> <c>TEL</c>: Canonical number strings for telephone numbers for 
        /// telephony communication with the vCard object. <c>(2,3,4)</c></summary>
        internal const string TEL = "TEL";

        /// <summary> <c>TITLE</c>: Specifies the job title, functional position or function
        /// of the individual, associated with the vCard object, within an organization.
        /// <c>(2,3,4)</c></summary>
        internal const string TITLE = "TITLE";

        /// <summary> <c>TZ</c>: The time zone(s) of the vCard object. <c>(2,3,4)</c></summary>
        internal const string TZ = "TZ";

        /// <summary> <c>UID</c>: Specifies a value that represents a persistent, globally
        /// unique identifier, associated with the object. <c>(2,3,4)</c></summary>
        internal const string UID = "UID";

        /// <summary> <c>URL</c>: URLs, pointing to websites that represent the person in
        /// some way. <c>(2,3,4)</c></summary>
        internal const string URL = "URL";

        /// <summary> <c>VERSION</c>: Version of the vCard standard. <c>(2,3,4)</c></summary>
        internal const string VERSION = "VERSION";

        /// <summary> <c>XML</c>: Any XML data that is attached to the vCard. <c>(4)</c></summary>
        internal const string XML = "XML";


        internal static class NonStandard
        {
            /// <summary> <c>EXPERTISE</c>: A professional subject area, that the person has
            /// knowledge of. <c>(RFC 6715)</c></summary>
            internal const string EXPERTISE = "EXPERTISE";

            /// <summary> <c>HOBBY</c>: Recreational activities that the person actively engages
            /// in. <c>(4 - RFC 6715)</c></summary>
            internal const string HOBBY = "HOBBY";

            /// <summary> <c>INTEREST</c>: Recreational activities that the person is interested
            /// in, but does not necessarily take part in. <c>(4 - RFC 6715)</c></summary>
            internal const string INTEREST = "INTEREST";

            /// <summary> <c>BIRTHPLACE</c>: The location of the individual's birth. <c>(4 -
            /// RFC 6474)</c></summary>
            internal const string BIRTHPLACE = "BIRTHPLACE";

            /// <summary> <c>DEATHDATE</c>: The individual's time of death. <c>(4 - RFC 6474)</c></summary>
            internal const string DEATHDATE = "DEATHDATE";

            /// <summary> <c>DEATHPLACE</c>: The location of the individual's death. <c>(4 -
            /// RFC 6474)</c></summary>
            internal const string DEATHPLACE = "DEATHPLACE";

            /// <summary> <c>ORG-DIRECTORY</c>: A URI representing the person's work place,
            /// which can be used to look up information on the person's co-workers. <c>(RFC
            /// 6715)</c></summary>
            internal const string ORG_DIRECTORY = "ORG-DIRECTORY";

            /// <summary>Sex (values: <c>Male</c> or <c>Female</c>) </summary>
            internal const string X_GENDER = "X-GENDER";

            /// <summary>Sex (values: <c>1</c> == female, <c>2</c> == male) [Outlook Express] </summary>
            internal const string X_WAB_GENDER = "X-WAB-GENDER";

            /// <summary>Wedding anniversary or any anniversary (in addition to <see cref="PropKeys.BDAY"/>).
            /// (value: <c>YYYY-MM-DD</c>)</summary>
            internal const string X_ANNIVERSARY = "X-ANNIVERSARY";

            /// <summary>Wedding anniversary. [Outlook Express] (value: <c>YYYYMMDD</c>)</summary>
            /// <remarks />
            internal const string X_WAB_WEDDING_ANNIVERSARY = "X-WAB-WEDDING_ANNIVERSARY";

            /// <summary>Spouse's Name</summary>
            internal const string X_SPOUSE = "X-SPOUSE";

            /// <summary>Spouse's Name [Outlook Express]</summary>
            internal const string X_WAB_SPOUSE_NAME = "X-WAB-SPOUSE_NAME";

            /// <summary>Assistant's name (instead of <see cref="AGENT"/>)</summary>
            internal const string X_ASSISTANT = "X-ASSISTANT";


            internal static class InstantMessenger
            {
                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_AIM = "X-AIM";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_ICQ = "X-ICQ";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_GOOGLE_TALK = "X-GOOGLE-TALK";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_GTALK = "X-GTALK";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_JABBER = "X-JABBER";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_MSN = "X-MSN";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_YAHOO = "X-YAHOO";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_TWITTER = "X-TWITTER";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_SKYPE = "X-SKYPE";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_SKYPE_USERNAME = "X-SKYPE-USERNAME";

                /// <summary>
                /// Contact information for instant messaging (IM); <c>TYPE</c> parameters 
                /// as for <see cref="PropKeys.TEL"/>
                /// </summary>
                internal const string X_GADUGADU = "X-GADUGADU";

                /// <summary>GroupWise address</summary>
                internal const string X_GROUPWISE = "X-GROUPWISE";

                /// <summary>IM address in the VCF attachment of Microsoft Outlook</summary>
                /// <remarks>bei Rechtsklick auf den Kontakt-Eintrag → Vollständigen Kontakt senden → Im Internetformat</remarks>
                internal const string X_MS_IMADDRESS = "X-MS-IMADDRESS";

                /// <summary>Instant Messending address</summary>
                internal const string X_KADDRESSBOOK_X_IMADDRESS = "X-KADDRESSBOOK-X-IMADDRESS";
            }

            internal static class KAddressbook
            {
                /// <summary>Spouse's Name</summary>
                internal const string X_KADDRESSBOOK_X_SPOUSENAME = "X-KADDRESSBOOK-X-SPOUSENAME";

                /// <summary>Any anniversary (in addition to <see cref="PropKeys.BDAY"/>).
                /// </summary>
                internal const string X_KADDRESSBOOK_X_ANNIVERSARY = "X-KADDRESSBOOK-X-ANNIVERSARY";

                /// <summary>Assistant's name (instead of <see cref="AGENT"/>)</summary>
                internal const string X_KADDRESSBOOK_X_ASSISTANTSNAME = "X-KADDRESSBOOK-X-ASSISTANTSNAME";
            }

            internal static class Evolution
            {
                /// <summary>Spouse's Name</summary>
                internal const string X_EVOLUTION_SPOUSE = "X-EVOLUTION-SPOUSE";

                /// <summary>Any anniversary (in addition to <see cref="PropKeys.BDAY"/>).
                /// </summary>
                internal const string X_EVOLUTION_ANNIVERSARY = "X-EVOLUTION-ANNIVERSARY";

                /// <summary>Assistant's name (instead of <see cref="AGENT"/>)</summary>
                internal const string X_EVOLUTION_ASSISTANT = "X-EVOLUTION-ASSISTANT";
            }
        }
    }
}
