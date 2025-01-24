using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V4Tests
{
    [TestMethod]
    public void Parse()
    {
        IReadOnlyList<VCard>? vcard = Vcf.Load(TestFiles.V4vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void WriteEmptyVCard()
    {
        var vcard = new VCard();
        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IReadOnlyList<VCard>? cards = Vcf.Parse(s);

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

        vcard.Keys = new DataProperty(RawData.FromText(ASCIITEXT));

        vcard.Photos = new DataProperty(RawData.FromBytes(bytes, "image/jpeg"));

        string s = vcard.ToVcfString(VCdVersion.V4_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split([VCard.NewLine], StringSplitOptions.None)
            .All(x => x is not null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = Vcf.Parse(s);

        Assert.AreEqual(vcard.Keys?.First()?.Value.String, ASCIITEXT);
        Assert.AreEqual("image/jpeg", vcard.Photos?.First()?.Parameters.MediaType);
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
    public void SerializeVCard()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V4_0, options: VcfOpts.All);

        Assert.IsNotNull(s);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];

        //string theString = vcard.ToString();

        Assert.AreEqual(VCdVersion.V4_0, vcard.Version);

        Assert.IsNull(vcard.DirectoryName);
        Assert.IsNotNull(vcard.Updated);
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

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0, options: VcfOpts.WriteRfc6474Extensions));

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
        vc = list[0];

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.IsNotNull(vc.DeathDateViews);


        list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0, options: VcfOpts.None));

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
            Members = new RelationProperty(Relation.Create(ContactID.Create("http://folkers-website.de"))),
            Kind = new KindProperty(Kind.Group)
        };

        Assert.IsNotNull(vc.Members);

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

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
            Members = new RelationProperty(Relation.Create(ContactID.Create("Important Member"))),
        };

        Assert.IsNotNull(vc.Members);

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

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
            Members = new RelationProperty(Relation.Create(VCardBuilder.Create(setContactID: false)
                                                              .DisplayNames.Add("Important Member")
                                                              .ContactID.Set(Guid.Empty)
                                                              .VCard)),
        };

        Assert.IsNotNull(vc.Members);

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);
        vc = list[1];

        Assert.AreNotEqual(Guid.Empty, vc.ContactID?.Value.Guid);
    }

    [TestMethod]
    public void MembersTest4()
    {
        var vc = new VCard
        {
            Members = new RelationProperty(Relation.Create(VCardBuilder.Create(setContactID: false).DisplayNames.Add("Important Member").VCard)),
        };

        Assert.IsNotNull(vc.Members);
        VCard? firstMembersVCard = vc.Members.First()?.Value.VCard;
        Assert.IsNotNull(firstMembersVCard);
        Assert.IsNull(firstMembersVCard.ContactID);

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);
        vc = list[1];

        Assert.IsNotNull(vc.ContactID);
    }

    [TestMethod]
    public void MembersTest5()
    {
        var guid = Guid.NewGuid();
        var vc = new VCard
        {
            Members = new RelationProperty(Relation.Create(VCardBuilder.Create(setContactID: false)
                                                              .DisplayNames.Add("Important Member")
                                                              .ContactID.Set(guid)
                                                              .VCard))
                      .Concat(new RelationProperty(Relation.Create(ContactID.Create(guid)))),
        };

        Assert.IsNotNull(vc.Members);

        IReadOnlyList<VCard> list = Vcf.Parse(vc.ToVcfString(version: VCdVersion.V4_0));

        Assert.AreEqual(2, list.Count);

        list = Vcf.Parse(list.ToVcfString(VCdVersion.V4_0));
        Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void MembersTest6()
    {
        var vc = new VCard
        {
            Members = new RelationProperty(Relation.Empty),
        };

        Assert.IsNotNull(vc.Members);

        string vcf = vc.ToVcfString(version: VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.WriteEmptyProperties));

        IReadOnlyList<VCard> list = Vcf.Parse(vcf);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
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

        IReadOnlyList<VCard> list = Vcf.Parse(s);

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
        var nm = new NameProperty(NameBuilder.Create().AddSurname("Kinzel").AddGiven("Folker").Build());

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

        string vcfString = vcard.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default);
        vcard = Vcf.Parse(vcfString)[0];

        whatsAppImpp = vcard.Messengers?.First();

        Assert.AreEqual(mobilePhoneNumber, whatsAppImpp?.Value);
        Assert.AreEqual(PCl.Home | PCl.Work, whatsAppImpp?.Parameters.PropertyClass);
    }

    [TestMethod]
    public void CalendarTest1()
    {
        var prop = new DateAndOrTimeProperty(DateAndOrTime.Create(new DateOnly(4, 5, 5), ignoreYear: true));
        Assert.AreEqual("gregorian", prop.Parameters.Calendar);
        prop.Parameters.Calendar = "GREGORIAN";
        Assert.AreEqual("GREGORIAN", prop.Parameters.Calendar);
        prop.Parameters.Calendar = "X-JULIAN";
        Assert.AreEqual("X-JULIAN", prop.Parameters.Calendar);
        prop.Parameters.Calendar = null;
        Assert.AreEqual("gregorian", prop.Parameters.Calendar);
    }

    [TestMethod]
    public void CalendarTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(1900, 2, 3, parameters: p => p.Calendar = "   ")
            .AnniversaryViews.Add(1924, 7, 24, parameters: p => p.Calendar = "GREGORIAN")
            .DeathDateViews.Add(1998, 2, 4, parameters: p => p.Calendar = "X-JULIAN")
            .VCard;

        string serialized = vc.ToVcfString(VCdVersion.V4_0);

        vc = Vcf.Parse(serialized)[0];

        Assert.AreEqual("gregorian", vc.BirthDayViews!.First()!.Parameters.Calendar);
        Assert.AreEqual("gregorian", vc.AnniversaryViews!.First()!.Parameters.Calendar);
        Assert.AreEqual("X-JULIAN", vc.DeathDateViews!.First()!.Parameters.Calendar);
    }

    [TestMethod]
    public void LevelTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Expertises.Add("C#", parameters: p => p.NonStandard = [new KeyValuePair<string, string>("LEVEL", "SuperExpert"), new KeyValuePair<string, string>("TYPE", "NERD")])
            .Interests.Add("Linq", parameters: p => p.NonStandard = [new KeyValuePair<string, string>("LEVEL", "VeryInterested")])
            .VCard;

        string serialized = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.WriteNonStandardParameters));

        vc = Vcf.Parse(serialized)[0];
        TextProperty expertise = vc.Expertises!.First()!;
        TextProperty interest = vc.Interests!.First()!;

        Assert.IsTrue(expertise.Parameters.NonStandard!.Any(kvp => kvp.Key == "LEVEL" && kvp.Value == "SuperExpert"));
        Assert.IsTrue(expertise.Parameters.NonStandard!.Any(kvp => kvp.Key == "TYPE" && kvp.Value == "NERD"));

        Assert.IsTrue(interest.Parameters.NonStandard!.Any(kvp => kvp.Key == "LEVEL" && kvp.Value == "VeryInterested"));
    }

    [TestMethod]
    public void ParameterTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddGiven("Folker").Build(), parameters: p => p.SortAs = [])
            .AnniversaryViews.Add("In summer", parameters: p => p.Language = "en")
            .Phones.Add("tel:123",
                        parameters: p =>
                        {
                            p.DataType = Data.Uri;
                            p.MediaType = "application/octet-stream";
                            p.PropertyIDs = [];
                        })
            .Expertises.Add("C#", parameters: p => p.NonStandard = [new KeyValuePair<string, string>("LEVEL", "  ")])
            .Interests.Add("Linq", parameters: p => p.NonStandard = [new KeyValuePair<string, string>("LEVEL", "")])
            .NonStandards.Edit(props => [null])
            .VCard;

        string serialized = vc.ToVcfString(VCdVersion.V4_0,
            options: VcfOpts.Default.Set(VcfOpts.WriteNonStandardParameters)
                                 .Set(VcfOpts.WriteNonStandardProperties)
                                 .Unset(VcfOpts.SetPropertyIDs));

        vc = Vcf.Parse(serialized)[0];

        Assert.IsNotNull(vc.AnniversaryViews);
        Assert.AreEqual("en", vc.AnniversaryViews.First()!.Parameters.Language);

        Assert.IsNotNull(vc.Phones);
        Assert.AreEqual("application/octet-stream", vc.Phones.First()!.Parameters.MediaType);

        Assert.IsNotNull(vc.Expertises);
        Assert.IsNull(vc.Expertises.First()!.Parameters.NonStandard);

        Assert.IsNotNull(vc.Interests);
        Assert.IsNull(vc.Interests.First()!.Parameters.NonStandard);
    }

    [TestMethod]
    public void NormalizeMembersTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Members.Add((string?)null)
            .VCard;

        string serialized = vc.ToVcfString(VCdVersion.V4_0);

        vc = Vcf.Parse(serialized)[0];
        Assert.IsNull(vc.Members);
    }

    [TestMethod]
    public void DereferenceTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .DisplayNames.Add("Test")
            .VCard;

        VCardBuilder
            .Create(vc)
            .Relations.Add(vc.ContactID!.Value)
            .Relations.Add(vc);

        vc.Dereference();

        Assert.AreEqual(2, vc.Relations!.Count());
    }

    [TestMethod]
    public void NonStandardParameterTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Add("Mausi", parameters: p => p.NonStandard = [new("TYPE", "X-NIX")])
            .Phones.Add("123", parameters: p => p.NonStandard = [])
            .DisplayNames.Add("Test", parameters:
            p => p.NonStandard =
            [
                new("ALTID", "Test"),
                new("CALSCALE", "Test"),
                new("CHARSET", "Test"),
                new("CONTEXT", "Test"),
                new("ENCODING", "Test"),
                new("GEO", "Test"),
                new("INDEX", "Test"),
                new("LABEL", "Test"),
                new("LANGUAGE", "Test"),
                new("LEVEL", "Test"),
                new("MEDIATYPE", "Test"),
                new("PID", "Test"),
                new("PREF", "Test"),
                new("SORT-AS", "Test"),
                new("TYPE", "Test"),
                new("TZ", "Test"),
                new("VALUE", "  "),
                new("VALUE", "Test")
            ])
            .VCard;

        string serialized = Vcf.AsString(vc, VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.WriteNonStandardParameters));

        vc = Vcf.Parse(serialized)[0];

        Assert.AreEqual(2, vc.DisplayNames!.First()!.Parameters.NonStandard!.Count());
        Assert.AreEqual(1, vc.Relations!.First()!.Parameters.NonStandard!.Count());

    }

    [TestMethod]
    public void LogoPhotoSoundTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Logos.AddText("text")
            .Photos.AddText("text")
            .Sounds.AddText("text")
            .Logos.Edit(props => props.Append(null))
            .Photos.Edit(props => props.Append(null))
            .Sounds.Edit(props => props.Append(null))
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V4_0);

        Assert.IsFalse(vcf.Contains("LOGO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("PHOTO", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcf.Contains("SOUND", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void LogoPhotoSoundTest2()
    {
        byte[] bytes = [1,2,3];
        VCard vc = VCardBuilder
            .Create()
            .Logos.AddBytes(bytes)
            .Photos.AddBytes(bytes)
            .Sounds.AddBytes(bytes)
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V4_0);

        Assert.IsTrue(vcf.Contains("LOGO", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(vcf.Contains("PHOTO", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(vcf.Contains("SOUND", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void AltIdTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Edit(props => [null])
            .BirthDayViews.Add("After midnight")
            .BirthDayViews.Add(1980, 7, 2, parameters: p => p.AltID = "@007")
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V4_0);
        StringAssert.Contains(vcf, "ALTID=");
    }

    [TestMethod]
    public void SortAsTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddGiven("Folker").Build(), p => p.SortAs = [])
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V4_0);
        Assert.IsFalse(vcf.Contains("SORT-AS="));
    }
}

