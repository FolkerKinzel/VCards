﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class VcfSerializerTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetSerializerTest1()
    {
        using var serializer = VcfSerializer.GetSerializer(new MemoryStream(), false, (VCdVersion)(-10000), VcfOpts.Default, null);
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

        vc.DisplayNames = tProp1.Append(null).Concat(tProp2).Concat(tProp3);
        vc.ProductID = prodID;

        tProp1.Parameters.Index = 3;
        tProp2.Parameters.Index = 2;
        tProp3.Parameters.Index = 1;

        string s = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default | VcfOpts.WriteEmptyProperties | VcfOpts.SetPropertyIDs);

        uri = new Uri("http://other.com/");
        VCard.SyncTestReset();
        VCard.RegisterApp(uri);

        vc = Vcf.Parse(s)[0];

        tProp1 = vc.DisplayNames!.First()!;
        tProp2 = vc.DisplayNames!.ElementAt(1)!;
        tProp3 = vc.DisplayNames!.ElementAt(2)!;
        prodID = vc.ProductID!;
        prodID.Parameters.Index = 42;

        vc.DisplayNames = Enumerable.Empty<TextProperty?>().Append(null).Concat(vc.DisplayNames!);

        const VcfOpts options = VcfOpts.Default | VcfOpts.SetIndexes;

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

        _ = vc.ToVcfString(VCdVersion.V4_0, options: options | VcfOpts.SetPropertyIDs);

        Assert.AreEqual(1, tProp1.Parameters.Index);
        Assert.AreEqual(null, tProp2.Parameters.Index);
        Assert.AreEqual(2, tProp3.Parameters.Index);
        Assert.AreEqual(null, prodID.Parameters.Index);
        Assert.AreEqual(null, vc.AppIDs!.ElementAt(1).Parameters.Index);

        _ = vc.ToVcfString(VCdVersion.V4_0, options: options | VcfOpts.WriteEmptyProperties);

        Assert.AreEqual(1, tProp1.Parameters.Index);
        Assert.AreEqual(2, tProp2.Parameters.Index);
        Assert.AreEqual(3, tProp3.Parameters.Index);
        Assert.AreEqual(null, prodID.Parameters.Index);
        Assert.AreEqual(null, vc.AppIDs!.ElementAt(1).Parameters.Index);

    }

    [TestMethod]
    public void AppendGenderViewsTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Edit(props => [null])
            .GenderViews.Add(null, "something other")
            .GenderViews.Add(Gender.Empty)
            .GenderViews.Add(Sex.Other)
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        vc = Vcf.Parse(vcf)[0];

        Assert.IsNull(vc.GenderViews);
    }

    [TestMethod]
    public void AppendGenderViewsTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Add(Sex.Male)
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0, options: VcfOpts.Default.Set(VcfOpts.WriteWabExtensions));

        vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc.GenderViews);
    }

    [TestMethod]
    public void AppendGenderViewsTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Edit(props => [null])
            .VCard;

        string vcf = vc.ToVcfString(VCdVersion.V3_0);

        vc = Vcf.Parse(vcf)[0];

        Assert.IsNull(vc.GenderViews);
    }
}
