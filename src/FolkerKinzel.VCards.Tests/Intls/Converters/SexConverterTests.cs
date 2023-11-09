using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class SexConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (Sex sex in (Sex[])Enum.GetValues(typeof(Sex)))
        {
            Sex? sex2 = SexConverter.Parse(sex.ToString().Substring(0, 1));
            Assert.AreEqual(sex, sex2);
            Sex? sex3 = SexConverter.Parse(SexConverter.ToVcfString(sex));
            Assert.AreEqual(sex, sex3);
        }

        // Test auf null
        Assert.AreEqual(null, SexConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Sex?)4711).ToVcfString());
    }


}
