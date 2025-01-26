using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class TimeStampPropertyTests
{
    [TestMethod]
    public void TimeStampPropertyTest1()
    {
        var prop = new TimeStampProperty();

        Assert.IsNull(prop.Group);
        Assert.AreNotEqual(default, prop.Value);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.ToString());  
    }

    [TestMethod]
    public void TimeStampPropertyTest2()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var prop = new TimeStampProperty(now);

        Assert.IsNull(prop.Group);
        Assert.AreEqual(now, prop.Value);
    }

    [TestMethod]
    public void TimeStampPropertyTest3()
    {
        var prop = new TimeStampProperty(new DateTime(1848, 3, 18), "G");

        Assert.AreEqual("G", prop.Group);
        Assert.AreNotEqual(default, prop.Value);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.ToString());
    }

    [TestMethod]
    public void TimeStampPropertyTest4()
    {
        var info = new VcfDeserializationInfo();
        VcfRow row = VcfRow.Parse("REV:2023-04-04".AsMemory(), info)!;

        var prop = new TimeStampProperty(row, info);

        Assert.IsTrue(prop.IsEmpty);
    }

    [TestMethod]
    public void GetValueTest()
    {
        VCardProperty prop = new TimeStampProperty();
        Assert.IsNotNull(prop.Value);
    }
}
