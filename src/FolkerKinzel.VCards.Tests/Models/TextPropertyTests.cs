using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class TextPropertyTests
{
    [TestMethod]
    public void IEnumerableTest()
    {
        var tProp = new TextProperty("Good value");

        TextProperty value = tProp.AsWeakEnumerable().Cast<TextProperty>().First();

        Assert.AreSame(tProp, value);

    }

    [TestMethod]
    public void IEnumerableTTest()
    {
        var tProp = new TextProperty("Good value");

        TextProperty value = tProp.First();

        Assert.AreSame(tProp, value);

    }


    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextProperty(null).ToString());
}