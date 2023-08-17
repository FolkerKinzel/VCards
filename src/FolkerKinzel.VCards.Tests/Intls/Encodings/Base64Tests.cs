using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Intls.Encodings.Tests;

[TestClass]
public class Base64Tests
{
    [TestMethod]
    public void DecodeTest1() => Assert.IsNull(Base64.Decode(null));


    [TestMethod]
    public void DecodeTest2()
    {
        byte[]? val = Base64.Decode("ABCDABC");
        Assert.AreEqual(5, val?.Length);
    }
}
