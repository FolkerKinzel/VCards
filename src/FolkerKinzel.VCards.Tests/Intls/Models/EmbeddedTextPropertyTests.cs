using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class EmbeddedTextPropertyTests
{
    [TestMethod]
    public void GetFileTypeExtensionTest1()
    {
        var prop = DataProperty.FromText(null);
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());
    }

    [TestMethod]
    public void GetFileTypeExtensionTest2()
    {
        const string group = "gr1";
        const string mimeString = "text/html";
        var prop = DataProperty.FromText("text", mimeString, group);

        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(mimeString, prop.Parameters.MediaType);
        Assert.AreEqual(VCdDataType.Text, prop.Parameters.DataType);

        string? ext = prop.GetFileTypeExtension();
        Assert.AreEqual(".htm", ext);
    }
}

