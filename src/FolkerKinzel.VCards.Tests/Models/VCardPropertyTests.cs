using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class VCardPropertyTests
{
    private class ArgumentNullTester(ParameterSection parameters) : VCardProperty(parameters, null)
    {
        public override object Clone() => throw new NotImplementedException();
        protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CtorTest1() => _ = new ArgumentNullTester(null!);
}
