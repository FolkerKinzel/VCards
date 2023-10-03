using System;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        string now = "Früh morgens";

        var prop = DateAndOrTimeProperty.Create(now, GROUP);

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

        DateTimeTextProperty? prop2 = vcard.BirthDayViews!.First() as DateTimeTextProperty;

        Assert.IsNotNull(prop2);
        Assert.AreEqual(now, prop2!.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);
        Assert.AreEqual(VCdDataType.Text, prop2.Parameters.DataType);
    }

    [TestMethod]
    public void DateTimeTextPropertyTest2()
    {
        var prop = DateAndOrTimeProperty.Create("   ");
        Assert.IsNull(prop.Value);
    }
    [TestMethod]
    public void IsEmptyTest1()
    {
        var prop = DateAndOrTimeProperty.Create(null);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNull(prop.Value);
    }

    [TestMethod]
    public void IsEmptyTest2()
    {
        const string test = "test";
        var prop = DateAndOrTimeProperty.Create(test);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value);
        Assert.AreEqual(test, prop.Value!.String);
    }
}

