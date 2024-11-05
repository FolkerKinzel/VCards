using System.Globalization;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class DateAndOrTimeTests
{
    [TestMethod]
    public void ValueTest1()
    {
        var rel = new DateAndOrTime(new DateOnly(2023, 10, 14));
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.DateOnly);
        Assert.IsNull(rel.DateTimeOffset);
        Assert.IsNull(rel.TimeOnly);
        Assert.IsNull(rel.String);

        Assert.IsTrue(rel.TryAsDateOnly(out _));
        Assert.IsTrue(rel.TryAsDateTimeOffset(out _));
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = new DateAndOrTime(DateTimeOffset.Now);
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.DateOnly);
        Assert.IsNotNull(rel.DateTimeOffset);
        Assert.IsNull(rel.TimeOnly);
        Assert.IsNull(rel.String);

        Assert.IsTrue(rel.TryAsDateOnly(out _));
        Assert.IsTrue(rel.TryAsDateTimeOffset(out _));
    }


    [TestMethod]
    public void ValueTest3()
    {
        var rel = new DateAndOrTime(new TimeOnly(23, 10));
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.DateOnly);
        Assert.IsNull(rel.DateTimeOffset);
        Assert.IsNotNull(rel.TimeOnly);
        Assert.IsNull(rel.String);

        Assert.IsFalse(rel.TryAsDateOnly(out _));
        Assert.IsTrue(rel.TryAsDateTimeOffset(out DateTimeOffset dto));
        Assert.IsFalse(dto.HasDate());
    }

    [TestMethod]
    public void ValueTest4()
    {
        var rel = new DateAndOrTime("Midnight");
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.DateOnly);
        Assert.IsNull(rel.DateTimeOffset);
        Assert.IsNull(rel.TimeOnly);
        Assert.IsNotNull(rel.String);

        Assert.IsFalse(rel.TryAsDateOnly(out _));
        Assert.IsFalse(rel.TryAsDateTimeOffset(out _));
    }

    [TestMethod]
    public void ValueTest5()
    {
        var rel = new DateAndOrTime(DateTimeOffset.Now.ToString(CultureInfo.CurrentCulture));
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.DateOnly);
        Assert.IsNull(rel.DateTimeOffset);
        Assert.IsNull(rel.TimeOnly);
        Assert.IsNotNull(rel.String);

        Assert.IsFalse(rel.TryAsDateOnly(out _));
        Assert.IsTrue(rel.TryAsDateTimeOffset(out _));
    }

    [TestMethod]
    public void SwitchTest1()
    {
        var rel = new DateAndOrTime(new DateOnly(2023, 10, 14));
        rel.Switch(s => rel = null, null!, null!, null!);
        Assert.IsNull(rel);
    }

    [TestMethod]
    public void MatchTest1()
    {
        const int expected = 42;
        var rel = new DateAndOrTime(new DateOnly(2023, 10, 14));

        int result = rel.Convert(s => expected, null!, null!, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TryAsDateTest1()
        => Assert.IsFalse(new DateAndOrTime(new DateTimeOffset(2, 1, 1, 17, 24, 32, TimeSpan.FromHours(1))).TryAsDateOnly(out _));

    [TestMethod]
    public void TryAsDateTest2()
        => Assert.IsTrue(new DateAndOrTime(new DateOnly(2023, 11, 11).ToString(CultureInfo.CurrentCulture)).TryAsDateOnly(out _));


}
