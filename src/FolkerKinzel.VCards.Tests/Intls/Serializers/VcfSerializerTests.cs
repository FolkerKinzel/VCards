using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class VcfSerializerTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetSerializerTest1()
    {
        using var serializer = VcfSerializer.GetSerializer(new MemoryStream(), false, (VCdVersion)(-10000), VcfOptions.Default, null);
    }

    [TestMethod]
    public void ResetBuildersTest1()
    {
        using var stream = new MemoryStream();
        using var serializer = VcfSerializer.GetSerializer(stream, false, VCdVersion.V3_0, VcfOptions.Default, null);

        serializer.Builder.Capacity = 20000;
        serializer.Worker.Capacity = 20000;

        var vc = new VCard { DisplayNames = new TextProperty("Goofy") };

        serializer.Serialize(vc);

        Assert.IsTrue(serializer.Builder.Capacity < 10000);
        Assert.IsTrue(serializer.Worker.Capacity < 10000);
    }

    [TestMethod]
    public void AppendLineFoldingTest1()
    {
        var vc = new VCard { DisplayNames = new TextProperty("A" + new string('ä', 80)) };

        string s = vc.ToVcfString();

        Assert.IsTrue(s.GetLinesCount() > 5);
    }
}
