using System;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeOffsetPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeOffsetPropertyTest1()
    {

        var prop = DateAndOrTimeProperty.Create(DateTimeOffset.UtcNow, GROUP) as DateTimeOffsetProperty;

        Assert.IsNotNull(prop);
        Assert.IsNotNull(prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.AreNotEqual(DateTimeOffset.MinValue, prop.Value);

        DateTimeOffset dto = prop.Value;

        VCardProperty vcProp = prop;
        Assert.AreEqual(vcProp.Value, dto);
    }


    [TestMethod()]
    public void DateTimeOffsetPropertyTest2()
    {
        var now = new DateTimeOffset(2021, 4, 4, 12, 41, 2, TimeSpan.FromHours(2));

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

        DateTimeOffsetProperty? prop2 = vcard.BirthDayViews!.First() as DateTimeOffsetProperty;

        Assert.IsNotNull(prop2);
        Assert.AreEqual(now, prop2!.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);

        Assert.AreEqual(VCdDataType.DateAndOrTime, prop2.Parameters.DataType);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest3()
    {
        var prop = DateAndOrTimeProperty.Create(DateTimeOffset.MinValue, GROUP);
        Assert.IsNull(prop.Value);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest4()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.First();
        Assert.AreSame(first, prop);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest5()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.AsWeakEnumerable().First();
        Assert.AreSame(first, prop);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest6()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        foreach (DateAndOrTimeProperty item in prop)
        {
            Assert.IsNotNull(item.Value);
        }
    }

    [TestMethod]
    public void CloneTest1()
    {
        DateTimeOffset dto = DateTimeOffset.Now;
        const string group = "gr1";

        var prop = DateAndOrTimeProperty.Create(dto, group);

        Assert.IsInstanceOfType(prop, typeof(DateTimeOffsetProperty));
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value?.DateTimeOffset);
        Assert.AreEqual(VCdDataType.DateAndOrTime, prop.Parameters.DataType);

        var clone = (DateAndOrTimeProperty)prop.Clone();
        Assert.IsInstanceOfType(prop, typeof(DateTimeOffsetProperty));
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value?.DateTimeOffset);
        Assert.AreEqual(VCdDataType.DateAndOrTime, prop.Parameters.DataType);

        Assert.AreNotSame(clone, prop);
    }
}

