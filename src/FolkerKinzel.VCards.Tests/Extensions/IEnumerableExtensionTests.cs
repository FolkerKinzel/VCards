using System.Diagnostics.CodeAnalysis;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass]
public class IEnumerableExtensionTests
{
    [NotNull]
    public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext? TestContext { get; set; }

    private static List<VCard?> GenerateVCardList()
    {
        var agent = new VCard()
        {
            DisplayNames =
            [
                null,
                new("The Agent", "myGroup")
            ]
        };

        return
        [
            null,
            new()
            {
                Relations =
                [
                    null,
                    new RelationVCardProperty(agent, Rel.Agent | Rel.CoWorker, "otherGroup" )
                ]
            }
        ];
    }

    [TestMethod]
    public void ReferenceVCardsTest()
    {
        List<VCard?>? list = GenerateVCardList();

        list = list.Reference().ToList()!;

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
        Assert.IsNotNull(vc2?.ID);

        Guid? o2 = vc2?.ID?.Value;

        Assert.IsTrue(o2.HasValue);
        Assert.AreEqual((Guid)o1!, o2!.Value);
    }

    [TestMethod]
    public void DereferenceVCardsTest()
    {
        List<VCard?>? list = GenerateVCardList();

        list = list.Reference().ToList()!;

        Assert.AreEqual(2, list.Count);
        Assert.IsNull(list[0]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));

