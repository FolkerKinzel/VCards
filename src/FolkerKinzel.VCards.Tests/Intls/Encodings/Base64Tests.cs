using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Intls.Encodings.Tests;

[TestClass]
public class Base64Tests
{
    [TestMethod]
    public void DecodeTest1() => Assert.IsNull(Base64.Decode(null));
}
