using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal sealed class ParameterSerializer2_1 : ParameterSerializer
    {
        private AddressTypesCollector? _addressTypesCollector;
        private TelTypesCollector? _telTypesCollector;

        private readonly PropertyClassTypesCollector _propertyClassTypesCollector
            = new PropertyClassTypesCollector();

        private AddressTypesCollector AddressTypesCollector
        {
            get
            {
                _addressTypesCollector ??= new EnumValueCollectors.AddressTypesCollector();
                return _addressTypesCollector;
            }
        }


        private TelTypesCollector TelTypesCollector
        {
            get
            {
                _telTypesCollector ??= new EnumValueCollectors.TelTypesCollector();
                return _telTypesCollector;
            }
        }

        private readonly List<string> _stringCollectionList = new List<string>();
        private readonly List<Action<ParameterSerializer2_1>> _actionList = new List<Action<ParameterSerializer2_1>>(2);



        private readonly Action<ParameterSerializer2_1> _collectPropertyClassTypes =
        serializer =>
            serializer._propertyClassTypesCollector.CollectValueStrings(
                serializer.ParaSection.PropertyClass, serializer._stringCollectionList);


        private readonly Action<ParameterSerializer2_1> _collectTelTypes =
        serializer =>
        {
            const TelTypes DEFINED_TELTYPES = TelTypes.Voice | TelTypes.Fax | TelTypes.Msg | TelTypes.Cell |
            TelTypes.Pager | TelTypes.BBS | TelTypes.Modem | TelTypes.Car | TelTypes.ISDN | TelTypes.Video;

            serializer.TelTypesCollector.CollectValueStrings(
                    serializer.ParaSection.TelephoneType & DEFINED_TELTYPES, serializer._stringCollectionList);
        };


        private readonly Action<ParameterSerializer2_1> _collectAddressTypes =
        serializer =>
            serializer.AddressTypesCollector.CollectValueStrings(
                serializer.ParaSection.AddressType, serializer._stringCollectionList);

        public ParameterSerializer2_1(VcfOptions options) : base(options) { }


        #region Build

        protected override void BuildAdrPara(bool isPref)
        {
            _actionList.Clear();
            _actionList.Add(_collectAddressTypes);
            _actionList.Add(_collectPropertyClassTypes);

            AppendType(false);
            AppendEncodingAndCharset();
            AppendLanguage();
            //AppendNonStandardParameters();

        }

        protected override void BuildAgentPara()
        {
            AppendEncodingAndCharset();
            AppendValue();
            //AppendNonStandardParameters();
        }

        protected override void BuildBdayPara()
        {
            //AppendNonStandardParameters();
        }

        //protected override void BuildCategoriesPara() // steht nicht im Standard
        //{
        //    AppendEncodingAndCharset();
        //    AppendLanguage();
        //}

        protected override void BuildEmailPara(bool isPref)
        {
            AppendEmailType();
            AppendEncodingAndCharset();
            //AppendNonStandardParameters();
        }

        protected override void BuildFnPara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
            //AppendNonStandardParameters();
        }

        protected override void BuildGeoPara()
        {
            // keine Parameter
        }

        protected override void BuildKeyPara()
        {
            AppendKeyType();
            AppendEncodingAndCharset();
            AppendValue();
        }

        protected override void BuildLabelPara(bool isPref)
        {
            _actionList.Clear();
            _actionList.Add(_collectAddressTypes);
            _actionList.Add(_collectPropertyClassTypes);

            AppendType(false);
            AppendEncodingAndCharset();
            AppendLanguage();
            //AppendNonStandardParameters();
        }

        protected override void BuildLogoPara()
        {
            AppendEncodingAndCharset();
            AppendImageType();
            AppendValue();
            //AppendNonStandardParameters();
        }

        protected override void BuildMailerPara() => AppendEncodingAndCharset();

        protected override void BuildNPara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
            //AppendNonStandardParameters();
        }

        //protected override void BuildNamePara() // RFC 2425
        //{
        //    AppendEncodingAndCharset();
        //    AppendLanguage();
        //}

        protected override void BuildNotePara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
        }

        protected override void BuildOrgPara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
        }

        protected override void BuildPhotoPara()
        {
            AppendEncodingAndCharset();
            AppendImageType();
            AppendValue();
            //AppendNonStandardParameters();
        }

        //protected override void BuildProfilePara() // RFC 2425
        //{
        //    // keine Parameter
        //}

        protected override void BuildRevPara()
        {
            // keine Parameter
        }

        protected override void BuildRolePara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
        }

        //protected override void BuildSortStringPara() // steht nicht im Standard
        //{
        //    AppendEncodingAndCharset();
        //    AppendLanguage();
        //}


        protected override void BuildSoundPara()
        {
            AppendEncodingAndCharset();
            AppendSoundType();
            AppendValue();
        }

        //protected override void BuildSourcePara() // RFC 2425
        //{
        //    AppendEncodingAndCharset();
        //    AppendContext();
        //}

        protected override void BuildTelPara(bool isPref)
        {
            _actionList.Clear();
            _actionList.Add(_collectPropertyClassTypes);
            _actionList.Add(_collectTelTypes);


            AppendType(isPref);
            AppendEncodingAndCharset();
            //AppendNonStandardParameters();
        }

        protected override void BuildTitlePara()
        {
            AppendEncodingAndCharset();
            AppendLanguage();
        }

        protected override void BuildTzPara()
        {
            // keine Parameter
        }

        protected override void BuildUidPara() => AppendValue(); // z.B. Inhalt: Url zu einem Webservice, der die neueste Version der vCard liefert

        protected override void BuildUrlPara() => AppendEncodingAndCharset();

        protected override void BuildXMessengerPara(bool isPref)
        {
            _actionList.Clear();
            _actionList.Add(_collectPropertyClassTypes);
            _actionList.Add(_collectTelTypes);


            AppendType(isPref);
            AppendEncodingAndCharset();
            //AppendNonStandardParameters();
        }


        protected override void BuildXSpousePara() => AppendEncodingAndCharset();

        protected override void BuildNonStandardPropertyPara(bool isPref)
        {
            _actionList.Clear();
            _actionList.Add(_collectPropertyClassTypes);

            AppendType(isPref);
            AppendEncodingAndCharset();
            AppendLanguage();
            AppendValue();
            AppendNonStandardParameters();
        }


        protected override void BuildOrgDirectoryPara() { }
        protected override void BuildInterestPara() { }
        protected override void BuildHobbyPara() { }
        protected override void BuildExpertisePara() { }
        protected override void BuildDeathPlacePara() { }
        protected override void BuildDeathDatePara() { }
        protected override void BuildBirthPlacePara() { }
        protected override void BuildXmlPara() { }
        protected override void BuildSourcePara() { }
        protected override void BuildSortStringPara() { }
        protected override void BuildRelatedPara() { }
        protected override void BuildProfilePara() { }
        protected override void BuildProdidPara() { }
        protected override void BuildNicknamePara() { }
        protected override void BuildNamePara() { }
        protected override void BuildMemberPara() { }
        protected override void BuildLangPara() { }
        protected override void BuildKindPara() { }
        protected override void BuildImppPara(bool isPref) { }
        protected override void BuildGenderPara() { }
        protected override void BuildFburlPara() { }
        protected override void BuildClientpidmapPara() { }
        protected override void BuildClassPara() { }
        protected override void BuildCategoriesPara() { }
        protected override void BuildCaluriPara() { }
        protected override void BuildCaladruriPara() { }
        protected override void BuildAnniversaryPara() { }


        #endregion

        #region Append

        private void AppendEncodingAndCharset()
        {
            switch (ParaSection.Encoding)
            {
                case VCdEncoding.Base64:
                    AppendParameter(ParameterSection.ParameterKey.ENCODING, "BASE64");
                    break;
                case VCdEncoding.QuotedPrintable:
                    AppendParameter(ParameterSection.ParameterKey.ENCODING, "QUOTED-PRINTABLE");
                    AppendParameter(ParameterSection.ParameterKey.CHARSET, VCard.DEFAULT_CHARSET);
                    break;
                case VCdEncoding.Ansi:
                    AppendParameter(ParameterSection.ParameterKey.ENCODING, "8BIT");
                    break;
                default:
                    break;
            }
        }


        private void AppendLanguage()
        {
            string? lang = ParaSection.Language;

            if (lang != null && lang != "en-US")
            {
                AppendParameter(ParameterSection.ParameterKey.LANGUAGE, lang);
            }
        }


        private void AppendType(bool isPref)
        {
            this._stringCollectionList.Clear();

            for (int i = 0; i < this._actionList.Count; i++)
            {
                _actionList[i](this);
            }

            if (isPref)
            {
                _stringCollectionList.Add(ParameterSection.TypeValue.PREF);
            }

            if (this._stringCollectionList.Count != 0)
            {
                AppendV2_1Type(ConcatValues());
            }

            string ConcatValues()
            {
                _ = this._worker.Clear();
                int count = this._stringCollectionList.Count;

                Debug.Assert(count != 0);

                for (int i = 0; i < count - 1; i++)
                {
                    _ = _worker.Append(_stringCollectionList[i]).Append(';');
                }

                _ = _worker.Append(_stringCollectionList[count - 1]);
                return _worker.ToString();
            }
        }

        private void AppendImageType()
        {
            string? s = MimeTypeConverter.ImageTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.TYPE, s);
            }
        }

        private void AppendSoundType()
        {
            string? s = MimeTypeConverter.SoundTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.TYPE, s);
            }
        }

        private void AppendKeyType()
        {
            string? s = MimeTypeConverter.KeyTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.TYPE, s);
            }
        }

        private void AppendEmailType() => AppendParameter(ParameterSection.ParameterKey.TYPE, ParaSection.EmailType ?? EmailType.SMTP);

        private void AppendValue()
        {
            //if (ParaSection.DataType == VCdDataType.Uri && ParaSection.ContentLocation == VCdContentLocation.Inline)
            //{
            //    ParaSection.ContentLocation = VCdContentLocation.Url;
            //}

            VCdContentLocation contentLocation = ParaSection.ContentLocation;

            if (contentLocation != VCdContentLocation.Inline)
            {
                AppendParameter(ParameterSection.ParameterKey.VALUE, contentLocation.ToVCardString());
            }
        }

        

        //private void AppendContext()
        //{
        //    string? s = ParaSection.Context;

        //    if (s != null)
        //    {
        //        AppendParameter(ParameterSection.ParameterKey.CONTEXT, Mask(s));
        //    }
        //}

        #endregion


    }
}
