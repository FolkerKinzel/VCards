#if !NETCOREAPP3_1
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class PropertyIDTest
{
    [TestMethod]
    public void CtorTest()
    {
        var pid = new PropertyID(5, new PropertyIDMapping(7, new Uri("http://folkerkinzel.de/")));

        Assert.AreEqual(5, pid.ID);
        Assert.AreEqual(7, pid.Mapping);

        pid = new PropertyID(5);

        Assert.AreEqual(5, pid.ID);
        Assert.IsNull(pid.Mapping);
    }


    [TestMethod]
    public void ParseIntoTest1()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "");

        Assert.AreEqual(0, list.Count);
    }


    [TestMethod]
    public void ParseIntoTest2()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4");

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(new PropertyID(4), list[0]);
    }


    [TestMethod]
    public void ParseIntoTest3()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4.9");

        Assert.AreEqual(1, list.Count);
        var pidMap = new PropertyIDMapping(9, new Uri("http://folkerkinzel.de/"));

        Assert.AreEqual(new PropertyID(4, pidMap), list[0]);
    }


    [TestMethod]
    public void ParseIntoTest4()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4.9,7.5");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMapping(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMapping(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest5()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, " 4 . 9 , 7 . 5 ");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMapping(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMapping(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest6()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4.9,6.0,7.5");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMapping(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMapping(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest7()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "9,22.15,7");

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual(new PropertyID(9), list[0]);
        Assert.AreEqual(new PropertyID(7), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest8()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, " \"4.9\"");

        Assert.AreEqual(1, list.Count);

        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMapping(9, uri)), list[0]);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
    public void CtorExceptionTest1() => _ = new PropertyID(0);


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
    public void CtorExceptionTest3() => _ = new PropertyID(10);


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
    public void CtorExceptionTest4() => _ = new PropertyID(10, null);


    [TestMethod]
    public void ToStringTest1()
    {
        var uri = new Uri("http://folker.de/");
        var pid = new PropertyID(5, new PropertyIDMapping(7, uri));

        string s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Contains('.'));
        Assert.AreEqual(3, s.Length);
    }


    [TestMethod]
    public void ToStringTest2()
    {
        var pid = new PropertyID(5);

        var s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsFalse(s.Contains('.'));
        Assert.AreEqual(1, s.Length);
    }

}
