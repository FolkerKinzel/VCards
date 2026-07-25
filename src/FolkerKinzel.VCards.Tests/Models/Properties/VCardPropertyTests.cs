using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class VCardPropertyTests
{
    private class ArgumentNullTester(ParameterSection parameters) : VCardProperty(parameters, null)
    {
        public override bool IsEmpty => throw new NotImplementedException();

        public override object Clone() => throw new NotImplementedException();

        protected override object GetVCardPropertyValue() => throw new NotImplementedException();

        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    public void CtorTest1()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => new ArgumentNullTester(null!));
}
