using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer3_0 : ParameterSerializer
{
    private readonly List<string> _stringCollectionList = new();
    private readonly List<Action<ParameterSerializer3_0>> _actionList = new(2);

    private readonly Action<ParameterSerializer3_0> _collectPropertyClassTypes =
    serializer => PropertyClassTypesCollector.CollectValueStrings(
            serializer.ParaSection.PropertyClass, serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectTelTypes =
    serializer =>
    {
        const TelTypes DEFINED_TELTYPES = TelTypes.Voice | TelTypes.Fax | TelTypes.Msg |
        TelTypes.Cell | TelTypes.Pager | TelTypes.BBS | TelTypes.Modem | TelTypes.Car | TelTypes.ISDN |
        TelTypes.Video | TelTypes.PCS;

        TelTypesCollector.CollectValueStrings(
                serializer.ParaSection.TelephoneType & DEFINED_TELTYPES, serializer._stringCollectionList);
    };

    private readonly Action<ParameterSerializer3_0> _collectAddressTypes =
    serializer => AddressTypesCollector.CollectValueStrings(
            serializer.ParaSection.AddressType, serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectImppTypes =
    serializer => ImppTypesCollector.CollectValueStrings(
            serializer.ParaSection.InstantMessengerType, serializer._stringCollectionList);

    private readonly Action<ParameterSerializer3_0> _collectKeyType = serializer => serializer.DoCollectKeyType();
    private readonly Action<ParameterSerializer3_0> _collectImageType = serializer => serializer.DoCollectImageType();
    private readonly Action<ParameterSerializer3_0> _collectEmailType = serializer => serializer.DoCollectEmailType();
    private readonly Action<ParameterSerializer3_0> _collectSoundType = serializer => serializer.DoCollectSoundType();
    private readonly Action<ParameterSerializer3_0> _collectMediaType = serializer => serializer.DoCollectMediaType();


    public ParameterSerializer3_0(VcfOptions options) : base(options) { }


    #region Build

    protected override void BuildAdrPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectAddressTypes);


        AppendType(isPref, true);
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
        // keine Parameter
    }

    protected override void BuildEmailPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectEmailType);

        // EmailType ist eine string-Property.
        // Deshalb können X-Values dort direkt eingegeben werden.
        // Auch ist die Kombination mehrerer Values nicht erlaubt.
        AppendType(isPref, false);
    }

    protected override void BuildFnPara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildGeoPara()
    {
        // keine Parameter
    }

    protected override void BuildImppPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectImppTypes);

        AppendType(isPref, true);
    }


    protected override void BuildKeyPara()
    {
        _actionList.Clear();
        _actionList.Add(this._collectKeyType);

        if (ParaSection.DataType == VCdDataType.Text)
        {
            AppendValue(VCdDataType.Text);
        }
        else
        {
            AppendBase64Encoding();
        }

        AppendType(false, true);
    }


    protected override void BuildLabelPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectAddressTypes);


        AppendType(isPref, true);
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
        else
        {
            AppendBase64Encoding();
        }

        AppendType(false, false);
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
        else
        {
            AppendBase64Encoding();
        }

        AppendType(false, false);
    }

    protected override void BuildProdidPara()
    {
        // keine Parameter
    }

    protected override void BuildProfilePara()
    {
        // keine Parameter
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
        else
        {
            AppendBase64Encoding();
        }

        AppendType(false, false);
    }


    protected override void BuildSourcePara()
    {
        AppendValue(ParaSection.DataType);
        AppendContext();
    }


    protected override void BuildTelPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectTelTypes);

        AppendType(isPref, true);
    }

    protected override void BuildTitlePara()
    {
        AppendValue(ParaSection.DataType);
        AppendLanguage();
        AppendNonStandardParameters();
    }

    protected override void BuildTzPara()
    {
        // keine Parameter
    }

    protected override void BuildUidPara()
    {
        // keine Parameter
    }

    protected override void BuildUrlPara()
    {
        // keine Parameter
    }

    protected override void BuildXMessengerPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectTelTypes);

        AppendType(isPref, true);
    }

    protected override void BuildNonStandardPropertyPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectMediaType);

        AppendValue(ParaSection.DataType);

        if (ParaSection.Encoding == VCdEncoding.Base64)
        {
            AppendBase64Encoding();
        }

        AppendType(isPref, true);
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


    private void AppendBase64Encoding() => AppendParameter(ParameterSection.ParameterKey.ENCODING, "b");


    private void AppendLanguage()
    {
        string? lang = ParaSection.Language;

        if (lang != null)
        {
            AppendParameter(ParameterSection.ParameterKey.LANGUAGE, Mask(lang));
        }
    }

    private void AppendType(bool isPref, bool appendXName)
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


        if (appendXName && Options.IsSet(VcfOptions.WriteNonStandardParameters) && ParaSection.NonStandardParameters != null)
        {
            foreach (KeyValuePair<string, string> kvp in ParaSection.NonStandardParameters)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, ParameterSection.ParameterKey.TYPE) && !string.IsNullOrWhiteSpace(kvp.Value))
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
        string? s = MimeTypeConverter.KeyTypeValueFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }
    }


    private void DoCollectImageType()
    {
        string? s = MimeTypeConverter.ImageTypeValueFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }

    }


    private void DoCollectSoundType()
    {
        string? s = MimeTypeConverter.SoundTypeValueFromMimeType(ParaSection.MediaType);

        if (s != null)
        {
            _stringCollectionList.Add(Mask(s));
        }

    }


    private void DoCollectEmailType() => _stringCollectionList.Add(ParaSection.EmailType ?? EmailType.SMTP);


    private void DoCollectMediaType()
    {
        string? m = ParaSection.MediaType;

        if (m != null)
        {
            _stringCollectionList.Add(Mask(m));
        }
    }

    #endregion


    private string Mask(string? s) => _worker.Clear().Append(s).Mask(VCdVersion.V3_0).ToString();

}
