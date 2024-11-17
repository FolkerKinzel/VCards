using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class TimeStampPropertyTests
{
    [TestMethod]
    public void TimeStampPropertyTest1a()
    {
        var prop = new TimeStampProperty();

        Assert.IsNull(prop.Group);
        Assert.AreNotEqual(default, prop.Value);
    }

    [TestMethod]
    public void TimeStampPropertyTest1b()
    {
        var prop = new TimeStampProperty();
        Assert.AreNotEqual(default, prop.Value);
    }

    [TestMethod]
    public void TimeStampPropertyTest2a()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var prop = new TimeStampProperty(now);

        Assert.IsNull(prop.Group);
        Assert.AreEqual(now, prop.Value);
    }

    [TestMethod]
    public void TimeStampPropertyTest2b()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var prop = new TimeStampProperty(now);
        Assert.AreEqual(now, prop.Value);
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
