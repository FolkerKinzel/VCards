using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class EmbeddedTextPropertyTests
{
    [TestMethod]
    public void GetFileTypeExtensionTest1()
    {
        var prop = RawData.FromText("");
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());
    }

    [TestMethod]
    public void GetFileTypeExtensionTest2()
    {
        const string group = "gr1";
        const string mimeString = "text/html";
        var prop = new DataProperty(RawData.FromText("text", mimeString), group);

        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(mimeString, prop.Parameters.MediaType);

        string? ext = prop.Value.GetFileTypeExtension();
        Assert.AreEqual(".htm", ext);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new DataProperty(RawData.FromText("abc"));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType(prop1.Value.Object, typeof(string));
        Assert.IsInstanceOfType(prop2.Value.Object, typeof(string));
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value!.String);
        Assert.IsNotNull(prop2.Value!.String);

        Assert.AreSame(prop1.Value!.String, prop2.Value!.String);
    }
}

