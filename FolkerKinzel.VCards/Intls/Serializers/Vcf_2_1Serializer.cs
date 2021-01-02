using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal class Vcf_2_1Serializer : VcfSerializer
    {
        internal Vcf_2_1Serializer(TextWriter writer, VcfOptions options) : base(writer, options, new ParameterSerializer2_1(options)) { }

        internal override VCdVersion Version => VCdVersion.V2_1;

        protected override string VersionString => "2.1";


        protected override void ReplenishRequiredProperties()
        {
            if (VCardToSerialize.NameViews is null)
            {
#if NET40
                VCardToSerialize.NameViews = new NameProperty[0];
#else
                VCardToSerialize.NameViews = Array.Empty<NameProperty>();
#endif
            }
        }


        internal void WrapBase64Data()
        {
            // mindestens 1 Zeichen muss in der letzten Datenzeile verbleiben
            for (int i = VCard.MAX_BYTES_PER_LINE; i < Builder.Length - 1; i += VCard.MAX_BYTES_PER_LINE)
            {
                Builder.Insert(i, VCard.NewLine);
                i += VCard.NewLine.Length;
            }

            Builder.Append(VCard.NewLine); //Leerzeile nach den Daten
        }


        protected override void AppendLineFolding()
        {
            int counter = 0;

            for (int i = 0; i < Builder.Length - 1; i++)
            {
                char c = Builder[i];

                if (c == '\r') //Quoted-Printable-Softlinebreak
                {
                    i++;         // '\n'
                    counter = 0;
                    continue;
                }
                else
                {
                    counter++; //nach Quoted-Printable nur noch ASCII-Zeichen

                    if (counter == VCard.MAX_BYTES_PER_LINE)
                    {
                        if (Builder[i + 1] == '\r') //Quoted-Printable-Softlinebreak
                        {
                            i += QuotedPrintableConverter.STANDARD_LINEBREAK.Length;
                            counter = 0;
                            continue;
                        }

                        // mindestens 1 Zeichen pro Zeile:
                        for (int j = 0; j < counter - 1; j++) // suche das letzte vorhergehende 
                        {                                     // Leerzeichen und füge
                            int current = i - j;              // davor einen Linebreak ein
                            c = Builder[current];

                            if (c == ' ' || c == '\t')
                            {
                                Builder.Insert(current, VCard.NewLine);
                                i = current += VCard.NewLine.Length + 1; // das Leerzeichen
                                counter = 1; // um das Leerzeichen vorschieben

                                break; // innere for-Schleife
                            }
                        }// for
                    }
                    else if (counter > VCard.MAX_BYTES_PER_LINE) // das tritt nur auf, wenn vorher kein 
                    {                                            // Leerzeichen gefunden wurde
                        if (c == ' ' || c == '\t')
                        {
                            Builder.Insert(i, VCard.NewLine);
                            i += VCard.NewLine.Length + 1; // das Leerzeichen
                            counter = 1; // um das Leerzeichen vorschieben
                        }
                    }
                }
            }
        }


        private void BuildPrefProperty(string propertyKey, IEnumerable<IVcfSerializableData?> serializables)
        {
            Debug.Assert(serializables != null);

            IVcfSerializableData? pref = serializables.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

            if (pref != null)
            {
                this.BuildProperty(propertyKey, pref);
            }
        }

        private void BuildPropertyCollection(string propertyKey, IEnumerable<IVcfSerializableData?> serializables)
        {
            Debug.Assert(serializables != null);

            IVcfSerializableData[] arr = serializables.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).ToArray()!;

            for (int i = 0; i < arr.Length; i++)
            {
                IVcfSerializableData prop = arr[i];
                this.BuildProperty(propertyKey, prop, i == 0 && prop.Parameters.Preference < 100);
            }
        }


        // TODO: die Methoden von VcfSerializer überschreiben

        protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
        {
            Debug.Assert(value != null);

            AddressProperty? adr = value.Where(x => x != null).OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

            if (adr != null)
            {
                this.BuildProperty(VCard.PropKeys.ADR, adr, false);

                string? label = adr.Parameters.Label;

                if (label != null)
                {
                    var labelProp = new TextProperty(label, adr.Group);
                    labelProp.Parameters.Assign(adr.Parameters);
                    this.BuildProperty(VCard.PropKeys.LABEL, labelProp);
                }
            }
        }


        protected override void AppendAnniversaryViews(IEnumerable<DateTimeProperty?> value)
        {
            base.AppendAnniversaryViews(value);
        }

        protected override void AppendBirthDayViews(IEnumerable<DateTimeProperty?> value)
        {
            Debug.Assert(value != null);


            if (value.FirstOrDefault(x => x != null && x is DateTimeOffsetProperty) is DateTimeOffsetProperty pref)
            {
                BuildProperty(VCard.PropKeys.BDAY, pref);
            }
        }

        protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
        {
            Debug.Assert(value != null);

            TextProperty displayName = value
                .Where(x => x != null && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty))
                .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

                ?? new TextProperty(Options.IsSet(VcfOptions.WriteEmptyProperties) ? null : "?");


            BuildProperty(VCard.PropKeys.FN, displayName);
        }


        protected override void AppendEmailAddresses(IEnumerable<TextProperty?> value)
        {
            BuildPropertyCollection(VCard.PropKeys.EMAIL, value);
        }


        protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        {
            base.AppendGenderViews(value);
        }

        protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.GEO, value);
        }

        protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
        {
            BuildXImpps(value);
        }


        protected override void AppendKeys(IEnumerable<DataProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.KEY, value);
        }

        protected override void AppendLastRevision(TimestampProperty value)
        {
            BuildProperty(VCard.PropKeys.REV, value);
        }

        protected override void AppendLogos(IEnumerable<DataProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.LOGO, value);
        }

        protected override void AppendMailer(TextProperty value)
        {
            this.BuildProperty(VCard.PropKeys.MAILER, value);
        }

        protected override void AppendNameViews(IEnumerable<NameProperty?> value)
        {
            Debug.Assert(value != null);

            NameProperty? name = value.FirstOrDefault(x => x != null && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty))
                ?? (Options.IsSet(VcfOptions.WriteEmptyProperties) ? new NameProperty() : new NameProperty(new string[] { "?" }));

            BuildProperty(VCard.PropKeys.N, name);
        }

        protected override void AppendNotes(IEnumerable<TextProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.NOTE, value);
        }

        protected override void AppendOrganizations(IEnumerable<OrganizationProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.ORG, value);
        }

        protected override void AppendPhoneNumbers(IEnumerable<TextProperty?> value)
        {
            BuildPropertyCollection(VCard.PropKeys.TEL, value);
        }

        protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.PHOTO, value);
        }

        protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        {
            base.AppendRelations(value);
        }

        protected override void AppendRoles(IEnumerable<TextProperty?> value)
        {
            this.BuildPrefProperty(VCard.PropKeys.ROLE, value);
        }

        protected override void AppendSounds(IEnumerable<DataProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.SOUND, value);
        }

        protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.TZ, value);
        }

        protected override void AppendTitles(IEnumerable<TextProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.TITLE, value);
        }

        protected override void AppendUniqueIdentifier(UuidProperty value)
        {
            this.BuildProperty(VCard.PropKeys.UID, value);
        }

        protected override void AppendURLs(IEnumerable<TextProperty?> value)
        {
            BuildPrefProperty(VCard.PropKeys.URL, value);
        }

    }
}
