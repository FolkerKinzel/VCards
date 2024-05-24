using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class SexConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (Sex sex in (Sex[])Enum.GetValues(typeof(Sex)))
        {
            Sex? sex2 = SexConverter.Parse(sex.ToString()[0]);
            Assert.AreEqual(sex, sex2);
            Sex? sex3 = SexConverter.Parse(SexConverter.ToVcfString(sex)![0]);
            Assert.AreEqual(sex, sex3);
        }

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Sex?)4711).ToVcfString());
    }


}
