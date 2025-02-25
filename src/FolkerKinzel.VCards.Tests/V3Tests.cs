using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V3Tests
{
    [TestMethod]
    public void ParseTest()
    {
        IReadOnlyList<VCard>? vcard = Vcf.Load(TestFiles.V3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreEqual(2, vcard.Count);
    }


    [TestMethod]
    public void ParseTest2()
    {
        IReadOnlyList<VCard>? vcard = Vcf.Load(@"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf");

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void ParseTest3()
    {
        IReadOnlyList<VCard>? vcard = Vcf.Load(TestFiles.PhotoV3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCardTest()
    {
        var vcard = new VCard();

        string s = vcard.ToVcfString(VCdVersion.V3_0);

        IReadOnlyList<VCard>? cards = Vcf.Parse(s);

        Assert.AreEqual(cards.Count, 1);

        vcard = cards[0];

        Assert.AreEqual(VCdVersion.V3_0, vcard.Version);

        Assert.IsNotNull(vcard.DisplayNames);
        Assert.AreEqual(vcard.DisplayNames!.Count(), 1);
        Assert.IsNotNull(vcard.DisplayNames!.First());
        Assert.IsNotNull(vcard.NameViews);
        Assert.AreEqual(vcard.NameViews!.Count(), 1);
        Assert.IsNotNull(vcard.NameViews!.First());
    }


    [TestMethod]
    public void LineWrappingTest()
    {
        var vcard = new VCard();

        string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        const string ASCIITEXT = "This is really a very very long ASCII-Text. This is needed to test the " +
            "LineWrapping. That's why I have to write so much even though I have nothing to say." +
            "For a better test, I write the same again: " +
            "This is really a very very long ASCII-Text. This is needed to test the " +
            "LineWrapping. That's why I have to write so much even though I have nothing to say.";

        byte[] bytes = CreateBytes();

        vcard.Notes =
        [
                new(UNITEXT)
        ];

        vcard.Keys = new DataProperty(RawData.FromText(ASCIITEXT));

        vcard.Photos = new DataProperty(RawData.FromBytes(bytes, "image/jpeg"));

        string s = vcard.ToVcfString(VCdVersion.V3_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split([VCard.NewLine], StringSplitOptions.None)
            .All(x => x is not null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = Vcf.Parse(s);

        Assert.AreEqual(vcard.Keys?.First()?.Value.String, ASCIITEXT);
        Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, "image/jpeg");
        Assert.IsTrue(vcard.Photos?.First()?.Value.Bytes?.SequenceEqual(bytes) ?? false);


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
    public void SaveTest()
    {
        var vcard = new VCard();

        string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        vcard.NameViews =
        [
                new(NameBuilder.Create().AddSurname("Test").AddGiven("Paul").Build())
        ];

        vcard.DisplayNames =
        [
                new("Paul Test")
        ];

        vcard.Notes =
        [
                new(UNITEXT)
        ];

        vcard.SaveVcf(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Paul Test.vcf"));
    }


    [TestMethod]
    public void SerializeVCardTest1()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V3_0, options: VcfOpts.All);

        Assert.IsNotNull(s);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);

        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];

        Assert.AreEqual(VCdVersion.V3_0, vcard.Version);
        Assert.IsNotNull(vcard.DirectoryName);
        Assert.IsNotNull(vcard.Mailer);
    }

    [TestMethod]
    public void SerializeVCardTest2()
    {
        List<VCard> vc = [.. Vcf.Load(TestFiles.PhotoV3vcf)];
        vc.Add(vc[0]);

        string s = vc.ToVcfString();

        IReadOnlyList<VCard> vc2 = Vcf.Parse(s);

        Assert.AreEqual(2, vc2.Count);
    }

    [TestMethod]
    public void SerializeVCardTest3()
    {
        const string octetStream = "application/octet-stream";
        string base64 = Convert.ToBase64String([1, 2, 3]);

        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(new Uri("http://the_agent.com"), Rel.Agent)
            .Logos.AddBytes([4, 5, 6])
            .Keys.AddBytes([7, 8])
            .Sounds.AddBytes([8, 9])
            .NonStandards.Add("X-DATA", base64, parameters: p => { p.Encoding = Enc.Base64; p.MediaType = octetStream; })
            .VCard;

        string vcf = Vcf.AsString(vc, VCdVersion.V3_0, options: VcfOpts.Default.Set(VcfOpts.WriteNonStandardProperties));
        vc = Vcf.Parse(vcf)[0];

        Assert.AreEqual(Data.Uri, vc.Relations!.First()!.Parameters.DataType);
        Assert.AreEqual(Enc.Base64, vc.Logos!.First()!.Parameters.Encoding);
        Assert.AreEqual(Enc.Base64, vc.Keys!.First()!.Parameters.Encoding);
        Assert.AreEqual(Enc.Base64, vc.Sounds!.First()!.Parameters.Encoding);
        Assert.AreEqual(Enc.Base64, vc.NonStandards!.First()!.Parameters.Encoding);
        Assert.AreEqual(octetStream, vc.NonStandards!.First()!.Parameters.NonStandard!.First().Value);
    }


    [TestMethod]
    public void TimeDataTypeTest()
    {
        const string vcardString = @"BEGIN:VCARD
VERSION:3.0
BDAY;Value=Time:05:30:00
END:VCARD";

        VCard vcard = Vcf.Parse(vcardString)[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        DateAndOrTimeProperty? bday = vcard.BirthDayViews!.First();

        Assert.AreEqual(new TimeOnly(5, 30, 0), bday?.Value.TimeOnly);
    }


    [TestMethod]
    public void NonStandardParameterTest1()
    {
        const string whatsAppNumber = "+1-234-567-89";
        var xiamoiMobilePhone = new TextProperty(whatsAppNumber);
        xiamoiMobilePhone.Parameters.NonStandard =
        [
                new("TYPE", "WhatsApp")
        ];

        var vcard = new VCard
        {
            Phones = xiamoiMobilePhone
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = vcard.ToVcfString(options: VcfOpts.Default | VcfOpts.WriteNonStandardParameters);
        vcard = Vcf.Parse(vcfString)[0];

        // Find the WhatsApp number:
        string? readWhatsAppNumber = vcard.Phones?
            .FirstOrDefault(x => x?.Parameters.NonStandard?.Any(x => x.Key == "TYPE" && x.Value == "WhatsApp") ?? false)?
            .Value;
        Assert.AreEqual(whatsAppNumber, readWhatsAppNumber);

    }

    [TestMethod]
    public void ImppTest1()
    {
        const string mobilePhoneNumber = "tel:+1-234-567-89";
        var whatsAppImpp = new TextProperty(mobilePhoneNumber);

        const Impp messengerTypes = Impp.Personal
                                | Impp.Business
                                | Impp.Mobile;
        whatsAppImpp.Parameters.InstantMessengerType = Impp.Personal
                                                     | Impp.Business
                                                     | Impp.Mobile;

        var vcard = new VCard
        {
            Messengers = whatsAppImpp
        };

        string vcfString = vcard.ToVcfString(options: VcfOpts.Default | VcfOpts.WriteXExtensions);
        vcard = Vcf.Parse(vcfString)[0];

        whatsAppImpp = vcard.Messengers?.First();

        Assert.AreEqual(mobilePhoneNumber, whatsAppImpp?.Value);
        Assert.AreEqual(messengerTypes, whatsAppImpp?.Parameters.InstantMessengerType);
    }

    [TestMethod]
    public void ImppTest2()
    {
        const string mobilePhoneNumber = "tel:+1-234-567-89";
        var whatsAppImpp = new TextProperty(mobilePhoneNumber);

        const Impp messengerTypes = Impp.Personal
                                | Impp.Business
                                | Impp.Mobile;

        whatsAppImpp.Parameters.InstantMessengerType = Impp.Mobile;
        whatsAppImpp.Parameters.PropertyClass = PCl.Home | PCl.Work;

        var vcard = new VCard
        {
            Messengers = whatsAppImpp
        };

        string vcfString = vcard.ToVcfString(options: VcfOpts.Default);
        vcard = Vcf.Parse(vcfString)[0];

        whatsAppImpp = vcard.Messengers?.First();

        Assert.AreEqual(mobilePhoneNumber, whatsAppImpp?.Value);
        Assert.AreEqual(messengerTypes, whatsAppImpp?.Parameters.InstantMessengerType);
    }

    [TestMethod]
    public void ImppTest3()
    {
        const string serialized = """
            BEGIN:VCARD
            VERSION:3.0
            IMPP;TYPE=OTHER:abc
            END:VCARD
            """;

        VCard vc = Vcf.Parse(serialized)[0];
        Assert.IsNotNull(vc.Messengers?.First()!.Parameters.NonStandard);
    }

    [TestMethod]
    public void XMessengerTest1()
    {
        const string mobilePhoneNumber = "skype:+1-234-567-89";
        var prop = new TextProperty(mobilePhoneNumber);
        const Impp imppTypes = Impp.Mobile | Impp.Personal;

        prop.Parameters.InstantMessengerType = imppTypes;

        var vcard = new VCard
        {
            Messengers = prop
        };

        string vcfString = vcard.ToVcfString(options: (VcfOpts.Default | VcfOpts.WriteXExtensions).Unset(VcfOpts.WriteImppExtension));
        vcard = Vcf.Parse(vcfString)[0];

        Assert.AreEqual(1, vcard.Messengers!.Count());
        prop = vcard.Messengers?.First();
        Assert.AreEqual(mobilePhoneNumber, prop?.Value);
        Assert.AreEqual(PCl.Home, prop?.Parameters.PropertyClass);
        Assert.AreEqual(imppTypes, prop?.Parameters.InstantMessengerType);

    }

    [TestMethod]
    public void XMessengerTest2()
    {
        const string mobilePhoneNumber = "skype:+1-234-567-89";
        var prop = new TextProperty(mobilePhoneNumber);
        const Impp imppTypes = Impp.Mobile | Impp.Personal;
        prop.Parameters.InstantMessengerType = imppTypes;

        var vcard = new VCard
        {
            Messengers = prop
        };

        string vcfString = vcard.ToVcfString(options: VcfOpts.Default | VcfOpts.WriteXExtensions);
        vcard = Vcf.Parse(vcfString)[0];
        Assert.AreEqual(1, vcard.Messengers!.Count());
        prop = vcard.Messengers!.First();
        Assert.AreEqual(mobilePhoneNumber, prop?.Value);
        Assert.AreEqual(imppTypes, prop?.Parameters.InstantMessengerType);
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

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.Addresses);
        adr = vCard.Addresses.First();
        Assert.IsNotNull(adr);
        Assert.IsNotNull(adr.Parameters.Label);
        Assert.IsNotNull(adr.Parameters.GeoPosition);
        Assert.IsNotNull(adr.Parameters.TimeZone);
    }

    [TestMethod]
    public void PreserveTimeZoneAndGeoTest2()
    {
        var adr = new AddressProperty(AddressBuilder.Create().AddStreet("1").Build());
        adr.Parameters.TimeZone = TimeZoneID.Parse("Europe/Berlin");
        adr.Parameters.GeoPosition = new GeoCoordinate(52, 13);
        adr.Parameters.Label = AddressFormatter.Default.ToLabel(adr);
        Assert.IsNotNull(adr.Parameters.Label);

        var vCard = new VCard { Addresses = adr };
        adr.Group = vCard.NewGroup();

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.Addresses);
        adr = vCard.Addresses.First();
        Assert.IsNotNull(adr!.Parameters.Label);
        Assert.IsNotNull(adr.Parameters.GeoPosition);
        Assert.IsNotNull(adr.Parameters.TimeZone);
    }

    [TestMethod]
    public void DisplayNameTest1()
    {
        var nm = new NameProperty(NameBuilder.Create().AddSurname("Kinzel").AddGiven("Folker").Build());

        var vCard = new VCard { NameViews = nm };

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.AreEqual("Folker Kinzel", tProp.Value);
    }

    [TestMethod]
    public void DisplayNameTest2()
    {
        var vCard = new VCard { NameViews = [null] };

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.IsFalse(tProp.IsEmpty);
    }

    [TestMethod]
    public void DisplayNameTest3()
    {
        var vCard = new VCard();

        string vcf = vCard.ToVcfString(VCdVersion.V3_0, options: VcfOpts.Default.Set(VcfOpts.WriteEmptyProperties));

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.IsTrue(tProp.IsEmpty);

        Assert.IsNotNull(vCard.NameViews);
        NameProperty nProp = vCard.NameViews.First()!;
        Assert.IsTrue(nProp.IsEmpty);
    }

    [TestMethod]
    public void SortAsTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddSurname("Name").Build(), parameters: p => p.SortAs = ["abc", "123"])
            .VCard;

        string serialized = vc.ToVcfString();

        vc = Vcf.Parse(serialized)[0];

        IEnumerable<string>? sortAs = vc.NameViews!.First()!.Parameters.SortAs;
        Assert.IsNotNull(sortAs);
        Assert.AreEqual(1, sortAs.Count());
        Assert.AreEqual("abc", sortAs.First());
    }

    [TestMethod]
    public void SortAsTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddSurname("Name").Build())
            .Organizations.Add("Org", parameters: p => p.SortAs = ["abc", "123"])
            .VCard;

        string serialized = vc.ToVcfString();

        vc = Vcf.Parse(serialized)[0];

        IEnumerable<string>? sortAs = vc.NameViews!.First()!.Parameters.SortAs;
        Assert.IsNotNull(sortAs);
        Assert.AreEqual(1, sortAs.Count());
        Assert.AreEqual("abc", sortAs.First());
    }

    [TestMethod]
    public void SortAsTest3()
    {
        const string serialized = """
            BEGIN:VCARD
            VERSION:3.0
            ORG:Org
            SORT-STRING:abc
            END:VCARD
            """;

        VCard vc = Vcf.Parse(serialized)[0];

        IEnumerable<string>? sortAs = vc.Organizations!.First()!.Parameters.SortAs;
        Assert.IsNotNull(sortAs);
        Assert.AreEqual(1, sortAs.Count());
        Assert.AreEqual("abc", sortAs.First());
    }

    [TestMethod]
    public void SortAsTest4()
    {
        const string serialized = """
            BEGIN:VCARD
            VERSION:3.0
            SORT-STRING:abc
            END:VCARD
            """;

        VCard vc = Vcf.Parse(serialized)[0];

        IEnumerable<string>? sortAs = vc.NameViews!.First()!.Parameters.SortAs;
        Assert.IsNotNull(sortAs);
        Assert.AreEqual(1, sortAs.Count());
        Assert.AreEqual("abc", sortAs.First());
    }

    [TestMethod]
    public void SortAsTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddSurname("Name").Build(), parameters: p => p.SortAs = ["abc", "123"])
            .Organizations.Add("Org", parameters: p => p.SortAs = ["xyz"])
            .VCard;

        string serialized = vc.ToVcfString();

        vc = Vcf.Parse(serialized)[0];

        IEnumerable<string>? sortAs = vc.NameViews!.First()!.Parameters.SortAs;
        Assert.IsNotNull(sortAs);
        Assert.AreEqual(1, sortAs.Count());
        Assert.AreEqual("abc", sortAs.First());
    }


    [TestMethod]
    public void Rfc2739Test1()
    {
        VCard vc = VCardBuilder
           .Create(false)
           .CalendarAccessUris.Add("CalendarAccessUriPref")
           .CalendarAccessUris.Add("CalendarAccessUriOther")
           .CalendarAddresses.Add("CalendarAddressPref")
           .CalendarAddresses.Add("CalenderAddressOther")
           .CalendarUserAddresses.Add("CalenderUserAddressPref")
           .CalendarUserAddresses.Add("CalenderUserAddressOther")
           .FreeOrBusyUrls.Add("FbUrlPref")
           .FreeOrBusyUrls.Add("FbUrlOther")
           .CalendarAccessUris.SetPreferences()
           .CalendarAddresses.SetPreferences()
           .CalendarUserAddresses.SetPreferences()
           .FreeOrBusyUrls.SetPreferences()
           .VCard;

        string serialized = vc.ToVcfString();

        vc = Vcf.Parse(serialized)[0];

        Assert.IsNotNull(vc.CalendarAccessUris);
        Assert.AreEqual(2, vc.CalendarAccessUris.Count());
        Assert.AreEqual("CalendarAccessUriPref", vc.CalendarAccessUris.PrefOrNull()!.Value);

        Assert.IsNotNull(vc.CalendarAddresses);
        Assert.AreEqual(2, vc.CalendarAddresses.Count());
        Assert.AreEqual("CalendarAddressPref", vc.CalendarAddresses.PrefOrNull()!.Value);

        Assert.IsNotNull(vc.CalendarUserAddresses);
        Assert.AreEqual(2, vc.CalendarUserAddresses.Count());
        Assert.AreEqual("CalenderUserAddressPref", vc.CalendarUserAddresses.PrefOrNull()!.Value);

        Assert.IsNotNull(vc.FreeOrBusyUrls);
        Assert.AreEqual(2, vc.FreeOrBusyUrls.Count());
        Assert.AreEqual("FbUrlPref", vc.FreeOrBusyUrls.PrefOrNull()!.Value);
    }

    [TestMethod]
    public void Rfc2739Test2()
    {
        VCard vc = VCardBuilder
           .Create(false)
           .CalendarAccessUris.Add("CalendarAccessUriPref")
           .CalendarAccessUris.Add("CalendarAccessUriOther")
           .CalendarAddresses.Add("CalendarAddressPref")
           .CalendarAddresses.Add("CalenderAddressOther")
           .CalendarUserAddresses.Add("CalenderUserAddressPref")
           .CalendarUserAddresses.Add("CalenderUserAddressOther")
           .FreeOrBusyUrls.Add("FbUrlPref")
           .FreeOrBusyUrls.Add("FbUrlOther")
           .CalendarAccessUris.SetPreferences()
           .CalendarAddresses.SetPreferences()
           .CalendarUserAddresses.SetPreferences()
           .FreeOrBusyUrls.SetPreferences()
           .VCard;

        string serialized = vc.ToVcfString(options: VcfOpts.Default.Unset(VcfOpts.WriteRfc2739Extensions));

        vc = Vcf.Parse(serialized)[0];

        Assert.IsNull(vc.CalendarAccessUris);
        Assert.IsNull(vc.CalendarAddresses);
        Assert.IsNull(vc.CalendarUserAddresses);
        Assert.IsNull(vc.FreeOrBusyUrls);
    }

    [TestMethod]
    public void EmptyParameterTest1()
    {
        string vcString = """
            BEGIN:VCARD
            VERSION:3.0
            ADR;TYPE=dom,,postal,parcel:;;123 Main
              Street;Any Town;CA;91921-1234
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcString)[0];
        Assert.AreEqual(Adr.Dom | Adr.Postal | Adr.Parcel,
                        vc.Addresses!.First()!.Parameters.AddressType);
    }

    [TestMethod]
    public void BirthdayTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(DateTimeOffset.Now)
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        StringAssert.Contains(vcf, "BDAY", StringComparison.Ordinal);
    }

    [TestMethod]
    public void BirthdayTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add("After midnight")
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

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

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        Assert.IsFalse(vcf.Contains("LOGO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("PHOTO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("SOUND", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void AddressTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add(AddressBuilder.Create().AddStreet("street").Build())
            .Addresses.Add(AddressBuilder.Create().AddStreet("street2").Build())
            .Addresses.SetPreferences()
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        Assert.IsTrue(vcf.Contains("PREF", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void OrgTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Organizations.Edit(props => [null])
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        Assert.IsFalse(vcf.Contains("ORG", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void SortStringTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Organizations.Add("Contoso", parameters: p => p.SortAs = ["Org"])
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc.Organizations);
        Assert.IsNull(vc.Organizations.First()!.Parameters.SortAs);
        Assert.IsNotNull(vc.NameViews);
        Assert.IsNotNull(vc.NameViews.First()!.Parameters.SortAs);
        Assert.AreEqual("Org", vc.NameViews.First()!.Parameters.SortAs![0]);
    }

    [TestMethod]
    public void AgentTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            VERSION:3.0
            FN:Test
            AGENT:BEGIN:VCARDThis is not a vCard.
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vc.Relations);
        RelationProperty? rel = vc.Relations.First();
        Assert.IsNotNull(rel);
        Assert.IsNotNull(rel.Value);
        Assert.IsNotNull(rel.Value.ContactID?.Uri);
        Assert.AreEqual(Rel.Agent, rel.Parameters.RelationType);
    }
}
