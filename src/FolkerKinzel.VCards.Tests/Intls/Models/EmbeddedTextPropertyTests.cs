using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.ObjectModel;

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

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = DataProperty.FromText("abc");
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType(prop1, typeof(EmbeddedTextProperty));
        Assert.IsInstanceOfType(prop2, typeof(EmbeddedTextProperty));
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsTrue(prop1.Value!.TryGetString(out string? str1));
        Assert.IsTrue(prop2.Value!.TryGetString(out string? str2));
        
        Assert.AreSame(str1, str2);
    }
}

