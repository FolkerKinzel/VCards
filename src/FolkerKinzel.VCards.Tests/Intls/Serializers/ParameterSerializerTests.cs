using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class ParameterSerializerTests
{
    [TestMethod]
    public void NonStandardParameterTest1()
    {
        var prop = new NonStandardProperty("X-PROP", "01234");
        var dic = new Dictionary<string, string>
        {
            ["X-TEST"] = "test",
            ["X-NOTHING"] = "   ",
            [""] = "not there",
            ["TYPE"] = "NON-STANDARD"
        };
        prop.Parameters.NonStandard = dic;
        var vc = new VCard { NonStandards = prop };

        string s = vc.ToVcfString(options: VcfOpts.All);

        vc = Vcf.Parse(s)[0];
        prop = vc.NonStandards!.First();
        Assert.AreEqual(2, prop!.Parameters.NonStandard!.Count());
    }
}
