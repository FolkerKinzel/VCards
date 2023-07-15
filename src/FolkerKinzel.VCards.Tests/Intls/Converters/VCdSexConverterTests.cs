using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class VCdSexConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (Gender sex in (Gender[])Enum.GetValues(typeof(Gender)))
        {
            Gender? sex2 = VCdSexConverter.Parse(sex.ToString().Substring(0, 1));
            Assert.AreEqual(sex, sex2);
            Gender? sex3 = VCdSexConverter.Parse(VCdSexConverter.ToVcfString(sex));
            Assert.AreEqual(sex, sex3);
        }

        // Test auf null
        Assert.AreEqual(null, VCdSexConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Gender?)4711).ToVcfString());
    }


}
