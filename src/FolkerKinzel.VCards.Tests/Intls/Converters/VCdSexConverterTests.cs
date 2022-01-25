using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class VCdSexConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (VCdSex sex in (VCdSex[])Enum.GetValues(typeof(VCdSex)))
        {
            VCdSex? sex2 = VCdSexConverter.Parse(sex.ToString().Substring(0, 1));
            Assert.AreEqual(sex, sex2);
            VCdSex? sex3 = VCdSexConverter.Parse(VCdSexConverter.ToVcfString(sex));
            Assert.AreEqual(sex, sex3);
        }

        // Test auf null
        Assert.AreEqual(null, VCdSexConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((VCdSex?)4711).ToVcfString());
    }


}
