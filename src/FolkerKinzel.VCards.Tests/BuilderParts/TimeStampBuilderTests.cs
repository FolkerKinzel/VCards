﻿namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TimeStampBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new TimeStampBuilder().Set(DateTimeOffset.UtcNow);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TimeStampBuilder().Clear();

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TimeStampBuilder().Equals((TimeStampBuilder?)null));

        var builder = new TimeStampBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TimeStampBuilder().ToString());
}
