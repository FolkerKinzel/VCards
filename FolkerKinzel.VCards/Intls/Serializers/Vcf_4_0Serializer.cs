using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FolkerKinzel.VCards.Models.Interfaces;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal class Vcf_4_0Serializer : VcfSerializer
    {
        internal Vcf_4_0Serializer(TextWriter writer, VcfOptions options) : base(writer, options, new ParameterSerializer4_0(options)) { }

        internal override VCdVersion Version => VCdVersion.V4_0;

        protected override string VersionString => "4.0";


        protected override void ReplenishRequiredProperties()
        {
            if (VCardToSerialize.Members != null
                && VCardToSerialize.Members.Any(m => m != null && (!m.IsEmpty || Options.IsSet(VcfOptions.WriteEmptyProperties)))
                && (VCardToSerialize.Kind?.Value != VCdKind.Group))
            {
                VCardToSerialize.Kind = new KindProperty(VCdKind.Group);
            }

            if (VCardToSerialize.DisplayNames is null)
            {
#if NET40
                VCardToSerialize.DisplayNames = new TextProperty?[0];
#else
                VCardToSerialize.DisplayNames = Array.Empty<TextProperty?>();
#endif
            }
        }


        private void BuildPropertyCollection(string propertyKey, IEnumerable<IVcfSerializable?> serializables)
        {
            Debug.Assert(serializables != null);

            foreach (IVcfSerializable? serializable in serializables)
            {
                if (serializable is null)
                {
                    continue;
                }

                this.BuildProperty(propertyKey, serializable);
            }
        }


        private static void SetAltID(IEnumerable<IVCardData?> props)
        {
            Debug.Assert(props != null);

            IVCardData[] arr = props.Where(x => x != null).ToArray()!;

            if (arr.Length <= 1)
            {
                return;
            }

            string altID = arr.FirstOrDefault(x => x.Parameters.AltID != null)?.Parameters.AltID ?? "1";

            foreach (IVCardData prop in arr)
            {
                prop.Parameters.AltID = altID;
            }
        }


        #region Append

        protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.ADR, value);
        }

        protected override void AppendAnniversaryViews(IEnumerable<DateTimeProperty?> value)
        {
            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.ANNIVERSARY, value);
        }

        protected override void AppendBirthDayViews(IEnumerable<DateTimeProperty?> value)
        {
            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.BDAY, value);
        }

        protected override void AppendBirthPlaceViews(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6474Extensions))
            {
                return;
            }

            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.BIRTHPLACE, value);
        }

        protected override void AppendCalendarAddresses(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.CALURI, value);
        }

        protected override void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.CALADRURI, value);
        }


        protected override void AppendCategories(IEnumerable<StringCollectionProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.CATEGORIES, value);
        }

        protected override void AppendDeathDateViews(IEnumerable<DateTimeProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6474Extensions))
            {
                return;
            }

            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.DEATHDATE, value);
        }

        protected override void AppendDeathPlaceViews(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6474Extensions))
            {
                return;
            }

            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.DEATHPLACE, value);
        }



        protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            TextProperty[] displNames = value
                .Where(x => !(x is null) && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty)).ToArray()!;

            if (displNames.Length == 0)
            {
                string? displName = Options.IsSet(VcfOptions.WriteEmptyProperties) ? null : "?";

                var textProp = new TextProperty(displName);

                BuildProperty(VCard.PropKeys.FN, textProp);
            }
            else
            {
                for (int i = 0; i < displNames.Length; i++)
                {
                    BuildProperty(VCard.PropKeys.FN, displNames[i]);
                }
            }
        }


        protected override void AppendEmailAddresses(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.EMAIL, value);
        }

        protected override void AppendExpertises(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
            {
                return;
            }

            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.EXPERTISE, value);
        }


        protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        {
            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.GENDER, value);
        }

        protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.GEO, value);
        }

        protected override void AppendHobbies(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
            {
                return;
            }

            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.HOBBY, value);
        }

        protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            foreach (TextProperty? prop in value)
            {
                if (prop is null)
                {
                    continue;
                }

                if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Personal))
                {
                    prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Home);
                }

                if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Business))
                {
                    prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Work);
                }

                this.BuildProperty(VCard.PropKeys.IMPP, prop);
            }
        }

        protected override void AppendInterests(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
            {
                return;
            }

            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.INTEREST, value);
        }


        protected override void AppendKind(KindProperty value)
        {
            Debug.Assert(value != null);

            this.BuildProperty(VCard.PropKeys.KIND, value);
        }


        protected override void AppendKeys(IEnumerable<DataProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.KEY, value);
        }

        protected override void AppendLanguages(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.LANG, value);
        }

        protected override void AppendLastRevision(TimestampProperty value)
        {
            this.BuildProperty(VCard.PropKeys.REV, value);
        }


        protected override void AppendLogos(IEnumerable<DataProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.LOGO, value);
        }


        protected override void AppendMembers(IEnumerable<RelationProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.MEMBER, value);
        }

        protected override void AppendNameViews(IEnumerable<NameProperty?> value)
        {
            SetAltID(value);
            this.BuildPropertyCollection(VCard.PropKeys.N, value);
        }

        protected override void AppendNickNames(IEnumerable<StringCollectionProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.NICKNAME, value);
        }

        protected override void AppendNotes(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.NOTE, value);
        }

        protected override void AppendOrganizations(IEnumerable<OrganizationProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.ORG, value);
        }

        protected override void AppendOrgDirectories(IEnumerable<TextProperty?> value)
        {
            if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
            {
                return;
            }

            this.BuildPropertyCollection(VCard.PropKeys.NonStandard.ORG_DIRECTORY, value);
        }

        protected override void AppendPhoneNumbers(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.TEL, value);
        }

        protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.PHOTO, value);
        }

        protected override void AppendProdID(TextProperty value)
        {
            this.BuildProperty(VCard.PropKeys.PRODID, value);
        }

        protected override void AppendPropertyIDMappings(IEnumerable<PropertyIDMappingProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.CLIENTPIDMAP, value);
        }

        protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.RELATED, value);
        }

        protected override void AppendRoles(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.ROLE, value);
        }

        protected override void AppendSounds(IEnumerable<DataProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.SOUND, value);
        }

        protected override void AppendSources(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.SOURCE, value);
        }

        protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.TZ, value);
        }

        protected override void AppendTitles(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.TITLE, value);
        }

        protected override void AppendUniqueIdentifier(UuidProperty value)
        {
            this.BuildProperty(VCard.PropKeys.UID, value);
        }

        protected override void AppendURLs(IEnumerable<TextProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.URL, value);
        }

        protected override void AppendXmlProperties(IEnumerable<XmlProperty?> value)
        {
            this.BuildPropertyCollection(VCard.PropKeys.XML, value);
        }

        #endregion
    }
}
