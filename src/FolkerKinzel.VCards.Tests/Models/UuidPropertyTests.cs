﻿namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class UuidPropertyTests
{
    [TestMethod]
    public void EqualsTest1() => Assert.IsFalse(new IDProperty() == new IDProperty());

    [TestMethod]
    public void EqualsTest2()
    {
        var uid1 = new IDProperty();
        var uid2 = new IDProperty(uid1.Value);
        Assert.IsTrue(uid1 == uid2);
        Assert.IsFalse(uid1 != uid2);
        object o1 = uid1;
        object o2 = uid2;
        Assert.IsTrue(o1.Equals(o2));
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());

        Assert.IsFalse(o1.Equals(42));
    }

    [TestMethod]
    public void EqualityOperatorTest1()
    {
        var uid1 = new IDProperty();
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.IsTrue(uid1 == uid1);
#pragma warning restore CS1718 // Comparison made to same variable

        IDProperty? uid2 = null;
        Assert.IsTrue(uid2 is null);
        Assert.IsFalse(uid2 == uid1);
    }
}
