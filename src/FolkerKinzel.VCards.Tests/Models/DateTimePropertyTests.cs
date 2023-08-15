using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class DateTimePropertyTests
{
    private class TestIEnumerable : DateAndOrTimeProperty
    {
        public TestIEnumerable() : base(DateAndOrTimeProperty.Create(DateTimeOffset.Now)) { }
        public override object Clone() => throw new NotImplementedException();
        protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    public void IEnumerableTest1() => Assert.AreEqual(1, new TestIEnumerable().AsWeakEnumerable().Count());
}
