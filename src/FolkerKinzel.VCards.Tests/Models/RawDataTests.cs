using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class RawDataTests
{
    [TestMethod]
    public void SwitchTest1()
    {
        var rel = RawData.FromBytes([]);
        rel.Switch(s => rel = null, null!, null!);
        Assert.IsNull(rel);
    }

    [TestMethod]
    public void ValueTest1()
    {
        var rel = RawData.FromText("Hi");
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = RawData.FromUri(new Uri("http://folker.de/"));
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNotNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest3()
    {
        var rel = RawData.FromBytes([]);
        Assert.IsNotNull(rel.Object);
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
}
