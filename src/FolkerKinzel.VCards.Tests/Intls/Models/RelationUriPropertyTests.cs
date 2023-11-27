using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class RelationUriPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod]
    public void RelationUriPropertyTest1()
    {
        const Rel relation = Rel.Acquaintance;
        var uri = new Uri("http://test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(new UriProperty(uri, new ParameterSection() { RelationType = relation }, GROUP));

        Assert.AreEqual(uri, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }


    [TestMethod]
    public void RelationUriPropertyTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const Rel relation = Rel.Acquaintance;
        var uri = new Uri("http://test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(new UriProperty(uri, new ParameterSection() { RelationType = relation }, GROUP));

        var vcard = new VCard
        {
            Relations = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        var list = VCard.ParseVcf(s).ToList();

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count());

        vcard = list.First();

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }


    [TestMethod]
    public void RelationUriPropertyTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const Rel relation = Rel.Agent;
        var uri = new Uri("http://test.ääh.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(new UriProperty(uri, new ParameterSection() { RelationType = relation }, GROUP));

        var vcard = new VCard
        {
            Relations = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IList<VCard> list = VCard.ParseVcf(s).ToList();

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list.First();

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }

    [TestMethod]
    public void RelationUriPropertyTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const Rel relation = Rel.Agent;
        var uri = new Uri("cid:test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(new UriProperty(uri, new ParameterSection() { RelationType = relation }, GROUP));

        var vcard = new VCard
        {
            Relations = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V2_1);

        var list = VCard.ParseVcf(s).ToList();

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count());

        vcard = list.First();

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }




    [TestMethod]
    public void PrepareForVcfSerializationTest1()
    {
        var prop = RelationProperty.FromUri(new Uri("http://folker.de/"));
        prop.Parameters.Clear();

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, VcfOptions.Default);

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = RelationProperty.FromUri(new Uri("http://folker.de/", UriKind.Absolute));

        var prop2 = (RelationProperty)prop1.Clone();

        Assert.AreSame(prop1.Value!.Uri, prop2.Value!.Uri);
        Assert.AreNotSame(prop1, prop2);

    }

}
