using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer2_1(VcfOpts options) : ParameterSerializer(VCdVersion.V2_1, options)
{
    private readonly List<string> _stringCollectionList = [];
    private readonly List<Action<ParameterSerializer2_1>> _actionList = new(2);

    private readonly Action<ParameterSerializer2_1> _collectPropertyClassTypes = CollectPropertyClassTypes;
    private readonly Action<ParameterSerializer2_1> _collectPhoneTypes = CollectPhoneTypes;
    private readonly Action<ParameterSerializer2_1> _collectAddressTypes = CollectAddressTypes;

    #region Collect

    private static void CollectPropertyClassTypes(ParameterSerializer2_1 serializer)
        => EnumValueCollector.Collect(serializer.ParaSection.PropertyClass,
                                      serializer._stringCollectionList);

    private static void CollectPhoneTypes(ParameterSerializer2_1 serializer)
    {
        const Tel DEFINED_PHONE_TYPES = Tel.Voice | Tel.Fax | Tel.Msg | Tel.Cell |
        Tel.Pager | Tel.BBS | Tel.Modem | Tel.Car | Tel.ISDN | Tel.Video;

        EnumValueCollector.Collect(serializer.ParaSection.PhoneType & DEFINED_PHONE_TYPES,
                                   serializer._stringCollectionList);
    }

    private static void CollectAddressTypes(ParameterSerializer2_1 serializer)
    {
        const Adr DEFINED_ADDRESS_TYPES = Adr.Intl | Adr.Parcel | Adr.Postal | Adr.Dom;

        EnumValueCollector.Collect(serializer.ParaSection.AddressType & DEFINED_ADDRESS_TYPES,
                                          serializer._stringCollectionList);
    }

    #endregion

    #region Build

    protected override void BuildAdrPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectAddressTypes);
        _actionList.Add(_collectPropertyClassTypes);

        AppendType(isPref);
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
        AppendEmailType(isPref);
        AppendEncodingAndCharset();
        //AppendNonStandardParameters();
    }

    protected override void BuildFnPara()
    {
        AppendEncodingAndCharset();
        AppendLanguage();
        //AppendNonStandardParameters();
    }

    //protected override void BuildGeoPara()
    //{
    //    // none parameters
    //}

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

        AppendType(isPref);
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

    //protected override void BuildRevPara()
    //{
    //    // none parameters
    //}

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
        _actionList.Add(_collectPhoneTypes);


        AppendType(isPref);
        AppendEncodingAndCharset();
        //AppendNonStandardParameters();
    }

    protected override void BuildTitlePara()
    {
        AppendEncodingAndCharset();
        AppendLanguage();
    }

    //protected override void BuildTzPara()
    //{
    //    // none parameters
    //}

    protected override void BuildUidPara() => AppendValue(); // z.B. Inhalt: Url zu einem Webservice, der die neueste Version der vCard liefert

    protected override void BuildUrlPara() => AppendEncodingAndCharset();

    protected override void BuildXMessengerPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectPhoneTypes);


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

    #endregion

    #region Append

    private void AppendEncodingAndCharset()
    {
        switch (ParaSection.Encoding)
        {
            case Enc.Base64:
                AppendParameter(ParameterSection.ParameterKey.ENCODING, "BASE64");
                break;
            case Enc.QuotedPrintable:
                AppendParameter(ParameterSection.ParameterKey.ENCODING, "QUOTED-PRINTABLE");
                AppendParameter(ParameterSection.ParameterKey.CHARSET, VCard.DEFAULT_CHARSET);
                break;
            //case VCdEncoding.Ansi:
            //    AppendParameter(ParameterSection.ParameterKey.ENCODING, "8BIT");
            //    break;
            default:
                break;
        }
    }

    private void AppendLanguage()
    {
        string? lang = ParaSection.Language;

        if (lang is not null and not "en-US")
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

        for (int i = 0; i < _stringCollectionList.Count; i++)
        {
            AppendV2_1Type(_stringCollectionList[i]);
        }

        if (isPref)
        {
            AppendV2_1Type(ParameterSection.TypeValue.PREF);
        }

        if (Options.HasFlag(VcfOpts.WriteNonStandardParameters))
        {
            IEnumerable<KeyValuePair<string, string>>? nonStandard = ParaSection.NonStandard;

            if (nonStandard is null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> kvp in nonStandard)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key,
                                                            ParameterSection.ParameterKey.TYPE)
                    && !string.IsNullOrWhiteSpace(kvp.Value))
                {
                    AppendV2_1Type(kvp.Value);
                }
            }
        }
    }

    private void AppendImageType()
    {
        string? s = MimeTypeConverter.ImageTypeFromMimeType(ParaSection.MediaType);

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.TYPE, s);
        }
    }

    protected override void BuildSocialProfilePara()
    {
        // X-SOCIALPROFILE
        Debug.Assert(Options.HasFlag(VcfOpts.WriteXExtensions));

        string? serviceType = this.ParaSection.ServiceType;

        if (serviceType is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.NonStandard.X_SERVICE_TYPE, serviceType);
        }

        AppendNonStandardParameters();
    }

    private void AppendSoundType()
    {
        string? s = MimeTypeConverter.SoundTypeFromMimeType(ParaSection.MediaType);

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.TYPE, s);
        }
    }

    private void AppendKeyType()
    {
        string? s = MimeTypeConverter.KeyTypeFromMimeType(ParaSection.MediaType);

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.TYPE, s);
        }
    }

    private void AppendEmailType(bool isPref)
    {
        // PREF ist im Standard nur für Telefonnummern vermerkt,
        // wurde aber vom Windows-Addressbuch verwendet
        if (isPref)
        {
            AppendV2_1Type(ParameterSection.TypeValue.PREF);
        }
        AppendV2_1Type(ParaSection.EMailType ?? EMail.SMTP);
    }

    private void AppendValue()
    {
        Loc contentLocation = ParaSection.ContentLocation;

        if (contentLocation != Loc.Inline)
        {
            AppendParameter(ParameterSection.ParameterKey.VALUE, contentLocation.ToVcfString());
        }
    }


    #endregion
}
