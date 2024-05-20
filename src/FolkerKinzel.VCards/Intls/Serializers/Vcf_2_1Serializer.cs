using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using Base64Bcl = System.Buffers.Text.Base64;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_2_1Serializer : VcfSerializer
{
    internal Vcf_2_1Serializer(TextWriter writer,
                               Opts options,
                               ITimeZoneIDConverter? tzConverter)
        : base(writer, options, new ParameterSerializer2_1(options), tzConverter) { }

    internal override VCdVersion Version => VCdVersion.V2_1;

    protected override string VersionString => "2.1";

    protected override void ReplenishRequiredProperties()
    {
        if (VCardToSerialize.NameViews is null)
        {
            VCardToSerialize.NameViews = [];
        }
    }

    protected override void SetPropertyIDs()
    {
        // Do nothing
    }

    protected override void SetIndexes()
    {
        // Do nothing
    }

    internal override void AppendBase64EncodedData(byte[]? data)
    {
        // Append the NewLine in any case: The parser
        // needs it to detect the end of the Base64
        _ = Builder.Append(VCard.NewLine);

        if (data is null || data.Length == 0)
        {
            return;
        }

        int base64CharsCount = Base64Bcl.GetMaxEncodedToUtf8Length(data.Length);

        using ArrayPoolHelper.SharedArray<byte> byteBuf = ArrayPoolHelper.Rent<byte>(base64CharsCount);
        _ = Base64Bcl.EncodeToUtf8(data, byteBuf.Array.AsSpan(), out _, out _);

        using ArrayPoolHelper.SharedArray<char> charBuf = ArrayPoolHelper.Rent<char>(base64CharsCount);
        _ = Encoding.UTF8.GetChars(byteBuf.Array, 0, base64CharsCount, charBuf.Array, 0);

        Builder.EnsureCapacity(
            Builder.Length + base64CharsCount + (base64CharsCount / VCard.MAX_BYTES_PER_LINE + 1) * 3);

        const int chunkLength = VCard.MAX_BYTES_PER_LINE - 1;
        int end = base64CharsCount - chunkLength;
        int i = 0;

        for (; i < end; i += chunkLength)
        {
            _ = Builder.Append(' ')
                       .Append(charBuf.Array, i, chunkLength)
                       .Append(VCard.NewLine);
        }

        _ = Builder.Append(' ')
                   .Append(charBuf.Array, i, base64CharsCount - i)
                   .Append(VCard.NewLine);
    }


    protected override void WriteLineFolded()
    {
        Debug.Assert(Builder.Length > 0);

        using var shared = ArrayPoolHelper.Rent<char>(Builder.Length);
        Builder.CopyTo(0, shared.Array, 0, Builder.Length);
        ReadOnlySpan<char> span = shared.Array.AsSpan(0, Builder.Length);

        int counter = 0;
        int chunkStart = 0;

        for (int i = 0; i < span.Length - 1; i++)
        {
            char c = span[i];

            if (c == '\r') //Quoted-Printable soft line break in an embedded AGENT vCard
            {
                i++;         // '\n'
                counter = 0;
                continue;
            }
            else
            {
                counter++; // normal ASCII characters

                if (counter == VCard.MAX_BYTES_PER_LINE)
                {
                    if (span[i + 1] == '\r') //Quoted-Printable soft line break in an embedded AGENT vCard
                    {
                        i += QuotedPrintable.NEW_LINE.Length;
                        counter = 0;
                        continue;
                    }

                    // at least 1 char per line:
                    for (int j = i; j > chunkStart; j--) // find the last white-space character
                    {                                     // and in insert a line break before it

                        c = span[j];

                        if (c == ' ' || c == '\t')
                        {
                            _writer.WriteLine(shared.Array, chunkStart, j - chunkStart);

                            chunkStart = j;
                            i = j;
                            counter = 1; // offset of the SPACE or TAB char

                            break; // inner for-loop
                        }
                    }// for
                }
                else if (counter > VCard.MAX_BYTES_PER_LINE) // if none SPACE or TAB char 
                {                                            // has been found before
                    if (c == ' ' || c == '\t')
                    {
                        _writer.WriteLine(shared.Array, chunkStart, i - chunkStart);
                        chunkStart = i;
                        counter = 1; // offset of the SPACE or TAB char
                    }
                }
            }//else
        }// for

        _writer.WriteLine(shared.Array, chunkStart, span.Length - chunkStart);
    }

    protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
    {
        Debug.Assert(value is not null);

        bool multiple = Options.IsSet(Opts.AllowMultipleAdrAndLabelInVCard21);
        bool first = true;

        foreach (AddressProperty prop in value.OrderByPrefIntl(IgnoreEmptyItems))
        {
            bool isPref = first && multiple && prop.Parameters.Preference < 100;

            // AddressProperty.IsEmpty returns false if only
            // AddressProperty.Parameters.Label  is not null:
            if (!prop!.Value.IsEmpty || !IgnoreEmptyItems)
            {
                BuildProperty(VCard.PropKeys.ADR, prop, isPref);
            }

            PreserveLabel(prop, isPref);

            if (first)
            {
                PreserveGeoCoordinate(prop);
                PreserveTimeZoneID(prop);
            }

            first = false;


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
        => BuildPrefProperty(VCard.PropKeys.FN, value);

    protected override void AppendEMails(IEnumerable<TextProperty?> value)
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
        Debug.Assert(value is not null);

        NameProperty? name = value.FirstOrNullIntl(IgnoreEmptyItems)
                             ?? (IgnoreEmptyItems
                                   ? new NameProperty("?")
                                   : new NameProperty());
        BuildProperty(VCard.PropKeys.N, name);
    }

    protected override void AppendNotes(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.NOTE, value);

    protected override void AppendOrganizations(IEnumerable<OrgProperty?> value)
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

    protected override void AppendUniqueIdentifier(IDProperty value)
        => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.URL, value);

    private void PreserveLabel(AddressProperty prop, bool isPref)
    {
        string? label = prop.Parameters.Label;

        if (label is not null)
        {
            var labelProp = new TextProperty(label, prop.Group);
            labelProp.Parameters.Assign(prop.Parameters);
            BuildProperty(VCard.PropKeys.LABEL, labelProp, isPref);
        }
    }

    private void PreserveGeoCoordinate(AddressProperty prop)
    {
        GeoCoordinate? geo = prop.Parameters.GeoPosition;

        if (geo is not null)
        {
            GeoProperty? geoProp = VCardToSerialize
                                      .GeoCoordinates?
                                      .PrefOrNullIntl(IgnoreEmptyItems);

            if (geoProp is null)
            {
                BuildProperty(VCard.PropKeys.GEO,
                              new GeoProperty(geo, prop.Group),
                              false);
            }
        }
    }

    private void PreserveTimeZoneID(AddressProperty prop)
    {
        TimeZoneID? tz = prop.Parameters.TimeZone;

        if (tz is not null)
        {
            TimeZoneProperty? tzProp = VCardToSerialize
                                            .TimeZones?
                                            .PrefOrNullIntl(IgnoreEmptyItems);

            if (tzProp is null)
            {
                BuildProperty(VCard.PropKeys.TZ,
                              new TimeZoneProperty(tz, prop.Group),
                              false);
            }
        }
    }

}
