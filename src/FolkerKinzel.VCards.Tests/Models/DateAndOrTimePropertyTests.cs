using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

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
    public void CreateTest1()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY:T102200-0800
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeOffsetProperty));
    }

    [TestMethod]
    public void CreateTest2()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:102200-0800
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeOffsetProperty));
    }

    [TestMethod]
    public void CreateTest3()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        BDAY;VALUE=TIME:bla
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];
        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.BirthDayViews);
        DateAndOrTimeProperty? bdayProp = vcard.BirthDayViews!.First();
        Assert.IsInstanceOfType(bdayProp, typeof(DateTimeTextProperty));
    }
}
