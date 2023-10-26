using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class DateTimeOffsetExtensionTests
{
    [TestMethod()]
    public void HasYearTest1()
    {
        Assert.IsTrue(new DateTimeConverter().TryParse("--0304T170000".AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> oneOf));
        Assert.IsTrue(oneOf.IsT1);
        Assert.IsFalse(oneOf.AsT1.HasYear());
        Assert.IsTrue(oneOf.AsT1.HasDate());
    }

    [TestMethod()]
    public void HasYearTest2()
    {
        Assert.IsTrue(new DateTimeConverter().TryParse("16850304T170000".AsSpan(), out OneOf.OneOf<DateOnly, DateTimeOffset> oneOf));
        Assert.IsTrue(oneOf.IsT1);
        Assert.IsTrue(oneOf.AsT1.HasYear());
        Assert.IsTrue(oneOf.AsT1.HasDate());
    }

    [TestMethod()]
    public void HasYearTest3()
    {
        Assert.IsTrue(new TimeConverter().TryParse("T170000+0200".AsSpan(), out OneOf.OneOf<TimeOnly, DateTimeOffset> oneOf));
        Assert.IsTrue(oneOf.IsT1);
        Assert.IsFalse(oneOf.AsT1.HasYear());
        Assert.IsFalse(oneOf.AsT1.HasDate());
    }

    [TestMethod()]
    public void HasYearTest4()
    {
        Assert.IsTrue(new TimeConverter().TryParse("T--17+0200".AsSpan(), out OneOf.OneOf<TimeOnly, DateTimeOffset> oneOf));
        Assert.IsTrue(oneOf.IsT1);
        Assert.IsFalse(oneOf.AsT1.HasYear());
        Assert.IsFalse(oneOf.AsT1.HasDate());
    }
}