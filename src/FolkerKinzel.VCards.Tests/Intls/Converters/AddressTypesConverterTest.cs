using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AddressTypesConverterTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = AddressTypesConverter.ToVcfString((AddressTypes)4711);

    [TestMethod]
    public void ParseTest() => Assert.IsNull(AddressTypesConverter.Parse(null));
}

