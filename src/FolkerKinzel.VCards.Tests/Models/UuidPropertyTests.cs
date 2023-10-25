using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class UuidPropertyTests
{
    [TestMethod]
    public void EqualsTest1() => Assert.IsFalse(new UuidProperty() == new UuidProperty());

    [TestMethod]
    public void EqualsTest2()
    {
        var uid1 = new UuidProperty();
        var uid2 = new UuidProperty(uid1.Value);
        Assert.IsTrue(uid1 == uid2);
        object o1 = uid1;
        object o2 = uid2;
        Assert.IsTrue(o1.Equals(o2));
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());

        Assert.IsFalse(o1.Equals(42));
    }
}
