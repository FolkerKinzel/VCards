using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class PhoneTypesConverterTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = PhoneTypesConverter.ToVcfString((PhoneTypes)4711);


    [TestMethod]
    public void ParseTest() => Assert.IsNull(PhoneTypesConverter.Parse(null));
}

