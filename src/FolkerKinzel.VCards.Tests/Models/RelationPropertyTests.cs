using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Tests;
using FolkerKinzel.VCards.Intls.Models;

namespace FolkerKinzel.VCards.Models.Tests;

internal class RelationPropertyDerived : RelationProperty
{
    public RelationPropertyDerived(RelationProperty prop) : base(prop)
    {
    }

    public RelationPropertyDerived(ParameterSection parameters, string? propertyGroup)
        : base(parameters, propertyGroup)
    {
    }

    public RelationPropertyDerived(RelationTypes? relation, string? propertyGroup)
        : base(relation, propertyGroup)
    {
    }

    public override object Clone() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

[TestClass]
public class RelationPropertyTests
{
    private class TestIEnumerable : RelationProperty
    {
        public TestIEnumerable() : base(Enums.RelationTypes.Contact, null) { }
        public override object Clone() => throw new NotImplementedException();
        protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    public void IEnumerableTest() => Assert.AreEqual(1, new TestIEnumerable().AsWeakEnumerable().Count());

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromUriTest1()
    {
        Assert.IsTrue(Uri.TryCreate("../bla.txt", UriKind.Relative, out Uri? uri));
        _ = RelationProperty.FromUri(uri);
    }

    [TestMethod]
    public void FromUriTest2() => Assert.IsTrue(RelationProperty.FromUri(null).IsEmpty);

    [TestMethod]
    public void IsEmptyTest1()
    {
        RelationProperty prop = new RelationPropertyDerived(RelationProperty.FromText("Hi"));
        Assert.IsTrue(prop.IsEmpty);
    }


    [DataTestMethod]
    [DataRow("   ")]
    [DataRow("../bla.txt")]
    public void ParseTest1(string input)
    {
        VcfRow? row = VcfRow.Parse("RELATED:" + input, new VcfDeserializationInfo());
        Assert.IsNotNull(row);

        RelationProperty prop = RelationProperty.Parse(row!, VCdVersion.V4_0);
        Assert.IsInstanceOfType(prop, typeof(RelationTextProperty));
    }
}
