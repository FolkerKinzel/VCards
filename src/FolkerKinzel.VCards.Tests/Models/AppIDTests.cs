using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class AppIDTests
{
    [TestMethod]
    public void AppIDTest1()
    {
        var pidMap = new AppID(5, "http://folkerkinzel.de/");
        Assert.AreEqual(5, pidMap.LocalID);
    }

    [TestMethod]
    public void TryParseTest1()
    {
        string pidMap = "2;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        Assert.IsTrue(AppID.TryParse(pidMap.AsSpan(), out AppID? client));

        Assert.AreEqual(2, client.LocalID);
        Assert.AreEqual(pidMap.Substring(2), client.GlobalID);
    }

    [TestMethod]
    public void TryParseTest2()
    {
        string pidMap = "  2 ; urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        Assert.IsTrue(AppID.TryParse(pidMap.AsSpan(), out AppID? client));

        Assert.AreEqual(2, client.LocalID);
        Assert.AreEqual(pidMap.Substring(6), client.GlobalID);
    }

    [TestMethod]
    public void TryParseTest3()
    {
        string pidMap = "22;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        Assert.IsTrue(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest4()
    {
        string pidMap = "2;http://d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        Assert.IsTrue(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest5()
    {
        string pidMap = "2";

        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest6()
    {
        string pidMap = "";

        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest7()
    {
        string pidMap = "a";

        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest8()
    {
        string pidMap = "xyz;http://folker.de/";
        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest9()
    {
        string pidMap = "1;    ";
        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }

    [TestMethod]
    public void TryParseTest10()
    {
        string pidMap = "-7;http://folker.de/";
        Assert.IsFalse(AppID.TryParse(pidMap.AsSpan(), out _));
    }


    [TestMethod]
    public void ToStringTest1()
    {
        int i = 4;

        var pidmap = new AppID(i, "http://folkerkinzel.de/");

        string s = pidmap.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Contains(';'));
        Assert.IsTrue(5 < s.Length);
    }

}
