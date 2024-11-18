using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V2Tests
{
    [TestMethod]
    public void Parse()
    {
        IReadOnlyList<VCard> vcard = Vcf.Load(fileName: TestFiles.V2vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void ParseOutlook()
    {
        IReadOnlyList<VCard> vcard = Vcf.Load(fileName: TestFiles.OutlookV2vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);

        //string s = vcard[0].ToString();

        DataProperty? photo = vcard[0].Photos?.FirstOrDefault();
        Assert.IsNotNull(photo);

        Assert.AreEqual(photo?.Parameters.MediaType, "image/jpeg");
        //System.IO.File.WriteAllBytes(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"Testbild{dataUrl.GetFileExtension()}"), dataUrl.GetEmbeddedBytes());

        Vcf.Save(vcard,
            System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"TestV2.1.vcf"), VCdVersion.V2_1, options: Opts.Default.Set(Opts.WriteNonStandardProperties));
    }

    [TestMethod]
    public void WriteEmptyVCard()
    {
        var vcard = new VCard();
        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IReadOnlyList<VCard> cards = Vcf.Parse(s);

        Assert.AreEqual(cards.Count, 1);

        vcard = cards[0];

        Assert.AreEqual(vcard.Version, VCdVersion.V2_1);

        Assert.IsNotNull(vcard.NameViews);
        Assert.AreEqual(vcard.NameViews!.Count(), 1);
        Assert.IsNotNull(vcard.NameViews!.First());
    }

    [TestMethod]
    public void TestLineWrapping()
    {
        var vcard = new VCard();

        const string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping mit Quoted-Printable-Encoding getestet werden kann. " +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        const string ASCIITEXT = "This is really a very very long ASCII-Text. This is needed to test the " +
            "vCard 2.1 - LineWrapping. That's why I have to write so much even though I have nothing to say.";

        byte[] bytes = CreateBytes();

        vcard.Notes =
        [
                new(UNITEXT)
        ];

        vcard.Keys = new DataProperty(RawData.FromText(ASCIITEXT));
        vcard.Photos = new DataProperty(RawData.FromBytes(bytes, "image/jpeg"));

        string s = vcard.ToVcfString(VCdVersion.V2_1);

        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x is not null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = Vcf.Parse(s);

        Assert.AreEqual(vcard.Keys?.First()?.Value?.String, ASCIITEXT);
        Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, "image/jpeg");
        Assert.IsTrue(vcard.Photos?.First()?.Value?.Bytes?.SequenceEqual(bytes) ?? false);


        static byte[] CreateBytes()
        {
            const int DATA_LENGTH = 200;

            byte[] arr = new byte[DATA_LENGTH];

            for (byte i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }

            return arr;
        }
    }

    [TestMethod]
    public void SerializeVCard()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V2_1, options: Opts.All);

        Assert.IsNotNull(s);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];
        Assert.AreEqual(VCdVersion.V2_1, vcard.Version);
    }

    [TestMethod]
    public void MoreThanOneAddressTest1()
    {
        const string label0 = "Elmstreet 13";
        const string label1 = "Sackgasse 5";

        var addr0 = new AddressProperty(AddressBuilder.Create().AddStreet(label0).AddLocality("Entenhausen").AddPostalCode("01234").Build());
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr0.Parameters.PropertyClass = PCl.Home;

        var addr1 = new AddressProperty(AddressBuilder.Create().AddStreet(label1).AddLocality("Borna").AddPostalCode("43210").Build());
        addr1.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr1.Parameters.PropertyClass = PCl.Work;
        addr1.Parameters.Label = label1;

        var vc = new VCard
        {
            Addresses = [addr1, null, addr0]
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        IReadOnlyList<VCard> vCards = Vcf.Parse(vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.AreEqual(2, addresses!.Count());
        Assert.IsNotNull(addresses!.FirstOrDefault(x => x!.Parameters.Label == label0 && x.Value.Street[0] == label0));
        Assert.IsNotNull(addresses!.FirstOrDefault(x => x!.Parameters.Label == label1 && x.Value.Street[0] == label1));

    }

    [TestMethod]
    public void MoreThanOneAddressTest2()
    {
        const string label0 = "Elmstreet 13";
        const string label1 = "Sackgasse 5";

        var addr0 = new AddressProperty(AddressBuilder.Create().AddStreet(label0).AddLocality("Entenhausen").AddPostalCode("01234").Build());
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr0.Parameters.PropertyClass = PCl.Home;

        var addr1 = new AddressProperty(AddressBuilder.Create().AddStreet(label1).AddLocality("Borna").AddPostalCode("43210").Build());
        addr1.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr1.Parameters.PropertyClass = PCl.Work;
        addr1.Parameters.Label = label1;

        var vc = new VCard
        {
            Addresses = [addr1, null, addr0]
        };

        var arr = new VCard[] { vc };

        string vcf = vc.ToVcfString(VCdVersion.V2_1, options: Opts.Default.Unset(Opts.AllowMultipleAdrAndLabelInVCard21));
        IReadOnlyList<VCard> vCards = Vcf.Parse(vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.AreEqual(1, addresses!.Count());
        Assert.AreEqual(100, addresses!.First()!.Parameters.Preference);
        Assert.IsNotNull(addresses!.FirstOrDefault(x => x!.Parameters.Label == label0 && x.Value.Street[0] == label0));

    }

    [TestMethod]
    public void SpouseTest1()
    {
        var relProp = new RelationProperty(
            Relation.Create(new VCard { NameViews = new NameProperty(NameBuilder.Create()
                                                                                .AddSurname("wife")
                                                                                .AddGiven("best")
                                                                                .Build()) }));
        relProp.Parameters.RelationType = Rel.Spouse;

        var vc = new VCard
        {
            Relations = relProp
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(Rel.Spouse, vc.Relations?.First()?.Parameters.RelationType);

    }

    [TestMethod]
    public void SpouseTest2()
    {
        var relProp = new RelationProperty(Relation.Create(new VCard()));
        relProp.Parameters.RelationType = Rel.Spouse;
        var vc = new VCard
        {
            Relations = relProp
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        vc = Vcf.Parse(vcf)[0];

        Assert.IsNull(vc.Relations);
    }

    [TestMethod]
    public void SpouseTest3()
    {
        var relProp = new RelationProperty(Relation.Create(new VCard { DisplayNames = new TextProperty("Mausi") }));
        relProp.Parameters.RelationType = Rel.Spouse;

        var vc = new VCard
        {
            Relations = relProp
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(Rel.Spouse, vc.Relations?.First()?.Parameters.RelationType);

    }

    [TestMethod]
    public void SpouseTest4()
    {
        var relProp = new RelationProperty(Relation.Create(new VCard { DisplayNames = new TextProperty(null) }));
        relProp.Parameters.RelationType = Rel.Spouse;
        var vc = new VCard
        {
            Relations = relProp
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        vc = Vcf.Parse(vcf)[0];

        Assert.IsNull(vc.Relations);
    }

    [TestMethod]
    public void PreserveTimeZoneAndGeoTest1()
    {
        var adr = new AddressProperty(AddressBuilder.Create().AddStreet("1").Build());
        adr.Parameters.TimeZone = TimeZoneID.Parse("Europe/Berlin");
        adr.Parameters.GeoPosition = new GeoCoordinate(52, 13);
        adr.Parameters.Label = AddressFormatter.Default.ToLabel(adr);
        Assert.IsNotNull(adr.Parameters.Label);

        var vCard = new VCard { Addresses = adr };

        string vcf = vCard.ToVcfString(VCdVersion.V2_1);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.Addresses);
        adr = vCard.Addresses.First();
        Assert.IsNotNull(adr!.Parameters.Label);
        Assert.IsNotNull(adr.Parameters.GeoPosition);
        Assert.IsNotNull(adr.Parameters.TimeZone);
    }

    [TestMethod]
    public void EmbeddedBytesTest1()
    {
        var vCard = new VCard { Photos = new DataProperty(RawData.FromBytes([])) };

        string s = vCard.ToVcfString(VCdVersion.V2_1, options: Opts.Default.Set(Opts.WriteEmptyProperties));

        vCard = Vcf.Parse(s)[0];
        Assert.IsNotNull(vCard.Photos);
        Assert.IsTrue(vCard.Photos!.First()!.IsEmpty);
    }

    [TestMethod]
    public void LineWrappingTest1()
    {
        byte[] bytes = Enumerable.Range(0 - 255, 255).Select(x => (byte)x).ToArray();
        var agent = new VCard
        {
            DisplayNames = new TextProperty("007"),
            Notes = new TextProperty("ÄÖÜ Veeeeeeeeeeeeeeeeeeeeeeeeeery veeeeeeeeeeeeeeeeeeeeeeeeeeeery looooooooooooooooooooooooooooooooong Quoted-Printable text"),
            Photos = new DataProperty(RawData.FromBytes(bytes))
        };

        var relProp = new RelationProperty(Relation.Create(agent));
        relProp.Parameters.RelationType = Rel.Agent | Rel.Colleague;

        var vCard = new VCard
        {
            DisplayNames = new TextProperty("Secret Service"),

            Relations = relProp
        };

        string s = vCard.ToVcfString(VCdVersion.V2_1, options: Opts.Default.Set(Opts.AppendAgentAsSeparateVCard));

        IReadOnlyList<VCard> vCards = Vcf.Parse(s);
        Assert.AreEqual(2, vCards.Count);
        Assert.IsNotNull(vCards[0].Relations);
    }

    [TestMethod]
    public void EmptyAgentTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const string agent = """
            BEGIN:VCARD
            VERSION:2.1
            FN:Secret Service
            N:?;;;;
            AGENT:

            END:VCARD

            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(agent);
        Assert.AreEqual(1, vcs.Count);
        Assert.IsNotNull(vcs[0].Relations);
    }

    [TestMethod]
    public void UsingTheWhatsAppTypeTest()
    {
        const string whatsAppNumber = "+1-234-567-89";
        var xiamoiMobilePhone = new TextProperty(whatsAppNumber);
        xiamoiMobilePhone.Parameters.NonStandard =
        [
                new("TYPE", "WhatsApp")
        ];

        // Initialize the VCard:
        var vcard = new VCard
        {
            NameViews = new NameProperty(NameBuilder.Create().AddSurname("").AddGiven("zzMad Perla 45").Build()),
            DisplayNames = new TextProperty("zzMad Perla 45"),
            Phones = xiamoiMobilePhone
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = vcard.ToVcfString(version: VCdVersion.V2_1, options: Opts.Default | Opts.WriteNonStandardParameters);

        // Parse the VCF string:
        vcard = Vcf.Parse(vcfString)[0];

        // Find the WhatsApp number:
        Assert.AreEqual(whatsAppNumber, vcard.Phones?
            .FirstOrDefault(x => x?.Parameters.NonStandard?.Any(x => x.Key == "TYPE" && x.Value == "WhatsApp") ?? false)?
            .Value);
    }

    [TestMethod]
    public void KeyTypeTest()
    {
        const string pgpMime = "application/pgp-keys";
        VCard vc = VCardBuilder
            .Create()
            .Keys.AddBytes([1, 2, 3], pgpMime)
            .EMails.Add("goofy@contoso.com", parameters: p => p.EMailType = null)
            .VCard;

        string s = vc.ToVcfString(version: VCdVersion.V2_1);

        vc = Vcf.Parse(s)[0];
        DataProperty? key = vc.Keys.FirstOrNull();
        Assert.IsNotNull(key);
        Assert.AreEqual(pgpMime, key.Parameters.MediaType);

    }

    [TestMethod]
    public void ParseCroppedEmbeddedVCardTest1()
    {
        const string cropped = """
            BEGIN:VCARD
            VERSION:2.1
            AGENT:
            BEGIN:VCARD
            VERSION:2.1
            N:Friday;Fred
            TEL; WORK;VOICE:+1-213-555-1234
            TEL;WORK;FAX:+1-213-555-5678
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(cropped);

        Assert.IsNotNull(vcs);
        Assert.AreEqual(1, vcs.Count);
        Assert.IsFalse(vcs[0].Entities.Any());
    }

    [TestMethod]
    public void ParseCroppedVCardTest1()
    {
        const string cropped = """
            BEGIN:VCARD
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(cropped);

        Assert.IsNotNull(vcs);
        Assert.AreEqual(0, vcs.Count);
    }

    [TestMethod]
    public void Rfc2425Test1()
    {
        const string s = """
            begin:vcard
            source:ldap://cn=Meister%20Berger,o=Universitaet%20Goerlitz,c=DE
            name:Meister Berger
            fn:Meister Berger
            n:Berger;Meister
            bday;value=date:1963-09-21
            o:Universit=E6t G=F6rlitz
            title:Mayor
            title;language=de;value=text:Burgermeister
            note:The Mayor of the great city of
              Goerlitz in the great country of Germany.
            email;internet:mb@goerlitz.de
            home.tel;type=fax,voice,msg:+49 3581 123456
            home.label:Hufenshlagel 1234\n
             02828 Goerlitz\n
             Deutschland
            key;type=X509;encoding=b:MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQ
             AwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05ldHNjYXBlIENvbW11bmljYXRpb25zI
             ENvcnBvcmF0aW9uMRwwGgYDVQQLExNJbmZvcm1hdGlvbiBTeXN0ZW1zMRwwGgYDVQQD
             ExNyb290Y2EubmV0c2NhcGUuY29tMB4XDTk3MDYwNjE5NDc1OVoXDTk3MTIwMzE5NDc
             1OVowgYkxCzAJBgNVBAYTAlVTMSYwJAYDVQQKEx1OZXRzY2FwZSBDb21tdW5pY2F0aW
             9ucyBDb3JwLjEYMBYGA1UEAxMPVGltb3RoeSBBIEhvd2VzMSEwHwYJKoZIhvcNAQkBF
             hJob3dlc0BuZXRzY2FwZS5jb20xFTATBgoJkiaJk/IsZAEBEwVob3dlczBcMA0GCSqG
             SIb3DQEBAQUAA0sAMEgCQQC0JZf6wkg8pLMXHHCUvMfL5H6zjSk4vTTXZpYyrdN2dXc
             oX49LKiOmgeJSzoiFKHtLOIboyludF90CgqcxtwKnAgMBAAGjNjA0MBEGCWCGSAGG+E
             IBAQQEAwIAoDAfBgNVHSMEGDAWgBT84FToB/GV3jr3mcau+hUMbsQukjANBgkqhkiG9
             w0BAQQFAAOBgQBexv7o7mi3PLXadkmNP9LcIPmx93HGp0Kgyx1jIVMyNgsemeAwBM+M
             SlhMfcpbTrONwNjZYW8vJDSoi//yrZlVt9bJbs7MNYZVsyF1unsqaln4/vy6Uawfg8V
             UMk1U7jt8LYpo4YULU7UZHPYVUaSgVttImOHZIKi4hlPXBOhcUQ==
            end:vcard
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(s);

        Assert.IsNotNull(vcs);
        Assert.AreEqual(1, vcs.Count);
        Assert.AreEqual(1, vcs[0].Keys!.Count());

        //string parsed = vcs[0].ToString();
    }

    [TestMethod]
    public void ParseCroppedBase64Test1()
    {
        const string s = """
            begin:vcard
            n:Berger;Meister
            key;type=X509;encoding=b:MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQ
            AwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05ldHNjYXBlIENvbW11bmljYXRpb25zI
            ENvcnBvcmF0aW9uMRwwGgYDVQQLExNJbmZvcm1hdGlvbiBTeXN0ZW1zMRwwGgYDVQQD
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(s);

        Assert.IsNotNull(vcs);
        Assert.AreEqual(1, vcs.Count);
        Assert.IsNull(vcs[0].Keys);

        //string parsed = vcs[0].ToString();
    }

    [TestMethod]
    public void GenderTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Add(Sex.Female, 
                             parameters: p => p.NonStandard = new Dictionary<string, string> { { "X-TEST", "test"} },
                             group: vc => "GROUP")
            .VCard;

        string serialized = vc.ToVcfString(VCdVersion.V2_1, options: Opts.All);

        StringAssert.Contains(serialized, "GROUP.X-GENDER;X-TEST=test:Female");
        StringAssert.Contains(serialized, "GROUP.X-WAB-GENDER;X-TEST=test:1");

        vc = Vcf.Parse(serialized)[0];

        Assert.IsNotNull(vc);
        Assert.IsNotNull(vc.GenderViews);
        Assert.AreEqual(1, vc.GenderViews.Count());

        GenderProperty prop = vc.GenderViews.First()!;
        Assert.AreEqual(Sex.Female, prop.Value.Sex);
        Assert.AreEqual("GROUP", prop.Group);
        Assert.IsNotNull(prop.Parameters.NonStandard);
    }

    [TestMethod]
    public void BirthdayTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(DateTimeOffset.Now)
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V2_1);

        StringAssert.Contains(vcf, "BDAY", StringComparison.Ordinal);
    }

    [TestMethod]
    public void BirthdayTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add("After midnight")
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V2_1);

        Assert.IsFalse(vcf.Contains("BDAY", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void LogoPhotoSoundTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Logos.AddText("text")
            .Photos.AddText("text")
            .Sounds.AddText("text")
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V2_1);

        Assert.IsFalse(vcf.Contains("LOGO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("PHOTO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("SOUND", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void QuotedPrintableEncodedUriTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            PHOTO;URL;QUOTED-PRINTABLE;UTF-8:http://k=C3=A4se.com
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc.Photos);
        Assert.AreEqual("http://käse.com", vc.Photos.First()!.Value?.Uri?.OriginalString);
    }

    [TestMethod]
    public void EmptyImppTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            X-SKYPE:
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vc.Messengers);
    }

    [TestMethod]
    public void XAssistantTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            X-SPOUSE:Spouse
            X-ASSISTANT:Assistant
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vc.Relations);
    }

    [TestMethod]
    public void XWabGenderTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            X-WAB-GENDER:2
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];
        GenderProperty? gender = vc.GenderViews.FirstOrNull();
        Assert.IsNotNull(gender);
        Assert.AreEqual(Sex.Male, gender.Value.Sex);
    }

    [TestMethod]
    public void XWabGenderTest2()
    {
        const string vcf = """
            BEGIN:VCARD
            X-WAB-GENDER:1
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];
        GenderProperty? gender = vc.GenderViews.FirstOrNull();
        Assert.IsNotNull(gender);
        Assert.AreEqual(Sex.Female, gender.Value.Sex);
    }
}
