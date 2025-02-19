﻿using System.Text;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

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
        Assert.IsTrue(_conv.TryParse(s.AsSpan(), out DateAndOrTime? dt));


        var builder = new StringBuilder();

        dt.Switch(
            dtoAction: dto => TimeConverter.AppendTimeTo(builder, dto, version),
            timeAction: to => TimeConverter.AppendTimeTo(builder, to, version)
            );
        string s2 = builder.ToString();

        if (stringRoundTrip)
        {
            Assert.AreEqual(s, s2);
        }

        Assert.IsTrue(_conv.TryParse(s2.AsSpan(), out DateAndOrTime? dt2));
        Assert.AreEqual(dt, dt2);
    }

    [TestMethod]
    public void TryParseTest1() => Assert.IsFalse(_conv.TryParse(null, out _));

    [DataTestMethod]
    [DataRow("T143522+02")]
    [DataRow("T143522+0200")]
    [DataRow("T14+0200")]
    [DataRow("T14+02")]
    [DataRow("T1435+02")]
    [DataRow("T1435+0200")]
    [DataRow("T-3522+02")]
    [DataRow("T-3522+0200")]
    [DataRow("T--22+02")]
    [DataRow("T--22+0200")]
    public void TryParseTest2(string? input)
    {
        Assert.IsTrue(_conv.TryParse(input.AsSpan(), out DateAndOrTime? oneOf));
        Assert.IsTrue(oneOf.DateTimeOffset.HasValue);
    }


    [DataTestMethod]
    [DataRow("T14")]
    [DataRow("T1435")]
    [DataRow("T143522")]
    [DataRow("T-3522")]
    [DataRow("T--22")]
    public void TryParseTest3(string? input)
    {
        Assert.IsTrue(_conv.TryParse(input.AsSpan(), out DateAndOrTime? oneOf));
        Assert.IsTrue(oneOf.TimeOnly.HasValue);
    }

    [TestMethod]
    public void TryParseTest4() => Assert.IsFalse(_conv.TryParse("TblablaZ".AsSpan(), out _));


    [DataTestMethod]
    //[DataRow("T14")]
    //[DataRow("T1435")]
    //[DataRow("T143522")]
    [DataRow("T143522+02")]
    [DataRow("T143522+0200")]
    [DataRow("T14+0200")]
    [DataRow("T14+02")]
    [DataRow("T1435+02")]
    [DataRow("T1435+0200")]
    //[DataRow("T-3522")]
    [DataRow("T-3522+02")]
    [DataRow("T-3522+0200")]
    //[DataRow("T--22")]
    [DataRow("T--22+02")]
    [DataRow("T--22+0200")]
    public void TryParseTest5(string? input)
    {
        Assert.IsTrue(_conv.TryParse(input.AsSpan(), out DateAndOrTime? oneOf));
        Assert.IsTrue(oneOf.DateTimeOffset.HasValue);
    }

    [DataTestMethod]
    [DataRow("T14")]
    [DataRow("T1435")]
    [DataRow("T143522")]
    [DataRow("T-3522")]
    [DataRow("T--22")]
    public void TryParseTest6(string? input)
    {
        Assert.IsTrue(_conv.TryParse(input.AsSpan(), out DateAndOrTime? oneOf));
        Assert.IsTrue(oneOf.TimeOnly.HasValue);
    }

}
