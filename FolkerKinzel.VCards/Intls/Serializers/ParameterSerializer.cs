using FolkerKinzel.VCards.Models.Helpers;
using System.Diagnostics;
using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Collections.Generic;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal abstract class ParameterSerializer
    {
        private readonly StringBuilder _builder = new StringBuilder();

        protected readonly StringBuilder _worker = new StringBuilder();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
#pragma warning disable CS8618 // Das Feld lässt keine NULL-Werte zu und ist nicht initialisiert. Deklarieren Sie das Feld ggf. als "Nullable".
        protected ParameterSerializer(VcfOptions options) => this.Options = options;

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


                case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
                case VCard.PropKeys.NonStandard.X_SPOUSE:
                case VcfSerializer.X_KADDRESSBOOK_X_SpouseName:
                case VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME:

                    //case VCard.PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
                    //case VcfSerializer.X_KADDRESSBOOK_X_AssistantsName:
                    //case VCard.PropKeys.NonStandard.X_ASSISTANT:
                    //case VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:

                    // kein Parameter-Teil
                    break;
                default:
                    BuildNonStandardPropertyPara(isPref);
                    break;
            }

            //if (Builder.Length == 1) Builder.Clear();

            return _builder;
        }


        #region BuildPara

        protected virtual void BuildXMessengerPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildNonStandardPropertyPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildOrgDirectoryPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildInterestPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildHobbyPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildExpertisePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildDeathPlacePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildDeathDatePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildBirthPlacePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildXmlPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildUrlPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildUidPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildTzPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildTitlePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildTelPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildSourcePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildSoundPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildSortStringPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildRolePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildRevPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildRelatedPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildProfilePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildProdidPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildPhotoPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildOrgPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildNotePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildNicknamePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildNamePara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildNPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildMemberPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildMailerPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildLogoPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildLangPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildLabelPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildKindPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildKeyPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildImppPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildGeoPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildGenderPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildFnPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildFburlPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildEmailPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildClientpidmapPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildClassPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildCategoriesPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildCaluriPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildCaladruriPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildBdayPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildAnniversaryPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildAgentPara() => Debug.Fail("Die Methode wurde nicht überschrieben.");

        protected virtual void BuildAdrPara(bool isPref) => Debug.Fail("Die Methode wurde nicht überschrieben.");

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
                if (string.IsNullOrWhiteSpace(parameter.Key) || string.IsNullOrWhiteSpace(parameter.Value))
                {
                    continue;
                }

                AppendParameter(parameter.Key.Trim().ToUpperInvariant(), parameter.Value);
            }
        }


    }
}
