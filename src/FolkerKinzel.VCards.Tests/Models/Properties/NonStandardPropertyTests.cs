using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

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
        Assert.AreEqual(KEY, prop.Key);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void NonStandardPropertyTest2() => _ = new NonStandardProperty("aaa", "ddd");


    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void NonStandardPropertyTest3() => _ = new NonStandardProperty(null!, "ddd");

    [TestMethod]
    public void NonStandardPropertyTest4()
    {
        const string vcf = """
            BEGIN:VCARD
            X-TEST:
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];

        IEnumerable<NonStandardProperty?>? nonStandards = vc.NonStandards;
        Assert.IsNotNull(nonStandards);

        NonStandardProperty? xTest = nonStandards.FirstOrDefault();
        Assert.IsNotNull(xTest);
        Assert.IsTrue(xTest.IsEmpty);
        Assert.IsNotNull(xTest.Value);
    }

    [TestMethod]
    public void NonStandardPropertyTest5()
    {
        const string vcf = """
            BEGIN:VCARD
            X-TEST:xYz
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];

        IEnumerable<NonStandardProperty?>? nonStandards = vc.NonStandards;
        Assert.IsNotNull(nonStandards);

        NonStandardProperty? xTest = nonStandards.FirstOrDefault();
        Assert.IsNotNull(xTest);
        Assert.AreEqual("xYz", xTest.Value);
    }


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

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new NonStandardProperty("X-TEST", "val");

        var prop2 = (NonStandardProperty)prop1.Clone();

        Assert.AreSame(prop1.Value, prop2.Value);
        Assert.AreSame(prop1.Key, prop2.Key);
        Assert.AreNotSame(prop1, prop2);
    }
}
