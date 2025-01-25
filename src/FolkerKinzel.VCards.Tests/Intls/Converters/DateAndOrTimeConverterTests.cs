using System.Text;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class DateAndOrTimeConverterTests
{
    private readonly DateAndOrTimeConverter _conv = new();

    [TestMethod]
    public void DateTest()
    {
        string s = "1963-08-17";

        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));

        var reference = new DateOnly(1963, 8, 17);
        Assert.IsTrue(dt.DateOnly.HasValue);
        Assert.AreEqual(reference, dt.DateOnly.Value);
    }

    [TestMethod]
    public void DateTest2()
    {
        string s = "--08";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));

        Assert.IsTrue(dt.DateOnly.HasValue);
        Assert.AreEqual(dt.DateOnly.Value.Year, 4);
        Assert.AreEqual(dt.DateOnly.Value.Month, 8);
    }

    [TestMethod]
    public void DateTest3()
    {
        string s = "--0803";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));

        Assert.IsTrue(dt.DateOnly.HasValue);
        Assert.AreEqual(dt.DateOnly.Value.Year, 4);
        Assert.AreEqual(dt.DateOnly.Value.Month, 8);
        Assert.AreEqual(dt.DateOnly.Value.Day, 3);
    }

    [TestMethod]
    public void DateTest4()
    {
        string s = "---03";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));

        Assert.IsTrue(dt.DateOnly.HasValue);
        Assert.AreEqual(dt.DateOnly.Value.Year, 4);
        Assert.AreEqual(dt.DateOnly.Value.Month, 1);
        Assert.AreEqual(dt.DateOnly.Value.Day, 3);
    }

    [TestMethod]
    public void RoundtripsTest()
    {
        Roundtrip("1984", true, VCdVersion.V4_0);

        Roundtrip("1984-02", true, VCdVersion.V4_0);

        Roundtrip("---17", true, VCdVersion.V4_0);

        Roundtrip("--12", true, VCdVersion.V4_0);

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
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));

        string s2 = ToDateTimeString(dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2.AsSpan(), out DateAndOrTime? dt2));

        Assert.AreEqual(dt, dt2);

        static string ToDateTimeString(DateAndOrTime dt, VCdVersion version)
        {
            var builder = new StringBuilder();
            dt.Switch(
                dateOnly => DateAndOrTimeConverter.AppendDateTo(builder, dateOnly, version, dt.HasYear, dt.HasMonth, dt.HasDay),
                dto => DateAndOrTimeConverter.AppendDateTimeOffsetTo(builder, dto, version, dt.HasYear, dt.HasMonth, dt.HasDay)
            );
            return builder.ToString();
        }
    }

    private static void RoundtripTimestamp(
        string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
    {
        var conv = new TimeStampConverter();
        Assert.IsTrue(conv.TryParse(s.AsSpan(), out DateTimeOffset dt));

        string s2 = ToTimeStamp(dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(conv.TryParse(s2.AsSpan(), out DateTimeOffset dt2));

        Assert.AreEqual(dt, dt2);

        static string ToTimeStamp(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();

            TimeStampConverter.AppendTo(builder, dt, version);
            return builder.ToString();
        }
    }

    [TestMethod]
    public void AppendDateTimeStringToTest1()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeOffsetTo(builder, default, VCdVersion.V3_0, false, false, false);
        Assert.AreEqual(0, builder.Length);
    }

    [TestMethod]
    public void AppendDateTimeStringToTest1a()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeOffsetTo(builder, default, VCdVersion.V3_0, true, true, true);
        Assert.AreNotEqual(0, builder.Length);
    }

    [TestMethod]
    public void AppendDateTimeStringToTest3()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeOffsetTo(builder, new DateTime(4, 1, 1), VCdVersion.V4_0, false, true, true);
        string s = builder.ToString();
        Assert.IsTrue(s.StartsWith("--"));
    }

    [TestMethod]
    public void AppendDateToTest1()
    {
        var builder = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTo(builder, new DateOnly(4, 5, 1), VCdVersion.V4_0, false, true, true);
        string s = builder.ToString();
        Assert.IsTrue(s.StartsWith("--"));
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("A very very loooooooooooooooooooooooooong string that is longer than 64 characters.")]
    public void TryParseTest1(string? input) => Assert.IsFalse(_conv.TryParse(input.AsSpan(), out _));

    [DataTestMethod]
    [DataRow("---bl")]
    [DataRow("---bTlZ")]
    [DataRow("--bb")]
    [DataRow("--bbTau")]
    public void TryParseTest2(string input) => Assert.IsFalse(_conv.TryParse(input.AsSpan(), out _));
}

