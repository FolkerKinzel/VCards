using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V3Tests
{
    [TestMethod]
    public void ParseTest()
    {
        IList<VCard>? vcard = VCard.LoadVcf(TestFiles.V3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreEqual(2, vcard.Count);
    }


    [TestMethod]
    public void ParseTest2()
    {
        IList<VCard>? vcard = VCard.LoadVcf(@"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf");

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void ParseTest3()
    {
        IList<VCard>? vcard = VCard.LoadVcf(TestFiles.PhotoV3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCardTest()
    {
        var vcard = new VCard();

        string s = vcard.ToVcfString(VCdVersion.V3_0);

        IList<VCard>? cards = VCard.ParseVcf(s);

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

        vcard.Notes = new TextProperty[]
        {
                new TextProperty(UNITEXT)
        };

        vcard.Keys = DataProperty.FromText(ASCIITEXT);

        vcard.Photos = DataProperty.FromBytes(bytes, "image/jpeg");

        string s = vcard.ToVcfString(VCdVersion.V3_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = VCard.ParseVcf(s);

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

        vcard.NameViews = new NameProperty[]
        {
                new NameProperty("Test", "Paul", null, null, null)
        };

        vcard.DisplayNames = new TextProperty[]
        {
                new TextProperty("Paul Test")
        };

        vcard.Notes = new TextProperty[]
        {
                new TextProperty(UNITEXT)
        };

        vcard.SaveVcf(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Paul Test.vcf"));
    }


    [TestMethod]
    public void SerializeVCardTest1()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V3_0, options: VcfOptions.All);

        Assert.IsNotNull(s);

        IList<VCard> list = VCard.ParseVcf(s);

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
        IList<VCard> vc = VCard.LoadVcf(TestFiles.PhotoV3vcf);
        vc.Add(vc[0]);

        string s = vc.ToVcfString();

        vc = VCard.ParseVcf(s);

        Assert.AreEqual(2, vc.Count);
    }


    [TestMethod]
    public void TimeDataTypeTest()
    {
        const string vcardString = @"BEGIN:VCARD
VERSION:3.0
BDAY;Value=Time:05:30:00
END:VCARD";

        VCard vcard = VCard.ParseVcf(vcardString)[0];

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
        xiamoiMobilePhone.Parameters.NonStandard = new KeyValuePair<string, string>[]
        {
                new KeyValuePair<string, string>("TYPE", "WhatsApp")
        };

        var vcard = new VCard
        {
            Phones = xiamoiMobilePhone
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = vcard.ToVcfString(options: VcfOptions.Default | VcfOptions.WriteNonStandardParameters);
        vcard = VCard.ParseVcf(vcfString)[0];

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
            InstantMessengers = whatsAppImpp
        };

        string vcfString = vcard.ToVcfString(options: VcfOptions.Default | VcfOptions.WriteXExtensions);
        vcard = VCard.ParseVcf(vcfString)[0];

        whatsAppImpp = vcard.InstantMessengers?.First();

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
            InstantMessengers = whatsAppImpp
        };

        string vcfString = vcard.ToVcfString(options: VcfOptions.Default);
        vcard = VCard.ParseVcf(vcfString)[0];

        whatsAppImpp = vcard.InstantMessengers?.First();

        Assert.AreEqual(mobilePhoneNumber, whatsAppImpp?.Value);
        Assert.AreEqual(messengerTypes, whatsAppImpp?.Parameters.InstantMessengerType);
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
            InstantMessengers = prop
        };

        string vcfString = vcard.ToVcfString(options: (VcfOptions.Default | VcfOptions.WriteXExtensions).Unset(VcfOptions.WriteImppExtension));
        vcard = VCard.ParseVcf(vcfString)[0];

        Assert.AreEqual(1, vcard.InstantMessengers!.Count());
        prop = vcard.InstantMessengers?.First();
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
            InstantMessengers = prop
        };

        string vcfString = vcard.ToVcfString(options: VcfOptions.Default | VcfOptions.WriteXExtensions);
        vcard = VCard.ParseVcf(vcfString)[0];
        Assert.AreEqual(1, vcard.InstantMessengers!.Count());
        prop = vcard.InstantMessengers!.First();
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

        vCard = VCard.ParseVcf(vcf)[0];

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

        vCard = VCard.ParseVcf(vcf)[0];

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

        vCard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.AreEqual("Folker Kinzel", tProp.Value);
    }

    [TestMethod]
    public void DisplayNameTest2()
    {
        var vCard = new VCard { NameViews = new NameProperty?[] { null } };

        string vcf = vCard.ToVcfString(VCdVersion.V3_0);

        vCard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.IsFalse(tProp.IsEmpty);
    }

    [TestMethod]
    public void DisplayNameTest3()
    {
        var vCard = new VCard();

        string vcf = vCard.ToVcfString(VCdVersion.V3_0, options: VcfOptions.Default.Set(VcfOptions.WriteEmptyProperties));

        vCard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.IsTrue(tProp.IsEmpty);

        Assert.IsNotNull(vCard.NameViews);
        NameProperty nProp = vCard.NameViews.First()!;
        Assert.IsTrue(nProp.IsEmpty);
    }
}
