using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V4Tests
{
    [TestMethod]
    public void Parse()
    {
        IList<VCard>? vcard = Vcf.Load(TestFiles.V4vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCard()
    {
        var vcard = new VCard();
        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard>? cards = Vcf.Parse(s);

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

        vcard.Notes = [new(UNITEXT)];

        vcard.Keys = DataProperty.FromText(ASCIITEXT);

        vcard.Photos = DataProperty.FromBytes(bytes, "image/jpeg");

        string s = vcard.ToVcfString(VCdVersion.V4_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x is not null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = Vcf.Parse(s);

        Assert.AreEqual(vcard.Keys?.First()?.Value?.String, ASCIITEXT);
        Assert.AreEqual("image/jpeg", vcard.Photos?.First()?.Parameters.MediaType);
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
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V4_0, options: Opts.All);

        Assert.IsNotNull(s);

        IList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];

        //string theString = vcard.ToString();

        Assert.AreEqual(VCdVersion.V4_0, vcard.Version);

        Assert.IsNull(vcard.DirectoryName);
        Assert.IsNotNull(vcard.TimeStamp);
        Assert.IsNull(vcard.Mailer);
        Assert.IsNotNull(vcard.ProductID);
        vcard.ProductID = null;
        Assert.IsNull(vcard.ProductID);
    }

    [TestMethod]
    public void Rfc6474Test()
    {
        VCard vc = Utility.CreateVCard();

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.IsNotNull(vc.DeathDateViews);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0, options: Opts.WriteRfc6474Extensions));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.IsNotNull(vc.DeathDateViews);


        list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0, options: Opts.None));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNull(vc.BirthPlaceViews);
        Assert.IsNull(vc.DeathPlaceViews);
        Assert.IsNull(vc.DeathDateViews);
    }

    [TestMethod]
    public void MembersTest1()
    {
        var vc = new VCard
        {
            Members = RelationProperty.FromText("http://folkers-website.de"),
            Kind = new KindProperty(Kind.Group)
        };

        Assert.IsNotNull(vc.Members);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.Members);
    }

    [TestMethod]
    public void MembersTest2()
    {
        var vc = new VCard
        {
            Members = RelationProperty.FromText("Important Member"),
        };

        Assert.IsNotNull(vc.Members);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.Members);
        Assert.IsNotNull(vc.Kind);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
    }

    [TestMethod]
    public void MembersTest3()
    {
        var vc = new VCard
        {
            Members = RelationProperty.FromVCard(VCardBuilder.Create(setID: false)
                                                              .DisplayNames.Add("Important Member")
                                                              .ID.Set(Guid.Empty)
                                                              .VCard),
        };

        Assert.IsNotNull(vc.Members);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);
        vc = list[1];

        Assert.AreNotEqual(Guid.Empty, vc.ID?.Value);
    }

    [TestMethod]
    public void MembersTest4()
    {
        var vc = new VCard
        {
            Members = RelationProperty.FromVCard(VCardBuilder.Create(setID: false).DisplayNames.Add("Important Member").VCard),
        };

        Assert.IsNotNull(vc.Members);
        Assert.IsNull(vc.Members!.First()?.Value?.VCard?.ID);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);
        vc = list[1];

        Assert.IsNotNull(vc.ID);
    }

    [TestMethod]
    public void MembersTest5()
    {
        var guid = Guid.NewGuid();
        var vc = new VCard
        {
            Members = RelationProperty.FromVCard(VCardBuilder.Create(setID: false)
                                                              .DisplayNames.Add("Important Member")
                                                              .ID.Set(guid)
                                                              .VCard)
                      .Concat(RelationProperty.FromGuid(guid)),
        };

        Assert.IsNotNull(vc.Members);

        IList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);

        list = Vcf.Parse(list.ToVcfString(VCdVersion.V4_0));
        Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void FburlTest()
    {
        const string workUrl = "WorkUrl";
        const string homeUrl = "HomeUrl";

        const string calendar = "text/calendar";
        const string plain = "text/plain";

        var fburl1 = new TextProperty(workUrl);
        fburl1.Parameters.PropertyClass = PCl.Work;
        fburl1.Parameters.Preference = 1;
        fburl1.Parameters.DataType = Data.Uri;
        fburl1.Parameters.MediaType = calendar;

        var fburl2 = new TextProperty(homeUrl);
        fburl2.Parameters.PropertyClass = PCl.Home;
        fburl2.Parameters.Preference = 2;
        fburl2.Parameters.DataType = Data.Text;
        fburl2.Parameters.MediaType = plain;

        var vc = new VCard { FreeOrBusyUrls = [fburl1, fburl2] };

        string s = vc.ToVcfString(VCdVersion.V4_0);

        Assert.IsNotNull(s);

        IList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vc2 = list[0];

        Assert.IsNotNull(vc2);

        IEnumerable<TextProperty?>? fburls = vc2.FreeOrBusyUrls;

        Assert.IsNotNull(fburls);
        Assert.AreEqual(2, fburls!.Count());

        TextProperty fb1 = fburls!.FirstOrDefault(x => x is not null && x.Parameters.Preference == 1)!;

        Assert.IsNotNull(fb1);
        Assert.AreEqual(workUrl, fb1.Value);
        Assert.AreEqual(PCl.Work, fb1.Parameters.PropertyClass);
        Assert.AreEqual(calendar, fb1.Parameters.MediaType);
        Assert.AreEqual(Data.Uri, fb1.Parameters.DataType);

        TextProperty fb2 = fburls!.FirstOrDefault(x => x is not null && x.Parameters.Preference == 2)!;

        Assert.IsNotNull(fb2);
        Assert.AreEqual(homeUrl, fb2.Value);
        Assert.AreEqual(PCl.Home, fb2.Parameters.PropertyClass);
        Assert.AreEqual(plain, fb2.Parameters.MediaType);
        Assert.AreEqual(Data.Text, fb2.Parameters.DataType);
    }


    [TestMethod]
    public void DisplayNameTest1()
    {
        var nm = new NameProperty("Kinzel", "Folker");

        var vCard = new VCard { NameViews = nm };

        string vcf = vCard.ToVcfString(VCdVersion.V4_0);

        vCard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vCard.DisplayNames);
        TextProperty tProp = vCard.DisplayNames.First()!;
        Assert.AreEqual("Folker Kinzel", tProp.Value);
    }

    [TestMethod]
    public void ImppTest1()
    {
        const string mobilePhoneNumber = "tel:+1-234-567-89";
        var whatsAppImpp = new TextProperty(mobilePhoneNumber);

        const Impp messengerTypes = Impp.Personal
                                | Impp.Business
                                | Impp.Mobile;

        whatsAppImpp.Parameters.InstantMessengerType = messengerTypes;

        var vcard = new VCard { Messengers = [null, whatsAppImpp] };

        string vcfString = vcard.ToVcfString(VCdVersion.V4_0, options: Opts.Default);
        vcard = Vcf.Parse(vcfString)[0];

        whatsAppImpp = vcard.Messengers?.First();

        Assert.AreEqual(mobilePhoneNumber, whatsAppImpp?.Value);
        Assert.AreEqual(PCl.Home | PCl.Work, whatsAppImpp?.Parameters.PropertyClass);
    }
}

