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

        if (photo?.Value is DataUrl dataUrl)
        {
            Assert.AreEqual(dataUrl.MimeType.MediaType, "image/jpeg");
            //System.IO.File.WriteAllBytes(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"Testbild{dataUrl.GetFileExtension()}"), dataUrl.GetEmbeddedBytes());
        }
        else
        {
            Assert.Fail();
        }

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

        vcard.Keys = new DataProperty[] { new DataProperty(DataUrl.FromText(ASCIITEXT)) };
        vcard.Photos = new DataProperty[] { new DataProperty(DataUrl.FromBytes(bytes, "image/jpeg")) };

        string s = vcard.ToVcfString(VCdVersion.V2_1);

        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = VCard.ParseVcf(s);

        Assert.AreEqual(((DataUrl?)vcard.Keys?.First()?.Value)?.GetEmbeddedText(), ASCIITEXT);
        Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, "image/jpeg");
        Assert.IsTrue(((DataUrl?)vcard.Photos?.First()?.Value)?.GetEmbeddedBytes()?.SequenceEqual(bytes) ?? false);


        static byte[] CreateBytes()
        {
            const int DATA_LENGTH = 200;

            var arr = new byte[DATA_LENGTH];

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

        var addr0 = new AddressProperty(label0, "Entenhausen", "01234");
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = AddressTypes.Postal | AddressTypes.Parcel;
        addr0.Parameters.PropertyClass = PropertyClassTypes.Home;

        var addr1 = new AddressProperty(label1, "Borna", "43210");
        addr1.Parameters.AddressType = AddressTypes.Postal | AddressTypes.Parcel;
        addr1.Parameters.PropertyClass = PropertyClassTypes.Work;
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
        Assert.IsNotNull(addresses!.FirstOrDefault( x => x!.Parameters.Label == label0 && x.Value.Street[0] == label0));
        Assert.IsNotNull(addresses!.FirstOrDefault(x => x!.Parameters.Label == label1 && x.Value.Street[0] == label1));

    }

    [TestMethod]
    public void MoreThanOneAddressTest2()
    {
        const string label0 = "Elmstreet 13";
        const string label1 = "Sackgasse 5";

        var addr0 = new AddressProperty(label0, "Entenhausen", "01234");
        addr0.Parameters.Preference = 1;
        addr0.Parameters.Label = label0;
        addr0.Parameters.AddressType = AddressTypes.Postal | AddressTypes.Parcel;
        addr0.Parameters.PropertyClass = PropertyClassTypes.Home;

        var addr1 = new AddressProperty(label1, "Borna", "43210");
        addr1.Parameters.AddressType = AddressTypes.Postal | AddressTypes.Parcel;
        addr1.Parameters.PropertyClass = PropertyClassTypes.Work;
        addr1.Parameters.Label = label1;

        var vc = new VCard
        {
            Addresses = new AddressProperty?[] { addr1, null, addr0 }
        };

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

}
