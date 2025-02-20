using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

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
        var adr = new NameProperty(NameBuilder
            .Create()
            .AddSurname(LAST_NAME)
            .AddGiven(FIRST_NAME)
            .AddGiven2(MIDDLE_NAME)
            .AddPrefix(PREFIX)
            .AddSuffix(SUFFIX)
            .Build(), group: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(LAST_NAME, adr.Value.Surnames[0]);
        Assert.AreEqual(FIRST_NAME, adr.Value.Given[0]);
        Assert.AreEqual(MIDDLE_NAME, adr.Value.Given2[0]);
        Assert.AreEqual(PREFIX, adr.Value.Prefixes[0]);
        Assert.AreEqual(SUFFIX, adr.Value.Suffixes[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    [TestMethod]
    public void NamePropertyTest3()
    {
        VcfRow row = VcfRow.Parse("FN:".AsMemory(), new VcfDeserializationInfo())!;
        var prop = new NameProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }

    [TestMethod()]
    public void ToStringTest()
    {
        var name = new NameProperty(NameBuilder.Create().AddSurname(LAST_NAME).Build());

        string s = name.ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new NameProperty(NameBuilder.Create().AddSurname("Duck").Build());
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }

    [TestMethod]
    public void CountTest1()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create().Build());
        Assert.AreEqual(7, prop.Count);
    }

    [TestMethod]
    public void ItemTest1()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create().Build());
        Assert.IsNotNull(prop[prop.Count - 1]);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ItemTest2()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create().Build());
        Assert.IsNotNull(prop[prop.Count]);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ItemTest3()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create().Build());
        Assert.IsNotNull(prop[-1]);
    }
}
