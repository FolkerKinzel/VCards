using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer3_0(Opts options) : ParameterSerializer(VCdVersion.V3_0, options)
{
    private readonly List<string> _stringCollectionList = [];
    private readonly List<Action<ParameterSerializer3_0>> _actionList = new(2);

    private readonly Action<ParameterSerializer3_0> _collectPropertyClassTypes = CollectPropertyClassTypes;
    private readonly Action<ParameterSerializer3_0> _collectPhoneTypes = CollectPhoneTypes;
    private readonly Action<ParameterSerializer3_0> _collectAddressTypes = CollectAddressTypes;
    private readonly Action<ParameterSerializer3_0> _collectImppTypes = CollectImppTypes;
    private readonly Action<ParameterSerializer3_0> _collectKeyType = CollectKeyType;
    private readonly Action<ParameterSerializer3_0> _collectImageType = CollectImageType;
    private readonly Action<ParameterSerializer3_0> _collectEmailType = CollectEmailType;
    private readonly Action<ParameterSerializer3_0> _collectSoundType = CollectSoundType;
    private readonly Action<ParameterSerializer3_0> _collectMediaType = CollectMediaType;

    #region Collect

    private static void CollectPropertyClassTypes(ParameterSerializer3_0 serializer)
        => EnumValueCollector.Collect(serializer.ParaSection.PropertyClass,
                                      serializer._stringCollectionList);

    private static void CollectPhoneTypes(ParameterSerializer3_0 serializer)
    {
        const Tel DEFINED_PHONE_TYPES = Tel.Voice | Tel.Fax | Tel.Msg |
        Tel.Cell | Tel.Pager | Tel.BBS | Tel.Modem | Tel.Car |
        Tel.ISDN | Tel.Video | Tel.PCS;

        EnumValueCollector.Collect(serializer.ParaSection.PhoneType & DEFINED_PHONE_TYPES,
                                   serializer._stringCollectionList);
    }

    private static void CollectAddressTypes(ParameterSerializer3_0 serializer)
    {
        const Adr DEFINED_ADDRESS_TYPES = Adr.Intl | Adr.Parcel | Adr.Postal | Adr.Dom;

        EnumValueCollector.Collect(serializer.ParaSection.AddressType & DEFINED_ADDRESS_TYPES,
                                          serializer._stringCollectionList);
    }

    private static void CollectImppTypes(ParameterSerializer3_0 serializer)
         => EnumValueCollector.Collect(serializer.ParaSection.InstantMessengerType,
                                      serializer._stringCollectionList);

    private static void CollectKeyType(ParameterSerializer3_0 serializer)
    {
        string? s = MimeTypeConverter.KeyTypeFromMimeType(serializer.ParaSection.MediaType);

        if (s is not null)
        {
            serializer._stringCollectionList.Add(s);
        }
    }

    private static void CollectImageType(ParameterSerializer3_0 serializer)
    {
        string? s = MimeTypeConverter.ImageTypeFromMimeType(serializer.ParaSection.MediaType);

        if (s is not null)
        {
            serializer._stringCollectionList.Add(s);
        }

    }

    private static void CollectSoundType(ParameterSerializer3_0 serializer)
    {
        string? s = MimeTypeConverter.SoundTypeFromMimeType(serializer.ParaSection.MediaType);

        if (s is not null)
        {
            serializer._stringCollectionList.Add(s);
        }

    }

    private static void CollectEmailType(ParameterSerializer3_0 serializer)
        => serializer._stringCollectionList.Add(serializer.ParaSection.EMailType ?? EMail.SMTP);

    private static void CollectMediaType(ParameterSerializer3_0 serializer)
    {
        string? m = serializer.ParaSection.MediaType;

        if (m is not null)
        {
            serializer._stringCollectionList.Add(m);
        }
    }

    #endregion
    

    #region Build

    protected override void BuildAdrPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectAddressTypes);

        AppendType(isPref);
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildAgentPara()
    {
        if (ParaSection.DataType == Data.Uri)
        {
            AppendValue(Data.Uri);
        }
    }

    protected override void BuildBdayPara()
    {
        Debug.Assert(ParaSection.DataType != Data.Text);

        AppendValue(ParaSection.DataType);
    }

    protected override void BuildCapuriPara(bool isPref)
    {
        if(isPref) 
        { 
            AppendRfc2739Pref(); 
        }
    }

    protected override void BuildCaluriPara(bool isPref)
    {
        if (isPref)
        {
            AppendRfc2739Pref();
        }
    }

    protected override void BuildCaladruriPara(bool isPref)
    {
        if (isPref)
        {
            AppendRfc2739Pref();
        }
    }

    protected override void BuildFburlPara(bool isPref) 
    {
        if (isPref)
        {
            AppendRfc2739Pref();
        }
    }

    protected override void BuildCategoriesPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    //protected override void BuildClassPara()
    //{
    //    // none parameters
    //}

    protected override void BuildEmailPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectEmailType);

        // The ParameterSection.EMailType property is of Type string.
        // The property maybe set to an x-value. The combination of several
        // type values is not allowed.
        AppendType(isPref);
    }

    protected override void BuildFnPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    //protected override void BuildGeoPara()
    //{
    //    // none parameters
    //}

    protected override void BuildImppPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectImppTypes);

        AppendType(isPref);
        AppendServiceType();
        AppendNonStandardParameters();
    }

    protected override void BuildKeyPara()
    {
        _actionList.Clear();
        _actionList.Add(this._collectKeyType);

        if (ParaSection.DataType == Data.Text)
        {
            AppendValue(Data.Text);
        }

        if (ParaSection.Encoding == Enc.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(false);
    }

    protected override void BuildLabelPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectAddressTypes);

        AppendType(isPref);
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildLogoPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectImageType);

        if (ParaSection.DataType == Data.Uri)
        {
            AppendValue(Data.Uri);
        }

        if (ParaSection.Encoding == Enc.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(false);
    }

    protected override void BuildMailerPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildNPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    //protected override void BuildNamePara()
    //{
    //    // none parameters
    //}

    protected override void BuildNicknamePara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildNotePara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildOrgPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildPhotoPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectImageType);


        if (ParaSection.DataType == Data.Uri)
        {
            AppendValue(Data.Uri);
        }

        if (ParaSection.Encoding == Enc.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(false);
    }

    //protected override void BuildProdidPara()
    //{
    //    // none parameters
    //}

    //protected override void BuildProfilePara()
    //{
    //    // none parameters
    //}

    protected override void BuildRevPara()
    {
        // DateTime is default
        //AppendValue(VCdDataType.DateTime);
    }

    protected override void BuildRolePara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildSocialProfilePara()
    {
        // X-SOCIALPROFILE
        Debug.Assert(Options.HasFlag(Opts.WriteXExtensions));

        string? serviceType = this.ParaSection.ServiceType;

        if (serviceType is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.NonStandard.X_SERVICE_TYPE, serviceType, escapedAndQuoted: true);
        }

        AppendValue(ParaSection.DataType);
        AppendNonStandardParameters();
    }

    protected override void BuildSortStringPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildSoundPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectSoundType);


        if (ParaSection.DataType == Data.Uri)
        {
            AppendValue(Data.Uri);
        }

        if (ParaSection.Encoding == Enc.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(false);
    }

    protected override void BuildSourcePara()
    {
        AppendValue(ParaSection.DataType);
        AppendContext();
        AppendNonStandardParameters();
    }

    protected override void BuildTelPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectPhoneTypes);

        AppendType(isPref);
    }

    protected override void BuildTitlePara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    //protected override void BuildTzPara()
    //{
    //    // none parameters
    //}

    protected override void BuildUidPara()
    {
        // TYPE parameter should be allowed
        // See https://www.rfc-editor.org/errata/eid870
        _actionList.Clear();
        AppendType(false);
    }

    //protected override void BuildUrlPara()
    //{
    //    // none parameters
    //}

    protected override void BuildXMessengerPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectPhoneTypes);

        AppendType(isPref);
    }

    protected override void BuildNonStandardPropertyPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectMediaType);

        AppendValue(ParaSection.DataType);

        if (ParaSection.Encoding == Enc.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(isPref);
        AppendLanguage();
        AppendContext();
        AppendNonStandardParameters();
    }

    //protected override void BuildXSpousePara() 
    //{
    //    // none parameters
    //}

    #endregion

    #region Append

    private void AppendValue(Data? dataType)
    {
        const Data DEFINED_DATA_TYPES =
            Data.Uri | Data.Text | Data.Date | Data.Time | Data.DateTime |
            Data.Integer | Data.Boolean | Data.Float | Data.Binary |
            Data.PhoneNumber | Data.VCard | Data.UtcOffset;

        string? s = (dataType & DEFINED_DATA_TYPES).ToVcfString();

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.VALUE, s);
        }
    }

    private void AppendContext()
    {
        string? s = ParaSection.Context;

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.CONTEXT, s, escapedAndQuoted: true);
        }
    }

    private void AppendBase64Encoding()
        => AppendParameter(ParameterSection.ParameterKey.ENCODING, "b");

    private void AppendLanguage()
    {
        string? lang = ParaSection.Language;

        if (lang is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.LANGUAGE, lang, escapedAndQuoted: true);
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

        bool typeWritten = false;

        if (this._stringCollectionList.Count != 0)
        {
            AppendParameter(ParameterSection.ParameterKey.TYPE, "");
            typeWritten = true;

            for (int i = 0; i < _stringCollectionList.Count; i++)
            {
                Builder.AppendParameterValueEscapedAndQuoted(_stringCollectionList[i], VCdVersion.V3_0).Append(',');
            }
        }

        if (Options.HasFlag(Opts.WriteNonStandardParameters))
        {
            IEnumerable<KeyValuePair<string, string>>? nonStandard = ParaSection.NonStandard;

            if (nonStandard is null)
            {
                return;
            }

            _stringCollectionList.Clear();

            foreach (KeyValuePair<string, string> kvp in nonStandard)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, ParameterSection.ParameterKey.TYPE)
                    && !string.IsNullOrWhiteSpace(kvp.Value))
                {
                    _stringCollectionList.Add(kvp.Value);
                }
            }

            if (this._stringCollectionList.Count != 0)
            {
                if (!typeWritten)
                {
                    AppendParameter(ParameterSection.ParameterKey.TYPE, "");
                    typeWritten = true;
                }

                for (int i = 0; i < _stringCollectionList.Count; i++)
                {
                    Builder.Append(_stringCollectionList[i]).Append(',');
                }
            }
        }

        if (typeWritten)
        {
            --Builder.Length;
        }
    }


    private void AppendServiceType()
    {
        if (Options.HasFlag(Opts.WriteXExtensions) && ParaSection.ServiceType is string serviceType)
        {
            AppendParameter(ParameterSection.ParameterKey.NonStandard.X_SERVICE_TYPE, serviceType);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendRfc2739Pref() => Builder.Append(";TYPE=PREF");

    #endregion
}
