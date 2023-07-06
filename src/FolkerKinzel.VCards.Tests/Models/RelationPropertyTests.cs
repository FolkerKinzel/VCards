using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;
namespace FolkerKinzel.VCards.Models.Tests;

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
}
