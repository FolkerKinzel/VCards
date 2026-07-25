using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AdrConverterTests
{
    [TestMethod]
    public void ToVcfStringTest()
        => _ = Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => AdrConverter.ToVcfString((Adr)4711));

    [TestMethod]
    public void ParseTest() => Assert.IsNull(AdrConverter.Parse(null));
}

