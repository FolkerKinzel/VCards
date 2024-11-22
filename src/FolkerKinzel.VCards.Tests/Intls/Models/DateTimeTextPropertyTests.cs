using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        string now = "Früh morgens";

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

        Assert.IsNotNull(prop2?.Value.String);
        Assert.AreEqual(now, prop2.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);
        Assert.AreEqual(Data.Text, prop2.Parameters.DataType);
    }

    //[TestMethod]
    //public void DateTimeTextPropertyTest2()
    //{
    //    var prop = DateAndOrTimeProperty.FromText("   ");
    //    Assert.IsNull(prop.Value);
    //}

    [TestMethod]
    public void IsEmptyTest1()
    {
        var prop = new DateAndOrTimeProperty((string?)null);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.Value);
    }

    [TestMethod]
    public void IsEmptyTest2()
    {
        const string test = "test";
        var prop = new DateAndOrTimeProperty(test);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value);
        Assert.AreEqual(test, prop.Value.String);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop = new DateAndOrTimeProperty(DateAndOrTime.Empty);
        string s = prop.ToString();
        Assert.IsNotNull(s);
        Assert.IsTrue(s.Length > 0);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new DateAndOrTimeProperty("text");

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreSame(prop1.Value.String, prop2.Value.String);
        Assert.AreNotSame(prop1, prop2);
    }
}

