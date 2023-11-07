using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class ParameterSerializerTests
{
    [TestMethod]
    public void NonStandardParameterTest1()
    {
        var prop = new NonStandardProperty("X-PROP", "01234");
        var dic = new Dictionary<string, string>();
        dic["X-TEST"] = "test";
        dic["X-NOTHING"] = "   ";
        dic[""] = "not there";
        dic["TYPE"] = "NON-STANDARD";
        prop.Parameters.NonStandard = dic;
        var vc = new VCard { NonStandard = prop };

        string s = vc.ToVcfString(options: VcfOptions.All);

        vc = VCard.ParseVcf(s)[0];
        prop = vc.NonStandard!.First();
        Assert.AreEqual(2, prop!.Parameters.NonStandard!.Count());
    }
}
