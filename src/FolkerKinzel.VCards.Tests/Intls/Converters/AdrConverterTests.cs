using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AdrConverterTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = AdrConverter.ToVcfString((Addr)4711);

    [TestMethod]
    public void ParseTest() => Assert.IsNull(AdrConverter.Parse(null));
}

