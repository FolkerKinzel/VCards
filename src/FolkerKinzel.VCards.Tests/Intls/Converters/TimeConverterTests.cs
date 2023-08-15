using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class TimeConverterTests
{
    private readonly TimeConverter _conv = new();

    [TestMethod]
    public void TimeConverterTest()
    {
        Roundtrip("16:58:00", false, VCdVersion.V4_0);
        Roundtrip("165800Z", true, VCdVersion.V4_0);
        Roundtrip("16:58:00Z", true, VCdVersion.V2_1);
        Roundtrip("16:58:00-04:00", false, VCdVersion.V4_0);
        Roundtrip("16:58:00+04", false, VCdVersion.V4_0);
        Roundtrip("16:58:00-04:00", true, VCdVersion.V3_0);
        Roundtrip("16:58:00+04:00", true, VCdVersion.V3_0);
        Roundtrip("16:58:00+04", false, VCdVersion.V3_0);
    }

    private void Roundtrip(
        string s, bool stringRoundTrip, VCdVersion version)
    {
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<TimeOnly, DateTimeOffset> dt));

        
        var builder = new StringBuilder();

        dt.Switch(
            to => TimeConverter.AppendTimeTo(builder, to, version),
            dto => TimeConverter.AppendTimeTo(builder, dto, version)
            );
        string s2 = builder.ToString();

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2.AsSpan(), out OneOf.OneOf<TimeOnly, DateTimeOffset> dt2));
        Assert.AreEqual(dt, dt2);
    }

    [TestMethod]
    public void TryParseTest() => Assert.IsFalse(_conv.TryParse(null, out _));

}
