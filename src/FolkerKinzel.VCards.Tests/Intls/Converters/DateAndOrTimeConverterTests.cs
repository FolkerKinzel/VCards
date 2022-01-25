using System.Text;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class DateAndOrTimeConverterTests
{
    private readonly DateAndOrTimeConverter _conv = new();

    [TestMethod]
    public void DateTest()
    {
        string s = "1963-08-17";

        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        var reference = new DateTime(1963, 8, 17);
        Assert.AreEqual(reference, dt.DateTime);
    }


    [TestMethod]
    public void DateTest2()
    {
        string s = "--08";
        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        Assert.AreEqual(dt.Year, 4);
        Assert.AreEqual(dt.Month, 8);
    }


    [TestMethod]
    public void DateTest3()
    {
        string s = "--0803";
        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        Assert.AreEqual(dt.Year, 4);
        Assert.AreEqual(dt.Month, 8);
        Assert.AreEqual(dt.Day, 3);
    }


    [TestMethod]
    public void DateTest4()
    {
        string s = "---03";
        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        Assert.AreEqual(dt.Year, 4);
        Assert.AreEqual(dt.Month, 1);
        Assert.AreEqual(dt.Day, 3);
    }


    [TestMethod]
    public void RoundtripsTest()
    {
        Roundtrip("1972-01-31", true, VCdVersion.V3_0);
        Roundtrip("19720131T15-07", false, VCdVersion.V3_0);
        Roundtrip("19720131T15+04", false, VCdVersion.V3_0);

        Roundtrip("19720131", true, VCdVersion.V4_0);
        Roundtrip("19720131T15-07", false, VCdVersion.V4_0);
        Roundtrip("19720131T15+04", false, VCdVersion.V4_0);

        Roundtrip("1996-10-22T14:00:00Z", true, VCdVersion.V3_0);

        Roundtrip("19961022T140000Z", true, VCdVersion.V4_0);

        RoundtripTimestamp("19961022T140000", false, VCdVersion.V4_0);
        RoundtripTimestamp("19961022T140000Z", true, VCdVersion.V4_0);
        RoundtripTimestamp("19961022T140000-05", false, VCdVersion.V4_0);
        RoundtripTimestamp("19961022T140000-0500", false, VCdVersion.V4_0);
        RoundtripTimestamp("19961022T140000+0500", false, VCdVersion.V4_0);

        RoundtripTimestamp("19961022T140000", false, VCdVersion.V2_1);
        RoundtripTimestamp("19961022T140000Z", false, VCdVersion.V2_1);
        RoundtripTimestamp("19961022T140000-05", false, VCdVersion.V2_1);
        RoundtripTimestamp("19961022T140000-0500", false, VCdVersion.V2_1);
        RoundtripTimestamp("19961022T140000+0532", false, VCdVersion.V2_1);
    }


    private void Roundtrip(
        string s, bool stringRoundTrip, VCdVersion version)
    {
        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        string s2 = ToDateTimeString(dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2, out DateTimeOffset dt2));

        Assert.AreEqual(dt, dt2);

        static string ToDateTimeString(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();
            DateAndOrTimeConverter.AppendDateTimeStringTo(builder, dt, version);
            return builder.ToString();
        }
    }

    private void RoundtripTimestamp(
        string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
    {
        Assert.IsTrue(_conv.TryParse(s, out DateTimeOffset dt));

        string s2 = ToTimeStamp(dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2, out DateTimeOffset dt2));

        Assert.AreEqual(dt, dt2);

        static string ToTimeStamp(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();
            DateAndOrTimeConverter.AppendTimeStampTo(builder, dt, version);
            return builder.ToString();
        }
    }


    [TestMethod]
    public void AppendTimeStampToTest1()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendTimeStampTo(builder, null, VCdVersion.V3_0);
        Assert.AreEqual(0, builder.Length);
    }


    [TestMethod]
    public void AppendDateTimeStringToTest1()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeStringTo(builder, null, VCdVersion.V3_0);
        Assert.AreEqual(0, builder.Length);
    }


    [TestMethod]
    public void AppendDateTimeStringToTest2()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeStringTo(builder, new DateTime(2, 1, 1), VCdVersion.V4_0);
        Assert.IsTrue(builder[0] == '-' && builder[1] == '-');
    }


    [TestMethod]
    public void AppendDateTimeStringToTest2b()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeStringTo(builder, new DateTime(2, 1, 1), VCdVersion.V3_0);
        Assert.IsTrue(builder[0] == '-' && builder[1] == '-');
    }


    [TestMethod]
    public void AppendDateTimeStringToTest3()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeStringTo(builder, new DateTime(4, 1, 1), VCdVersion.V4_0);
        Assert.IsTrue(builder.ToString().StartsWith("0004"));
    }


    [TestMethod]
    [DataRow(null)]
    [DataRow("This is a very very long string that is longer than 64 characters.")]
    public void TryParseTest1(string? input) => Assert.IsFalse(new DateAndOrTimeConverter().TryParse(input, out _));
}

