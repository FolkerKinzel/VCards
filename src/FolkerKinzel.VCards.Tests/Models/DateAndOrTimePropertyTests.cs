using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Tests;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Tests;

internal class DateAndOrTimePropertyDerived : DateAndOrTimeProperty
{
    public DateAndOrTimePropertyDerived(DateAndOrTimeProperty prop) : base(prop)
    {
    }

    public DateAndOrTimePropertyDerived(ParameterSection parameters, string? group) 
        : base(parameters, group)
    {
    }

    public override object Clone() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

[TestClass]
public class DateAndOrTimePropertyTests
{
    private class TestIEnumerable : DateAndOrTimeProperty
    {
        public TestIEnumerable() : base(DateAndOrTimeProperty.FromDateTime(DateTimeOffset.Now)) { }
        public override object Clone() => throw new NotImplementedException();
        protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    public void IEnumerableTest1() => Assert.AreEqual(1, new TestIEnumerable().AsWeakEnumerable().Count());


    [TestMethod]
    public void ParseTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY:T102200-0800
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf).First();
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeOffsetProperty));
    }

    [TestMethod]
    public void ParseTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:102200-0800
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf).First();
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeOffsetProperty));
    }

    [TestMethod]
    public void ParseTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:bla
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf).First();
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeTextProperty));
    }

    [TestMethod]
    public void IsEmptyTest1()
    {
        DateAndOrTimeProperty prop = new DateAndOrTimePropertyDerived(DateAndOrTimeProperty.FromDate(2023, 10, 11));
        Assert.IsTrue(prop.IsEmpty);
    }

    [TestMethod]
    public void ValueTest1()
    {
        VCardProperty prop = DateAndOrTimeProperty.FromDate(2023, 10, 14);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }

    [TestMethod]
    public void ValueTest2()
    {
        VCardProperty prop = DateAndOrTimeProperty.FromDateTime(DateTime.Now);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }


    [TestMethod]
    public void ValueTest3()
    {
        VCardProperty prop = DateAndOrTimeProperty.FromTime(14, 24);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }

    [TestMethod]
    public void ValueTest4()
    {
        VCardProperty prop = DateAndOrTimeProperty.FromText("Midnight");
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType(prop.Value, typeof(DateAndOrTime));
    }


    [TestMethod]
    public void FromDateTest1()
    {
        const string group = "Group";
        var prop = DateAndOrTimeProperty.FromDate(2, 29, group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value.DateOnly);
        Assert.IsFalse(prop.Value.DateOnly.Value.HasYear());
        Assert.AreEqual(2, prop.Value.DateOnly.Value.Month);
        Assert.AreEqual(29, prop.Value.DateOnly.Value.Day);
    }

}
