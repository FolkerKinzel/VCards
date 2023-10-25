using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class UriPropertyTests
{
    [TestMethod]
    public void GetVCardPropertyValueTest1()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de", UriKind.Absolute, out Uri? uri));
        VCardProperty prop = new UriProperty(uri, new ParameterSection(), null);

        Assert.AreSame(uri, prop.Value);
    }
}

