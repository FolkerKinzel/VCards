using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer3_0 : ParameterSerializer
{
    private readonly List<string> _stringCollectionList = new();
    private readonly List<Action<ParameterSerializer3_0>> _actionList = new(2);

    private readonly Action<ParameterSerializer3_0> _collectPropertyClassTypes = static serializer
        => EnumValueCollector.Collect(serializer.ParaSection.PropertyClass, 
                                      serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectPhoneTypes = static serializer
        => {
               const PhoneTypes DEFINED_PHONE_TYPES = PhoneTypes.Voice | PhoneTypes.Fax | PhoneTypes.Msg |
               PhoneTypes.Cell | PhoneTypes.Pager | PhoneTypes.BBS | PhoneTypes.Modem | PhoneTypes.Car |
               PhoneTypes.ISDN | PhoneTypes.Video | PhoneTypes.PCS;
         
               EnumValueCollector.Collect(serializer.ParaSection.PhoneType & DEFINED_PHONE_TYPES,
                                          serializer._stringCollectionList);
           };

    private readonly Action<ParameterSerializer3_0> _collectAddressTypes = static serializer 
        => EnumValueCollector.Collect(serializer.ParaSection.AddressType,
                                      serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectImppTypes = static serializer 
        => EnumValueCollector.Collect(serializer.ParaSection.InstantMessengerType,
                                      serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectKeyType = static serializer => serializer.DoCollectKeyType();
    private readonly Action<ParameterSerializer3_0> _collectImageType = static serializer => serializer.DoCollectImageType();
    private readonly Action<ParameterSerializer3_0> _collectEmailType = static serializer => serializer.DoCollectEmailType();
    private readonly Action<ParameterSerializer3_0> _collectSoundType = static serializer => serializer.DoCollectSoundType();
    private readonly Action<ParameterSerializer3_0> _collectMediaType = static serializer => serializer.DoCollectMediaType();

    public ParameterSerializer3_0(VcfOptions options) : base(options) { }

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
        if (ParaSection.DataType == VCdDataType.Uri)
        {
            AppendValue(VCdDataType.Uri);
        }
    }

    protected override void BuildBdayPara()
    {
        Debug.Assert(ParaSection.DataType != VCdDataType.Text);

        AppendValue(ParaSection.DataType);
    }

    protected override void BuildCategoriesPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildClassPara()
    {
        // none parameters
    }

    protected override void BuildEmailPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectEmailType);

        // EmailType ist eine string-Property.
        // Deshalb können X-Values dort direkt eingegeben werden.
        // Auch ist die Kombination mehrerer Values nicht erlaubt.
        AppendType(isPref);
    }

    protected override void BuildFnPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildGeoPara()
    {
        // none parameters
    }

    protected override void BuildImppPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectImppTypes);

        AppendType(isPref);
    }

    protected override void BuildKeyPara()
    {
        _actionList.Clear();
        _actionList.Add(this._collectKeyType);

        if (ParaSection.DataType == VCdDataType.Text)
        {
            AppendValue(VCdDataType.Text);
        }

        if (ParaSection.Encoding == ValueEncoding.Base64)
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

        if (ParaSection.DataType == VCdDataType.Uri)
        {
            AppendValue(VCdDataType.Uri);
        }

        if (ParaSection.Encoding == ValueEncoding.Base64)
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

    protected override void BuildNamePara()
    {
        // keine Parameter
    }

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


        if (ParaSection.DataType == VCdDataType.Uri)
        {
            AppendValue(VCdDataType.Uri);
        }

        if (ParaSection.Encoding == ValueEncoding.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(false);
    }

    protected override void BuildProdidPara()
    {
        // none parameters
    }

    protected override void BuildProfilePara()
    {
        // none parameters
    }

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


        if (ParaSection.DataType == VCdDataType.Uri)
        {
            AppendValue(VCdDataType.Uri);
        }
        
        if(ParaSection.Encoding == ValueEncoding.Base64)
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

    protected override void BuildTzPara()
    {
        // none parameters
    }

    protected override void BuildUidPara()
    {
        // none parameters
    }

    protected override void BuildUrlPara()
    {
        // none parameters
    }

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

        if (ParaSection.Encoding == ValueEncoding.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(isPref);
        AppendLanguage();
        AppendContext();
        AppendNonStandardParameters();
    }

    protected override void BuildXSpousePara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildOrgDirectoryPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildInterestPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildHobbyPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildExpertisePara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildDeathPlacePara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildDeathDatePara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildBirthPlacePara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildXmlPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildRelatedPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildMemberPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildLangPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildKindPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildGenderPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildFburlPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildClientpidmapPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildCaluriPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildCaladruriPara() { }

    [ExcludeFromCodeCoverage]
    protected override void BuildAnniversaryPara() { }

    #endregion

    #region Append

    private void AppendValue(VCdDataType? dataType)
    {
        const VCdDataType DEFINED_DATA_TYPES =
            VCdDataType.Uri | VCdDataType.Text | VCdDataType.Date | VCdDataType.Time | VCdDataType.DateTime |
            VCdDataType.Integer | VCdDataType.Boolean | VCdDataType.Float | VCdDataType.Binary |
            VCdDataType.PhoneNumber | VCdDataType.VCard | VCdDataType.UtcOffset;

        string? s = (dataType & DEFINED_DATA_TYPES).ToVcfString();

        if (s != null)
        {
            AppendParameter(ParameterSection.ParameterKey.VALUE, s);
        }
    }

    private void AppendContext()
    {
        string? s = ParaSection.Context;

        if (s != null)
        {
            AppendParameter(ParameterSection.ParameterKey.CONTEXT, s);
        }
    }

    private void AppendBase64Encoding() 
        => AppendParameter(ParameterSection.ParameterKey.ENCODING, "b");

    private void AppendLanguage()
    {
        string? lang = ParaSection.Language;

        if (lang != null)
        {
            AppendParameter(ParameterSection.ParameterKey.LANGUAGE, Mask(lang));
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

        if (Options.HasFlag(VcfOptions.WriteNonStandardParameters) 
            && ParaSection.NonStandard != null)
        {
            foreach (KeyValuePair<string, string> kvp in ParaSection.NonStandard)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, ParameterSection.ParameterKey.TYPE) 
                    && !string.IsNullOrWhiteSpace(kvp.Value))
                {
                    _stringCollectionList.Add(kvp.Value);
                }
            }
        }

        if (this._stringCollectionList.Count != 0)
        {
            AppendParameter(ParameterSection.ParameterKey.TYPE, ConcatValues());
        }

        string ConcatValues()
        {
            _ = this._worker.Clear();
            int count = this._stringCollectionList.Count;

            Debug.Assert(count != 0);

            for (int i = 0; i < count - 1; i++)
            {
                _ = _worker.Append(_stringCollectionList[i]).Append(',');
            }

            _ = _worker.Append(_stringCollectionList[count - 1]);
            return _worker.ToString();
        }
    }

    #endregion


    #region Collect

    private void DoCollectKeyType()
    {
        string? s = MimeTypeConverter.KeyTypeFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }
    }

    private void DoCollectImageType()
    {
        string? s = MimeTypeConverter.ImageTypeFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }

    }

    private void DoCollectSoundType()
    {
        string? s = MimeTypeConverter.SoundTypeFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }

    }

    private void DoCollectEmailType() 
        => _stringCollectionList.Add(ParaSection.EMailType ?? EMailType.SMTP);

    private void DoCollectMediaType()
    {
        string? m = ParaSection.MediaType;

        if (m != null)
        {
            _stringCollectionList.Add(Mask(m));
        }
    }

    #endregion

    private string Mask(string s) => s.Mask(_worker, VCdVersion.V3_0);
}
