using System;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        string now = "Früh morgens";

        var prop = DateAndOrTimeProperty.FromText(now, GROUP);

        var vcard = new VCard
        {
            BirthDayViews = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        var prop2 = vcard.BirthDayViews!.First() as DateTimeTextProperty;

        Assert.IsNotNull(prop2);
        Assert.AreEqual(now, prop2!.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);
        Assert.AreEqual(Data.Text, prop2.Parameters.DataType);
    }

    [TestMethod]
    public void DateTimeTextPropertyTest2()
    {
        var prop = DateAndOrTimeProperty.FromText("   ");
        Assert.IsNull(prop.Value);
    }
    [TestMethod]
    public void IsEmptyTest1()
    {
        var prop = DateAndOrTimeProperty.FromText(null);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNull(prop.Value);
    }

    [TestMethod]
    public void IsEmptyTest2()
    {
        const string test = "test";
        var prop = DateAndOrTimeProperty.FromText(test);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value);
        Assert.AreEqual(test, prop.Value!.String);
    }

    [TestMethod]
    public void ToStringTest1() 
    {
        var prop = DateAndOrTimeProperty.FromText(null);
        string s = prop.ToString();
        Assert.IsNotNull(s);
        Assert.IsTrue(s.Length > 0);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = DateAndOrTimeProperty.FromText("text");

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreSame(prop1.Value!.String, prop2.Value!.String);
        Assert.AreNotSame(prop1, prop2);
    }
}

