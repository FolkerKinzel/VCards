using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal sealed class ParameterSerializer4_0 : ParameterSerializer
    {
        private RelationTypesCollector? _relationTypesCollector;
        private TelTypesCollector? _telTypesCollector;

        private const TelTypes DEFINED_TELTYPES
            = TelTypes.Voice | TelTypes.Text | TelTypes.Fax | TelTypes.Cell
            | TelTypes.Video | TelTypes.Pager | TelTypes.TextPhone;

        private readonly PropertyClassTypesCollector _propertyClassTypesCollector
            = new PropertyClassTypesCollector();

        private readonly List<string> _stringCollectionList = new List<string>();
        private readonly List<Action<ParameterSerializer4_0>> _actionList = new List<Action<ParameterSerializer4_0>>(2);

        private readonly Action<ParameterSerializer4_0> _collectPropertyClassTypes =
            serializer => serializer._propertyClassTypesCollector.CollectValueStrings(
                serializer.ParaSection.PropertyClass, serializer._stringCollectionList);


        private readonly Action<ParameterSerializer4_0> _collectTelTypes =
            serializer => serializer.TelTypesCollector.CollectValueStrings(
                serializer.ParaSection.TelephoneType & DEFINED_TELTYPES, serializer._stringCollectionList);


        private readonly Action<ParameterSerializer4_0> _collectRelationTypes =
            serializer => serializer.RelationTypesCollector.CollectValueStrings(
                serializer.ParaSection.RelationType, serializer._stringCollectionList);


        private RelationTypesCollector RelationTypesCollector
        {
            get
            {
                _relationTypesCollector ??= new EnumValueCollectors.RelationTypesCollector();
                return _relationTypesCollector;
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




        public ParameterSerializer4_0(VcfOptions options) : base(options) { }


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
            AppendNonStandardParameters();
        }



        protected override void BuildAnniversaryPara()
        {
            VCdDataType? dataType = this.ParaSection.DataType;

            if (dataType == VCdDataType.Text)
            {
                AppendValue(VCdDataType.Text);

                // Hinweis: LANGUAGE ist eigentlich nur bei BDAY erlaubt!
                // (Fehler im RFC?)
                AppendLanguage();
            }
            else if (ParaSection.Calendar != null)
            {
                AppendValue(VCdDataType.DateAndOrTime);
                AppendCalScale();
            }

            AppendAltId();
            AppendNonStandardParameters();
        }


        protected override void BuildBdayPara()
        {
            VCdDataType? dataType = this.ParaSection.DataType;

            if (dataType == VCdDataType.Text)
            {
                AppendValue(VCdDataType.Text);
                AppendLanguage();
            }
            else if (ParaSection.Calendar != null)
            {
                AppendValue(VCdDataType.DateAndOrTime);
                AppendCalScale();
            }

            AppendAltId();
            AppendNonStandardParameters();
        }


        protected override void BuildCaladruriPara()
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

        protected override void BuildCaluriPara()
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

        protected override void BuildFburlPara()
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

            if (this.ParaSection.DataType == VCdDataType.Text)
            {
                AppendValue(VCdDataType.Text);
            }

            if (ParaSection.DataType == VCdDataType.Uri)
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

            if (this.ParaSection.DataType == VCdDataType.Uri)
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

            if (this.ParaSection.DataType == VCdDataType.Uri)
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

            VCdDataType? dataType = this.ParaSection.DataType;

            AppendType();
            AppendPref();
            AppendValue(dataType == VCdDataType.VCard ? VCdDataType.Uri : dataType);


            if (ParaSection.DataType == VCdDataType.Text)
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

            if (this.ParaSection.DataType == VCdDataType.Uri)
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
            _actionList.Add(_collectTelTypes);

            AppendType();
            AppendPref();
            AppendValue(this.ParaSection.DataType);

            if (this.ParaSection.DataType == VCdDataType.Uri)
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

        protected override void BuildBirthPlacePara()
        {
            AppendValue(this.ParaSection.DataType);
            AppendLanguage();
            AppendAltId();
            AppendNonStandardParameters();
        }

        protected override void BuildDeathDatePara()
        {
            AppendValue(this.ParaSection.DataType);

            if (ParaSection.DataType == VCdDataType.Text)
            {
                AppendLanguage();
            }
            else if (ParaSection.DataType == VCdDataType.DateAndOrTime)
            {
                AppendCalScale();
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


        protected override void BuildXSpousePara() { }
        protected override void BuildXMessengerPara(bool isPref) { }
        protected override void BuildSortStringPara() { }
        protected override void BuildProfilePara() { }
        protected override void BuildNamePara() { }
        protected override void BuildMailerPara() { }
        protected override void BuildLabelPara(bool isPref) { }
        protected override void BuildClassPara() { }
        protected override void BuildAgentPara() { }

        #endregion

        #region Append


        private void AppendAltId()
        {
            string? altId = ParaSection.AltID;
            if (altId != null)
            {
                AppendParameter(ParameterSection.ParameterKey.ALTID, EscapeAndQuote(altId));
            }
        }

        private void AppendCalScale()
        {
            string? calScale = ParaSection.Calendar;
            if (calScale != null)
            {
                AppendParameter(ParameterSection.ParameterKey.ALTID, EscapeAndQuote(calScale));
            }
        }


        private void AppendExpertiseLevel()
        {
            string? exp = ParaSection.ExpertiseLevel.ToVCardString();

            if (exp != null)
            {
                AppendParameter(ParameterSection.ParameterKey.LEVEL, exp);
            }
            else if (Options.IsSet(VcfOptions.WriteNonStandardParameters) && ParaSection.NonStandardParameters != null)
            {
                foreach (KeyValuePair<string, string> kvp in ParaSection.NonStandardParameters)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, ParameterSection.ParameterKey.LEVEL) && !string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        AppendParameter(ParameterSection.ParameterKey.LEVEL, kvp.Value);
                        return;
                    }
                }
            }
        }

        private void AppendGeo()
        {
            GeoCoordinate? geo = ParaSection.GeoPosition;

            if (geo is null || geo.IsUnknown)
            {
                return;
            }

            _ = _worker.Clear().Append('"');
            GeoCoordinateConverter.AppendTo(_worker, geo, VCdVersion.V4_0);
            _ = _worker.Append('"');

            AppendParameter(ParameterSection.ParameterKey.GEO, _worker.ToString());
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
            string? interest = ParaSection.InterestLevel.ToVCardString();

            if (interest != null)
            {
                AppendParameter(ParameterSection.ParameterKey.LEVEL, interest);
            }
            else if (Options.IsSet(VcfOptions.WriteNonStandardParameters) && ParaSection.NonStandardParameters != null)
            {
                foreach (KeyValuePair<string, string> kvp in ParaSection.NonStandardParameters)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, ParameterSection.ParameterKey.LEVEL) && !string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        AppendParameter(ParameterSection.ParameterKey.LEVEL, kvp.Value);
                        return;
                    }
                }
            }
        }

        private void AppendLabel()
        {
            string? label = ParaSection.Label;

            if (label != null)
            {
                AppendParameter(ParameterSection.ParameterKey.LABEL, EscapeAndQuote(label));
            }
        }

        private void AppendLanguage()
        {
            string? lang = ParaSection.Language;

            if (lang != null)
            {
                AppendParameter(ParameterSection.ParameterKey.LANGUAGE, EscapeAndQuote(lang));
            }
        }

        private void AppendMediatype()
        {
            string? mediaType = ParaSection.MediaType;

            if (mediaType != null)
            {
                AppendParameter(ParameterSection.ParameterKey.MEDIATYPE, EscapeAndQuote(mediaType));
            }
        }


        private void AppendPid()
        {
            IEnumerable<Models.PropertyID>? pids = ParaSection.PropertyIDs;

            if (pids is null)
            {
                return;
            }

            _ = _worker.Clear();

            foreach (Models.PropertyID pid in pids)
            {
                if (pid.IsEmpty)
                {
                    continue;
                }
                pid.AppendTo(_worker);
                _ = _worker.Append(',');
            }

            if (_worker.Length != 0)
            {
                --_worker.Length;
                AppendParameter(ParameterSection.ParameterKey.PID, _worker.ToString());
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
            IEnumerable<string?>? sortAs = ParaSection.SortAs;

            if (sortAs is null)
            {
                return;
            }

            _ = _worker.Clear();

            foreach (string? item in sortAs)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                _ = _worker.Append(EscapeAndQuote(item));
                _ = _worker.Append(',');
            }

            if (_worker.Length != 0)
            {
                --_worker.Length;
                AppendParameter(ParameterSection.ParameterKey.SORT_AS, _worker.ToString());
            }


        }


        private void AppendType()
        {
            this._stringCollectionList.Clear();

            for (int i = 0; i < this._actionList.Count; i++)
            {
                _actionList[i](this);
            }

            if (Options.IsSet(VcfOptions.WriteNonStandardParameters) && ParaSection.NonStandardParameters != null)
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


        private void AppendTz()
        {
            TimeZoneInfo? tz = ParaSection.TimeZone;

            if (tz is null)
            {
                return;
            }

            _ = _worker.Clear();

            TimeZoneInfoConverter.AppendTo(_worker, tz, VCdVersion.V4_0);

            AppendParameter(ParameterSection.ParameterKey.TZ, EscapeAndQuote(_worker.ToString()));
        }

        private void AppendValue(VCdDataType? dataType)
        {
            const VCdDataType DEFINED_DATA_TYPES =
                VCdDataType.Boolean | VCdDataType.Date | VCdDataType.DateAndOrTime |
                VCdDataType.DateTime | VCdDataType.Float | VCdDataType.Integer | VCdDataType.LanguageTag |
                VCdDataType.Text | VCdDataType.Time | VCdDataType.Timestamp | VCdDataType.Uri | VCdDataType.UtcOffset;

            string? s = (dataType & DEFINED_DATA_TYPES).ToVCardString();

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.VALUE, s);
            }
        }

        #endregion

        //private bool IsValueDateAndOrTime()
        //{
        //    var value = this.ParaSection.DataType;
        //    return !value.HasValue ||
        //        value == VCdDataType.DateAndOrTime ||
        //        value == VCdDataType.DateTime ||
        //        value == VCdDataType.Date ||
        //        value == VCdDataType.Time ||
        //        value == VCdDataType.Timestamp;
        //}


        private string EscapeAndQuote(string s)
        {
            _ = _worker.Clear().Append(s).MaskNewLine();

            if (MustBeQuoted())
            {
                _ = _worker.Insert(0, '\"');
                _ = _worker.Append('\"');
            }

            return _worker.ToString();

            bool MustBeQuoted()
            {
                bool mustBeQuoted = false;

                for (int i = _worker.Length - 1; i >= 0; i--)
                {
                    char c = _worker[i];

                    if (c == ',' || c == ';')
                    {
                        mustBeQuoted = true;
                    }
                    else if (c == '\"')
                    {
                        _ = _worker.Remove(i, 1);
                    }
                }

                return mustBeQuoted;
            }
        }

        
    }
}
