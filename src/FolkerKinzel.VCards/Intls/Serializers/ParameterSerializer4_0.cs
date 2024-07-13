using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer4_0(Opts options) : ParameterSerializer(VCdVersion.V4_0, options)
{
    private readonly List<string> _stringCollectionList = [];
    private readonly List<Action<ParameterSerializer4_0>> _actionList = new(2);

    private readonly Action<ParameterSerializer4_0> _collectPropertyClassTypes = static serializer
        => EnumValueCollector.Collect(serializer.ParaSection.PropertyClass,
                                      serializer._stringCollectionList);

    private readonly Action<ParameterSerializer4_0> _collectPhoneTypes = static serializer
        =>
    {
        const Tel DEFINED_PHONE_TYPES = Tel.Voice | Tel.Text | Tel.Fax |
                                            Tel.Cell | Tel.Video | Tel.Pager |
                                            Tel.TextPhone;

        EnumValueCollector.Collect(serializer.ParaSection.PhoneType & DEFINED_PHONE_TYPES,
                                   serializer._stringCollectionList);
    };

    private readonly Action<ParameterSerializer4_0> _collectRelationTypes = static serializer
        => EnumValueCollector.Collect(serializer.ParaSection.RelationType,
                                      serializer._stringCollectionList);

    #region Build

    protected override void BuildAdrPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLabel();
        AppendGeo();
        AppendTz();
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendCC();
        AppendNonStandardParameters();
    }

    protected override void BuildAnniversaryPara()
    {
        Data? dataType = this.ParaSection.DataType;

        AppendValue(dataType);
        AppendCalScale();

        if (dataType == Data.Text)
        {
            // Is allowed. See https://www.rfc-editor.org/errata/eid3086
            AppendLanguage();
        }

        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildBdayPara()
    {
        Data? dataType = this.ParaSection.DataType;
        AppendValue(dataType);
        AppendCalScale();

        if (dataType == Data.Text)
        {
            AppendLanguage();
        }

        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildBirthPlacePara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildCaladruriPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildCaluriPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildCategoriesPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildClientpidmapPara()
    {
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildContactUriPara() 
    {
        AppendPref();
        AppendNonStandardParameters();
    }

    protected override void BuildDeathDatePara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendCalScale();

        if (ParaSection.DataType == Data.Text)
        {
            AppendLanguage();
        }

        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildDeathPlacePara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildEmailPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildFburlPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendNonStandardParameters();
    }

    protected override void BuildFnPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildGenderPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendNonStandardParameters();
    }

    protected override void BuildGeoPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildImppPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildKeyPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();

        if (this.ParaSection.DataType == Data.Text)
        {
            AppendValue(Data.Text);
        }

        if (ParaSection.DataType == Data.Uri)
        {
            AppendMediatype();
        }

        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildKindPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendNonStandardParameters();
    }

    protected override void BuildLangPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildLogoPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();

        if (this.ParaSection.DataType == Data.Uri)
        {
            //AppendValue(VCdDataType.Uri);
            AppendMediatype();
        }
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildMemberPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendPref();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildNPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendSortAs();
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildNicknamePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildNotePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildOrgPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendSortAs();
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildPhotoPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();

        if (this.ParaSection.DataType == Data.Uri)
        {
            //AppendValue(VCdDataType.Uri);
            AppendMediatype();
        }

        AppendPref();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildProdidPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendNonStandardParameters();
    }

    protected override void BuildRelatedPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectRelationTypes);

        Data? dataType = this.ParaSection.DataType;

        AppendType();
        AppendPref();
        Debug.Assert(dataType != Data.VCard);
        AppendValue(dataType);


        if (dataType == Data.Text)
        {
            AppendLanguage();
        }
        else
        {
            AppendMediatype();
        }

        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Ausdruckskörper für Methoden verwenden", Justification = "<Ausstehend>")]
    protected override void BuildRevPara()
    {
        // TimeStamp is default
        AppendNonStandardParameters();
    }

    protected override void BuildRolePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildSoundPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();

        if (this.ParaSection.DataType == Data.Uri)
        {
            //AppendValue(VCdDataType.Uri);
            AppendMediatype();
        }
        AppendPref();
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildSourcePara()
    {
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildTelPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectPhoneTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);

        if (this.ParaSection.DataType == Data.Uri)
        {
            AppendMediatype();
        }

        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildTitlePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildTzPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildUidPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendNonStandardParameters();
    }

    protected override void BuildUrlPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPid();
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildXmlPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendIndex();
    }

    protected override void BuildExpertisePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendExpertiseLevel();
        AppendIndex();
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildHobbyPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendInterestLevel();
        AppendIndex();
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildInterestPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendInterestLevel();
        AppendIndex();
        AppendLanguage();
        AppendAltId();
        AppendNonStandardParameters();
    }

    protected override void BuildOrgDirectoryPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendIndex();
        AppendLanguage();
        AppendAltId();
        AppendPid();
        AppendNonStandardParameters();
    }

    protected override void BuildNonStandardPropertyPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendIndex();
        AppendLanguage();
        AppendMediatype();
        AppendGeo();
        AppendCalScale();
        AppendTz();
        AppendSortAs();
        AppendAltId();
        AppendPid();
        AppendNonStandardParameters();
    }

    #endregion

    #region Append

    private void AppendAltId()
    {
        string? altId = ParaSection.AltID;

        if (altId is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.ALTID, altId, escapedAndQuoted: true);
        }
    }

    private void AppendCalScale()
    {
        string? calScale = ParaSection.GetCalendar();

        if (calScale is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.CALSCALE, calScale, escapedAndQuoted: true);
        }
    }

    private void AppendCC()
    {
        if (!Options.HasFlag(Opts.WriteRfc8605Extensions))
        {
            return;
        }

        string? countryCode = ParaSection.CountryCode;

        if (countryCode is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.CC, countryCode, escapedAndQuoted: true);
        }
    }

    private void AppendExpertiseLevel()
    {
        string? exp = ParaSection.Expertise.ToVcfString();

        if (exp is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.LEVEL, exp);
            return;
        }

        AppendNonStandardWithKey(ParameterSection.ParameterKey.LEVEL);
    }

    private void AppendGeo()
    {
        VCards.GeoCoordinate? geo = ParaSection.GeoPosition;

        if (geo is null)
        {
            return;
        }

        AppendParameter(ParameterSection.ParameterKey.GEO, "");
        Builder.Append('"');
        GeoCoordinateConverter.AppendTo(Builder, geo, VCdVersion.V4_0);
        Builder.Append('"');
    }

    private void AppendIndex()
    {
        int? val = ParaSection.Index;

        if (!val.HasValue)
        {
            return;
        }

        if (val < ParameterSection.PREF_MAX_VALUE)
        {
            AppendParameter(ParameterSection.ParameterKey.INDEX, val.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void AppendInterestLevel()
    {
        string? interest = ParaSection.Interest.ToVCardString();

        if (interest is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.LEVEL, interest);
            return;
        }

        AppendNonStandardWithKey(ParameterSection.ParameterKey.LEVEL);
    }

    private void AppendLabel()
    {
        string? label = ParaSection.Label;

        if (label is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.LABEL, label, escapedAndQuoted: true, isLabel: true);
        }
    }

    private void AppendLanguage()
    {
        string? lang = ParaSection.Language;

        if (lang is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.LANGUAGE, lang, escapedAndQuoted: true);
        }
    }

    private void AppendMediatype()
    {
        string? mediaType = ParaSection.MediaType;

        if (mediaType is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.MEDIATYPE, mediaType, escapedAndQuoted: true);
        }
    }

    private void AppendPid()
    {
        IEnumerable<PropertyID>? pids = ParaSection.PropertyIDs;

        if (pids is null)
        {
            return;
        }

        int startIdx = Builder.Length;
        bool rollBack = true;

        AppendParameter(ParameterSection.ParameterKey.PID, "");

        foreach (PropertyID pid in pids)
        {
            Debug.Assert(pid != null);
            rollBack = false;
            pid.AppendTo(Builder);
            _ = Builder.Append(',');
        }

        if (rollBack)
        {
            Builder.Length = startIdx;
        }
        else
        {
            --Builder.Length;
        }
    }

    private void AppendPref()
    {
        int val = ParaSection.Preference;

        if (val < ParameterSection.PREF_MAX_VALUE)
        {
            AppendParameter(ParameterSection.ParameterKey.PREF, val.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void AppendSortAs()
    {
        IEnumerable<string>? sortAs = ParaSection.SortAs;

        if (sortAs is null)
        {
            return;
        }

        int startIdx = Builder.Length;
        bool rollBack = true;

        AppendParameter(ParameterSection.ParameterKey.SORT_AS, "");
    
        foreach (string item in sortAs)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(item));

            rollBack = false;
            _ = Builder.AppendParameterValueEscapedAndQuoted(item, VCdVersion.V4_0).Append(',');
        }

        if (rollBack)
        {
            Builder.Length = startIdx;
        }
        else
        {
            --Builder.Length;
        }
    }

    private void AppendType()
    {
        this._stringCollectionList.Clear();

        for (int i = 0; i < this._actionList.Count; i++)
        {
            _actionList[i](this);
        }

        if (Options.HasFlag(Opts.WriteNonStandardParameters))
        {
            IEnumerable<KeyValuePair<string, string>>? nonStandard = ParaSection.NonStandard;

            if (nonStandard is null) 
            {
                return; 
            }

            foreach (KeyValuePair<string, string> kvp in nonStandard)
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
            AppendParameter(ParameterSection.ParameterKey.TYPE, "");

            for (int i = 0; i < _stringCollectionList.Count; i++)
            {
                Builder.Append(_stringCollectionList[i]).Append(',');
            }

            --Builder.Length;
        }
    }

    private void AppendTz()
    {
        TimeZoneID? tz = ParaSection.TimeZone;

        if (tz is null)
        {
            return;
        }

        AppendParameter(ParameterSection.ParameterKey.TZ, "");
        tz.AppendTo(Builder, VCdVersion.V4_0, null, asParameter: true);
    }

    private void AppendValue(Data? dataType)
    {
        const Data DEFINED_DATA_TYPES =
            Data.Boolean | Data.Date | Data.DateAndOrTime |
            Data.DateTime | Data.Float | Data.Integer | Data.LanguageTag |
            Data.Text | Data.Time | Data.TimeStamp | Data.Uri | Data.UtcOffset;

        string? s = (dataType & DEFINED_DATA_TYPES).ToVcfString();

        if (s is not null)
        {
            AppendParameter(ParameterSection.ParameterKey.VALUE, s);
            return;
        }

        AppendNonStandardWithKey(ParameterSection.ParameterKey.VALUE);
    }

    private void AppendNonStandardWithKey(string key)
    {
        if (!Options.HasFlag(Opts.WriteNonStandardParameters))
        {
            return;
        }

        IEnumerable<KeyValuePair<string, string>>? nonStandard = ParaSection.NonStandard;

        if (nonStandard is null)
        {
            return;
        }

        foreach (KeyValuePair<string, string> kvp in nonStandard)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, key)
                && !string.IsNullOrWhiteSpace(kvp.Value))
            {
                AppendParameter(ParameterSection.ParameterKey.LEVEL, kvp.Value);
                return;
            }
        }
    }

    #endregion
}
