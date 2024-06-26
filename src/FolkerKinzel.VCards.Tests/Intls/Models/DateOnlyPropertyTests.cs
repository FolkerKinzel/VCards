﻿using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateOnlyPropertyTests
{
    [TestMethod]
    public void DateOnlyPropertyTest1()
    {
        var dateOnly = DateOnly.FromDateTime(DateTime.Now);
        var prop = DateAndOrTimeProperty.FromDate(dateOnly);

        Assert.IsNotNull(prop);
        Assert.IsInstanceOfType(prop, typeof(DateOnlyProperty));
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(dateOnly, prop.Value!.DateOnly);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = DateAndOrTimeProperty.FromDate(3, 15);

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreEqual(prop1.Value!.DateOnly, prop2.Value!.DateOnly);
        Assert.AreNotSame(prop1, prop2);

    }
}

