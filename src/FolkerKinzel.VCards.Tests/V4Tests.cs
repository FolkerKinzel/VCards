using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V4Tests
{
    [TestMethod]
    public void Parse()
    {
        IList<VCard>? vcard = VCard.LoadVcf(TestFiles.V4vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCard()
    {
        var vcard = new VCard();
        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard>? cards = VCard.ParseVcf(s);

        Assert.AreEqual(cards.Count, 1);

        vcard = cards[0];

        Assert.AreEqual(VCdVersion.V4_0, vcard.Version);
        Assert.IsNotNull(vcard.DisplayNames);
        Assert.AreEqual(vcard.DisplayNames!.Count(), 1);
        Assert.IsNotNull(vcard.DisplayNames!.First());
    }


    [TestMethod]
    public void TestLineWrapping()
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

        string s = vcard.ToVcfString(VCdVersion.V4_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = VCard.ParseVcf(s);

        Assert.AreEqual(vcard.Keys?.First()?.Value, ASCIITEXT);
        Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, null);
        Assert.IsTrue(((byte[]?)vcard.Photos?.First()?.Value)?.SequenceEqual(bytes) ?? false);


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
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V4_0, options: VcfOptions.All);

        Assert.IsNotNull(s);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];

        Assert.AreEqual(VCdVersion.V4_0, vcard.Version);

        Assert.IsNull(vcard.DirectoryName);
        Assert.IsNotNull(vcard.TimeStamp);
        Assert.IsNull(vcard.Mailer);
        Assert.IsNotNull(vcard.ProdID);
        vcard.ProdID = null;
        Assert.IsNull(vcard.ProdID);
    }

    [TestMethod]
    public void Rfc6474Test()
    {
        VCard vc = Utility.CreateVCard();

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.IsNotNull(vc.DeathDateViews);

        IList<VCard> list = VCard.ParseVcf(vc.ToVcfString(version: VCdVersion.V4_0, options: VcfOptions.WriteRfc6474Extensions));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.IsNotNull(vc.DeathDateViews);


        list = VCard.ParseVcf(vc.ToVcfString(version: VCdVersion.V4_0, options: VcfOptions.None));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNull(vc.BirthPlaceViews);
        Assert.IsNull(vc.DeathPlaceViews);
        Assert.IsNull(vc.DeathDateViews);
    }

    [TestMethod]
    public void MembersTest()
    {
        var vc = new VCard
        {
            Members = new RelationTextProperty("http://folkers-website.de"),
            Kind = new KindProperty(VCdKind.Group)
        };

        Assert.IsNotNull(vc.Members);

        IList<VCard> list = VCard.ParseVcf(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.Members);
    }

    [TestMethod]
    public void FburlTest()
    {
        const string workUrl = "WorkUrl";
        const string homeUrl = "HomeUrl";

        const string calendar = "text/calendar";
        const string plain = "text/plain";

        var fburl1 = new TextProperty(workUrl);
        fburl1.Parameters.PropertyClass = PropertyClassTypes.Work;
        fburl1.Parameters.Preference = 1;
        fburl1.Parameters.DataType = VCdDataType.Uri;
        fburl1.Parameters.MediaType = calendar;

        var fburl2 = new TextProperty(homeUrl);
        fburl2.Parameters.PropertyClass = PropertyClassTypes.Home;
        fburl2.Parameters.Preference = 2;
        fburl2.Parameters.DataType = VCdDataType.Text;
        fburl2.Parameters.MediaType = plain;

        var vc = new VCard
        {
            FreeOrBusyUrls = new TextProperty[] { fburl1, fburl2 }
        };

        string s = vc.ToVcfString(VCdVersion.V4_0);

        Assert.IsNotNull(s);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vc2 = list[0];

        Assert.IsNotNull(vc2);

        IEnumerable<TextProperty?>? fburls = vc2.FreeOrBusyUrls;

        Assert.IsNotNull(fburls);
        Assert.AreEqual(2, fburls!.Count());

        TextProperty fb1 = fburls!.FirstOrDefault(x => x != null && x.Parameters.Preference == 1)!;

        Assert.IsNotNull(fb1);
        Assert.AreEqual(workUrl, fb1.Value);
        Assert.AreEqual(PropertyClassTypes.Work, fb1.Parameters.PropertyClass);
        Assert.AreEqual(calendar, fb1.Parameters.MediaType);
        Assert.AreEqual(VCdDataType.Uri, fb1.Parameters.DataType);

        TextProperty fb2 = fburls!.FirstOrDefault(x => x != null && x.Parameters.Preference == 2)!;

        Assert.IsNotNull(fb2);
        Assert.AreEqual(homeUrl, fb2.Value);
        Assert.AreEqual(PropertyClassTypes.Home, fb2.Parameters.PropertyClass);
        Assert.AreEqual(plain, fb2.Parameters.MediaType);
        Assert.AreEqual(VCdDataType.Text, fb2.Parameters.DataType);
    }

}

