using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_2_1Serializer : VcfSerializer
{
    internal Vcf_2_1Serializer(TextWriter writer,
                               VcfOptions options,
                               ITimeZoneIDConverter? tzConverter)
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

    internal override void AppendBase64EncodedData(byte[]? data)
    {
        if (data is null)
        {
            return;
        }

        _ = Builder.Append(VCard.NewLine);

        int i = Builder.Length;
        _ = Builder.AppendBase64(data);

        while (i < Builder.Length)
        {
            _ = Builder.Insert(i, ' ');

            i = Math.Min(i + 64, Builder.Length);

            _ = Builder.Insert(i, VCard.NewLine);

            i += VCard.NewLine.Length;
        }
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
                        i += QuotedPrintable.NEW_LINE.Length;
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

    protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
    {
        Debug.Assert(value != null);

        bool multiple = Options.IsSet(VcfOptions.AllowMultipleAdrAndLabelInVCard21);
        bool first = true;

        foreach (AddressProperty prop in value.OrderByPrefIntl(IgnoreEmptyItems))
        {
            bool isPref = first && multiple && prop!.Parameters.Preference < 100;
            first = false;

            // AddressProperty.IsEmpty ist auch dann false, wenn lediglich
            // AddressProperty.Parameters.Label != null ist:
            if (!prop!.Value.IsEmpty || !IgnoreEmptyItems)
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

    protected override void AppendAnniversaryViews(IEnumerable<DateAndOrTimeProperty?> value)
        => base.AppendAnniversaryViews(value);

    protected override void AppendBirthDayViews(IEnumerable<DateAndOrTimeProperty?> value)
        => BuildFirstProperty(VCard.PropKeys.BDAY, 
                              value, 
                              static x => x is DateOnlyProperty or DateTimeOffsetProperty);
   
    protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value != null);

        TextProperty? displayName = value.PrefOrNullIntl(IgnoreEmptyItems);

        if (displayName is null)
        {
            var name = VCardToSerialize.NameViews?.FirstOrNullIntl(IgnoreEmptyItems);

            if (name is not null)
            {
                displayName = new TextProperty(name.ToDisplayName());
            }
        }

        BuildProperty(VCard.PropKeys.FN, 
                      displayName ?? new TextProperty(IgnoreEmptyItems ? "?" : null));
    }

    protected override void AppendEmailAddresses(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.EMAIL, value);

    protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        => base.AppendGenderViews(value);

    protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.GEO, value);

    protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
        => BuildXImpps(value);

    protected override void AppendKeys(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.KEY, value,
                             static x => x is EmbeddedBytesProperty or EmbeddedTextProperty);

    protected override void AppendLastRevision(TimeStampProperty value)
        => BuildProperty(VCard.PropKeys.REV, value);

    protected override void AppendLogos(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.LOGO, 
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendMailer(TextProperty value)
        => BuildProperty(VCard.PropKeys.MAILER, value);

    protected override void AppendNameViews(IEnumerable<NameProperty?> value)
    {
        Debug.Assert(value != null);

        NameProperty? name = value.FirstOrNullIntl(IgnoreEmptyItems)
                             ?? (IgnoreEmptyItems
                                   ? new NameProperty(new string[] { "?" })
                                   : new NameProperty());
        BuildProperty(VCard.PropKeys.N, name);
    }

    protected override void AppendNotes(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.NOTE, value);

    protected override void AppendOrganizations(IEnumerable<OrganizationProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.ORG, value);

    protected override void AppendPhones(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TEL, value);

    protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.PHOTO, 
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        => base.AppendRelations(value);

    protected override void AppendRoles(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.ROLE, value);

    protected override void AppendSounds(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.SOUND,
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.TZ, value);

    protected override void AppendTitles(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.TITLE, value);

    protected override void AppendUniqueIdentifier(UuidProperty value)
        => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.URL, value);
}
