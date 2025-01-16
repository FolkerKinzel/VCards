using System.Text;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests.Intls.Serializers;

[TestClass]
public class TimeZoneIDSerializerTests
{
    [TestMethod]
    public void AppendToTest1()
    {
        var sb = new StringBuilder();
        TimeZoneIDSerializer.AppendTo(sb, TimeZoneID.Empty, Enums.VCdVersion.V4_0, null, false);
        Assert.AreEqual(0, sb.Length);
    }
}
