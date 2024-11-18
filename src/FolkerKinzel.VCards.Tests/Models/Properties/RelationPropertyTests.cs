using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

//internal class RelationPropertyDerived : RelationProperty
//{
//    public RelationPropertyDerived(RelationProperty prop) : base(prop)
//    {
//    }

//    public RelationPropertyDerived(ParameterSection parameters, string? group)
//        : base(parameters, group)
//    {
//    }

//    public RelationPropertyDerived(Rel? relation, string? group)
//        : base(relation, group)
//    {
//    }

//    public override object Clone() => throw new NotImplementedException();
//    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
//}

[TestClass]
public class RelationPropertyTests
{
    //private class TestIEnumerable : RelationProperty
    //{
    //    public TestIEnumerable() : base(Rel.Contact, null) { }
    //    public override object Clone() => throw new NotImplementedException();
    //    protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
    //    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    //}

    [TestMethod]
    public void IEnumerableTest()
        => Assert.AreEqual(1, new RelationProperty(Relation.Empty).AsWeakEnumerable().Count());

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromUriTest1()
    {
        Assert.IsTrue(Uri.TryCreate("../bla.txt", UriKind.Relative, out Uri? uri));
        _ = new RelationProperty(Relation.Create(ContactID.Create(uri)));
    }

    [TestMethod]
    public void FromUriTest2() => Assert.IsTrue(new RelationProperty(Relation.Empty).IsEmpty);


    [TestMethod]
    public void FromUriTest3()
    {
        Assert.IsTrue(Uri.TryCreate("mailto:jemand@beispiel.com", UriKind.Absolute, out Uri? uri));
        Assert.IsInstanceOfType<RelationProperty>(new RelationProperty(Relation.Create(ContactID.Create(uri))));
    }

    [TestMethod]
    public void FromUriTest4()
    {
        Assert.IsTrue(Uri.TryCreate("urn:uuid:550e8400-e29b-11d4-a716-446655440000", UriKind.Absolute, out Uri? uri));
        var prop = new RelationProperty(Relation.Create(ContactID.Create(uri)));
        Assert.IsInstanceOfType<RelationProperty>(prop);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    public void IsEmptyTest1()
    {
        var prop = new RelationProperty(Relation.Empty);
        Assert.IsTrue(prop.IsEmpty);
    }


    [DataTestMethod]
    [DataRow("   ")]
    [DataRow("../bla.txt")]
    public void ParseTest1(string input)
    {
        var row = VcfRow.Parse(("RELATED:" + input).AsMemory(), new VcfDeserializationInfo());
        Assert.IsNotNull(row);

        var prop = RelationProperty.Parse(row!, VCdVersion.V4_0);
        Assert.IsInstanceOfType<RelationProperty>(prop);
    }

    [TestMethod]
    public void ParseTest2()
    {
        var row = VcfRow.Parse("X-ASSISTANT;QUOTED-PRINTABLE;UTF-8:M=C3=BCller".AsMemory(), new VcfDeserializationInfo());
        Assert.IsNotNull(row);
        Assert.AreEqual(Enc.QuotedPrintable, row.Parameters.Encoding);

        var prop = RelationProperty.Parse(row, VCdVersion.V2_1);
        Assert.AreEqual("Müller", prop.Value.ContactID?.String);
    }

    [TestMethod]
    public void ValueTest1()
    {
        VCardProperty prop = new RelationProperty(Relation.Create(ContactID.Create("abc")));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<Relation>(prop.Value);
    }

    [TestMethod]
    public void ValueTest2()
    {
        VCardProperty prop = new RelationProperty(Relation.Create(ContactID.Create()));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<Relation>(prop.Value);
    }

    [TestMethod]
    public void ValueTest3()
    {
        VCardProperty prop = new RelationProperty(Relation.Create(new VCard { DisplayNames = new TextProperty("Folker") }));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<Relation>(prop.Value);
    }

}
