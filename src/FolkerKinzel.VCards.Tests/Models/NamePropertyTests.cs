using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class NamePropertyTests
{
    private const string LAST_NAME = "Duck";
    private const string FIRST_NAME = "Donald";
    private const string MIDDLE_NAME = "Willie";
    private const string PREFIX = "Dr.";
    private const string SUFFIX = "Jr.";

    private const string GROUP = "myGroup";

    [TestMethod()]
    public void NamePropertyTest1()
    {
        var adr = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX, propertyGroup: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(LAST_NAME, adr.Value.LastName[0]);
        Assert.AreEqual(FIRST_NAME, adr.Value.FirstName[0]);
        Assert.AreEqual(MIDDLE_NAME, adr.Value.MiddleName[0]);
        Assert.AreEqual(PREFIX, adr.Value.Prefix[0]);
        Assert.AreEqual(SUFFIX, adr.Value.Suffix[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    [TestMethod()]
    public void NamePropertyTest2()
    {
        var adr = new NameProperty(
            new string[] { LAST_NAME },
            new string[] { FIRST_NAME },
            new string[] { MIDDLE_NAME },
            new string[] { PREFIX },
            new string[] { SUFFIX },

            propertyGroup: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(LAST_NAME, adr.Value.LastName[0]);
        Assert.AreEqual(FIRST_NAME, adr.Value.FirstName[0]);
        Assert.AreEqual(MIDDLE_NAME, adr.Value.MiddleName[0]);
        Assert.AreEqual(PREFIX, adr.Value.Prefix[0]);
        Assert.AreEqual(SUFFIX, adr.Value.Suffix[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    [TestMethod]
    public void NamePropertyTest3()
    {
        VcfRow row = VcfRow.Parse("FN:", new VcfDeserializationInfo())!;
        var prop = new NameProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }

    [TestMethod()]
    public void ToStringTest()
    {
        var name = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX);

        string s = name.ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
    }

    [TestMethod]
    public void ToDisplayNameTest1()
    {
        var name = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX);
        string? s = name.ToDisplayName();
        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
        Assert.AreEqual(1, s.GetLinesCount());
    }

    [TestMethod]
    public void ToDisplayNameTest2()
    {
        var name = new NameProperty(LAST_NAME, FIRST_NAME);
        string? s = name.ToDisplayName();
        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
        Assert.AreEqual(1, s.GetLinesCount());
    }

    [TestMethod]
    public void ToDisplayNameTest3()
    {
        var name = new NameProperty(null, FIRST_NAME);
        string? s = name.ToDisplayName();
        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
        Assert.AreEqual(1, s.GetLinesCount());
    }

    [TestMethod]
    public void ToDisplayNameTest4()
    {
        var name = new NameProperty();
        string? s = name.ToDisplayName();
        Assert.IsNull(s);
    }
}
