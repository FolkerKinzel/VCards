using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class NonStandardPropertyTests
{
    [TestMethod()]
    public void NonStandardPropertyTest1()
    {
        const string GROUP = "theGroup";
        const string KEY = "X-Test";
        const string VALUE = "The value";

        var prop = new NonStandardProperty(KEY, VALUE, GROUP);

        Assert.AreEqual(GROUP, prop.Group);
        Assert.AreEqual(VALUE, prop.Value);
        Assert.AreEqual(KEY, prop.XName);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void NonStandardPropertyTest2() => _ = new NonStandardProperty("aaa", "ddd");


    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void NonStandardPropertyTest3() => _ = new NonStandardProperty(null!, "ddd");


    [TestMethod()]
    public void ToStringTest()
    {
        const string GROUP = "theGroup";
        const string KEY = "X-Test";
        const string VALUE = "The value";

        var prop = new NonStandardProperty(KEY, VALUE, GROUP);

        Assert.IsNotNull(prop.ToString());
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new NonStandardProperty("X-TEST", "val");
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
