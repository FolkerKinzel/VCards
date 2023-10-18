namespace FolkerKinzel.VCards;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Bezeichner müssen ein korrektes Suffix aufweisen", Justification = "<Ausstehend>")]
public sealed partial class VCard
{
    /// <summary>Newline used in VCF files ("\r\n").</summary>
    public const string NewLine = "\r\n";

    internal const int MAX_BYTES_PER_LINE = 75;
    internal const string DEFAULT_CHARSET = "UTF-8";
    internal const VCdVersion DEFAULT_VERSION = VCdVersion.V3_0;

    private const int DESERIALIZER_QUEUE_INITIAL_CAPACITY = 64;

    internal static class PropKeys
    {
    /// <summary />
        internal const string ADR = "ADR";

    /// <summary />
        internal const string AGENT = "AGENT";

    /// <summary />
        internal const string ANNIVERSARY = "ANNIVERSARY";

    /// <summary />
        internal const string BDAY = "BDAY";

    /// <summary />
        internal const string CALADRURI = "CALADRURI";

    /// <summary />
        internal const string CALURI = "CALURI";

    /// <summary />
        internal const string CATEGORIES = "CATEGORIES";

    /// <summary />
        internal const string CLASS = "CLASS";

    /// <summary />
        internal const string CLIENTPIDMAP = "CLIENTPIDMAP";

    /// <summary />
        internal const string EMAIL = "EMAIL";

    /// <summary />
        internal const string FBURL = "FBURL";

    /// <summary />
        internal const string FN = "FN";

    /// <summary />
        internal const string GENDER = "GENDER";

    /// <summary />
        internal const string GEO = "GEO";

    /// <summary />
        internal const string IMPP = "IMPP";

    /// <summary />
    /// <remarks />
        internal const string KEY = "KEY";

    /// <summary />
    /// <remarks />
        internal const string KIND = "KIND";

    /// <summary />
        internal const string LABEL = "LABEL";

    /// <summary />
        internal const string LANG = "LANG";

    /// <summary />
        internal const string LOGO = "LOGO";

    /// <summary />
        internal const string MAILER = "MAILER";

    /// <summary />
    /// <remarks />
        internal const string MEMBER = "MEMBER";

    /// <summary />
        internal const string N = "N";

    /// <summary />
        internal const string NAME = "NAME";

    /// <summary />
        internal const string NICKNAME = "NICKNAME";

    /// <summary />
        internal const string NOTE = "NOTE";

    /// <summary />
    /// <remarks />
        internal const string ORG = "ORG";

    /// <summary />
    /// <remarks />
        internal const string PHOTO = "PHOTO";

    /// <summary />
        internal const string PRODID = "PRODID";

    /// <summary />
        internal const string PROFILE = "PROFILE";

    /// <summary />
    /// <remarks />
        internal const string RELATED = "RELATED";

    /// <summary />
        internal const string REV = "REV";

    /// <summary />
        internal const string ROLE = "ROLE";

    /// <summary />
        internal const string SORT_STRING = "SORT-STRING";

    /// <summary />
    /// <remarks />
        internal const string SOUND = "SOUND";

    /// <summary />
        internal const string SOURCE = "SOURCE";

    /// <summary />
        internal const string TEL = "TEL";


    /// <summary />
        internal const string TITLE = "TITLE";


    /// <summary />
        internal const string TZ = "TZ";

    /// <summary />
        internal const string UID = "UID";

    /// <summary />
        internal const string URL = "URL";

    /// <summary />
    /// <remarks />
        internal const string VERSION = "VERSION";


    /// <summary />
        internal const string XML = "XML";



        internal static class NonStandard
        {

    /// <summary />
            internal const string EXPERTISE = "EXPERTISE";

    /// <summary />
            internal const string HOBBY = "HOBBY";

    /// <summary />
            internal const string INTEREST = "INTEREST";

    /// <summary />
            internal const string BIRTHPLACE = "BIRTHPLACE";

    /// <summary />
            internal const string DEATHDATE = "DEATHDATE";

    /// <summary />
            internal const string DEATHPLACE = "DEATHPLACE";

    /// <summary />
            internal const string ORG_DIRECTORY = "ORG-DIRECTORY";

    /// <summary />
            internal const string X_GENDER = "X-GENDER";

    /// <summary />
            internal const string X_WAB_GENDER = "X-WAB-GENDER";

    /// <summary />
    /// <remarks />
            internal const string X_ANNIVERSARY = "X-ANNIVERSARY";

    /// <summary />
    /// <remarks />
            internal const string X_WAB_WEDDING_ANNIVERSARY = "X-WAB-WEDDING_ANNIVERSARY";

    /// <summary />
            internal const string X_SPOUSE = "X-SPOUSE";

    /// <summary />
            internal const string X_WAB_SPOUSE_NAME = "X-WAB-SPOUSE_NAME";

    /// <summary />
            internal const string X_ASSISTANT = "X-ASSISTANT";


            internal static class InstantMessenger
            {
    /// <summary />
                internal const string X_AIM = "X-AIM";

    /// <summary />
                internal const string X_ICQ = "X-ICQ";

    /// <summary />
                internal const string X_GOOGLE_TALK = "X-GOOGLE-TALK";

    /// <summary />
                internal const string X_GTALK = "X-GTALK";

    /// <summary />
                internal const string X_JABBER = "X-JABBER";

    /// <summary />
                internal const string X_MSN = "X-MSN";

                internal const string X_YAHOO = "X-YAHOO";

    /// <summary />
                internal const string X_TWITTER = "X-TWITTER";

    /// <summary />
                internal const string X_SKYPE = "X-SKYPE";

    /// <summary />
                internal const string X_SKYPE_USERNAME = "X-SKYPE-USERNAME";

    /// <summary />
                internal const string X_GADUGADU = "X-GADUGADU";

    /// <summary />
                internal const string X_GROUPWISE = "X-GROUPWISE";

    /// <summary />
                internal const string X_MS_IMADDRESS = "X-MS-IMADDRESS";

    /// <summary />
                internal const string X_KADDRESSBOOK_X_IMADDRESS = "X-KADDRESSBOOK-X-IMADDRESS";
            }


            internal static class KAddressbook
            {
    /// <summary />
                internal const string X_KADDRESSBOOK_X_SPOUSENAME = "X-KADDRESSBOOK-X-SPOUSENAME";

    /// <summary />
                internal const string X_KADDRESSBOOK_X_ANNIVERSARY = "X-KADDRESSBOOK-X-ANNIVERSARY";

    /// <summary />
                internal const string X_KADDRESSBOOK_X_ASSISTANTSNAME = "X-KADDRESSBOOK-X-ASSISTANTSNAME";
            }

            internal static class Evolution
            {
    /// <summary />
                internal const string X_EVOLUTION_SPOUSE = "X-EVOLUTION-SPOUSE";

    /// <summary />
                internal const string X_EVOLUTION_ANNIVERSARY = "X-EVOLUTION-ANNIVERSARY";

    /// <summary />
                internal const string X_EVOLUTION_ASSISTANT = "X-EVOLUTION-ASSISTANT";
            }
        }

    }
}
