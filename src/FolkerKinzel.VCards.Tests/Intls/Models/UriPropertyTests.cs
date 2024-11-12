using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

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

