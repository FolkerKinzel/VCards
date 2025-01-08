using System.Globalization;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class DateAndOrTimeTests
{
    [TestMethod]
    public void ValueTest1()
    {
        DateAndOrTime rel = new DateOnly(2023, 10, 14);
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
        DateAndOrTime rel = DateTimeOffset.Now;
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
        DateAndOrTime rel = new TimeOnly(23, 10);
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
        DateAndOrTime rel = "Midnight";
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
        DateAndOrTime rel = DateTimeOffset.Now.ToString(CultureInfo.CurrentCulture);
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
        DateAndOrTime rel = new DateOnly(2023, 10, 14);
        rel.Switch(s => rel = null!, null, null, null);
        Assert.IsNull(rel);
    }

    [TestMethod]
    public void MatchTest1()
    {
        const int expected = 42;
        DateAndOrTime rel = new DateOnly(2023, 10, 14);

        int result = rel.Convert(s => expected, null!, null!, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TryAsDateTest1()
        => Assert.IsFalse(DateAndOrTime.Create(new DateTimeOffset(2, 1, 1, 17, 24, 32, TimeSpan.FromHours(1))).TryAsDateOnly(out _));

    [TestMethod]
    public void TryAsDateTest2()
        => Assert.IsTrue(DateAndOrTime.Create(new DateOnly(2023, 11, 11).ToString(CultureInfo.CurrentCulture)).TryAsDateOnly(out _));


}
