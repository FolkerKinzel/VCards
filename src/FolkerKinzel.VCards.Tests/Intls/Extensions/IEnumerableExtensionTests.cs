using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class IEnumerableExtensionTests
{
    [TestMethod]
    public void FirstOrNullIntlTest1()
        => Assert.IsNotNull(new TextProperty[] { new TextProperty(null) }.FirstOrNullIntl(false));

    [TestMethod]
    public void PrefOrNullIntlTest1()
        => Assert.IsNotNull(new TextProperty[] { new TextProperty(null) }.PrefOrNullIntl(static x => true, false));

    [TestMethod]
    public void OrderByIndexTest1()
    {
        var props = new TextProperty[] { new TextProperty(null), new TextProperty("1") };
        CollectionAssert.AreNotEqual(props, props.OrderByIndexIntl(true).ToArray());
    }
}
