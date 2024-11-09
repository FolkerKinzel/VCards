using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class IEnumerableExtensionTests
{
    [TestMethod]
    public void FirstOrNullIntlTest1()
        => Assert.IsNotNull(new TextProperty[] { new(null) }.FirstOrNullIntl(false));

    [TestMethod]
    public void FirstOrNullIntlTest2()
        => Assert.IsNull(new TextProperty?[] { null }.FirstOrNullIntl(false));

    [TestMethod]
    public void FirstOrNullIntlTest3()
        => Assert.IsNull(new TextProperty[] { new(null) }.FirstOrNullIntl(true));

    [TestMethod]
    public void FirstOrNullIntlTest4()
        => Assert.IsNotNull(new TextProperty[] { new("Hi") }.FirstOrNullIntl(true));

    [TestMethod]
    public void FirstOrNullIntlTest5()
        => Assert.IsNull(new TextProperty?[] { null }.FirstOrNullIntl(true));

    [TestMethod]
    public void PrefOrNullIntlTest1()
        => Assert.IsNotNull(new TextProperty[] { new(null) }.PrefOrNullIntl(static x => true, false));

    [TestMethod]
    public void OrderByIndexTest1()
    {
        var props = new TextProperty[] { new(null), new("1") };
        TextProperty[] arr = props.OrderByIndexIntl(true).ToArray();
        Assert.AreNotEqual(props.Length, arr.Length);
    }

    [TestMethod]
    public void IsSingleTest1()
    {
        var propEmpty = new TextProperty(null);
        var prop = new TextProperty("Hi");
        TextProperty? nullProp = null;

        Assert.IsFalse(Array.Empty<TextProperty>().IsSingle(true));
        Assert.IsFalse(Array.Empty<TextProperty>().IsSingle(false));
        Assert.IsFalse(propEmpty.Concat(propEmpty).IsSingle(true));
        Assert.IsFalse(propEmpty.Concat(propEmpty).IsSingle(false));
        Assert.IsTrue(prop.IsSingle(true));
        Assert.IsTrue(prop.IsSingle(false));
        Assert.IsFalse(nullProp.IsSingle(true));
        Assert.IsFalse(nullProp.IsSingle(false));
    }
}
