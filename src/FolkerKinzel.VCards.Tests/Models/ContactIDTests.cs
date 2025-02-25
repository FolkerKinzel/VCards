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
    public void CreateTest3()
    {
        var id = ContactID.Create(new Uri("urn:uuid:A0CD4379-64AB-4BFA-9CEC-66DC76CA585E", UriKind.Absolute));
        Assert.IsNotNull(id.Guid);
    }

    [TestMethod]
    public void CreateTest4()
    {
        var id = ContactID.Create(new Uri("urn:uuid:blabla", UriKind.Absolute));
        Assert.IsNotNull(id.Uri);
    }

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
    public void ConvertTest1b()
    {
        const string test = "test";
        string? result = null;

        result = ContactID.Create(new Uri("http://folker.com/")).Convert(test, null!, (guid, str) => str, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest1c()
    {
        const string test = "test";
        string? result = null;

        result = ContactID.Create("Hi").Convert(test, null!, null!, (guid, str) => str);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest2() => _ = ContactID.Create().Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest3() => _ = ContactID.Create(new Uri("http://folker.com/")).Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest4() => _ = ContactID.Create("Hi").Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest5() => _ = ContactID.Create().Convert<string>(null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest6() => _ = ContactID.Create(new Uri("http://folker.com/")).Convert<string>(null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest7() => _ = ContactID.Create("Hi").Convert<string>(null!, null!, null!);

    [TestMethod]
    public void ConvertTest8()
    {
        const string test = "test";
        string? result = null;

        result = ContactID.Create().Convert(guid => test, null!, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest8b()
    {
        const string test = "test";
        string? result = null;

        result = ContactID.Create(new Uri("http://folker.com/")).Convert(null!, (guid) => test, null!);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest8c()
    {
        const string test = "test";

        string? result = null;

        result = ContactID.Create("Hi").Convert(null!, null!, (guid) => test);

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

    [TestMethod]
    public void EqualsTest2()
    {
        var uid1 = ContactID.Create();
        Assert.IsTrue(uid1.Guid.HasValue);
        Assert.IsNull(uid1.Uri);
        Assert.IsNull(uid1.String);
        ContactID? uid2 = null;
        Assert.IsFalse(uid1.Equals(uid2));
        var uid3 = ContactID.Create(uid1.Guid.Value);

        Assert.IsTrue(uid1.Equals(uid3));
        Assert.AreEqual(uid1.GetHashCode(), uid3.GetHashCode());
    }

    [TestMethod]
    public void EqualsTest3()
    {
        var uid1 = ContactID.Create("Hi");
        ContactID? uid2 = ContactID.Create();
        var uid3 = ContactID.Create("Hi");
        Assert.IsFalse(uid1.Equals(uid2));
        Assert.IsFalse(uid1.Equals(null));
        Assert.IsTrue(uid1.Equals(uid3));
        Assert.AreEqual(uid1.GetHashCode(), uid3.GetHashCode());
    }

    [TestMethod]
    public void EqualityTest4()
    {
        const string test = "http://folker.com/";
        var uid1 = ContactID.Create(test);
        var uid2 = ContactID.Create(new Uri(test, UriKind.Absolute));
        Assert.IsTrue(uid1.Equals(uid2));
        Assert.AreEqual(uid1.GetHashCode(), uid2.GetHashCode());

        Assert.IsFalse(uid1.Equals(null));
    }
}


