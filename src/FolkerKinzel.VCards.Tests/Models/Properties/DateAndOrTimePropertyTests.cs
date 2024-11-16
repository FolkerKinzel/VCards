using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class DateAndOrTimePropertyTests
{
    [TestMethod]
    public void IEnumerableTest1() => Assert.AreEqual(1, new DateAndOrTimeProperty(DateTime.Now).AsWeakEnumerable().Count());


    [TestMethod]
    public void ParseTest1()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY:T102200-0800
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsNotNull(bdayProp);
        Assert.IsTrue(bdayProp.Value.DateTimeOffset.HasValue);
    }

    [TestMethod]
    public void ParseTest2()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:102200-0800
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsNotNull(bdayProp);
        Assert.IsTrue(bdayProp.Value.DateTimeOffset.HasValue);
    }

    [TestMethod]
    public void ParseTest3()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:bla
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsNotNull(bdayProp);
        Assert.IsTrue(bdayProp.IsEmpty);
    }

    [TestMethod]
    public void ValueTest1()
    {
        VCardProperty prop = new DateAndOrTimeProperty(new DateTime(2023, 10, 14));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }

    [TestMethod]
    public void ValueTest3()
    {
        VCardProperty prop = new DateAndOrTimeProperty(DateAndOrTime.Create(14, 24));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }

    [TestMethod]
    public void ValueTest4()
    {
        VCardProperty prop = new DateAndOrTimeProperty("Midnight");
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop = new DateAndOrTimeProperty(DateTimeOffset.Now);
        Assert.IsNotNull(prop.ToString());
    }
}
