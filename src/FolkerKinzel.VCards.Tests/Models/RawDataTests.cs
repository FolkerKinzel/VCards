using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class RawDataTests
{
    [TestMethod]
    public void ValueTest1()
    {
        var rel = RawData.FromText("Hi");
        Assert.IsNotNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNull(rel.Uri);
    }

    [TestMethod]
    public void ValueTest2()
    {
        var rel = RawData.FromUri(new Uri("http://folker.de/"));
        Assert.IsNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNotNull(rel.Uri);
    }

    [TestMethod]
    public void ValueTest3()
    {
        var rel = RawData.FromBytes([]);
        Assert.IsNull(rel.String);
        Assert.IsNotNull(rel.Bytes);
        Assert.IsNull(rel.Uri);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop = new DataProperty(RawData.FromBytes([1, 2, 3]));
        string s = prop.ToString();
        StringAssert.Contains(s, "3 Bytes");
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var prop = new DataProperty(RawData.FromUri(new Uri("http://contoso.com")));
        string s = prop.ToString();
        StringAssert.Contains(s, "contoso");
    }

    [TestMethod]
    public void ToStringTest3()
    {
        const string passWord = "Simsalabim";
        var prop = new DataProperty(RawData.FromText(passWord));
        string s = prop.ToString();
        StringAssert.Contains(s, passWord);
    }

    [TestMethod]
    public void ToStringTest4() => Assert.IsNotNull(new DataProperty(RawData.FromText("")).ToString());

    [TestMethod]
    public void GetFileTypeExtensionTest1()
        => Assert.AreEqual(".htm", RawData.FromUri(new Uri("http://folker.de/", UriKind.Absolute)).GetFileTypeExtension(), false);

    [TestMethod]
    public void GetFileTypeExtensionTest2()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/test.txt", UriKind.Absolute, out Uri? uri));

        var prop = RawData.FromUri(uri, "image/png");
        Assert.AreEqual(".png", prop.GetFileTypeExtension());

        prop = RawData.FromUri(uri);
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());
    }

    [TestMethod]
    public void GetFileTypeExtensionTest3()
    {
        var prop = RawData.FromText("");
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());
    }

    [TestMethod]
    public void GetFileTypeExtensionTest4()
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
    public void SwitchTest1()
    {
        byte[]? bytes = null;
        var rel = RawData.FromBytes([]);
        rel.Switch(b => bytes = b, null!, null!);
        Assert.IsNotNull(bytes);
    }

    [TestMethod]
    public void SwitchTest2() => RawData.FromBytes([]).Switch("");

    [TestMethod]
    public void SwitchTest3()
    {
        Uri? result = null;
        RawData.FromUri(new Uri("http://folker.com/")).Switch(null, uri => result = uri);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SwitchTest4()
    {
        string? result = null;
        RawData.FromText("text").Switch(null, null, str => result = str);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ConvertTest1()
    {
        const string test = "test";
        string? result = null;

        result = RawData.FromBytes([]).Convert(test, (bytes, str) => str, null!, null!);
        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest2()
    {
        const string test = "test";
        string? result = null;

        result = RawData.FromUri(new Uri("http://folker.com/")).Convert(test, null!, (uri, str) => str, null!);
        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest3()
    {
        const string test = "test";
        string? result = null;

        result = RawData.FromText("TEXT").Convert(test, null!, null!, (text, str) => str);
        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void ConvertTest4()
    {
        string? result = null;

        result = RawData.FromBytes([]).Convert(bytes => bytes.ToString(), null!, null!);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ConvertTest5()
    {
        string? result = null;

        result = RawData.FromUri(new Uri("http://folker.com/")).Convert(null!, uri => uri.AbsoluteUri, null!);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ConvertTest6()
    {
        string? result = null;

        result = RawData.FromText("TEXT").Convert(null!, null!, text => text);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest7() => _ = RawData.FromBytes([]).Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest8() => _ = RawData.FromUri(new Uri("http://folker.com/")).Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest9() => _ = RawData.FromText("TEXT").Convert<string, string>("", null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest10() => _ = RawData.FromBytes([]).Convert<string>(null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest11() => _ = RawData.FromUri(new Uri("http://folker.com/")).Convert<string>(null!, null!, null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest12() => _ = RawData.FromText("TEXT").Convert<string>(null!, null!, null!);
}
