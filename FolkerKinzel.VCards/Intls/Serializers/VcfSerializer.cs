using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    abstract class VcfSerializer
    {
        internal ParameterSerializer ParameterSerializer { get; }

        [NotNull]
        protected VCard? VCardToSerialize { get; private set; }
        internal readonly StringBuilder Builder = new StringBuilder();
        internal readonly StringBuilder Worker = new StringBuilder();
        internal abstract VCdVersion Version { get; }
        internal VcfOptions Options { get; }

        [NotNull]
        internal string? PropertyKey { get; private set; }
        internal bool IsPref { get; private set; }
        private readonly TextWriter Writer;



        protected VcfSerializer(TextWriter writer, VcfOptions options, ParameterSerializer parameterSerializer)
        {
            this.Options = options;
            this.ParameterSerializer = parameterSerializer;
            this.Writer = writer;
            writer.NewLine = VCard.NewLine;
        }


        /// <summary>
        /// Name des Ehepartners
        /// </summary>
        internal const string X_KADDRESSBOOK_X_SpouseName = "X-KADDRESSBOOK-X-SpouseName";

        /// <summary>
        /// beliebiges Jubiläum (zusätzlich zu BDAY, Geburtstag) 
        /// </summary>
        internal const string X_KADDRESSBOOK_X_Anniversary = "X-KADDRESSBOOK-X-Anniversary";

        ///// <summary>
        ///// Assistenzname (anstelle von AGENT) 
        ///// </summary>
        //internal const string X_KADDRESSBOOK_X_AssistantsName = "X-KADDRESSBOOK-X-AssistantsName";

        /// <summary>
        /// Instant-Messenger-Adresse
        /// </summary>
        internal const string X_KADDRESSBOOK_X_IMAddress = "X-KADDRESSBOOK-X-IMAddress";

        internal static VcfSerializer GetSerializer(TextWriter writer, VCdVersion version, VcfOptions options)
        {
            return version switch
            {
                VCdVersion.V2_1 => new Vcf_2_1Serializer(writer, options),
                VCdVersion.V3_0 => new Vcf_3_0Serializer(writer, options),
                VCdVersion.V4_0 => new Vcf_4_0Serializer(writer, options),
                _ => new Vcf_3_0Serializer(writer, options),
            };
        }


        internal void Serialize(VCard vCard)
        {
            Debug.Assert(vCard != null);

            VCardToSerialize = vCard;
            //var builder = Builder;

            ReplenishRequiredProperties();

            Builder.Clear();
            Writer.WriteLine("BEGIN:VCARD");
            Writer.Write(VCard.PropKeys.VERSION);
            Writer.Write(':');
            Writer.WriteLine(VersionString);

            AppendProperties();

            Writer.WriteLine("END:VCARD");

        }

        protected abstract void ReplenishRequiredProperties();

        protected abstract string VersionString { get; }

        private void AppendProperties()
        {
            foreach (var kvp in VCardToSerialize.OrderBy(x => x.Key))
            {
                switch (kvp.Key)
                {
                    case VCdProp.Profile:
                        AppendProfile((ProfileProperty)kvp.Value);
                        break;
                    case VCdProp.Kind:
                        AppendKind((KindProperty)kvp.Value);
                        break;
                    case VCdProp.Mailer:
                        AppendMailer((TextProperty)kvp.Value);
                        break;
                    case VCdProp.ProdID:
                        AppendProdID((TextProperty)kvp.Value);
                        break;
                    case VCdProp.LastRevision:
                        AppendLastRevision((TimestampProperty)kvp.Value);
                        break;
                    case VCdProp.UniqueIdentifier:
                        AppendUniqueIdentifier((UuidProperty)kvp.Value);
                        break;
                    case VCdProp.Categories:
                        AppendCategories((IEnumerable<StringCollectionProperty?>)kvp.Value);
                        break;
                    case VCdProp.TimeZones:
                        AppendTimeZones((IEnumerable<TimeZoneProperty?>)kvp.Value);
                        break;
                    case VCdProp.GeoCoordinates:
                        AppendGeoCoordinates((IEnumerable<GeoProperty?>)kvp.Value);
                        break;
                    case VCdProp.Access:
                        AppendAccess((AccessProperty)kvp.Value);
                        break;
                    case VCdProp.Sources:
                        AppendSources((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.DirectoryName:
                        AppendDirectoryName((TextProperty)kvp.Value);
                        break;
                    case VCdProp.DisplayNames:
                        AppendDisplayNames((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.NameViews:
                        AppendNameViews((IEnumerable<NameProperty?>)kvp.Value);
                        break;
                    case VCdProp.GenderViews:
                        AppendGenderViews((IEnumerable<GenderProperty?>)kvp.Value);
                        break;
                    case VCdProp.NickNames:
                        AppendNickNames((IEnumerable<StringCollectionProperty?>)kvp.Value);
                        break;
                    case VCdProp.Titles:
                        AppendTitles((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Roles:
                        AppendRoles((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Organizations:
                        AppendOrganizations((IEnumerable<OrganizationProperty?>)kvp.Value);
                        break;
                    case VCdProp.BirthDayViews:
                        AppendBirthDayViews((IEnumerable<DateTimeProperty?>)kvp.Value);
                        break;
                    case VCdProp.BirthPlaceViews:
                        AppendBirthPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.AnniversaryViews:
                        AppendAnniversaryViews((IEnumerable<DateTimeProperty?>)kvp.Value);
                        break;
                    case VCdProp.DeathDateViews:
                        AppendDeathDateViews((IEnumerable<DateTimeProperty?>)kvp.Value);
                        break;
                    case VCdProp.DeathPlaceViews:
                        AppendDeathPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Addresses:
                        AppendAddresses((IEnumerable<AddressProperty?>)kvp.Value);
                        break;
                    case VCdProp.PhoneNumbers:
                        AppendPhoneNumbers((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.EmailAddresses:
                        AppendEmailAddresses((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.URLs:
                        AppendURLs((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.InstantMessengerHandles:
                        AppendInstantMessengerHandles((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Keys:
                        AppendKeys((IEnumerable<DataProperty?>)kvp.Value);
                        break;
                    case VCdProp.CalendarAddresses:
                        AppendCalendarAddresses((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.CalendarUserAddresses:
                        AppendCalendarUserAddresses((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Relations:
                        AppendRelations((IEnumerable<RelationProperty?>)kvp.Value);
                        break;
                    case VCdProp.Members:
                        AppendMembers((IEnumerable<RelationProperty?>)kvp.Value);
                        break;
                    case VCdProp.OrgDirectories:
                        AppendOrgDirectories((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Expertises:
                        AppendExpertises((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Interests:
                        AppendInterests((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Hobbies:
                        AppendHobbies((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Languages:
                        AppendLanguages((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.Notes:
                        AppendNotes((IEnumerable<TextProperty?>)kvp.Value);
                        break;
                    case VCdProp.XmlProperties:
                        AppendXmlProperties((IEnumerable<XmlProperty?>)kvp.Value);
                        break;
                    case VCdProp.Logos:
                        AppendLogos((IEnumerable<DataProperty?>)kvp.Value);
                        break;
                    case VCdProp.Photos:
                        AppendPhotos((IEnumerable<DataProperty?>)kvp.Value);
                        break;
                    case VCdProp.Sounds:
                        AppendSounds((IEnumerable<DataProperty?>)kvp.Value);
                        break;
                    case VCdProp.PropertyIDMappings:
                        AppendPropertyIDMappings((IEnumerable<PropertyIDMappingProperty?>)kvp.Value);
                        break;
                    case VCdProp.NonStandardProperties:
                        AppendNonStandardProperties((IEnumerable<NonStandardProperty?>)kvp.Value);
                        break;
                    default:
                        break;
                }//switch
            }//foreach
        }


        protected void BuildProperty(string propertyKey, IVcfSerializable prop, bool isPref = false)
        {
            PropertyKey = propertyKey;
            //PropertyStartIndex = Builder.Length;
            IsPref = isPref;

            prop.BuildProperty(this);

            AppendLineFolding();

            Writer.WriteLine(Builder);
        }


        protected void BuildXImpps(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            if (Options.IsSet(VcfOptions.WriteXExtensions))
            {
                var arr = value.Where(x => x?.Value != null).OrderBy(x => x!.Parameters.Preference).ToArray();

                for (int i = 0; i < arr.Length; i++)
                {
                    TextProperty prop = arr[i]!;

                    if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Personal))
                    {
                        prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Home);
                    }

                    if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Business))
                    {
                        prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Work);
                    }

                    if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Mobile))
                    {
                        prop.Parameters.TelephoneType = prop.Parameters.TelephoneType.Set(TelTypes.PCS);
                    }

                    string val = prop.Value!;

                    if (val.StartsWith("aim:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_AIM, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("gg:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GADUGADU, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("gtalk:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GTALK, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("com.google.hangouts:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("icq:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_ICQ, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("xmpp:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_JABBER, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("msnim:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MSN, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("sip:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("skype:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_SKYPE, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("twitter:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_TWITTER, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                    else if (val.StartsWith("ymsgr:", StringComparison.OrdinalIgnoreCase))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_YAHOO, prop, i == 0 && prop.Parameters.Preference < 100);
                    }
                }
            }

            if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
            {
                var prop = value.Where(x => x?.Value != null).OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

                if (prop != null)
                {
                    this.BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_IMAddress, prop, prop.Parameters.Preference < 100);
                }
            }
        }



        //protected string Mask(string value)
        //{
        //    return Worker.Clear().Append(value).Mask(this.Version).ToString();
        //}



        protected virtual void AppendLineFolding()
        {
            int counter = 0;

            var enc = Encoding.UTF8;
            char[] arr = new char[1];

            // nach einem Softlinebreak muss noch mindestens 1 Zeichen
            // folgen:
            for (int i = 0; i < Builder.Length - 1; i++)
            {
                char c = Builder[i];


                arr[0] = c;
                counter += enc.GetByteCount(arr);

                if (counter < VCard.MAX_BYTES_PER_LINE)
                {
                    continue;
                }
                else if (counter > VCard.MAX_BYTES_PER_LINE)
                {
                    i--; // ein Zeichen zurück
                }


                Builder.Insert(++i, VCard.NewLine);
                i += VCard.NewLine.Length;
                Builder.Insert(i, ' ');
                counter = 1; // um das Leerzeichen vorschieben

            }
        }








        #region Append

        protected virtual void AppendAccess(AccessProperty value) { }

        protected virtual void AppendAddresses(IEnumerable<AddressProperty?> value) { }


        protected virtual void AppendAnniversaryViews(IEnumerable<DateTimeProperty?> value)
        {
            Debug.Assert(value != null);


            if (value.FirstOrDefault(x => x != null && x is DateTimeOffsetProperty) is DateTimeOffsetProperty pref
                && (!pref.IsEmpty || Options.IsSet(VcfOptions.WriteEmptyProperties)))
            {
                if (Options.IsSet(VcfOptions.WriteXExtensions))
                {
                    BuildAnniversary(VCard.PropKeys.NonStandard.X_ANNIVERSARY, pref.Group);
                }

                if (Options.IsSet(VcfOptions.WriteEvolutionExtensions))
                {
                    BuildAnniversary(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY, pref.Group);
                }

                if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
                {
                    BuildAnniversary(VcfSerializer.X_KADDRESSBOOK_X_Anniversary, pref.Group);
                }

                if (Options.IsSet(VcfOptions.WriteWabExtensions))
                {
                    BuildAnniversary(VCard.PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY, pref.Group);
                }

                void BuildAnniversary(string propKey, string? group)
                {
                    var xAnniversary = new NonStandardProperty(
                        propKey,
                        string.Format(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}",
                                      pref.DateTimeOffset.Year, pref.DateTimeOffset.Month, pref.DateTimeOffset.Day),
                        group);

                    BuildProperty(propKey, xAnniversary);
                }
            }
        }

        protected virtual void AppendBirthDayViews(IEnumerable<DateTimeProperty?> value) { }

        protected virtual void AppendBirthPlaceViews(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendCalendarAddresses(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendCategories(IEnumerable<StringCollectionProperty?> value) { }

        protected virtual void AppendDeathDateViews(IEnumerable<DateTimeProperty?> value) { }

        protected virtual void AppendDeathPlaceViews(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendDirectoryName(TextProperty value) { }

        protected virtual void AppendDisplayNames(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendEmailAddresses(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendExpertises(IEnumerable<TextProperty?> value) { }


        protected virtual void AppendGenderViews(IEnumerable<GenderProperty?> value)
        {
            Debug.Assert(value != null);

            if (value.FirstOrDefault(x => x?.Value?.Sex != null) is GenderProperty pref)
            {
                VCdSex sex = pref.Value.Sex!.Value;

                if (sex != VCdSex.Male && sex != VCdSex.Female) return;

                if (Options.IsSet(VcfOptions.WriteXExtensions))
                {
                    string propKey = VCard.PropKeys.NonStandard.X_GENDER;

                    var xGender = new NonStandardProperty(
                        propKey,
                        sex == VCdSex.Male ? "Male" : "Female", pref.Group);

                    BuildProperty(propKey, xGender);
                }


                if (Options.IsSet(VcfOptions.WriteWabExtensions))
                {
                    string propKey = VCard.PropKeys.NonStandard.X_WAB_GENDER;

                    var xGender = new NonStandardProperty(
                        propKey,
                        sex == VCdSex.Male ? "2" : "1", pref.Group);

                    BuildProperty(propKey, xGender);
                }
            }
        }

        protected virtual void AppendGeoCoordinates(IEnumerable<GeoProperty?> value) { }

        protected virtual void AppendHobbies(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendInterests(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendKeys(IEnumerable<DataProperty?> value) { }

        protected virtual void AppendKind(KindProperty value) { }

        protected virtual void AppendLanguages(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendLastRevision(TimestampProperty value) { }

        protected virtual void AppendLogos(IEnumerable<DataProperty?> value) { }

        protected virtual void AppendMailer(TextProperty value) { }

        protected virtual void AppendMembers(IEnumerable<RelationProperty?> value) { }

        protected virtual void AppendNameViews(IEnumerable<NameProperty?> value) { }

        protected void AppendNonStandardProperties(IEnumerable<NonStandardProperty?> value)
        {
            Debug.Assert(value != null);

            if (!this.Options.IsSet(VcfOptions.WriteNonStandardProperties)) return;

            foreach (var nonStandardProp in value)
            {
                if (nonStandardProp is null) continue;
                this.BuildProperty(nonStandardProp.PropertyKey, nonStandardProp, nonStandardProp.Parameters.Preference == 1);
            }
        }

        protected virtual void AppendNotes(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendNickNames(IEnumerable<StringCollectionProperty?> value) { }

        protected virtual void AppendOrganizations(IEnumerable<OrganizationProperty?> value) { }

        protected virtual void AppendOrgDirectories(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendPhoneNumbers(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendPhotos(IEnumerable<DataProperty?> value) { }

        protected virtual void AppendProdID(TextProperty value) { }

        protected virtual void AppendProfile(ProfileProperty value) { }

        protected virtual void AppendPropertyIDMappings(IEnumerable<PropertyIDMappingProperty?> value) { }

        protected virtual void AppendRelations(IEnumerable<RelationProperty?> value)
        {
            var agent = Options.IsSet(VcfOptions.WriteEmptyProperties)
                ? value.Where(x => x != null && x.Parameters.RelationType.IsSet(RelationTypes.Agent))
                       .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

                : value.Where(x => x != null && !x.IsEmpty && x.Parameters.RelationType.IsSet(RelationTypes.Agent))
                       .OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

            if (agent != null)
            {
                this.BuildProperty(VCard.PropKeys.AGENT, agent);
            }


            var spouse = Options.IsSet(VcfOptions.WriteEmptyProperties)
                ? value.Where(x => x != null && x.Parameters.RelationType.IsSet(RelationTypes.Spouse))
                       .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

                : value.Where(x => x != null && !x.IsEmpty && x.Parameters.RelationType.IsSet(RelationTypes.Spouse))
                       .OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

            if (spouse != null)
            {
                if (spouse is RelationVCardProperty vCardProp)
                {
                    spouse = ConvertToRelationTextProperty(vCardProp);
                }

                if (spouse is RelationTextProperty)
                {

                    if (Options.IsSet(VcfOptions.WriteXExtensions))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.X_SPOUSE, spouse);
                    }

                    if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
                    {
                        this.BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_SpouseName, spouse);
                    }

                    if (Options.IsSet(VcfOptions.WriteEvolutionExtensions))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE, spouse);
                    }

                    if (Options.IsSet(VcfOptions.WriteWabExtensions))
                    {
                        this.BuildProperty(VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME, spouse);
                    }
                }
            }


            static RelationTextProperty ConvertToRelationTextProperty(RelationVCardProperty vcardProp)
            {
                string? name = vcardProp.VCard?.DisplayNames?.Where(x => x != null && !x.IsEmpty)
                    .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()?.Value;

                if (name is null)
                {
                    var vcdName = vcardProp.VCard?.NameViews?.Where(x => x != null && !x.IsEmpty).FirstOrDefault()?.Value;

                    if (vcdName != null)
                    {
                        name = $"{vcdName.FirstName} {vcdName.MiddleName}".Trim() + " " + vcdName.LastName;
                        name = name.Trim();
                    }
                }


                return new RelationTextProperty(name, vcardProp.Parameters.RelationType, vcardProp.Group);
            }
        }

        protected virtual void AppendRoles(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendSounds(IEnumerable<DataProperty?> value) { }

        protected virtual void AppendSources(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendTimeZones(IEnumerable<TimeZoneProperty?> value) { }

        protected virtual void AppendTitles(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendUniqueIdentifier(UuidProperty value) { }

        protected virtual void AppendURLs(IEnumerable<TextProperty?> value) { }

        protected virtual void AppendXmlProperties(IEnumerable<XmlProperty?> value) { }

        #endregion

    }
}
