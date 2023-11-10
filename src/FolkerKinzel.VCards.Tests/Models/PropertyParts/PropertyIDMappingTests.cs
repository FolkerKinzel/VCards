#if !NETCOREAPP3_1
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class PropertyIDMappingTests
{
    [TestMethod]
    public void PropertyIDMappingTest1()
    {
        var pidMap = new PropertyIDMapping(5, new Uri("http://folkerkinzel.de/"));
        Assert.AreEqual(5, pidMap.LocalID);
    }

    [DataTestMethod()]
    [DataRow(-1)]
    [DataRow(10)]
    [DataRow(0)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void PropertyIDMappingTest2(int mappingNumber)
    {
        var uri = new Uri("http://folker.de/");
        _ = new PropertyIDMapping(mappingNumber, uri);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
    public void PropertyIDMappingTest3() => _ = new PropertyIDMapping(0, new Uri("http://folkerkinzel.de/"));

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = false)]
    public void PropertyIDMappingTest4() => _ = new PropertyIDMapping(3, null!);


    [TestMethod]
    public void ParseTest1()
    {
        string pidMap = "2;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        var mapping = PropertyIDMapping.Parse(pidMap);

        Assert.AreEqual(2, mapping.LocalID);
        Assert.AreEqual(new Uri(pidMap.Substring(2)), mapping.GlobalID);
    }

    [TestMethod]
    public void ParseTest2()
    {
        string pidMap = "  2 ; urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        var mapping = PropertyIDMapping.Parse(pidMap);

        Assert.AreEqual(2, mapping.LocalID);
        Assert.AreEqual(new Uri(pidMap.Substring(6)), mapping.GlobalID);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest3()
    {
        string pidMap = "22;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        _ = PropertyIDMapping.Parse(pidMap);
    }

    [TestMethod]
    public void ParseTest4()
    {
        string pidMap = "2;http://d89c9c7a-2e1b-4832-82de-7e992d95faa5";

        _ = PropertyIDMapping.Parse(pidMap);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest5()
    {
        string pidMap = "2";

        _ = PropertyIDMapping.Parse(pidMap);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest6()
    {
        string pidMap = "";

        _ = PropertyIDMapping.Parse(pidMap);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest7()
    {
        string pidMap = "a";

        _ = PropertyIDMapping.Parse(pidMap);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest8()
    {
        string pidMap = "2;http:////////////////// ";
        _ = PropertyIDMapping.Parse(pidMap);
    }


    [TestMethod]
    public void ToStringTest1()
    {
        int i = 4;

        var pidmap = new PropertyIDMapping(i, new Uri("http://folkerkinzel.de/"));

        string s = pidmap.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Contains(';'));
        Assert.IsTrue(5 < s.Length);
    }

}
