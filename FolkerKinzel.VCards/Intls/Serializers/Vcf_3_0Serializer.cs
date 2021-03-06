﻿using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal sealed class Vcf_3_0Serializer : VcfSerializer
    {
        internal Vcf_3_0Serializer(TextWriter writer, VcfOptions options) : base(writer, options, new ParameterSerializer3_0(options)) { }

        internal override VCdVersion Version => VCdVersion.V3_0;

        protected override string VersionString => "3.0";

        protected override void ReplenishRequiredProperties()
        {
            if (VCardToSerialize.NameViews is null)
            {
#if NET40
                VCardToSerialize.NameViews = new NameProperty?[0];
#else
                VCardToSerialize.NameViews = Array.Empty<NameProperty?>();
#endif
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


        private void BuildPropertyCollection(string propertyKey, IEnumerable<VCardProperty?> serializables)
        {
            Debug.Assert(serializables != null);

            VCardProperty[] arr = serializables.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).ToArray()!;

            for (int i = 0; i < arr.Length; i++)
            {
                VCardProperty prop = arr[i];
                BuildProperty(propertyKey, prop, i == 0 && prop.Parameters.Preference < 100);
            }
        }


        private void BuildPrefProperty(string propertyKey, IEnumerable<VCardProperty?> serializables)
        {
            Debug.Assert(serializables != null);

            VCardProperty? pref = serializables.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

            if (pref != null)
            {
                BuildProperty(propertyKey, pref);
            }
        }


        protected override void AppendAccess(AccessProperty value) => BuildProperty(VCard.PropKeys.CLASS, value, false);

        protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
        {
            Debug.Assert(value != null);

            AddressProperty[] arr = value.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).ToArray()!;

            for (int i = 0; i < arr.Length; i++)
            {
                AddressProperty prop = arr[i];
                BuildProperty(VCard.PropKeys.ADR, prop, i == 0 && prop.Parameters.Preference < 100);

                string? label = prop.Parameters.Label;

                if (label != null)
                {
                    var labelProp = new TextProperty(label, prop.Group);
                    labelProp.Parameters.Assign(prop.Parameters);
                    BuildProperty(VCard.PropKeys.LABEL, labelProp);
                }
            }
        }

        protected override void AppendAnniversaryViews(IEnumerable<DateTimeProperty?> value) => base.AppendAnniversaryViews(value);

        protected override void AppendBirthDayViews(IEnumerable<DateTimeProperty?> value)
        {
            Debug.Assert(value != null);


            if (value.FirstOrDefault(x => x != null && x is DateTimeOffsetProperty) is DateTimeOffsetProperty pref)
            {
                BuildProperty(VCard.PropKeys.BDAY, pref);
            }
        }


        protected override void AppendCategories(IEnumerable<StringCollectionProperty?> value) => BuildPrefProperty(VCard.PropKeys.CATEGORIES, value);

        protected override void AppendDirectoryName(TextProperty value) => BuildProperty(VCard.PropKeys.NAME, value);

        protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            TextProperty displayName = value
                .Where(x => x != null && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty))
                .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

                ?? new TextProperty(Options.IsSet(VcfOptions.WriteEmptyProperties) ? null : "?");


            BuildProperty(VCard.PropKeys.FN, displayName);
        }


        protected override void AppendEmailAddresses(IEnumerable<TextProperty?> value) => BuildPropertyCollection(VCard.PropKeys.EMAIL, value);

        protected override void AppendGenderViews(IEnumerable<GenderProperty?> value) => base.AppendGenderViews(value);

        protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value) => BuildPrefProperty(VCard.PropKeys.GEO, value);

        protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            if (Options.IsSet(VcfOptions.WriteImppExtension))
            {
                TextProperty[] arr = value.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).ToArray()!;

                for (int i = 0; i < arr.Length; i++)
                {
                    TextProperty prop = arr[i];

                    if (prop.Parameters.PropertyClass.IsSet(PropertyClassTypes.Home))
                    {
                        prop.Parameters.InstantMessengerType = prop.Parameters.InstantMessengerType.Set(ImppTypes.Personal);
                    }

                    if (prop.Parameters.PropertyClass.IsSet(PropertyClassTypes.Work))
                    {
                        prop.Parameters.InstantMessengerType = prop.Parameters.InstantMessengerType.Set(ImppTypes.Business);
                    }

                    BuildProperty(VCard.PropKeys.IMPP, prop, i == 0 && prop.Parameters.Preference < 100);
                }

                return;
            }
            else
            {
                BuildXImpps(value);
            }
        }


        protected override void AppendKeys(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.KEY, value);

        protected override void AppendLastRevision(TimeStampProperty value) => BuildProperty(VCard.PropKeys.REV, value);

        protected override void AppendLogos(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.LOGO, value);

        protected override void AppendMailer(TextProperty value) => BuildProperty(VCard.PropKeys.MAILER, value);

        protected override void AppendNameViews(IEnumerable<NameProperty?> value)
        {
            Debug.Assert(value != null);

            NameProperty name = value.FirstOrDefault(x => x != null && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty))

                ?? (Options.IsSet(VcfOptions.WriteEmptyProperties) ? new NameProperty() : new NameProperty(new string[] { "?" }));

            BuildProperty(VCard.PropKeys.N, name);

            string? sortString = name.Parameters.SortAs?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim();

            if (sortString != null)
            {
                var sortStringProp = new TextProperty(sortString, name.Group);
                sortStringProp.Parameters.Language = name.Parameters.Language;
                BuildProperty(VCard.PropKeys.SORT_STRING, sortStringProp);
            }
        }

        protected override void AppendNickNames(IEnumerable<StringCollectionProperty?> value) => BuildPrefProperty(VCard.PropKeys.NICKNAME, value);

        protected override void AppendNotes(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.NOTE, value);

        protected override void AppendOrganizations(IEnumerable<OrganizationProperty?> value) => BuildPrefProperty(VCard.PropKeys.ORG, value);

        protected override void AppendPhoneNumbers(IEnumerable<TextProperty?> value) => BuildPropertyCollection(VCard.PropKeys.TEL, value);

        protected override void AppendPhotos(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.PHOTO, value);

        protected override void AppendProdID(TextProperty value) => BuildProperty(VCard.PropKeys.PRODID, value);

        protected override void AppendProfile(ProfileProperty value) => BuildProperty(VCard.PropKeys.PROFILE, value);

        protected override void AppendRelations(IEnumerable<RelationProperty?> value) => base.AppendRelations(value);

        protected override void AppendRoles(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.ROLE, value);

        protected override void AppendSounds(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.SOUND, value);

        protected override void AppendSources(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.SOURCE, value);

        protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value) => BuildPrefProperty(VCard.PropKeys.TZ, value);

        protected override void AppendTitles(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.TITLE, value);

        protected override void AppendUniqueIdentifier(UuidProperty value) => BuildProperty(VCard.PropKeys.UID, value);

        protected override void AppendURLs(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.URL, value);

    }
}
