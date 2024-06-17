using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V3Tests
{
    [TestMethod]
    public void ParseTest()
    {
        IList<VCard>? vcard = Vcf.Load(TestFiles.V3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreEqual(2, vcard.Count);
    }


    [TestMethod]
    public void ParseTest2()
    {
        IList<VCard>? vcard = Vcf.Load(@"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf");

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void ParseTest3()
    {
        IList<VCard>? vcard = Vcf.Load(TestFiles.PhotoV3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCardTest()
    {
        var vcard = new VCard();

        string s = vcard.ToVcfString(VCdVersion.V3_0);

        IList<VCard>? cards = Vcf.Parse(s);

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

        vcard.Keys = DataProperty.FromText(ASCIITEXT);

        vcard.Photos = DataProperty.FromBytes(bytes, "image/jpeg");

        string s = vcard.ToVcfString(VCdVersion.V3_0);


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
    public void SaveTest()
    {
        var vcard = new VCard();

        string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        vcard.NameViews =
        [
                new("Test", "Paul", null, null, null)
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
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V3_0, options: Opts.All);

        Assert.IsNotNull(s);

        IList<VCard> list = Vcf.Parse(s);

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
        IList<VCard> vc = Vcf.Load(TestFiles.PhotoV3vcf).ToList();
        vc.Add(vc[0]);

        string s = vc.ToVcfString();

        vc = Vcf.Parse(s);

        Assert.AreEqual(2, vc.Count);
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

        string vcf = Vcf.ToString(vc, VCdVersion.V3_0, options: Opts.Default.Set(Opts.WriteNonStandardProperties));
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

        if (bday is TimeOnlyProperty prop)
        {
            Assert.AreEqual(new TimeOnly(5, 30, 0), prop.Value);
        }
        else
        {
            Assert.Fail();
        }
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
        string vcfString = vcard.ToVcfString(options: Opts.Default | Opts.WriteNonStandardParameters);
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

        string vcfString = vcard.ToVcfString(options: Opts.Default | Opts.WriteXExtensions);
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

        string vcfString = vcard.ToVcfString(options: Opts.Default);
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

        string vcfString = vcard.ToVcfString(options: (Opts.Default | Opts.WriteXExtensions).Unset(Opts.WriteImppExtension));
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

        string vcfString = vcard.ToVcfString(options: Opts.Default | Opts.WriteXExtensions);
        vcard = Vcf.Parse(vcfString)[0];
        Assert.AreEqual(1, vcard.Messengers!.Count());
        prop = vcard.Messengers!.First();
        Assert.AreEqual(mobilePhoneNumber, prop?.Value);
        Assert.AreEqual(imppTypes, prop?.Parameters.InstantMessengerType);
    }

    [TestMethod]
    public void PreserveTimeZoneAndGeoTest1()
    {
        var adr = new AddressProperty("1", "", "", "", "");
        adr.Parameters.TimeZone = TimeZoneID.Parse("Europe/Berlin");
        adr.Parameters.GeoPosition = new GeoCoordinate(52, 13);
        Assert.IsNotNull(adr.Parameters.Label);

        var vCard = new VCard { Addresses = adr };

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.Addresses);
        adr = vCard.Addresses.First();
        Assert.IsNotNull(adr!.Parameters.Label);
        Assert.IsNotNull(adr.Parameters.GeoPosition);
        Assert.IsNotNull(adr.Parameters.TimeZone);
    }

    [TestMethod]
    public void PreserveTimeZoneAndGeoTest2()
    {
        var adr = new AddressProperty("1", "", "", "", "");
        adr.Parameters.TimeZone = TimeZoneID.Parse("Europe/Berlin");
        adr.Parameters.GeoPosition = new GeoCoordinate(52, 13);
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
        var nm = new NameProperty("Kinzel", "Folker");

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

        string vcf = vCard.ToVcfString(VCdVersion.V3_0, options: Opts.Default.Set(Opts.WriteEmptyProperties));

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
            .NameViews.Add("Name", parameters: p => p.SortAs = ["abc", "123"])
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
            .NameViews.Add("Name")
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
            .NameViews.Add("Name", parameters: p => p.SortAs = ["abc", "123"])
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

        string serialized = vc.ToVcfString(options: Opts.Default.Unset(Opts.WriteRfc2739Extensions));

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
}
