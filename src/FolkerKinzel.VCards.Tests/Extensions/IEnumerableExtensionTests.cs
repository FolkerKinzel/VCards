using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass]
public class IEnumerableExtensionTests
{
    public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext? TestContext { get; set; }

    private static List<VCard?> GenerateVCardList()
    {
        var agent = new VCard()
        {
            DisplayNames = new TextProperty?[]
            {
                null,
                new TextProperty("The Agent", "myGroup")
            }
        };

        return new List<VCard?>
        {
            null,
            new VCard()
            {
                Relations = new RelationProperty?[]
                {
                    null,
                    new RelationVCardProperty(agent, RelationTypes.Agent | RelationTypes.CoWorker, "otherGroup" )
                }
            }
        };
    }

    [TestMethod]
    public void ReferenceVCardsTest()
    {
        List<VCard?>? list = GenerateVCardList();

        list = list.ReferenceVCards().ToList()!;

        Assert.AreEqual(2, list.Count);

        VCard? vc1 = list[0];

        Assert.IsInstanceOfType(vc1, typeof(VCard));
        Assert.IsNotNull(vc1?.Relations);

        var uuidProp = vc1?.Relations?.FirstOrDefault(x => x is RelationUuidProperty) as RelationUuidProperty;
        Assert.IsNotNull(uuidProp);
        Guid o1 = uuidProp!.Value;

        Assert.IsFalse(uuidProp.IsEmpty);

        VCard? vc2 = list[1];

        Assert.IsInstanceOfType(vc2, typeof(VCard));
        Assert.IsNotNull(vc2?.UniqueIdentifier);

        Guid? o2 = vc2?.UniqueIdentifier?.Value;

        Assert.IsTrue(o2.HasValue);
        Assert.AreEqual((Guid)o1!, o2!.Value);
    }

    [TestMethod]
    public void DereferenceVCardsTest()
    {
        List<VCard?>? list = GenerateVCardList();

        list = list.ReferenceVCards().ToList()!;

        Assert.AreEqual(2, list.Count);
        Assert.IsNull(list[0]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));

        list = list.DereferenceVCards().ToList()!;

