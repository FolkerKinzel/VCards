
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class DateTimeOffsetExtensionTests
{
    [TestMethod()]
    public void HasYearTest1()
    {
        Assert.IsTrue(new DateAndOrTimeConverter().TryParse("--0304T170000".AsSpan(), out DateAndOrTime? dto));
        Assert.IsTrue(dto.DateTimeOffset.HasValue);
        Assert.IsFalse(dto.DateTimeOffset.Value.HasYear());
        Assert.IsTrue(dto.DateTimeOffset.Value.HasDate());
    }

    [TestMethod()]
    public void HasYearTest2()
    {
        Assert.IsTrue(new DateAndOrTimeConverter().TryParse("16850304T170000".AsSpan(), out DateAndOrTime? dto));
        Assert.IsTrue(dto.DateTimeOffset.HasValue);
        Assert.IsTrue(dto.DateTimeOffset.Value.HasYear());
        Assert.IsTrue(dto.DateTimeOffset.Value.HasDate());
    }

    [TestMethod()]
    public void HasYearTest3()
    {
        Assert.IsTrue(new TimeConverter().TryParse("T170000+0200".AsSpan(), out DateAndOrTime? dto));
        Assert.IsTrue(dto.DateTimeOffset.HasValue);
        Assert.IsFalse(dto.DateTimeOffset.Value.HasYear());
        Assert.IsFalse(dto.DateTimeOffset.Value.HasDate());
    }

    [TestMethod()]
    public void HasYearTest4()
    {
        Assert.IsTrue(new TimeConverter().TryParse("T--17+0200".AsSpan(), out DateAndOrTime? dto));
        Assert.IsTrue(dto.DateTimeOffset.HasValue);
        Assert.IsFalse(dto.DateTimeOffset.Value.HasYear());
        Assert.IsFalse(dto.DateTimeOffset.Value.HasDate());
    }
}