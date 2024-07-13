using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal abstract class ParameterSerializer(VCdVersion version, Opts options)
{
    private readonly VCdVersion _version = version;

    [NotNull]
    protected StringBuilder? Builder { get; private set; }

    internal ParameterSection ParaSection { get; private set; } = ParameterSection.Empty;

    protected Opts Options { get; } = options;

    internal void AppendTo(StringBuilder builder,
                           ParameterSection vCardPropertyParameter,
                           string propertyKey,
                           bool isPref)
    {
        Builder = builder;
        ParaSection = vCardPropertyParameter;

        switch (propertyKey)
        {
            case VCard.PropKeys.ADR:
                BuildAdrPara(isPref);
                break;
            case VCard.PropKeys.AGENT:
                BuildAgentPara();
                break;
            case VCard.PropKeys.ANNIVERSARY:
                BuildAnniversaryPara();
                break;
            case VCard.PropKeys.BDAY:
                BuildBdayPara();
                break;
            case VCard.PropKeys.CAPURI:
                BuildCapuriPara(isPref);
                break;
            case VCard.PropKeys.CALADRURI:
                BuildCaladruriPara(isPref);
                break;
            case VCard.PropKeys.CALURI:
                BuildCaluriPara(isPref);
                break;
            case VCard.PropKeys.CATEGORIES:
                BuildCategoriesPara();
                break;
            case VCard.PropKeys.CLASS:
                BuildClassPara();
                break;
            case VCard.PropKeys.CLIENTPIDMAP:
                BuildClientpidmapPara();
                break;
            case VCard.PropKeys.EMAIL:
                BuildEmailPara(isPref);
                break;
            case VCard.PropKeys.FBURL:
                BuildFburlPara(isPref);
                break;
            case VCard.PropKeys.FN:
                BuildFnPara();
                break;
            case VCard.PropKeys.GENDER:
                BuildGenderPara();
                break;
            case VCard.PropKeys.GEO:
                BuildGeoPara();
                break;
            case VCard.PropKeys.IMPP:
                BuildImppPara(isPref);
                break;
            case VCard.PropKeys.KEY:
                BuildKeyPara();
                break;
            case VCard.PropKeys.KIND:
                BuildKindPara();
                break;
            case VCard.PropKeys.LABEL:
                BuildLabelPara(isPref);
                break;
            case VCard.PropKeys.LANG:
                BuildLangPara();
                break;
            case VCard.PropKeys.LOGO:
                BuildLogoPara();
                break;
            case VCard.PropKeys.MAILER:
                BuildMailerPara();
                break;
            case VCard.PropKeys.MEMBER:
                BuildMemberPara();
                break;
            case VCard.PropKeys.N:
                BuildNPara();
                break;
            case VCard.PropKeys.NAME:
                BuildNamePara();
                break;
            case VCard.PropKeys.NICKNAME:
                BuildNicknamePara();
                break;
            case VCard.PropKeys.NOTE:
                BuildNotePara();
                break;
            case VCard.PropKeys.ORG:
                BuildOrgPara();
                break;
            case VCard.PropKeys.PHOTO:
                BuildPhotoPara();
                break;
            case VCard.PropKeys.PRODID:
                BuildProdidPara();
                break;
            case VCard.PropKeys.PROFILE:
                BuildProfilePara();
                break;
            case VCard.PropKeys.RELATED:
                BuildRelatedPara();
                break;
            case VCard.PropKeys.REV:
                BuildRevPara();
                break;
            case VCard.PropKeys.ROLE:
                BuildRolePara();
                break;
            case VCard.PropKeys.SORT_STRING:
                BuildSortStringPara();
                break;
            case VCard.PropKeys.SOUND:
                BuildSoundPara();
                break;
            case VCard.PropKeys.SOURCE:
                BuildSourcePara();
                break;
            case VCard.PropKeys.TEL:
                BuildTelPara(isPref);
                break;
            case VCard.PropKeys.TITLE:
                BuildTitlePara();
                break;
            case VCard.PropKeys.TZ:
                BuildTzPara();
                break;
            case VCard.PropKeys.UID:
                BuildUidPara();
                break;
            case VCard.PropKeys.URL:
                BuildUrlPara();
                break;
            //case VCard.PropKeys.VERSION:
            //break;
            case VCard.PropKeys.XML:
                BuildXmlPara();
                break;
            case VCard.PropKeys.NonStandard.BIRTHPLACE:
                BuildBirthPlacePara();
                break;
            case VCard.PropKeys.NonStandard.DEATHDATE:
                BuildDeathDatePara();
                break;
            case VCard.PropKeys.NonStandard.DEATHPLACE:
                BuildDeathPlacePara();
                break;
            case VCard.PropKeys.NonStandard.EXPERTISE:
                BuildExpertisePara();
                break;
            case VCard.PropKeys.NonStandard.HOBBY:
                BuildHobbyPara();
                break;
            case VCard.PropKeys.NonStandard.INTEREST:
                BuildInterestPara();
                break;
            case VCard.PropKeys.NonStandard.ORG_DIRECTORY:
                BuildOrgDirectoryPara();
                break;
            case VCard.PropKeys.NonStandard.InstantMessenger.X_AIM:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_GADUGADU:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_GTALK:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_ICQ:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_JABBER:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_MSN:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_SKYPE:
            //case VCard.PropKeys.NonStandard.InstantMessenger.X_SKYPE_USERNAME:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_TWITTER:
            case VCard.PropKeys.NonStandard.InstantMessenger.X_YAHOO:
                BuildXMessengerPara(isPref);
                break;

            case VCard.PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS:
            case VcfSerializer.X_KADDRESSBOOK_X_IMAddress:


            case VCard.PropKeys.NonStandard.X_GENDER:
            case VCard.PropKeys.NonStandard.X_WAB_GENDER:

            case VCard.PropKeys.NonStandard.X_ANNIVERSARY:
            case VCard.PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY:
            case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY:
            case VcfSerializer.X_KADDRESSBOOK_X_Anniversary:
                // no parameters
                break;

            case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
            case VCard.PropKeys.NonStandard.X_SPOUSE:
            case VcfSerializer.X_KADDRESSBOOK_X_SpouseName:
            case VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                BuildXSpousePara();
                break;

            // ASSISTENT will be parsed and interpreted as AGENT, but
            // it will not be written because AGENT is the standard property

            //case VCard.PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
            ////case VcfSerializer.X_KADDRESSBOOK_X_AssistantsName:
            //case VCard.PropKeys.NonStandard.X_ASSISTANT:
            //case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:
            //break;
            case VCard.PropKeys.NonStandard.CONTACT_URI:
                BuildContactUriPara();
                break;
            default:
                BuildNonStandardPropertyPara(isPref);
                break;
        }
    }



    #region BuildPara

    [ExcludeFromCodeCoverage]
    protected virtual void BuildXSpousePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildXMessengerPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildNonStandardPropertyPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildOrgDirectoryPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildInterestPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildHobbyPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildExpertisePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildDeathPlacePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildDeathDatePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildBirthPlacePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildXmlPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildUrlPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildUidPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildTzPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildTitlePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildTelPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildSourcePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildSoundPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildSortStringPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildRolePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildRevPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildRelatedPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildProfilePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildProdidPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildPhotoPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildOrgPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildNotePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildNicknamePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildNamePara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildNPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildMemberPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildMailerPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildLogoPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildLangPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildLabelPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildKindPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildKeyPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildImppPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildGeoPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildGenderPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildFnPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildFburlPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildEmailPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildContactUriPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildClientpidmapPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildClassPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildCategoriesPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildCapuriPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildCaluriPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildCaladruriPara(bool isPref) { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildBdayPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildAnniversaryPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildAgentPara() { }

    [ExcludeFromCodeCoverage]
    protected virtual void BuildAdrPara(bool isPref) { }

    #endregion

    protected void AppendParameter(string key, string value, bool escapedAndQuoted = false, bool isLabel = false)
    {
        Builder.Append(';').Append(key).Append('=');

        if (escapedAndQuoted)
        {
            Builder.AppendParameterValueEscapedAndQuoted(value, _version, isLabel);
        }
        else
        {
            Builder.Append(value);
        }
    }

    protected void AppendV2_1Type(string value) => Builder.Append(';').Append(value);

    protected void AppendNonStandardParameters()
    {
        if (!Options.IsSet(Opts.WriteNonStandardParameters))
        {
            return;
        }

        IEnumerable<KeyValuePair<string, string>>? nonStandard = ParaSection.NonStandard;

        if (nonStandard is null)
        {
            return; 
        }

        foreach (KeyValuePair<string, string> parameter in nonStandard)
        {
            string key = parameter.Key;

            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            key = key.Trim();

            if (string.IsNullOrWhiteSpace(parameter.Value) || IsKnownParameter(key))
            {
                continue;
            }

            AppendParameter(key, parameter.Value);
        }

        ////////////////////////////////////////////

        static bool IsKnownParameter(string key)
        {
            Debug.Assert(key is not null);
            Debug.Assert(StringComparer.Ordinal.Equals(key, key.Trim()));

            switch (key.ToUpperInvariant())
            {
                case ParameterSection.ParameterKey.ALTID:
                case ParameterSection.ParameterKey.CALSCALE:
                case ParameterSection.ParameterKey.CHARSET:
                case ParameterSection.ParameterKey.CONTEXT:
                case ParameterSection.ParameterKey.ENCODING:
                case ParameterSection.ParameterKey.GEO:
                case ParameterSection.ParameterKey.INDEX:
                case ParameterSection.ParameterKey.LABEL:
                case ParameterSection.ParameterKey.LANGUAGE:
                case ParameterSection.ParameterKey.LEVEL:
                case ParameterSection.ParameterKey.MEDIATYPE:
                case ParameterSection.ParameterKey.PID:
                case ParameterSection.ParameterKey.PREF:
                case ParameterSection.ParameterKey.SORT_AS:
                case ParameterSection.ParameterKey.TYPE:
                case ParameterSection.ParameterKey.TZ:
                case ParameterSection.ParameterKey.VALUE:
                    return true;
                default:
                    return false;
            }
        }
    }
}
