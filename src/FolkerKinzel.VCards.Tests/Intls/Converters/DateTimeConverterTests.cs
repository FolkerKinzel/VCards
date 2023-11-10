using System.Text;
using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class DateTimeConverterTests
{
    private readonly DateTimeConverter _conv = new();

    [TestMethod]
    public void DateTest()
    {
        string s = "1963-08-17";

        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        var reference = new DateOnly(1963, 8, 17);
        Assert.IsTrue(dt.IsT0);
        Assert.AreEqual(reference, dt.AsT0);
    }


    [TestMethod]
    public void DateTest2()
    {
        string s = "--08";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        Assert.IsTrue(dt.IsT0);
        Assert.AreEqual(dt.AsT0.Year, 4);
        Assert.AreEqual(dt.AsT0.Month, 8);
    }


    [TestMethod]
    public void DateTest3()
    {
        string s = "--0803";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        Assert.IsTrue(dt.IsT0);
        Assert.AreEqual(dt.AsT0.Year, 4);
        Assert.AreEqual(dt.AsT0.Month, 8);
        Assert.AreEqual(dt.AsT0.Day, 3);
    }


    [TestMethod]
    public void DateTest4()
    {
        string s = "---03";
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        Assert.IsTrue(dt.IsT0);
        Assert.AreEqual(dt.AsT0.Year, 4);
        Assert.AreEqual(dt.AsT0.Month, 1);
        Assert.AreEqual(dt.AsT0.Day, 3);
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
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        string s2 = ToDateTimeString(in dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt2));

        Assert.AreEqual(dt, dt2);

        static string ToDateTimeString(in OneOf.OneOf<DateOnly, DateTimeOffset> dt, VCdVersion version)
        { 
            var builder = new StringBuilder();
            dt.Switch(
                dateOnly => DateTimeConverter.AppendDateTo(builder, dateOnly, version),
                dto => DateTimeConverter.AppendDateAndOrTimeTo(builder, dto, version)
            );
            return builder.ToString();
        }
    }

    private void RoundtripTimestamp(
        string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
    {
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt));

        string s2 = ToTimeStamp(in dt, version);

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2.AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> dt2));

        Assert.AreEqual(dt, dt2);

        static string ToTimeStamp(in OneOf.OneOf<DateOnly, DateTimeOffset> dt, VCdVersion version)
        {
            var builder = new StringBuilder();
            dt.Switch(
                dateOnly => DateTimeConverter.AppendDateTo(builder, dateOnly, version),
                dto => DateTimeConverter.AppendTimeStampTo(builder, dto, version)
            );
            return builder.ToString();
        }
    }


    //[TestMethod]
    //public void AppendTimeStampToTest1()
    //{
    //    var builder = new StringBuilder();
    //    DateAndOrTimeConverter.AppendTimeStampTo(builder, null, VCdVersion.V3_0);
    //    Assert.AreEqual(0, builder.Length);
    //}


    [TestMethod]
    public void AppendDateTimeStringToTest1()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateAndOrTimeTo(builder, default, VCdVersion.V3_0);
        Assert.AreEqual(0, builder.Length);
    }


    [TestMethod]
    public void AppendDateTimeStringToTest2a()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateAndOrTimeTo(builder, new DateTime(2, 1, 1, 0, 0, 0, DateTimeKind.Utc), VCdVersion.V4_0);
        Assert.AreEqual(0, builder.Length);
    }

    [TestMethod]
    public void AppendDateTimeStringToTest2b()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateAndOrTimeTo(builder, new DateTime(2, 1, 2, 0,0,0, DateTimeKind.Utc), VCdVersion.V4_0);
        Assert.AreEqual(0, builder.Length);
    }


    [TestMethod]
    public void AppendDateTimeStringToTest2c()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateAndOrTimeTo(builder, new DateTime(2, 1, 1, 0, 0, 0, DateTimeKind.Utc), VCdVersion.V3_0);
        Assert.AreEqual(0, builder.Length);
    }

    //[TestMethod]
    //public void AppendDateTimeStringToTest2d()
    //{
    //    var builder = new StringBuilder();
    //    DateAndOrTimeConverter.AppendDateAndOrTimeTo(builder, new DateTime(2, 1, 2), VCdVersion.V3_0);
    //    Assert.IsTrue(builder[0] == '-' && builder[1] == '-');
    //}


    [TestMethod]
    public void AppendDateTimeStringToTest3()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateAndOrTimeTo(builder, new DateTime(4, 1, 1), VCdVersion.V4_0);
        string s = builder.ToString();
        Assert.IsTrue(s.StartsWith("--"));
    }

    [TestMethod]
    public void AppendDateToTest1()
    {
        var builder = new StringBuilder();
        DateTimeConverter.AppendDateTo(builder, new DateOnly(4, 5, 1), VCdVersion.V3_0);
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

