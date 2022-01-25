using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class TelTypesConverterTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = TelTypesConverter.ToVcfString((TelTypes)4711);


    [TestMethod]
    public void ParseTest() => Assert.IsNull(TelTypesConverter.Parse(null));
}