        list = list.Dereference().ToList()!;

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
        var list = new List<VCard?>() { new() };

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

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, "SaveVcfTest_Empty.vcf");

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

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, "SaveVcfTest_Empty.vcf");

        list!.SaveVcf(path, version);
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveVcfTest_fileNameNull(VCdVersion version)
    {
        var list = new List<VCard?>() { new() };

        list.SaveVcf(null!, version);
    }

    [TestMethod]
    public void SaveVcfTest_vCard2_1()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, "SaveVcfTest_v2.1.vcf");

        list.SaveVcf(path, VCdVersion.V2_1);

        IList<VCard> list2 = Vcf.Load(path);

        Assert.AreNotEqual(list.Count, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
    }

    [TestMethod]
    public void SaveVcfTest_vCard3_0()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, "SaveVcfTest_v3.0.vcf");

        list.SaveVcf(path, VCdVersion.V3_0);

        IList<VCard> list2 = Vcf.Load(path);

        Assert.AreNotEqual(list.Count, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
    }

    [TestMethod]
    public void SaveVcfTest_vCard4_0()
    {
        List<VCard?> list = GenerateVCardList();

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, "SaveVcfTest_v4.0.vcf");

        list.SaveVcf(path, VCdVersion.V4_0);

        IList<VCard> list2 = Vcf.Load(path);

        Assert.AreEqual(2, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SaveTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, $"SaveTest_{version}.vcf");

        vcard.SaveVcf(path, version);

        IList<VCard> list = Vcf.Load(path);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveTest_fileNameNull()
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        vcard.SaveVcf(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SaveTest_InvalidFileName()
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        vcard.SaveVcf("   ");
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SerializeTest_StreamNull(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        vcard.SerializeVcf(null!, version);
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SerializeTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: true);

        Assert.AreNotEqual(0, ms.Length);

        ms.Position = 0;

        Assert.AreNotEqual(-1, ms.ReadByte());
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SerializeTest_CloseStream(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: false);

        _ = ms.Length;
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
    public void SerializeTest_StreamClosed(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        using var ms = new MemoryStream();
        ms.Close();

        vcard.SerializeVcf(ms, version);
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

        IList<VCard> list2 = Vcf.Parse(s);

        Assert.AreNotEqual(0, list2.Count);
        Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
        Assert.AreEqual(version, list2[0].Version);
    }

    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void ToVcfStringTest1(VCdVersion version)
    {
        VCard.SyncTestReset();

        var vcard = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        string s = vcard.ToVcfString(version);

        IList<VCard> list = Vcf.Parse(s);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
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
        VCardProperty[]? props = new[] { new TextProperty(null) };
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
    public void PrefOrNullTest8()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.PrefOrNull(null));
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
    public void FirstOrNullTest8()
    {
        VCardProperty[]? props = null;
        Assert.IsNull(props.FirstOrNull(null));
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
    public void GroupByAltIDTest1()
    {
        TextProperty[]? props = null;
        Assert.IsNotNull(props.GroupByAltID());
    }

    [TestMethod]
    public void GroupByAltIDTest2()
    {
        var prop1 = new TextProperty("1");
        var prop2 = new TextProperty("2");
        var prop3 = new TextProperty("3");
        var prop4 = new TextProperty("4");
        var prop5 = new TextProperty("5");
        prop1.Parameters.AltID = "A";
        prop2.Parameters.AltID = "A";
        prop3.Parameters.AltID = "a";

        TextProperty?[]? props = [null, prop1, null, prop2, null, prop3, null, prop4, null, prop5, null];
        IEnumerable<IGrouping<string?, TextProperty>> groups = props.GroupByAltID();

        Assert.AreEqual(3, groups.Count());
        Assert.IsTrue(groups.Any(gr => gr.Key is null));
        Assert.IsTrue(groups.All(gr => gr.SelectMany(x => x).All(x => x is not null)));
    }

    [TestMethod]
    public void NewAltIDTest1() => Assert.AreEqual("0", ((IEnumerable<TextProperty?>?)null).NewAltID());

    [TestMethod]
    public void NewAltIDTest2()
    {
        var props = new TextProperty[] { new("1"), new("2"), new("3") };
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
    public void ConcatenateTest2()
    {
        var vc = new VCard
        {
            DisplayNames = [null]
        };

        vc.DisplayNames = vc.DisplayNames.ConcatWith(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.ConcatWith(null);
        vc.DisplayNames = vc.DisplayNames.ConcatWith(new TextProperty("Hi"));
        vc.DisplayNames = vc.DisplayNames.ConcatWith(null);
        CollectionAssert.AllItemsAreNotNull(vc.DisplayNames.ToArray());
        Assert.AreEqual(2, vc.DisplayNames.Count());

        vc.DisplayNames = new TextProperty("Hi");
        vc.DisplayNames = vc.DisplayNames.ConcatWith(new TextProperty("Hi"));
        Assert.AreEqual(2, vc.DisplayNames.Count());

        var props = new TextProperty?[] { new("1"), null, new("2") };
        vc.DisplayNames = vc.DisplayNames.ConcatWith(props);
        Assert.AreEqual(4, vc.DisplayNames.Count());

        var nested = new List<TextProperty?[]>
        {
            props
        };
        IEnumerable<IEnumerable<TextProperty?>> nested2 = nested;

        // This MUST not compile:
        //vc.DisplayNames = vc.DisplayNames.ConcatWith(nested2);

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
        CollectionAssert.AllItemsAreNotNull(vc.Relations.ToArray());
        Assert.AreEqual(2, vc.Relations.Count());

        vc.Relations = RelationProperty.FromText("Hi");
        vc.Relations = vc.Relations.ConcatWith(RelationProperty.FromText("Hi"));
        Assert.AreEqual(2, vc.Relations.Count());

    }

    [TestMethod]
    public void SetPreferencesTest1()
    {
        TextProperty?[]? arr = null;

        arr.SetPreferences();
        arr.UnsetPreferences();
        

        arr = [new("1"), null, new(null), new("2")];

        arr.SetPreferences();
        Assert.AreEqual(1, arr[0]!.Parameters.Preference);
        Assert.AreEqual(100, arr[2]!.Parameters.Preference);
        Assert.AreEqual(2, arr[3]!.Parameters.Preference);

        arr.UnsetPreferences();
        Assert.AreEqual(100, arr[0]!.Parameters.Preference);
        Assert.AreEqual(100, arr[2]!.Parameters.Preference);
        Assert.AreEqual(100, arr[3]!.Parameters.Preference);

        arr.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, arr[0]!.Parameters.Preference);
        Assert.AreEqual(2, arr[2]!.Parameters.Preference);
        Assert.AreEqual(3, arr[3]!.Parameters.Preference);

        arr.SetPreferences();
        Assert.AreEqual(1, arr[0]!.Parameters.Preference);
        Assert.AreEqual(100, arr[2]!.Parameters.Preference);
        Assert.AreEqual(2, arr[3]!.Parameters.Preference);
    }

    [TestMethod]
    public void SetIndexesTest1()
    {
        TextProperty?[]? arr = null;

        arr.SetIndexes();
        arr.UnsetIndexes();

        arr = [new("1"), null, new(null), new("2")];

        arr.SetIndexes();
        Assert.AreEqual(1, arr[0]!.Parameters.Index);
        Assert.AreEqual(null, arr[2]!.Parameters.Index);
        Assert.AreEqual(2, arr[3]!.Parameters.Index);

        arr.UnsetIndexes();
        Assert.AreEqual(null, arr[0]!.Parameters.Index);
        Assert.AreEqual(null, arr[2]!.Parameters.Index);
        Assert.AreEqual(null, arr[3]!.Parameters.Index);

        arr.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, arr[0]!.Parameters.Index);
        Assert.AreEqual(2, arr[2]!.Parameters.Index);
        Assert.AreEqual(3, arr[3]!.Parameters.Index);

        arr.SetIndexes();
        Assert.AreEqual(1, arr[0]!.Parameters.Index);
        Assert.AreEqual(null, arr[2]!.Parameters.Index);
        Assert.AreEqual(2, arr[3]!.Parameters.Index);
    }

    [TestMethod]
    public void SetAltIDTest1()
    {
        TextProperty?[]? arr = null;

        arr.SetAltID("1");

        arr = [new("1"), null, new(null), new("2")];

        arr.SetAltID("1");
        Assert.AreEqual("1", arr[0]!.Parameters.AltID);
        Assert.AreEqual("1", arr[2]!.Parameters.AltID);
        Assert.AreEqual("1", arr[3]!.Parameters.AltID);
    }

    [TestMethod]
    public void RemoveTest1()
    {
        IEnumerable<TextProperty?>? numerable = null;
        IEnumerable<TextProperty?> result = numerable.Remove(new TextProperty("Hi"));
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void RemoveTest1b()
    {
        IEnumerable<TextProperty?>? numerable = new TextProperty("Hi").Append(null).Append(null);
        IEnumerable<TextProperty?> result = numerable.Remove((TextProperty?)null);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.IsNotNull(result.First());
    }

    [TestMethod]
    public void RemoveTest1c()
    {
        var prop = new TextProperty("Hi");
        IEnumerable<TextProperty?>? numerable = prop.Append(null).Append(null);
        IEnumerable<TextProperty?> result = numerable.Remove(prop);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void RemoveTest2()
    {
        IEnumerable<TextProperty?>? numerable = null;
        IEnumerable<TextProperty?> result = numerable.Remove(x => x.Value == "Hi");
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void RemoveTest3()
    {
        var prop = new TextProperty("Hi");
        IEnumerable<TextProperty?>? numerable = [prop];
        Assert.IsNotNull(numerable.Remove(prop));
    }

    [TestMethod]
    public void RemoveTest4()
    {
        var prop = new TextProperty("Hi");
        IEnumerable<TextProperty?>? numerable = prop;
        IEnumerable<TextProperty?>? newProp = numerable.Remove(prop);
        Assert.IsNotNull(newProp);
        Assert.AreEqual(0, newProp.Count());
    }
}