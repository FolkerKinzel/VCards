using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class ImppTypesConverterTest
{
    [TestMethod()]
    public void ParseTest()
    {
        foreach (ImppTypes kind in (ImppTypes[])Enum.GetValues(typeof(ImppTypes)))
        {
            ImppTypes? kind2 = ImppTypesConverter.Parse(kind.ToString().ToUpperInvariant());

            Assert.AreEqual(kind, kind2);
        }

        Assert.IsNull(ImppTypesConverter.Parse("NICHT_VORHANDEN"));
    }


}
