using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class TypeConverterPolyfillTests
{
    [TestMethod]
    public void IntTryParseTest1()
    {
        for (int i = 0; i < 10; i++)
        {
            Assert.IsTrue(_Int.TryParse(i.ToString().AsSpan(), out int result));
            Assert.AreEqual(i, result);
        }
    }

    [TestMethod]
    public void IntTryParseTest2()
    {
        const int val = 42;

        Assert.IsTrue(_Int.TryParse(val.ToString().AsSpan(), out int result));
        Assert.AreEqual(val, result);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("/")]
    [DataRow(":")]
    [DataRow("\0")]
    [DataRow(" ")]
    [DataRow("ä")]
    public void IntTryParseTest3(string input)
    {
        Assert.IsFalse(_Int.TryParse(input.AsSpan(), out int result));
        Assert.AreEqual(default, result);
    }
}

[TestClass]
public class TelConverterTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = TelConverter.ToVcfString((Tel)4711);


    [TestMethod]
    public void ParseTest() => Assert.IsNull(TelConverter.Parse(null));
}

