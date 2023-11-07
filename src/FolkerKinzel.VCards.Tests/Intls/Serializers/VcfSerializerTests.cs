using FolkerKinzel.VCards.Intls.Serializers;
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
}