        Assert.AreEqual(2, list.Count);
        Assert.IsNotNull(list[0]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentException))]
    public void SaveVcfTest_InvalidFilename(VCdVersion version)
    {
        var list = new List<VCard?>() { new VCard() };

        string path = "   ";

        list.SaveVcf(path, version);

        Assert.IsFalse(File.Exists(path));
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SaveVcfTest_EmptyList(VCdVersion version)
    {
        var list = new List<VCard?>();

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_Empty.vcf");

        list.SaveVcf(path, version);

        Assert.IsFalse(File.Exists(path));
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveVcfTest_ListNull(VCdVersion version)
    {
        List<VCard?>? list = null;

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_Empty.vcf");

        list!.SaveVcf(path, version);
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveVcfTest_fileNameNull(VCdVersion version)
    {
        var list = new List<VCard?>() { new VCard() };

        list.SaveVcf(null!, version);
    }

    [TestMethod]
    public void SaveVcfTest_vCard2_1()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v2.1.vcf");

        list.SaveVcf(path, VCdVersion.V2_1);

        IList<VCard> list2 = VCard.LoadVcf(path);

        Assert.AreNotEqual(list.Count, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
    }

    [TestMethod]
    public void SaveVcfTest_vCard3_0()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v3.0.vcf");

        list.SaveVcf(path, VCdVersion.V3_0);

        IList<VCard> list2 = VCard.LoadVcf(path);

        Assert.AreNotEqual(list.Count, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);

    }

    [TestMethod]
    public void SaveVcfTest_vCard4_0()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v4.0.vcf");

        list.SaveVcf(path, VCdVersion.V4_0);

        IList<VCard> list2 = VCard.LoadVcf(path);

        Assert.AreEqual(2, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SerializeVcfTest1() => new List<VCard?>().SerializeVcf(null!, VCdVersion.V3_0);

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SerializeVcfTest2(VCdVersion version)
    {
        List<VCard?> list = GenerateVCardList();

        using var ms = new MemoryStream();

        list.SerializeVcf(ms, version, leaveStreamOpen: true);

        Assert.AreNotEqual(0, ms.Length);

        ms.Position = 0;

        Assert.AreNotEqual(-1, ms.ReadByte());
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SerializeVcfTest3(VCdVersion version)
    {
        List<VCard?> list = GenerateVCardList();

        using var ms = new MemoryStream();

        list.SerializeVcf(ms, version, leaveStreamOpen: false);

        _ = ms.Length;

    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void ToVcfStringTest(VCdVersion version)
    {
        List<VCard?> list = GenerateVCardList();

        string s = list.ToVcfString(version);

        IList<VCard> list2 = VCard.ParseVcf(s);

        Assert.AreNotEqual(0, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
        Assert.AreEqual(version, list2[0].Version);
    }

    [TestMethod]
    public void PrefOrNullTest1()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.PrefOrNull());
    }

    [TestMethod]
    public void PrefOrNullTest1b()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.PrefOrNull(true));
    }

    [TestMethod]
    public void PrefOrNullTest2()
    {
        VCardProperty[]? props = new[] {new TextProperty(null)};
        Assert.IsNull(props.PrefOrNull());
    }

    [TestMethod]
    public void PrefOrNullTest3()
    {
        VCardProperty[]? props = new[] { new TextProperty(null) };
        Assert.IsNotNull(props.PrefOrNull(ignoreEmptyItems: false));
    }

    [TestMethod]
    public void PrefOrNullTest4()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        Assert.IsNotNull(props.PrefOrNull());
    }

    [TestMethod]
    public void PrefOrNullTest5()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        Assert.IsNotNull(props.PrefOrNull(null));
    }

    [TestMethod]
    public void PrefOrNullTest6()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        props.PrefOrNull(static x => x is TextProperty);
    }

    [TestMethod]
    public void PrefOrNullTest7()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.PrefOrNull(static x => x is TextProperty));
    }

    [TestMethod]
    public void FirstOrNullTest1()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.FirstOrNull());
    }

    [TestMethod]
    public void FirstOrNullTest1b()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.FirstOrNull(true));
    }

    [TestMethod]
    public void FirstOrNullTest2()
    {
        VCardProperty[]? props = new[] { new TextProperty(null) };
        Assert.IsNull(props.FirstOrNull());
    }


    [TestMethod]
    public void FirstOrNullTest3()
    {
        VCardProperty[]? props = new[] { new TextProperty(null) };
        Assert.IsNotNull(props.FirstOrNull(ignoreEmptyItems: false));
    }

    [TestMethod]
    public void FirstOrNullTest4()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        Assert.IsNotNull(props.FirstOrNull());
    }

    [TestMethod]
    public void FirstOrNullTest5()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        Assert.IsNotNull(props.FirstOrNull(null));
    }

    [TestMethod]
    public void FirstOrNullTest6()
    {
        VCardProperty[]? props = new[] { new TextProperty("Hi") };
        props.FirstOrNull(static x => x is TextProperty);
    }

    [TestMethod]
    public void FirstOrNullTest7()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.FirstOrNull(static x => x is TextProperty));
    }

    [TestMethod]
    public void OrderByPrefTest1()
    {
        VCardProperty[]? props = null;
        Assert.IsFalse(props.OrderByPref().Any());
    }

    [TestMethod]
    public void OrderByIndexTest1()
    {
        VCardProperty[]? props = null;
        Assert.IsFalse(props.OrderByIndex().Any());
    }

    [TestMethod]
    public void OrderByPrefTest2()
    {
        var prop = new TextProperty("Hi");
        VCardProperty[]? props = new[] { prop };
        Assert.IsTrue(props.OrderByPref(false).Any());
    }

    [TestMethod]
    public void OrderByIndexTest2()
    {
        var prop = new TextProperty("Hi");
        prop.Parameters.Index = 1;
        VCardProperty[]? props = new[] { prop };
        Assert.IsTrue(props.OrderByIndex(false).Any());
    }

    [TestMethod]
    public void GroupByVCardGroupTest1()
    {
        TextProperty[]? props = null;
        Assert.IsNotNull(props.GroupByVCardGroup());
    }

    [TestMethod]
    public void GroupByAltIDTest1()
    {
        TextProperty[]? props = null;
        Assert.IsNotNull(props.GroupByAltID());
    }

    [TestMethod]
    public void GroupByAltIDTest2()
    {
        TextProperty prop1 = new TextProperty("1");
        TextProperty prop2 = new TextProperty("2");
        TextProperty prop3 = new TextProperty("3");
        TextProperty prop4 = new TextProperty("4");
        TextProperty prop5 = new TextProperty("5");
        prop1.Parameters.AltID = "A";
        prop2.Parameters.AltID = "A";
        prop3.Parameters.AltID = "a";

        TextProperty?[]? props = new[] { null, prop1, null, prop2, null, prop3, null, prop4, null, prop5, null };
        var groups = props.GroupByAltID();

        Assert.AreEqual(3, groups.Count());
        Assert.IsTrue(groups.Any(gr => gr.Key is null));
        Assert.IsTrue(groups.All(gr => gr.SelectMany(x => x).All(x => x != null)));
    }

    [TestMethod]
    public void NewAltIDTest1() => Assert.AreEqual("0", ((IEnumerable<TextProperty?>?)null).NewAltID());

    [TestMethod]
    public void NewAltIDTest2()
    {
        var props = new TextProperty[] { new TextProperty("1"), new TextProperty("2"), new TextProperty("3") };
        Assert.AreEqual("0", props.NewAltID());
        props[2].Parameters.AltID = "TheAltID";
        Assert.AreEqual("0", props.NewAltID());
        props[1].Parameters.AltID = "-25";
        Assert.AreEqual("0", props.NewAltID());
        props[0].Parameters.AltID = "41";
        Assert.AreEqual("42", props.NewAltID());
    }

    [TestMethod]
    public void ContainsGroupTest1()
    {
        const string group = "gr";
        TextProperty? prop = null;
        Assert.IsFalse(prop.ContainsGroup(group));
        prop = new TextProperty("");
        Assert.IsFalse(prop.ContainsGroup(group));
        prop.Group = group;
        Assert.IsFalse(prop.ContainsGroup(group));
        Assert.IsTrue(prop.ContainsGroup(group, ignoreEmptyItems: false));
        Assert.IsTrue(prop.ContainsGroup("GR", ignoreEmptyItems: false));
        Assert.IsFalse(prop.ContainsGroup("42", ignoreEmptyItems: false));
    }

    [TestMethod]
    public void ConcatenateTest1()
    {
        var vc = new VCard();

        vc.DisplayNames = vc.DisplayNames.Concatenate(null);

        vc.DisplayNames = vc.DisplayNames.Concatenate(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.Concatenate(null);
        vc.DisplayNames = vc.DisplayNames.Concatenate(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.Concatenate(null);
        vc.DisplayNames = new TextProperty("Hi");
        vc.DisplayNames = vc.DisplayNames.Concatenate(new TextProperty("Hi"));

    }

    [TestMethod]
    public void ConcatenateTest2()
    {
        var vc = new VCard();

        vc.DisplayNames = vc.DisplayNames.ConcatWith(null);

        vc.DisplayNames = vc.DisplayNames.ConcatWith(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.ConcatWith(null);
        vc.DisplayNames = vc.DisplayNames.ConcatWith(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.ConcatWith(null);
        vc.DisplayNames = new TextProperty("Hi");
        vc.DisplayNames = vc.DisplayNames.Concat(new TextProperty("Hi"));
    }

    [TestMethod]
    public void ConcatenateTest3()
    {
        var vc = new VCard();

        vc.Relations = vc.Relations.ConcatWith(null);
        vc.Relations = vc.Relations.ConcatWith(RelationProperty.FromText("Hi"));
        vc.Relations = vc.Relations.ConcatWith(null);
        vc.Relations = vc.Relations.ConcatWith(RelationProperty.FromText("Hi"));
        vc.Relations = vc.Relations.ConcatWith(null);
        vc.Relations = RelationProperty.FromText("Hi");
        vc.Relations = vc.Relations.ConcatWith(RelationProperty.FromText("Hi"));
    }


}