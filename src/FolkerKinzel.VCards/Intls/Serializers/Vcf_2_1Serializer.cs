using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_2_1Serializer : VcfSerializer
{
    internal Vcf_2_1Serializer(TextWriter writer, VcfOptions options, ITimeZoneIDConverter? tzConverter)
        : base(writer, options, new ParameterSerializer2_1(options), tzConverter) { }

    internal override VCdVersion Version => VCdVersion.V2_1;

    protected override string VersionString => "2.1";


    protected override void ReplenishRequiredProperties()
    {
        if (VCardToSerialize.NameViews is null)
        {
            VCardToSerialize.NameViews = Array.Empty<NameProperty>();
        }
    }


    internal void AppendBase64Data(string base64Data)
    {
        _ = Builder.Append(VCard.NewLine);
        int i = Builder.Length;
        _ = Builder.Append(base64Data);


        while (i < Builder.Length)
        {
            _ = Builder.Insert(i, ' ');

            i = Math.Min(i + 64, Builder.Length);

            _ = Builder.Insert(i, VCard.NewLine);

            i += VCard.NewLine.Length;
        }

        //// mindestens 1 Zeichen muss in der letzten Datenzeile verbleiben
        //for (int i = startOfBase64; i < Builder.Length - 1; i += VCard.MAX_BYTES_PER_LINE)
        //{
        //    _ = Builder.Insert(i, VCard.NewLine);
        //    i += VCard.NewLine.Length;
        //}

        //_ = Builder.Append(VCard.NewLine); //Leerzeile nach den Daten
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
                            _ = Builder.Insert(current, VCard.NewLine);
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
                        _ = Builder.Insert(i, VCard.NewLine);
                        i += VCard.NewLine.Length + 1; // das Leerzeichen
                        counter = 1; // um das Leerzeichen vorschieben
                    }
                }
            }
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

    protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
    {
        Debug.Assert(value != null);

        bool multiple = Options.IsSet(VcfOptions.AllowMultipleAdrAndLabelInVCard21);
        bool first = true;
        foreach (AddressProperty? prop in value.Where(x => x != null).OrderBy(x => x!.Parameters.Preference))
        {
            bool isPref = first && multiple && prop!.Parameters.Preference < 100;
            first = false;

            // AddressProperty.IsEmpty ist auch dann false, wenn lediglich
            // AddressProperty.Parameters.Label != null ist:
            if (!prop!.Value.IsEmpty || Options.IsSet(VcfOptions.WriteEmptyProperties))
            {
                BuildProperty(VCard.PropKeys.ADR, prop, isPref);
            }

            string? label = prop.Parameters.Label;

            if (label != null)
            {
                var labelProp = new TextProperty(label, prop.Group);
                labelProp.Parameters.Assign(prop.Parameters);
                BuildProperty(VCard.PropKeys.LABEL, labelProp, isPref);
            }

            if (!multiple) { break; }
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

    protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value) => BuildXImpps(value);

    protected override void AppendKeys(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.KEY, value);

    protected override void AppendLastRevision(TimeStampProperty value) => BuildProperty(VCard.PropKeys.REV, value);

    protected override void AppendLogos(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.LOGO, value);

    protected override void AppendMailer(TextProperty value) => BuildProperty(VCard.PropKeys.MAILER, value);

    protected override void AppendNameViews(IEnumerable<NameProperty?> value)
    {
        Debug.Assert(value != null);

        NameProperty? name = value.FirstOrDefault(x => x != null && (Options.IsSet(VcfOptions.WriteEmptyProperties) || !x.IsEmpty))
            ?? (Options.IsSet(VcfOptions.WriteEmptyProperties) ? new NameProperty() : new NameProperty(new string[] { "?" }));

        BuildProperty(VCard.PropKeys.N, name);
    }

    protected override void AppendNotes(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.NOTE, value);

    protected override void AppendOrganizations(IEnumerable<OrganizationProperty?> value) => BuildPrefProperty(VCard.PropKeys.ORG, value);

    protected override void AppendPhoneNumbers(IEnumerable<TextProperty?> value) => BuildPropertyCollection(VCard.PropKeys.TEL, value);

    protected override void AppendPhotos(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.PHOTO, value);

    protected override void AppendRelations(IEnumerable<RelationProperty?> value) => base.AppendRelations(value);

    protected override void AppendRoles(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.ROLE, value);

    protected override void AppendSounds(IEnumerable<DataProperty?> value) => BuildPrefProperty(VCard.PropKeys.SOUND, value);

    protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value) => BuildPrefProperty(VCard.PropKeys.TZ, value);

    protected override void AppendTitles(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.TITLE, value);

    protected override void AppendUniqueIdentifier(UuidProperty value) => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value) => BuildPrefProperty(VCard.PropKeys.URL, value);

}
