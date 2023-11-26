using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class ParameterSerializerTests
{
    [TestMethod]
    public void NonStandardParameterTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        string s = vc.ToVcfString(options: VcfOptions.All);

        vc = VCard.ParseVcf(s)[0];
        prop = vc.NonStandards!.First();
        Assert.AreEqual(2, prop!.Parameters.NonStandard!.Count());
    }
}
