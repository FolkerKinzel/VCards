using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Syncs;
using ParaKey = FolkerKinzel.VCards.Models.PropertyParts.ParameterSection.ParameterKey;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class ParameterSerializer4_0(Opts options) : ParameterSerializer(VCdVersion.V4_0, options)
{
    private readonly List<string> _stringCollectionList = [];
    private readonly List<Action<ParameterSerializer4_0>> _actionList = new(2);
    private readonly bool writeRfc9554 = options.HasFlag(Opts.WriteRfc9554Extensions);

    private readonly Action<ParameterSerializer4_0> _collectPropertyClassTypes = CollectPropertyClassTypes;
    private readonly Action<ParameterSerializer4_0> _collectPhoneTypes = CollectPhoneTypes;
    private readonly Action<ParameterSerializer4_0> _collectAddressTypes = CollectAddressTypes;
    private readonly Action<ParameterSerializer4_0> _collectRelationTypes = CollectRelationTypes;

    #region Collect

    private static void CollectPropertyClassTypes(ParameterSerializer4_0 serializer)
        => EnumValueCollector.Collect(serializer.ParaSection.PropertyClass,
                                      serializer._stringCollectionList);

    private static void CollectPhoneTypes(ParameterSerializer4_0 serializer)
    {
        const Tel DEFINED_PHONE_TYPES = Tel.Voice | Tel.Text | Tel.Fax |
                                            Tel.Cell | Tel.Video | Tel.Pager |
                                            Tel.TextPhone;

        EnumValueCollector.Collect(serializer.ParaSection.PhoneType & DEFINED_PHONE_TYPES,
                                   serializer._stringCollectionList);
    }

    private static void CollectAddressTypes(ParameterSerializer4_0 serializer)
    {
        if (!serializer.Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            return;
        }

        const Adr DEFINED_ADDRESS_TYPES = Adr.Billing | Adr.Delivery;

        EnumValueCollector.Collect(serializer.ParaSection.AddressType & DEFINED_ADDRESS_TYPES,
                                   serializer._stringCollectionList);
    }

    private static void CollectRelationTypes(ParameterSerializer4_0 serializer)
        => EnumValueCollector.Collect(serializer.ParaSection.RelationType,
                                      serializer._stringCollectionList);
    #endregion


    #region Build

    protected override void BuildAdrPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);
        _actionList.Add(_collectAddressTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendLabel();
        AppendGeo();
        AppendTz();
        AppendLanguage();
        AppendPhoneticAndScript();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendCC();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildBirthPlacePara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildCategoriesPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildClientpidmapPara()
    {
        AppendIndex();
        AppendNonStandardParameters();
    }

    protected override void BuildContactUriPara()
    {
        AppendPref();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildCreatedPara()
    {
        AppendValue(this.ParaSection.DataType);
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildDeathPlacePara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildEmailPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildFnPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendDerived();
        AppendValue(this.ParaSection.DataType);
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildGenderPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendDerived();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildGramGenderPara()
    {
        AppendPref();
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildImppPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        //AppendMediatype();
        AppendServiceTypeAndUsername();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildLanguagePara()
    {
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildMemberPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendPref();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildNPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendSortAs();
        AppendLanguage();
        AppendPhoneticAndScript();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildProdidPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildPronounsPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendLanguage();
        AppendType();
        AppendPref();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildSocialProfilePara()
    {
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendServiceTypeAndUsername();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildSourcePara()
    {
        AppendPref();
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendDerived();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
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
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildXmlPara()
    {
        AppendValue(this.ParaSection.DataType);
        AppendMediatype();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildExpertisePara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendExpertiseLevel();
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildHobbyPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendInterestLevel();
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildInterestPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendInterestLevel();
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildOrgDirectoryPara()
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendLanguage();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    protected override void BuildNonStandardPropertyPara(bool isPref)
    {
        _actionList.Clear();
        _actionList.Add(_collectPropertyClassTypes);

        AppendType();
        AppendPref();
        AppendLanguage();
        AppendPhoneticAndScript();
        AppendMediatype();
        AppendGeo();
        AppendCalScale();
        AppendServiceTypeAndUsername();
        AppendTz();
        AppendSortAs();
        AppendAltId();
        AppendPidAndPropID();
        AppendIndex();
        AppendDerived();
        AppendNonStandardParameters();
        AppendAuthorAndCreated();
    }

    #endregion

    #region Append

    private void AppendAltId()
    {
        if (ParaSection.AltID is string altId)
        {
            AppendParameter(ParameterSection.ParameterKey.ALTID, altId, escapedAndQuoted: true);
        }
    }

    private void AppendAuthorAndCreated()
    {
        if (!writeRfc9554)
        {
            return;
        }

        if (ParaSection.AuthorName is string authorName)
        {
            AppendParameter(ParaKey.Rfc9554.AUTHOR_NAME, authorName, escapedAndQuoted: true);
        }

        if (ParaSection.Author is Uri author)
        {
            Debug.Assert(author.IsAbsoluteUri);
            AppendParameter(ParaKey.Rfc9554.AUTHOR, author.AbsoluteUri, escapedAndQuoted: true);
        }

        DateTimeOffset? created = ParaSection.Created;

        if (created.HasValue)
        {
            AppendParameter(ParaKey.Rfc9554.CREATED, null);
            DateTimeConverter.AppendTimeStampTo(this.Builder, created.Value, VCdVersion.V4_0);
        }
    }

    private void AppendCalScale()
    {
        if (ParaSection.GetCalendar() is string calScale)
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

        if (ParaSection.CountryCode is string countryCode)
        {
            AppendParameter(ParameterSection.ParameterKey.CC, countryCode, escapedAndQuoted: true);
        }
    }

    private void AppendDerived()
    {
        if (writeRfc9554 && ParaSection.Derived)
        {
            AppendParameter(ParaKey.Rfc9554.DERIVED, "TRUE");
        }
    }

    private void AppendExpertiseLevel()
    {
        if (ParaSection.Expertise.ToVcfString() is string exp)
        {
            AppendParameter(ParameterSection.ParameterKey.LEVEL, exp);
            return;
        }

        AppendNonStandardWithKey(ParaKey.LEVEL);
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
            AppendParameter(ParaKey.INDEX, val.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void AppendInterestLevel()
    {
        if (ParaSection.Interest.ToVCardString() is string interest)
        {
            AppendParameter(ParaKey.LEVEL, interest);
            return;
        }

        AppendNonStandardWithKey(ParaKey.LEVEL);
    }

    private void AppendLabel()
    {
        if (ParaSection.Label is string label)
        {
            AppendParameter(ParaKey.LABEL, label, escapedAndQuoted: true, isLabel: true);
        }
    }

    private void AppendLanguage()
    {
        if (ParaSection.Language is string lang)
        {
            AppendParameter(ParaKey.LANGUAGE, lang, escapedAndQuoted: true);
        }
    }

    private void AppendMediatype()
    {
        if (ParaSection.MediaType is string mediaType)
        {
            AppendParameter(ParaKey.MEDIATYPE, mediaType, escapedAndQuoted: true);
        }
    }

    private void AppendPhoneticAndScript()
    {
        if (!writeRfc9554)
        {
            return;
        }

        if (PhoneticConverter.ToVcfString(ParaSection.Phonetic) is string phonetic)
        {
            AppendParameter(ParaKey.Rfc9554.PHONETIC, phonetic);
        }
        else
        {
            AppendNonStandardWithKey(ParaKey.Rfc9554.PHONETIC);
        }

        if (ParaSection.Script is string script)
        {
            AppendParameter(ParaKey.Rfc9554.SCRIPT, script);
        }
    }

    private void AppendPidAndPropID()
    {
        AppendPid();
        AppendPropId();
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

        AppendParameter(ParaKey.PID, "");

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

    private void AppendPropId()
    {
        if (writeRfc9554 && ParaSection.PropertyID is string propID)
        {
            AppendParameter(ParaKey.Rfc9554.PROP_ID, propID, escapedAndQuoted: true);
        }
    }

    private void AppendPref()
    {
        int val = ParaSection.Preference;

        if (val < ParameterSection.PREF_MAX_VALUE)
        {
            AppendParameter(ParaKey.PREF, val.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void AppendServiceTypeAndUsername()
    {
        if (writeRfc9554)
        {
            if (ParaSection.ServiceType is string serviceType)
            {
                AppendParameter(ParaKey.Rfc9554.SERVICE_TYPE, serviceType, escapedAndQuoted: true);
            }

            if (ParaSection.UserName is string userName)
            {
                AppendParameter(ParaKey.Rfc9554.USERNAME, userName, escapedAndQuoted: true);
            }

            return;
        }

        if (Options.HasFlag(Opts.WriteXExtensions) && ParaSection.ServiceType is string xServiceType)
        {
            AppendParameter(ParameterSection.ParameterKey.NonStandard.X_SERVICE_TYPE, xServiceType);
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

        AppendParameter(ParaKey.SORT_AS, "");

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
            AppendParameter(ParaKey.TYPE, "");

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

        AppendParameter(ParaKey.TZ, null);
        tz.AppendTo(Builder, VCdVersion.V4_0, null, asParameter: true);
    }

    private void AppendValue(Data? dataType)
    {
        const Data DEFINED_DATA_TYPES =
            Data.Boolean | Data.Date | Data.DateAndOrTime |
            Data.DateTime | Data.Float | Data.Integer | Data.LanguageTag |
            Data.Text | Data.Time | Data.TimeStamp | Data.Uri | Data.UtcOffset;

        if ((dataType & DEFINED_DATA_TYPES).ToVcfString() is string s)
        {
            AppendParameter(ParaKey.VALUE, s);
            return;
        }

        AppendNonStandardWithKey(ParaKey.VALUE);
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
