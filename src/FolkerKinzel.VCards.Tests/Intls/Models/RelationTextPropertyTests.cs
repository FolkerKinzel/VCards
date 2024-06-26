﻿using FolkerKinzel.VCards.Enums;

using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class RelationTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod]
    public void RelationTextPropertyTest1()
    {
        const Rel relation = Rel.Acquaintance;
        string text = "Bruno Hübchen";

        var prop = RelationProperty.FromText(text, relation, GROUP);

        Assert.AreEqual(text, prop.Value?.String);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);

        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Text, prop.Parameters.DataType);
    }


    [TestMethod]
    public void RelationTextPropertyTest2()
    {
        const Rel relation = Rel.Acquaintance;
        string text = "Bruno Hübchen";

        var prop = RelationProperty.FromText(text, relation, GROUP);

        var vcard = new VCard
        {
            Relations = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(text, prop!.Value?.String);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);

        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Text, prop.Parameters.DataType);
    }


    [TestMethod]
    public void RelationTextPropertyTest3()
    {
        const Rel relation = Rel.Agent;
        string text = "Bruno Hübchen";

        var prop = RelationProperty.FromText(text, relation, GROUP);

        var vcard = new VCard
        {
            Relations = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(text, prop!.Value?.String);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);

        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Text, prop.Parameters.DataType);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        const string hello = "Hello";
        var prop = RelationProperty.FromText(hello);
        string s = prop.ToString();

        Assert.IsNotNull(s);
        StringAssert.Contains(s, hello);
        Assert.IsTrue(s.Length > hello.Length);
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var prop = RelationProperty.FromText(null);
        string s = prop.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Length > 0);
    }

}
