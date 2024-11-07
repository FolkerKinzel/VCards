
using System.Diagnostics.CodeAnalysis;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VCardTests
{
    [NotNull]
    public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext? TestContext { get; set; }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void DeserializeTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: true);
        ms.Position = 0;

        IList<VCard> list = Vcf.Deserialize(ms);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var textProp = new TextProperty("Test");

        var pidMap1 = new AppID(5, "http://folkerkinzel.de/file1.htm");
        var pidMap2 = new AppID(8, "http://folkerkinzel.de/file2.htm");
        textProp.Parameters.PropertyIDs = [new(1, pidMap1), new(7, null), new(1, pidMap2)];

        var vc = new VCard()
        {
            DisplayNames =
            [
                    null,
                    textProp
            ]
        };

        string s = vc.ToString();

        Assert.IsNotNull(s);
        Assert.IsFalse(string.IsNullOrWhiteSpace(s));
    }

    [TestMethod]
    public void ToStringTest2()
    {
        const string group = "TheGroup";

        VCard vc = VCardBuilder
         .Create()
         .DisplayNames.Add("Mickey", group: v => group)
         .Notes.Add(null)
         .VCard;

        vc.ToVcfString();

        string s = Vcf.Parse(Vcf.ToString(vc, VCdVersion.V2_1, options: Opts.Default.Set(Opts.WriteEmptyProperties)))[0].ToString();
        StringAssert.Contains(s, group);
        StringAssert.Contains(s, "2.1");
        StringAssert.Contains(s, "<EMPTY>");
    }

    [TestMethod]
    public void ToStringTest3()
    {
        const string group = "TheGroup";

        VCard vc = VCardBuilder
         .Create()
         .DisplayNames.Add("Mickey", group: v => group)
         .VCard;

        vc.ToVcfString();

        string s = Vcf.Parse(Vcf.ToString(vc, VCdVersion.V3_0))[0].ToString();
        StringAssert.Contains(s, group);
        StringAssert.Contains(s, "3.0");
    }

    [TestMethod]
    public void ToStringTest4()
    {
        const string group = "TheGroup";

        VCard vc = VCardBuilder
         .Create()
         .DisplayNames.Add("Mickey", group: v => group)
         .VCard;

        vc.ToVcfString();

        string s = Vcf.Parse(Vcf.ToString(vc, VCdVersion.V4_0))[0].ToString();
        StringAssert.Contains(s, group);
        StringAssert.Contains(s, "4.0");
    }

    [TestMethod]
    public void GetEnumeratorTest1()
    {
        var vc = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        System.Collections.IEnumerable enumerable = vc;

        foreach (object? _ in enumerable)
        {
            return;
        }

        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SeralizeVcfTest1()
    {
        using var stream = new MemoryStream();
        Vcf.Serialize(null!, stream);
    }

    [TestMethod]
    public void DeserializeTest2()
    {
        var stream = new FailStream(new ArgumentOutOfRangeException());
        IList<VCard> vCards = Vcf.Deserialize(stream);
        Assert.IsNotNull(vCards);
    }

    [TestMethod]
    public void DeserializeTest3()
    {
        var stream = new FailStream(new OutOfMemoryException());
        IList<VCard> vCards = Vcf.Deserialize(stream);
        Assert.IsNotNull(vCards);
    }

    [TestMethod]
    public void LoadCropped_2_1Test()
    {
        IList<VCard> vCards = Vcf.Load(TestFiles.Cropped_2_1vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    public void IsEmptyTest1()
    {
        var vc = new VCard();
        Assert.IsTrue(vc.IsEmpty());

        var adr = new AddressProperty("  ", null, null, postalCode: "", autoLabel: false);
        adr.Parameters.Label = "  ";

        vc.Addresses = [null, adr];
        Assert.IsTrue(vc.IsEmpty());

        vc.BirthDayViews = DateAndOrTimeProperty.FromText(null);
        Assert.IsTrue(vc.IsEmpty());

        var text = new TextProperty("  ");
        var names = new TextProperty?[] { null, text, new(null) };
        vc.DisplayNames = names;
        Assert.IsTrue(vc.IsEmpty());

        adr.Parameters.Label = "Label";
        Assert.IsFalse(vc.IsEmpty());

        adr.Parameters.Label = null;
        Assert.IsTrue(vc.IsEmpty());

        text.Parameters.Label = "Label";
        Assert.IsTrue(vc.IsEmpty());

        names[0] = new TextProperty("Name");
        Assert.IsFalse(vc.IsEmpty());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DereferenceTest1() => _ = FolkerKinzel.VCards.VCard.Dereference(null!).Count;

    [TestMethod]
    public void DereferenceTest2()
    {
        var vc = new VCard(setID: true);
        vc.Relations = 
            [null, 
            RelationProperty.FromGuid(Guid.Empty), 
            RelationProperty.FromGuid(Guid.NewGuid()), 
            RelationProperty.FromGuid(vc.ID?.Value ?? Guid.Empty)];

        IList<VCard> dereferenced = vc.Dereference();

        Assert.AreEqual(1, dereferenced.Count);
        RelationProperty?[]? rel = dereferenced[0].Relations?.ToArray();

        Assert.IsNotNull(rel);
        Assert.AreEqual(4, rel.Length);
        Assert.IsTrue(rel.Any(x => x?.Value?.VCard is not null));
    }

    [TestMethod]
    public void DereferenceTest3()
    {
        var vc = new VCard(setID: true);
        vc.Relations =
            [null,
            RelationProperty.FromGuid(Guid.Empty),
            RelationProperty.FromGuid(Guid.NewGuid()),
            ];

        IList<VCard> dereferenced = new VCard?[] { null, new VCard(setID: false), new VCard(setID: true), vc }.Dereference();

        Assert.AreEqual(3, dereferenced.Count);
        RelationProperty?[]? rel = dereferenced[2].Relations?.ToArray();

        Assert.IsNotNull(rel);
        Assert.AreEqual(3, rel.Length);
        Assert.IsFalse(rel.Any(x => x?.Value?.VCard is not null));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ReferenceTest1() => _ = FolkerKinzel.VCards.VCard.Reference(null!);

    [TestMethod]
    public void GroupsTest1()
    {
        var vc = new VCard();

        IEnumerable<string> groups = vc.GroupIDs;
        Assert.IsFalse(groups.Any());

        vc.DisplayNames = new TextProperty("Donald", " 4 1 ");
        vc.TimeStamp = new TimeStampProperty();
        vc.Addresses = [ null,
                         new("1", null, null, null, group: " g r 1 "),
                         new("2", null, null, null, group: "41")
                       ];
        vc.GeoCoordinates = [
                              new(new GeoCoordinate(1, 1), group: "GR1"),
                              null,
                              new(new GeoCoordinate(2, 2))
                            ];
        Assert.AreEqual(2, vc.GroupIDs.Count());
        Assert.AreEqual("42", vc.NewGroup());
    }

    [TestMethod]
    public void GroupsTest2()
    {
        var vc = new VCard(setID: false, setCreated: false)
        {
            DisplayNames = [new TextProperty("1"),
                                new TextProperty("2"),
                                new TextProperty("3", "g"),
                                new TextProperty("4", "g")]
        };
        IEnumerable<IGrouping<string?, KeyValuePair<Prop, VCardProperty>>> result = vc.Groups;
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(static x => x.Count() == 2));
    }

    [TestMethod]
    public void RegisterAppTest1()
    {
        VCard.SyncTestReset();

        VCard.RegisterApp(null);
        VCard.RegisterApp(null);
    }

    [TestMethod]
    public void RegisterAppTest2()
    {
        const string uri = "http://absolute.com/";
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri(uri));
        VCard.RegisterApp(new Uri(uri));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RegisterAppTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("../IamRelative", UriKind.Relative));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisterAppTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("http://absolute.com/"));
        VCard.RegisterApp(new Uri("http://other.com/"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisterAppTest5()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);
        VCard.RegisterApp(new Uri("http://other.com/"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisterAppTest6()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("http://other.com/"));
        VCard.RegisterApp(null);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisterAppTest7()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("http://absolute.com/"));
        VCard.RegisterApp(new Uri("../IamRelative", UriKind.Relative));
    }

    [TestMethod]
    public void VCardTest1()
    {
        VCard.SyncTestReset();
        _ = new VCard();
        Assert.IsNull(VCard.App);
    }

    [TestMethod]
    public void CloneTest1()
    {
        VCard vc1 = Utility.CreateVCard();

        Assert.AreEqual(1, vc1.Xmls?.Count());
        Assert.AreEqual(1, vc1.BirthDayViews?.Count());
        Assert.AreEqual(2, vc1.Addresses?.Count());
        Assert.IsNotNull(vc1.Addresses);
        vc1.Addresses = vc1.Addresses.First();
        Assert.AreEqual(1, vc1.NickNames?.Count());
        Assert.AreEqual(1, vc1.GenderViews?.Count());
        Assert.AreEqual(1, vc1.GeoCoordinates?.Count());
        Assert.AreEqual(2, vc1.NonStandards?.Count());
        Assert.AreEqual(1, vc1.AppIDs?.Count());
        Assert.AreEqual(1, vc1.TimeZones?.Count());
        Assert.AreEqual(2, vc1.GramGenders?.Count());

        var vc2 = (VCard)vc1.Clone();
        Assert.AreEqual(1, vc2.Xmls?.Count());
        Assert.AreNotSame(vc1.Xmls!.First(), vc2.Xmls!.First());
        Assert.AreEqual(1, vc2.BirthDayViews?.Count());
        Assert.AreNotSame(vc1.BirthDayViews!.First(), vc2.BirthDayViews!.First());
        Assert.AreEqual(1, vc2.Addresses?.Count());
        Assert.AreNotSame(vc1.Addresses!.First(), vc2.Addresses!.First());
        Assert.AreEqual(1, vc2.NickNames?.Count());
        Assert.AreNotSame(vc1.NickNames!.First(), vc2.NickNames!.First());
        Assert.AreEqual(1, vc2.GenderViews?.Count());
        Assert.AreNotSame(vc1.GenderViews!.First(), vc2.GenderViews!.First());
        Assert.AreEqual(1, vc2.GeoCoordinates?.Count());
        Assert.AreNotSame(vc1.GeoCoordinates!.First(), vc2.GeoCoordinates!.First());
        Assert.AreEqual(2, vc2.NonStandards?.Count());
        Assert.AreNotSame(vc1.NonStandards!.First(), vc2.NonStandards!.First());
        Assert.AreNotSame(vc1.NonStandards!.ElementAt(1), vc2.NonStandards!.ElementAt(1));
        Assert.AreEqual(1, vc2.AppIDs?.Count());
        Assert.AreNotSame(vc1.AppIDs!.First(), vc2.AppIDs!.First());
        Assert.AreEqual(1, vc2.TimeZones?.Count());
        Assert.AreNotSame(vc1.TimeZones!.First(), vc2.TimeZones!.First());
        Assert.AreEqual(2, vc2.GramGenders?.Count());
        Assert.AreNotSame(vc1.GramGenders!.First(), vc2.GramGenders!.First());

        Assert.AreNotSame(vc1, vc2);
    }

    [TestMethod]
    public void CloneTest2()
    {
        VCard vc1 = Utility.CreateVCard();

        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("http://123.com"));
        vc1.Sync.SetPropertyIDs();

        vc1.Xmls = vc1.Xmls.ConcatWith((XmlProperty)vc1.Xmls!.First()!.Clone());
        Assert.AreEqual(2, vc1.Xmls?.Count());

        vc1.BirthDayViews =vc1.BirthDayViews.ConcatWith((DateAndOrTimeProperty)vc1.BirthDayViews!.First()!.Clone());
        Assert.AreEqual(2, vc1.BirthDayViews?.Count());

        vc1.Addresses = vc1.Addresses.ConcatWith((AddressProperty)vc1.Addresses!.First()!.Clone());
        Assert.AreEqual(3, vc1.Addresses?.Count());

        vc1.NickNames = vc1.NickNames.ConcatWith((StringCollectionProperty)vc1.NickNames!.First()!.Clone());
        Assert.AreEqual(2, vc1.NickNames?.Count());

        vc1.GenderViews = vc1.GenderViews.ConcatWith((GenderProperty)vc1.GenderViews!.First()!.Clone());
        Assert.AreEqual(2, vc1.GenderViews?.Count());

        vc1.GeoCoordinates = vc1.GeoCoordinates.ConcatWith((GeoProperty)vc1.GeoCoordinates!.First()!.Clone());
        Assert.AreEqual(2, vc1.GeoCoordinates?.Count());

        vc1.NonStandards = vc1.NonStandards!.First();
        Assert.AreEqual(1, vc1.NonStandards?.Count());
        
        Assert.AreEqual(2, vc1.AppIDs?.Count());

        vc1.TimeZones = vc1.TimeZones.ConcatWith((TimeZoneProperty)vc1.TimeZones!.First()!.Clone());
        Assert.AreEqual(2, vc1.TimeZones?.Count());

        var vc2 = (VCard)vc1.Clone();
        Assert.AreEqual(2, vc2.Xmls?.Count());
        Assert.AreNotSame(vc1.Xmls!.First(), vc2.Xmls!.First());
        Assert.AreEqual(2, vc2.BirthDayViews?.Count());
        Assert.AreNotSame(vc1.BirthDayViews!.First(), vc2.BirthDayViews!.First());
        Assert.AreEqual(3, vc2.Addresses?.Count());
        Assert.AreNotSame(vc1.Addresses!.First(), vc2.Addresses!.First());
        Assert.AreEqual(2, vc2.NickNames?.Count());
        Assert.AreNotSame(vc1.NickNames!.First(), vc2.NickNames!.First());
        Assert.AreEqual(2, vc2.GenderViews?.Count());
        Assert.AreNotSame(vc1.GenderViews!.First(), vc2.GenderViews!.First());
        Assert.AreEqual(2, vc2.GeoCoordinates?.Count());
        Assert.AreNotSame(vc1.GeoCoordinates!.First(), vc2.GeoCoordinates!.First());
        Assert.AreEqual(1, vc2.NonStandards?.Count());
        Assert.AreNotSame(vc1.NonStandards!.First(), vc2.NonStandards!.First());
        Assert.AreEqual(2, vc2.AppIDs?.Count());
        Assert.AreNotSame(vc1.AppIDs!.First(), vc2.AppIDs!.First());
        Assert.AreEqual(2, vc2.TimeZones?.Count());
        Assert.AreNotSame(vc1.TimeZones!.First(), vc2.TimeZones!.First());

        Assert.AreNotSame(vc1, vc2);
    }

    [TestMethod]
    public void XAssistantTest1()
    {
        const string vCardString = """
            BEGIN:VCARD
            VERSION:3.0
            X-ASSISTANT:John Doe
            END:VCARD
            """;

        VCard vCard = Vcf.Parse(vCardString)[0];

        Assert.AreEqual(1, vCard.Relations?.Count());
    }

    [TestMethod]
    public void XEvolutionAssistantTest1()
    {
        const string vCardString = """
            BEGIN:VCARD
            VERSION:3.0
            X-EVOLUTION-ASSISTANT:John Doe
            END:VCARD
            """;

        VCard vCard = Vcf.Parse(vCardString)[0];

        Assert.AreEqual(1, vCard.Relations?.Count());
    }

    [TestMethod]
    public void XKAdressbookXAssistentsnameTest1()
    {
        const string vCardString = """
            BEGIN:VCARD
            VERSION:3.0
            X-KADDRESSBOOK-X-ASSISTANTSNAME:John Doe
            END:VCARD
            """;

        VCard vCard = Vcf.Parse(vCardString)[0];

        Assert.AreEqual(1, vCard.Relations?.Count());
    }
}
