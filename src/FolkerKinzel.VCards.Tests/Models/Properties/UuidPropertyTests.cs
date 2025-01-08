namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class UuidPropertyTests
{
    [TestMethod]
    public void EqualsTest1() => Assert.IsFalse(new ContactIDProperty(ContactID.Create()) == new ContactIDProperty(ContactID.Create()));

    [TestMethod]
    public void EqualsTest2()
    {
        var uid1 = ContactID.Create();
        var uid2 = uid1;
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
        var uid1 = new ContactIDProperty(ContactID.Create());
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.IsTrue(uid1 == uid1);
#pragma warning restore CS1718 // Comparison made to same variable

        ContactIDProperty? uid2 = null;
        Assert.IsTrue(uid2 is null);
        Assert.IsFalse(uid2 == uid1);
    }
}
