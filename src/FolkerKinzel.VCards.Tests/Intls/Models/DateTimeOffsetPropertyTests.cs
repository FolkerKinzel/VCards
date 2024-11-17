using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeOffsetPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod]
    public void DateTimeOffsetPropertyTest1()
    {
        var prop = new DateAndOrTimeProperty(DateTimeOffset.UtcNow, GROUP);

        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop!.Value!.DateTimeOffset);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.AreNotEqual(DateTimeOffset.MinValue, prop.Value.DateTimeOffset!.Value);

        VCardProperty vcProp = prop;
        Assert.AreSame(vcProp.Value, prop.Value);
    }


    [TestMethod]
    public void DateTimeOffsetPropertyTest2()
    {
        var now = new DateTimeOffset(2021, 4, 4, 12, 41, 2, TimeSpan.FromHours(2));

        var prop = new DateAndOrTimeProperty(now, GROUP);

        var vcard = new VCard
        {
            BirthDayViews = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        DateAndOrTimeProperty? prop2 = vcard.BirthDayViews!.First();

        Assert.IsNotNull(prop2);
        Assert.IsTrue(prop2.Value.DateTimeOffset.HasValue);   
        Assert.AreEqual(now, prop2!.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);

        Assert.AreEqual(Data.DateAndOrTime, prop2.Parameters.DataType);
    }

    [TestMethod]
    public void DateTimeOffsetPropertyTest3()
    {
        var prop = new DateAndOrTimeProperty(DateTimeOffset.MinValue, GROUP);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.Value);
    }

    [TestMethod]
    public void DateTimeOffsetPropertyTest4()
    {
        IEnumerable<DateAndOrTimeProperty> prop = new DateAndOrTimeProperty(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.First();
        Assert.AreSame(first, prop);
    }

    [TestMethod]
    public void DateTimeOffsetPropertyTest5()
    {
        IEnumerable<DateAndOrTimeProperty> prop = new DateAndOrTimeProperty(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.AsWeakEnumerable().First();
        Assert.AreSame(first, prop);
    }

    [TestMethod]
    public void DateTimeOffsetPropertyTest6()
    {
        IEnumerable<DateAndOrTimeProperty> prop = new DateAndOrTimeProperty(DateTimeOffset.Now, GROUP);
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

        var prop = new DateAndOrTimeProperty(dto, group);

        Assert.IsTrue(prop.Value.DateTimeOffset.HasValue);
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value.DateTimeOffset);

        var clone = (DateAndOrTimeProperty)prop.Clone();
        Assert.IsTrue(clone.Value.DateTimeOffset.HasValue);
        Assert.AreEqual(group, clone.Group);
        Assert.AreEqual(dto, clone.Value.DateTimeOffset);

        Assert.AreNotSame(clone, prop);
    }
}

