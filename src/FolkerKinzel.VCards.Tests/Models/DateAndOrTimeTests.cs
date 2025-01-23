using System.Globalization;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        DateOnly? result = null;
        DateAndOrTime? rel = new DateOnly(2023, 10, 14);
        rel.Switch(dateOnly => result = dateOnly, null, null, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest2()
    {
        DateOnly? result = null;
        DateAndOrTime? rel = new DateOnly(2023, 10, 14);
        rel.Switch("", (dateOnly, str) => result = dateOnly, null, null, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest3()
    {
        DateTimeOffset? result = null;
        DateAndOrTime? rel = DateTimeOffset.Now;
        rel.Switch(null, dto => result = dto, null, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest4()
    {
        DateTimeOffset? result = null;
        DateAndOrTime? rel = DateTimeOffset.Now;
        rel.Switch("", null, (dto, str) => result = dto, null, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest5()
    {
        TimeOnly? result = null;
        DateAndOrTime? rel = TimeOnly.FromTimeSpan(TimeSpan.FromHours(12));
        rel.Switch(null, null,  timeOnly => result = timeOnly, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest6()
    {
        TimeOnly? result = null;
        DateAndOrTime? rel = TimeOnly.FromTimeSpan(TimeSpan.FromHours(12));
        rel.Switch("", null, null, (timeOnly, str) => result = timeOnly, null);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest7()
    {
        string? result = null;
        DateAndOrTime? rel = "heute";
        rel.Switch(null, null, null, str => result = str);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest8()
    {
        string? result = null;
        DateAndOrTime? rel = "heute";
        rel.Switch(true, null, null, null, (str, b) => result = str);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ConvertTest1()
    {
        const int expected = 42;
        DateAndOrTime rel = new DateOnly(2023, 10, 14);

        int result = rel.Convert(s => expected, null!, null!, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ConvertTest2()
    {
        const int expected = 42;
        DateAndOrTime rel = DateTimeOffset.Now;

        int result = rel.Convert(null!, s => expected, null!, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ConvertTest3()
    {
        const int expected = 42;
        DateAndOrTime rel = TimeOnly.FromDateTime(DateTime.Now);

        int result = rel.Convert(null!, null!, s => expected, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ConvertTest4()
    {
        const int expected = 42;
        DateAndOrTime rel = "Midnight";

        int result = rel.Convert(null!, null!, null!, s => expected);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ConvertTest5()
    {
        const string test = "test";
        string? result = null;

        result = DateAndOrTime.Create(DateOnly.FromDateTime(DateTime.Now)).Convert(test, (guid, str) => str, null!, null!, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest6()
    {
        const string test = "test";
        string? result = null;

        result = DateAndOrTime.Create(DateTimeOffset.Now).Convert(test, null!, (guid, str) => str, null!, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest7()
    {
        const string test = "test";
        string? result = null;

        result = DateAndOrTime.Create(TimeOnly.FromDateTime(DateTime.Now)).Convert(test, null!, null!, (guid, str) => str, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest8()
    {
        const string test = "test";
        string? result = null;

        result = DateAndOrTime.Create("Midnight").Convert(test, null!, null!, null!, (guid, str) => str);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest9() 
        => _ = DateAndOrTime.Create(DateOnly.FromDateTime(DateTime.Now)).Convert<string, string>("", null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest10()
        => _ = DateAndOrTime.Create(DateTimeOffset.Now).Convert<string, string>("", null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest11()
        => _ = DateAndOrTime.Create(TimeOnly.FromDateTime(DateTime.Now)).Convert<string, string>("", null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest12() 
        => _ = DateAndOrTime.Create("Midnight").Convert<string, string>("", null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest13()
       => _ = DateAndOrTime.Create(DateOnly.FromDateTime(DateTime.Now)).Convert<string>(null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest14()
        => _ = DateAndOrTime.Create(DateTimeOffset.Now).Convert<string>(null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest15()
        => _ = DateAndOrTime.Create(TimeOnly.FromDateTime(DateTime.Now)).Convert<string>(null!, null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest16()
        => _ = DateAndOrTime.Create("Midnight").Convert<string>(null!, null!, null!, null!);

    [TestMethod]
    public void TryAsDateTest1()
        => Assert.IsFalse(DateAndOrTime.Create(new DateTimeOffset(2, 1, 1, 17, 24, 32, TimeSpan.FromHours(1))).TryAsDateOnly(out _));

    [TestMethod]
    public void TryAsDateTest2()
        => Assert.IsTrue(DateAndOrTime.Create(new DateOnly(2023, 11, 11).ToString(CultureInfo.CurrentCulture)).TryAsDateOnly(out _));

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(DateAndOrTime.Empty.ToString());

    [TestMethod]
    public void TryAsTimeOnlyTest1() => Assert.IsTrue(DateAndOrTime.Create("17:30").TryAsTimeOnly(out _));

    [TestMethod]
    public void TryAsTimeOnlyTest2() 
        => Assert.IsTrue(DateAndOrTime.Create(TimeOnly.FromDateTime(DateTime.Now)).TryAsTimeOnly(out _));

    [TestMethod]
    public void EqualsTest1()
    {
        var daot = DateAndOrTime.Create(DateTimeOffset.Now);

        Assert.IsTrue(daot.Equals(daot));
        Assert.IsTrue(daot.Equals((object)daot));
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.IsTrue(daot == daot);
        Assert.IsFalse(daot != daot);
#pragma warning restore CS1718 // Comparison made to same variable
        Assert.AreEqual(daot.GetHashCode(), daot.GetHashCode());    
        Assert.IsFalse(daot.Equals(null));
        Assert.IsFalse(daot == null);
        Assert.IsFalse((DateAndOrTime?)null == daot);
        Assert.IsTrue(daot != null);
        Assert.IsFalse(daot.Equals(42));
    }

    [TestMethod]
    public void AsStringTest1()
    {
        var daot = DateAndOrTime.Create(2, 4);
        string str = daot.AsString(CultureInfo.InvariantCulture);
        Assert.AreEqual("02/04", str);
    }
}
