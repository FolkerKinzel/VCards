using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal class ParameterSerializer4_0 : ParameterSerializer
    {
        private RelationTypesCollector? _relationTypesCollector;
        private TelTypesCollector? _telTypesCollector;

        private const TelTypes DEFINED_TELTYPES
            = TelTypes.Voice | TelTypes.Text | TelTypes.Fax | TelTypes.Cell
            | TelTypes.Video | TelTypes.Pager | TelTypes.TextPhone;

        private readonly PropertyClassTypesCollector PropertyClassTypesCollector
            = new PropertyClassTypesCollector();

        private readonly List<string> StringCollectionList = new List<string>();
        private readonly List<Action<ParameterSerializer4_0>> ActionList = new List<Action<ParameterSerializer4_0>>(2);

        private readonly Action<ParameterSerializer4_0> CollectPropertyClassTypes =
            serializer => serializer.PropertyClassTypesCollector.CollectValueStrings(
                serializer.ParaSection.PropertyClass, serializer.StringCollectionList);


        private readonly Action<ParameterSerializer4_0> CollectTelTypes =
            serializer => serializer.TelTypesCollector.CollectValueStrings(
                serializer.ParaSection.TelephoneType & DEFINED_TELTYPES, serializer.StringCollectionList);


        private readonly Action<ParameterSerializer4_0> CollectRelationTypes =
            serializer => serializer.RelationTypesCollector.CollectValueStrings(
                serializer.ParaSection.RelationType, serializer.StringCollectionList);


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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectRelationTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectTelTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);

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
        }

        private void AppendGeo()
        {
            GeoCoordinate? geo = ParaSection.GeographicPosition;

            if (geo is null || geo.IsUnknown)
            {
                return;
            }

            Worker.Clear().Append('"');
            GeoCoordinateConverter.AppendTo(Worker, geo, VCdVersion.V4_0);
            Worker.Append('"');

            AppendParameter(ParameterSection.ParameterKey.GEO, Worker.ToString());
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

            Worker.Clear();

            foreach (Models.PropertyID pid in pids)
            {
                if(pid.IsEmpty)
                {
                    continue;
                }
                pid.AppendTo(Worker);
                Worker.Append(',');
            }

            if (Worker.Length != 0)
            {
                --Worker.Length;
                AppendParameter(ParameterSection.ParameterKey.PID, Worker.ToString());
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

            Worker.Clear();

            foreach (string? item in sortAs)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                Worker.Append(EscapeAndQuote(item));
                Worker.Append(',');
            }

            if (Worker.Length != 0)
            {
                --Worker.Length;
                AppendParameter(ParameterSection.ParameterKey.SORT_AS, Worker.ToString());
            }


        }


        private void AppendType()
        {
            this.StringCollectionList.Clear();

            for (int i = 0; i < this.ActionList.Count; i++)
            {
                ActionList[i](this);
            }

            if (this.StringCollectionList.Count != 0)
            {
                AppendParameter(ParameterSection.ParameterKey.TYPE, ConcatValues());
            }

            string ConcatValues()
            {
                this.Worker.Clear();
                int count = this.StringCollectionList.Count;

                Debug.Assert(count != 0);

                for (int i = 0; i < count - 1; i++)
                {
                    Worker.Append(StringCollectionList[i]).Append(',');
                }

                Worker.Append(StringCollectionList[count - 1]);
                return Worker.ToString();
            }
        }


        private void AppendTz()
        {
            TimeZoneInfo? tz = ParaSection.TimeZone;

            if (tz is null)
            {
                return;
            }

            Worker.Clear();

            TimeZoneInfoConverter.AppendTo(Worker, tz, VCdVersion.V4_0);

            AppendParameter(ParameterSection.ParameterKey.TZ, EscapeAndQuote(Worker.ToString()));
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
            Worker.Clear().Append(s).MaskNewLine();

            if (MustBeQuoted())
            {
                Worker.Insert(0, '\"');
                Worker.Append('\"');
            }

            return Worker.ToString();

            bool MustBeQuoted()
            {
                bool mustBeQuoted = false;

                for (int i = Worker.Length - 1; i >= 0; i--)
                {
                    char c = Worker[i];

                    if (c == ',' || c == ';')
                    {
                        mustBeQuoted = true;
                    }
                    else if (c == '\"')
                    {
                        Worker.Remove(i, 1);
                    }
                }

                return mustBeQuoted;
            }
        }

    }
}
