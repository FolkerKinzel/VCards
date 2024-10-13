using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class ImppConverterTest
{
    [TestMethod]
    public void ParseTest()
    {
        foreach (Impp kind in (Impp[])Enum.GetValues(typeof(Impp)))
        {
            Impp? kind2 = ImppConverter.Parse(kind.ToString().AsSpan());
            Assert.AreEqual(kind, kind2);
        }

        Assert.IsNull(ImppConverter.Parse("NICHT_VORHANDEN".AsSpan()));
        Assert.IsNull(ImppConverter.Parse(null));
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = ImppConverter.ToVcfString((Impp)4711);
}
