using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal abstract class ParameterSerializer
{
    private readonly StringBuilder _builder = new();

    protected readonly StringBuilder _worker = new();

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="options"></param>
#pragma warning disable CS8618 // Das Feld lässt keine NULL-Werte zu und ist nicht initialisiert. Deklarieren Sie das Feld ggf. als "Nullable".
    protected ParameterSerializer(VcfOptions options) => this.Options = options;
#pragma warning restore CS8618

    protected ParameterSection ParaSection { get; private set; }

    protected VcfOptions Options { get; }


    internal StringBuilder Serialize(ParameterSection vCardPropertyParameter, string propertyKey, bool isPref)
    {
        ParaSection = vCardPropertyParameter;

        _ = _builder.Clear();

        //Builder.Append(';');

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
            case VCard.PropKeys.CALADRURI:
                BuildCaladruriPara();
                break;
            case VCard.PropKeys.CALURI:
                BuildCaluriPara();
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
                BuildFburlPara();
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
                // kein Parameter-Teil
                break;

            case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
            case VCard.PropKeys.NonStandard.X_SPOUSE:
            case VcfSerializer.X_KADDRESSBOOK_X_SpouseName:
            case VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                BuildXSpousePara();
                break;


            // ASSISTENT wird zwar gelesen und als AGENT interpretiert, aber
            // nicht geschrieben, da AGENT ein Standard-Pendant ist.

            //case VCard.PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
            ////case VcfSerializer.X_KADDRESSBOOK_X_AssistantsName:
            //case VCard.PropKeys.NonStandard.X_ASSISTANT:
            //case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:


            //break;
            default:
                BuildNonStandardPropertyPara(isPref);
                break;
        }

        //if (Builder.Length == 1) Builder.Clear();

        return _builder;
    }



    #region BuildPara

    protected abstract void BuildXSpousePara();

    protected abstract void BuildXMessengerPara(bool isPref);

    protected abstract void BuildNonStandardPropertyPara(bool isPref);

    protected abstract void BuildOrgDirectoryPara();

    protected abstract void BuildInterestPara();

    protected abstract void BuildHobbyPara();

    protected abstract void BuildExpertisePara();

    protected abstract void BuildDeathPlacePara();

    protected abstract void BuildDeathDatePara();

    protected abstract void BuildBirthPlacePara();

    protected abstract void BuildXmlPara();

    protected abstract void BuildUrlPara();

    protected abstract void BuildUidPara();

    protected abstract void BuildTzPara();

    protected abstract void BuildTitlePara();

    protected abstract void BuildTelPara(bool isPref);

    protected abstract void BuildSourcePara();

    protected abstract void BuildSoundPara();

    protected abstract void BuildSortStringPara();

    protected abstract void BuildRolePara();

    protected abstract void BuildRevPara();

    protected abstract void BuildRelatedPara();

    protected abstract void BuildProfilePara();

    protected abstract void BuildProdidPara();

    protected abstract void BuildPhotoPara();

    protected abstract void BuildOrgPara();

    protected abstract void BuildNotePara();

    protected abstract void BuildNicknamePara();

    protected abstract void BuildNamePara();

    protected abstract void BuildNPara();

    protected abstract void BuildMemberPara();

    protected abstract void BuildMailerPara();

    protected abstract void BuildLogoPara();

    protected abstract void BuildLangPara();

    protected abstract void BuildLabelPara(bool isPref);

    protected abstract void BuildKindPara();

    protected abstract void BuildKeyPara();

    protected abstract void BuildImppPara(bool isPref);

    protected abstract void BuildGeoPara();

    protected abstract void BuildGenderPara();

    protected abstract void BuildFnPara();

    protected abstract void BuildFburlPara();

    protected abstract void BuildEmailPara(bool isPref);

    protected abstract void BuildClientpidmapPara();

    protected abstract void BuildClassPara();

    protected abstract void BuildCategoriesPara();

    protected abstract void BuildCaluriPara();

    protected abstract void BuildCaladruriPara();

    protected abstract void BuildBdayPara();

    protected abstract void BuildAnniversaryPara();

    protected abstract void BuildAgentPara();

    protected abstract void BuildAdrPara(bool isPref);

    #endregion


    protected void AppendParameter(string key, string value) => _builder.Append(';').Append(key).Append('=').Append(value);

    protected void AppendV2_1Type(string value) => _builder.Append(';').Append(value);

    protected void AppendNonStandardParameters()
    {
        if (this.ParaSection.NonStandardParameters is null
            || !Options.IsSet(VcfOptions.WriteNonStandardParameters))
        {
            return;
        }

        foreach (KeyValuePair<string, string> parameter in this.ParaSection.NonStandardParameters)
        {
            string key = parameter.Key;

            if (key is null)
            {
                continue;
            }

            key = key.Trim();

            if (key.Length == 0 || string.IsNullOrWhiteSpace(parameter.Value) || IsKnownParameter(key))
            {
                continue;
            }

            AppendParameter(key, parameter.Value);
        }

        ////////////////////////////////////////////

        static bool IsKnownParameter(string key)
        {
            Debug.Assert(key != null);
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
