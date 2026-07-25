namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class PropertyIDTests
{
    [TestMethod]
    public void CtorTest()
    {
        var pid = new PropertyID(5, new AppID(7, "http://folkerkinzel.de/"));

        Assert.AreEqual(5, pid.ID);
        Assert.AreEqual(7, pid.App);

        pid = new PropertyID(5, null);

        Assert.AreEqual(5, pid.ID);
        Assert.IsNull(pid.App);
    }


    [TestMethod]
    public void ParseTest1() => Assert.IsEmpty(PropertyID.Parse("".AsSpan()));


    [TestMethod]
    public void ParseTest2()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("4".AsSpan());
        Assert.HasCount(1, list);
        Assert.AreSequenceEqual(new PropertyID(4, null), list);
    }


    [TestMethod]
    public void ParseTest3()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("4.9".AsSpan());

        Assert.HasCount(1, list);
        var pidMap = new AppID(9, "http://folkerkinzel.de/");

        Assert.AreSequenceEqual(new PropertyID(4, pidMap), list);
    }


    [TestMethod]
    public void ParseTest4()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("4.9,7.5".AsSpan());

        Assert.HasCount(2, list);
        string uri = "http://folker.de/";
        Assert.AreSequenceEqual([new PropertyID(4, new AppID(9, uri)), new PropertyID(7, new AppID(5, uri))],
                                list);
    }


    [TestMethod]
    public void ParseTest5()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse(" 4 . 9 , 7 . 5 ".AsSpan());

        Assert.HasCount(2, list);
        string uri = "http://folker.de/";
        Assert.AreSequenceEqual([new PropertyID(4, new AppID(9, uri)), new PropertyID(7, new AppID(5, uri))],
                                list);
    }


    [TestMethod]
    public void ParseTest6()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("4.9,6.0,7.5".AsSpan());

        Assert.HasCount(2, list);
        string uri = "http://folker.de/";
        Assert.AreSequenceEqual([new PropertyID(4, new AppID(9, uri)), new PropertyID(7, new AppID(5, uri))],
                                list);
    }

    [TestMethod]
    public void ParseTest7()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("9,22.15,7,2.0".AsSpan());

        Assert.HasCount(3, list);

        PropertyID[] expectedResult = [new PropertyID(9, null),
                                       new PropertyID(22, new AppID(15, "http://www.contoso.com/")),
                                       new PropertyID(7, null)];

        Assert.AreSequenceEqual(expectedResult, list);
    }

    [TestMethod]
    public void ParseTest8()
    {
        IEnumerable<PropertyID> list = PropertyID.Parse("-7,22.-15,-2.8,xyz,xy.7,7.xy".AsSpan());

        Assert.IsEmpty(list);
    }

    [TestMethod]
    public void PropertyIDTest3() => _ = new PropertyID(10, null);

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MSTEST0065:Avoid Assert.AreEqual on collection types", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MSTEST0032:Assertion condition is always true", Justification = "<Pending>")]
    public void EqualsTest1()
    {
        const string uriStr = "http://folkers-website.de";
        var id1 = new PropertyID(7, null);
        var id2 = new PropertyID(7, new AppID(5, uriStr));
        var id3 = new PropertyID(7, new AppID(5, uriStr));
        var id4 = new PropertyID(5, new AppID(5, uriStr));
        var id5 = new PropertyID(7, new AppID(5, "http://other-website"));
        var id6 = new PropertyID(5, null);

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
        var propID = new PropertyID(7, null);
        Assert.IsFalse(propID.Equals(7));
    }

    [TestMethod]
    public void EqualsTest2b()
    {
        var propID = new PropertyID(7, null);

        Assert.IsTrue(propID.Equals((object)propID));
    }

    [TestMethod]
    public void EqualityOperatorTest1()
    {
        var propID1 = new PropertyID(7, null);
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
        var propID = new PropertyID(7, null);

        Assert.IsFalse(propID1 == propID);
    }

    [TestMethod]
    public void EqualityOperatorTest4()
    {
        var propID1 = new PropertyID(5, null);
        var propID2 = new PropertyID(7, null);

        Assert.IsFalse(propID1 == propID2);
    }

    [TestMethod]
    public void IEnumerableTest()
    {
        var id1 = new PropertyID(7, null);
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
        string uri = "http://folker.de/";
        var pid = new PropertyID(5, new AppID(7, uri));

        string s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsTrue(s.Contains('.'));
        Assert.AreEqual(3, s.Length);
    }


    [TestMethod]
    public void ToStringTest2()
    {
        var pid = new PropertyID(5, null);

        string s = pid.ToString();

        Assert.IsNotNull(s);
        Assert.IsFalse(s.Contains('.'));
        Assert.AreEqual(1, s.Length);
    }

}
