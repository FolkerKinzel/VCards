using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
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
        var adr = new NameProperty(NameBuilder
            .Create()
            .AddFamilyName(LAST_NAME)
            .AddGivenName(FIRST_NAME)
            .AddAdditionalName(MIDDLE_NAME)
            .AddPrefix(PREFIX)
            .AddSuffix(SUFFIX), group: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(LAST_NAME, adr.Value.FamilyNames[0]);
        Assert.AreEqual(FIRST_NAME, adr.Value.GivenNames[0]);
        Assert.AreEqual(MIDDLE_NAME, adr.Value.AdditionalNames[0]);
        Assert.AreEqual(PREFIX, adr.Value.Prefixes[0]);
        Assert.AreEqual(SUFFIX, adr.Value.Suffixes[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    //[TestMethod()]
    //public void NamePropertyTest2()
    //{
    //    var adr = new NameProperty(
    //        [LAST_NAME],
    //        [FIRST_NAME],
    //        [MIDDLE_NAME],
    //        [PREFIX],
    //        [SUFFIX],

    //        group: GROUP);

    //    Assert.IsNotNull(adr);
    //    Assert.AreEqual(LAST_NAME, adr.Value.FamilyNames[0]);
    //    Assert.AreEqual(FIRST_NAME, adr.Value.GivenNames[0]);
    //    Assert.AreEqual(MIDDLE_NAME, adr.Value.AdditionalNames[0]);
    //    Assert.AreEqual(PREFIX, adr.Value.Prefixes[0]);
    //    Assert.AreEqual(SUFFIX, adr.Value.Suffixes[0]);
    //    Assert.AreEqual(GROUP, adr.Group);
    //    Assert.IsFalse(adr.IsEmpty);
    //}

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
        var name = new NameProperty(NameBuilder.Create().AddFamilyName(LAST_NAME));

        string s = name.ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
    }

    //[TestMethod]
    //[Obsolete]
    //public void ToDisplayNameTest1()
    //{
    //    var name = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX);
    //    string? s = name.ToDisplayName();
    //    Assert.IsNotNull(s);
    //    Assert.AreNotEqual(0, s.Length);
    //    Assert.AreEqual(1, s.GetLinesCount());
    //}

    //[TestMethod]
    //[Obsolete]
    //public void ToDisplayNameTest2()
    //{
    //    var name = new NameProperty(LAST_NAME, FIRST_NAME);
    //    string? s = name.ToDisplayName();
    //    Assert.IsNotNull(s);
    //    Assert.AreNotEqual(0, s.Length);
    //    Assert.AreEqual(1, s.GetLinesCount());
    //}

    //[TestMethod]
    //[Obsolete]
    //public void ToDisplayNameTest3()
    //{
    //    var name = new NameProperty((string?) null, FIRST_NAME);
    //    string? s = name.ToDisplayName();
    //    Assert.IsNotNull(s);
    //    Assert.AreNotEqual(0, s.Length);
    //    Assert.AreEqual(1, s.GetLinesCount());
    //}

    //[TestMethod]
    //[Obsolete]
    //public void ToDisplayNameTest4()
    //{
    //    var name = new NameProperty();
    //    string? s = name.ToDisplayName();
    //    Assert.IsNull(s);
    //}

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new NameProperty(NameBuilder.Create().AddFamilyName("Duck"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }

    [TestMethod]
    public void CountTest1()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create());
        Assert.AreEqual(7, prop.Count);
    }

    [TestMethod]
    public void ItemTest1()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create());
        Assert.IsNotNull(prop[prop.Count - 1]);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ItemTest2()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create());
        Assert.IsNotNull(prop[prop.Count]);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ItemTest3()
    {
        ICompoundProperty prop = new NameProperty(NameBuilder.Create());
        Assert.IsNotNull(prop[-1]);
    }
}
