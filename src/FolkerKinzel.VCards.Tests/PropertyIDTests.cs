#if !NETCOREAPP3_1
using FolkerKinzel.Strings.Polyfills;
#endif

using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class PropertyIDTests
{
    [TestMethod]
    public void CtorTest()
    {
        var pid = new PropertyID(5, new PropertyIDMappingProperty(7, new Uri("http://folkerkinzel.de/")));

        Assert.AreEqual(5, pid.ID);
        Assert.AreEqual(7, pid.Client);

        pid = new PropertyID(5);

        Assert.AreEqual(5, pid.ID);
        Assert.IsNull(pid.Client);
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
        var pidMap = new PropertyIDMappingProperty(9, new Uri("http://folkerkinzel.de/"));

        Assert.AreEqual(new PropertyID(4, pidMap), list[0]);
    }


    [TestMethod]
    public void ParseIntoTest4()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4.9,7.5");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMappingProperty(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMappingProperty(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest5()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, " 4 . 9 , 7 . 5 ");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMappingProperty(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMappingProperty(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest6()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "4.9,6.0,7.5");

        Assert.AreEqual(2, list.Count);
        var uri = new Uri("http://folker.de/");
        Assert.AreEqual(new PropertyID(4, new PropertyIDMappingProperty(9, uri)), list[0]);
        Assert.AreEqual(new PropertyID(7, new PropertyIDMappingProperty(5, uri)), list[1]);
    }


    [TestMethod]
    public void ParseIntoTest7()
    {
        var list = new List<PropertyID>();

        PropertyID.ParseInto(list, "9,22.15,7,2.0");

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
        Assert.AreEqual(new PropertyID(4, new PropertyIDMappingProperty(9, uri)), list[0]);
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
    public void EqualsTest1()
    {
        const string uriStr = "http://folkers-website.de";
        var id1 = new PropertyID(7);
        var id2 = new PropertyID(7, new PropertyIDMappingProperty(5, new Uri(uriStr)));
        var id3 = new PropertyID(7, new PropertyIDMappingProperty(5, new Uri(uriStr)));
        var id4 = new PropertyID(5, new PropertyIDMappingProperty(5, new Uri(uriStr)));
        var id5 = new PropertyID(7, new PropertyIDMappingProperty(5, new Uri("http://other-website")));
        var id6 = new PropertyID(5);

        Assert.AreNotEqual(id1, id2);
        Assert.AreNotEqual(id1.GetHashCode(), id2.GetHashCode());

        Assert.AreEqual(id2, id2);

        Assert.AreEqual(id2, id3);
        Assert.AreEqual(id2.GetHashCode(), id3.GetHashCode());

        Assert.AreNotEqual(id3, id4);
        Assert.AreNotEqual(id3.GetHashCode(), id4.GetHashCode());

        Assert.AreEqual(id3, id5);
        Assert.AreEqual(id3.GetHashCode(), id5.GetHashCode());

        Assert.AreNotEqual(id1, id6);
        Assert.AreNotEqual(id1.GetHashCode(), id6.GetHashCode());
    }

    [TestMethod]
    public void EqualsTest2()
    {
        var propID = new PropertyID(7);
        Assert.IsFalse(propID.Equals(7));
    }

    [TestMethod]
    public void EqualityOperatorTest1()
    {
        var propID1 = new PropertyID(7);
        PropertyID propID2 = propID1;

        Assert.IsTrue(propID1 == propID2);
    }

    [TestMethod]
    public void EqualityOperatorTest2()
    {
        PropertyID? propID1 = null;
        Assert.IsTrue(propID1 == null);
    }

    [TestMethod]
    public void EqualityOperatorTest3()
    {
        PropertyID? propID1 = null;
        var propID = new PropertyID(7);

        Assert.IsFalse(propID1 == propID);
    }

    [TestMethod]
    public void EqualityOperatorTest4()
    {
        var propID1 = new PropertyID(5);
        var propID2 = new PropertyID(7);

        Assert.IsFalse(propID1 == propID2);
    }

    [TestMethod]
    public void IEnumerableTest()
    {
        var id1 = new PropertyID(7);
        PropertyID? id2 = id1;
        Assert.IsTrue(id1 == id2);
        id2 = null;
        Assert.IsFalse(id2 == id1);

        System.Collections.IEnumerable numerable = id1;

        foreach (object? item in numerable)
        {
            Assert.IsTrue((item as PropertyID) == id1);
            Assert.IsFalse((item as PropertyID) != id1);
        }
    }


    [TestMethod]
    public void ToStringTest1()
    {
        var uri = new Uri("http://folker.de/");
        var pid = new PropertyID(5, new PropertyIDMappingProperty(7, uri));

        string s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Contains('.'));
        Assert.AreEqual(3, s.Length);
    }


    [TestMethod]
    public void ToStringTest2()
    {
        var pid = new PropertyID(5);

        string s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsFalse(s.Contains('.'));
        Assert.AreEqual(1, s.Length);
    }

}
