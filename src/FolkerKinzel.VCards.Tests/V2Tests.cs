using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V2Tests
{
    [TestMethod]
    public void Parse()
    {
        IList<VCard> vcard = VCard.LoadVcf(fileName: TestFiles.V2vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void ParseOutlook()
    {
        IList<VCard> vcard = VCard.LoadVcf(fileName: TestFiles.OutlookV2vcf);

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.FirstOrDefault());

        //string s = vcard[0].ToString();

        DataProperty? photo = vcard[0].Photos?.FirstOrDefault();
        Assert.IsNotNull(photo);

        Assert.AreEqual(photo?.Parameters.MediaType, "image/jpeg");
        //System.IO.File.WriteAllBytes(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"Testbild{dataUrl.GetFileExtension()}"), dataUrl.GetEmbeddedBytes());

        VCard.SaveVcf(System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"TestV2.1.vcf"),
            vcard!, VCdVersion.V2_1, options: VcfOptions.Default.Set(VcfOptions.WriteNonStandardProperties));
    }


    [TestMethod]
    public void WriteEmptyVCard()
    {
        var vcard = new VCard();
        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IList<VCard> cards = VCard.ParseVcf(s);

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

        vcard.Notes = new TextProperty[]
        {
                new TextProperty(UNITEXT)
        };

        vcard.Keys = DataProperty.FromText(ASCIITEXT);
        vcard.Photos = DataProperty.FromBytes(bytes, "image/jpeg");

        string s = vcard.ToVcfString(VCdVersion.V2_1);

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
    public void SerializeVCard()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V2_1, options: VcfOptions.All);

        Assert.IsNotNull(s);

        IList<VCard> list = VCard.ParseVcf(s);

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

        var addr0 = new AddressProperty(label0, "Entenhausen", null, postalCode: "01234", autoLabel: false);
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr0.Parameters.PropertyClass = PCl.Home;

        var addr1 = new AddressProperty(label1, "Borna", null, postalCode: "43210", autoLabel: false);
        addr1.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr1.Parameters.PropertyClass = PCl.Work;
        addr1.Parameters.Label = label1;

        var vc = new VCard
        {
            Addresses = new AddressProperty?[] { addr1, null, addr0 }
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        IList<VCard> vCards = VCard.ParseVcf(vcf);
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

        var addr0 = new AddressProperty(label0, "Entenhausen", null, postalCode: "01234", autoLabel: false);
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr0.Parameters.PropertyClass = PCl.Home;

        var addr1 = new AddressProperty(label1, "Borna", null, postalCode: "43210", autoLabel: false);
        addr1.Parameters.AddressType = Adr.Postal | Adr.Parcel;
        addr1.Parameters.PropertyClass = PCl.Work;
        addr1.Parameters.Label = label1;

        var vc = new VCard
        {
            Addresses = new AddressProperty?[] { addr1, null, addr0 }
        };

        var arr = new VCard[] { vc };

        string vcf = vc.ToVcfString(VCdVersion.V2_1, options: VcfOptions.Default.Unset(VcfOptions.AllowMultipleAdrAndLabelInVCard21));
        IList<VCard> vCards = VCard.ParseVcf(vcf);
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
        var vc = new VCard
        {
            Relations = RelationProperty.FromVCard(new VCard { NameViews = new NameProperty("wife", "best") }, Rel.Spouse)
        };

        string vcf = vc.ToVcfString(VCdVersion.V2_1);
        vc = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(Rel.Spouse, vc.Relations?.First()?.Parameters.Relation);

    }

    [TestMethod]
    public void PreserveTimeZoneAndGeoTest1()
    {
        var adr = new AddressProperty("1", "", "", "", "");
        adr.Parameters.TimeZone = TimeZoneID.Parse("Europe/Berlin");
        adr.Parameters.GeoPosition = new GeoCoordinate(52, 13);
        Assert.IsNotNull(adr.Parameters.Label);

        var vCard = new VCard { Addresses = adr };

        string vcf = vCard.ToVcfString(VCdVersion.V2_1);

        vCard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vCard.Addresses);
        adr = vCard.Addresses.First();
        Assert.IsNotNull(adr!.Parameters.Label);
        Assert.IsNotNull(adr.Parameters.GeoPosition);
        Assert.IsNotNull(adr.Parameters.TimeZone);
    }

    [TestMethod]
    public void EmbeddedBytesTest1()
    {
        var vCard = new VCard { Photos = DataProperty.FromBytes(null) };

        string s = vCard.ToVcfString(VCdVersion.V2_1, options: VcfOptions.Default.Set(VcfOptions.WriteEmptyProperties));

        vCard = VCard.ParseVcf(s)[0];
        Assert.IsNotNull(vCard.Photos);
        Assert.IsTrue(vCard.Photos!.First()!.IsEmpty);
    }

    [TestMethod]
    public void LineWrappingTest1()
    {
        var bytes = Enumerable.Range(0 - 255, 255).Select(x => (byte)x).ToArray();
        var agent = new VCard
        {
            DisplayNames = new TextProperty("007"),
            Notes = new TextProperty("ÄÖÜ Veeeeeeeeeeeeeeeeeeeeeeeeeery veeeeeeeeeeeeeeeeeeeeeeeeeeeery looooooooooooooooooooooooooooooooong Quoted-Printable text"),
            Photos = DataProperty.FromBytes(bytes)
        };

        var vCard = new VCard
        {
            DisplayNames = new TextProperty("Secret Service"),
            Relations = RelationProperty.FromVCard(agent, Rel.Agent | Rel.Colleague)
        };

        string s = vCard.ToVcfString(VCdVersion.V2_1, options: VcfOptions.Default.Set(VcfOptions.AppendAgentAsSeparateVCard));

        IList<VCard> vCards = VCard.ParseVcf(s);
        Assert.AreEqual(2, vCards.Count);
        Assert.IsNotNull(vCards[0].Relations);
    }

    [TestMethod]
    public void EmptyAgentTest()
    {
        const string agent = """
            BEGIN:VCARD
            VERSION:2.1
            FN:Secret Service
            N:?;;;;
            AGENT:

            END:VCARD

            """;

        IList<VCard> vcs = VCard.ParseVcf(agent);
        Assert.AreEqual(1, vcs.Count);
        Assert.IsNotNull(vcs[0].Relations);
    }

    [TestMethod]
    public void UsingTheWhatsAppTypeTest()
    {
        const string whatsAppNumber = "+1-234-567-89";
        var xiamoiMobilePhone = new TextProperty(whatsAppNumber);
        xiamoiMobilePhone.Parameters.NonStandard = new KeyValuePair<string, string>[]
        {
                new KeyValuePair<string, string>("TYPE", "WhatsApp")
        };

        // Initialize the VCard:
        var vcard = new VCard
        {
            NameViews = new NameProperty[]
            {
                    new NameProperty(lastName: null, firstName: "zzMad Perla 45")
            },

            DisplayNames = new TextProperty[]
            {
                    new TextProperty("zzMad Perla 45")
            },

            Phones = new TextProperty[]
            {
                    xiamoiMobilePhone
            }
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = vcard.ToVcfString(version: VCdVersion.V2_1, options: VcfOptions.Default | VcfOptions.WriteNonStandardParameters);

        // Parse the VCF string:
        vcard = VCard.ParseVcf(vcfString)[0];

        // Find the WhatsApp number:
        Assert.AreEqual(whatsAppNumber, vcard.Phones?
            .FirstOrDefault(x => x?.Parameters.NonStandard?.Any(x => x.Key == "TYPE" && x.Value == "WhatsApp") ?? false)?
            .Value);
    }
}
