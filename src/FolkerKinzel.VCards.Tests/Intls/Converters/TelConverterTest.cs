using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class TelConverterTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = TelConverter.ToVcfString((Tel)4711);


    [TestMethod]
    public void ParseTest() => Assert.IsNull(TelConverter.Parse(null));
}

