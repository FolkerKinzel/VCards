using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class VcfSerializerTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetSerializerTest1()
    {
        using var serializer = VcfSerializer.GetSerializer(new MemoryStream(), false, (VCdVersion)(-10000), VcfOptions.Default, null);
    }

    [TestMethod]
    public void ResetBuildersTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        using var stream = new MemoryStream();
        using var serializer = VcfSerializer.GetSerializer(stream, false, VCdVersion.V3_0, VcfOptions.Default, null);

        serializer.Builder.Capacity = 20000;
        serializer.Worker.Capacity = 20000;

        var vc = new VCard { DisplayNames = new TextProperty("Goofy") };

        serializer.Serialize(vc);

        Assert.IsTrue(serializer.Builder.Capacity < 10000);
        Assert.IsTrue(serializer.Worker.Capacity < 10000);
    }

    [TestMethod]
    public void AppendLineFoldingTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var vc = new VCard { DisplayNames = new TextProperty("A" + new string('ä', 80)) };

        string s = vc.ToVcfString();

        Assert.IsTrue(s.GetLinesCount() > 5);
    }


    [TestMethod]
    public void SetIndexesTest()
    {
        var uri = new Uri("http://folker.de/");
        VCard.SyncTestReset();
        VCard.RegisterApp(uri);

        var vc = new VCard();
        TextProperty tProp1 = new("1");
        TextProperty tProp2 = new(null);
        TextProperty tProp3 = new("2");

        TextProperty prodID = new("product");

        vc.DisplayNames = tProp1.ConcatWith(null).Concat(tProp2).Concat(tProp3);
        vc.ProductID = prodID;

        tProp1.Parameters.Index = 3;
        tProp2.Parameters.Index = 2;
        tProp3.Parameters.Index = 1;

        string s = vc.ToVcfString(VCdVersion.V4_0, options: VcfOptions.Default | VcfOptions.WriteEmptyProperties);

        uri = new Uri("http://other.com/");
        VCard.SyncTestReset();
        VCard.RegisterApp(uri);

        vc = Vcf.ParseVcf(s)[0];

        tProp1 = vc.DisplayNames!.First()!;
        tProp2 = vc.DisplayNames!.ElementAt(1)!;
        tProp3 = vc.DisplayNames!.ElementAt(2)!;
        prodID = vc.ProductID!;
        prodID.Parameters.Index = 42;

        vc.DisplayNames = Enumerable.Empty<TextProperty?>().ConcatWith(null).Concat(vc.DisplayNames!);

        const VcfOptions options = VcfOptions.Default | VcfOptions.SetIndexes;

        _ = vc.ToVcfString(VCdVersion.V2_1, options: options);

        Assert.AreEqual(3, tProp1.Parameters.Index);
        Assert.AreEqual(2, tProp2.Parameters.Index);
        Assert.AreEqual(1, tProp3.Parameters.Index);
        Assert.AreEqual(42, prodID.Parameters.Index);

        _ = vc.ToVcfString(VCdVersion.V3_0, options: options);

        Assert.AreEqual(3, tProp1.Parameters.Index);
        Assert.AreEqual(2, tProp2.Parameters.Index);
        Assert.AreEqual(1, tProp3.Parameters.Index);
        Assert.AreEqual(42, prodID.Parameters.Index);

        //vc.Sync.SetPropertyIDs();

        _ = vc.ToVcfString(VCdVersion.V4_0, options: options);

        Assert.AreEqual(1, tProp1.Parameters.Index);
        Assert.AreEqual(null, tProp2.Parameters.Index);
        Assert.AreEqual(2, tProp3.Parameters.Index);
        Assert.AreEqual(null, prodID.Parameters.Index);
        Assert.AreEqual(null, vc.AppIDs!.ElementAt(1).Parameters.Index);

        _ = vc.ToVcfString(VCdVersion.V4_0, options: options | VcfOptions.WriteEmptyProperties);

        Assert.AreEqual(1, tProp1.Parameters.Index);
        Assert.AreEqual(2, tProp2.Parameters.Index);
        Assert.AreEqual(3, tProp3.Parameters.Index);
        Assert.AreEqual(null, prodID.Parameters.Index);
        Assert.AreEqual(null, vc.AppIDs!.ElementAt(1).Parameters.Index);

    }
}
