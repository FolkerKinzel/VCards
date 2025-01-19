using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class ContactIDTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateTest1() => ContactID.Create("  ");

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateTest2() => ContactID.Create(new Uri("../relative", UriKind.Relative));

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(ContactID.Empty.ToString());

    [TestMethod]
    public void ToStringTest2() => Assert.IsNotNull(ContactID.Create("Hi").ToString());

    [TestMethod]
    public void ToStringTest3() => Assert.IsNotNull(ContactID.Create(new Uri("http://folker.com/")).ToString());

    [TestMethod]
    public void ToStringTest4() => Assert.IsNotNull(ContactID.Create().ToString());

    [TestMethod]
    public void SwitchTest1()
    {
        Guid? guid = null;
        ContactID.Create().Switch("", (uuid, str) => guid = uuid);
        Assert.IsNotNull(guid);
    }

    [TestMethod]
    public void SwitchTest2() => ContactID.Create().Switch("");

    [TestMethod]
    public void SwitchTest3()
    {
        Guid? result = null;

        ContactID.Create().Switch(uuid => result = uuid);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest4()
    {
        Uri? result = null;

        ContactID.Create(new Uri("http://folker.com/")).Switch(null, uri => result = uri);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest5()
    {
        string? result = null;

        ContactID.Create("Hi").Switch(null, null, str => result = str);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ConvertTest1()
    {
        const string test = "test";
        string? result = null;

        result = ContactID.Create().Convert(test, (guid, str) => str, null!, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void EqualsTest1()
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
}


