using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class VCdSexConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (Gender sex in (Gender[])Enum.GetValues(typeof(Gender)))
        {
            Gender? sex2 = GenderConverter.Parse(sex.ToString().Substring(0, 1));
            Assert.AreEqual(sex, sex2);
            Gender? sex3 = GenderConverter.Parse(GenderConverter.ToVcfString(sex));
            Assert.AreEqual(sex, sex3);
        }

        // Test auf null
        Assert.AreEqual(null, GenderConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Gender?)4711).ToVcfString());
    }


}
