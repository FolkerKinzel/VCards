﻿using System.Text;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.Tests;

internal class TimeZoneIDConverterMock(bool success) : ITimeZoneIDConverter
{
    public bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset)
    {
        utcOffset = TimeSpan.Zero;
        return Success;
    }

    internal bool Success { get; } = success;
}

[TestClass]
public class TimeZoneIDTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest1() => _ = TimeZoneID.Parse("  ");

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ParseTest2() => _ = TimeZoneID.Parse(null!);

    [DataTestMethod]
    [DataRow("-0700")]
    [DataRow("-07:00")]
    [DataRow("-07")]
    [DataRow("+0400")]
    [DataRow("+04:00")]
    [DataRow("+04")]
    [DataRow("+09")]
    [DataRow("0400")]
    [DataRow("04:00")]
    [DataRow("04")]
    [DataRow("09")]
    [DataRow("-12")]
    public void TryGetUtcOffsetTest1(string s)
    {
        var tzInfo = TimeZoneID.Parse(s);
        Assert.IsTrue(tzInfo.TryGetUtcOffset(out _));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TryGetUtcOffsetTest3(bool success)
    {
        var tzInfo = TimeZoneID.Parse("TheTimeZone");

        Assert.AreEqual(success,
                        tzInfo.TryGetUtcOffset(out _, new TimeZoneIDConverterMock(success)));
    }

    //[TestMethod]
    //public void ParseTest2()
    //{
    //    TimeZoneInfo tz1 = TimeZoneInfo.Local;

    //    var sb = new StringBuilder();
    //    TimeZoneConverter.AppendTo(sb, tz1, Models.Enums.VCdVersion.V4_0);

    //    string s = sb.ToString();

    //    TimeZoneInfo? tz2 = _timeZoneInfoConverter.Parse(s);

    //    Assert.AreEqual(tz1, tz2);
    //}

    [DataTestMethod]
    [DataRow("-0700", true)]
    [DataRow("-07:00", true)]
    [DataRow("-07", true)]
    [DataRow("+0400", true)]
    [DataRow("+04:00", true)]
    [DataRow("+04", true)]
    [DataRow("+09", true)]
    [DataRow("0400", true)]
    [DataRow("04:00", true)]
    [DataRow("04", true)]
    [DataRow("09", true)]
    [DataRow("-12", true)]
    [DataRow("-22", false)]
    [DataRow("Text-07:00Text", false)]
    public void TryGetUtcOffsetTest2(string input, bool expected)
    {
        var tz = TimeZoneID.Parse(input);
        Assert.AreEqual(expected, tz.TryGetUtcOffset(out _));
    }

    [TestMethod]
    public void TryGetUtcOffsetTest4() => Assert.IsFalse(TimeZoneID.Empty.TryGetUtcOffset(out _));

    [TestMethod]
    public void AppendToTest1()
    {
        const string input = "unknown";
        var id = TimeZoneID.Parse(input);
        var builder = new StringBuilder();

        TimeZoneIDSerializer.AppendTo(builder, id, VCdVersion.V3_0, null, false);

        Assert.AreEqual(input, builder.ToString());
    }

    [DataTestMethod]
    [DataRow("-0100")]
    [DataRow("+0100")]
    public void AppendToTest2(string input)
    {
        var id = TimeZoneID.Parse(input);
        var builder = new StringBuilder();

        TimeZoneIDSerializer.AppendTo(builder, id, VCdVersion.V4_0, null, false);

        Assert.AreEqual(input, builder.ToString());
    }

    [TestMethod]
    public void ToStringTest() => Assert.IsNotNull(TimeZoneID.Parse("Europe/Berlin").ToString());

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    public void TryCreateTest1(string? input) => Assert.IsNull(TimeZoneID.TryCreate(input));

    [TestMethod]
    public void TryCreateTest2() => Assert.IsNotNull(TimeZoneID.TryCreate("Europe/Berlin"));

}
