using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class DateAndOrTimePropertyTests
{
    private const string GROUP = "myGroup";

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
        Assert.IsFalse(bdayProp.IsEmpty);
        Assert.AreEqual("bla", bdayProp.Value.String);
    }

    [TestMethod]
    public void ValueTest1()
    {
        VCardProperty prop = new DateAndOrTimeProperty(new DateTime(2023, 10, 14));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<DateAndOrTime>(prop.Value);
    }

    [TestMethod]
    public void ValueTest4()
    {
        VCardProperty prop = new DateAndOrTimeProperty("Midnight");
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<DateAndOrTime>(prop.Value);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop = new DateAndOrTimeProperty(DateTimeOffset.Now);
        Assert.IsNotNull(prop.ToString());
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var prop = new DateAndOrTimeProperty(DateAndOrTime.Empty);
        string s = prop.ToString();
        Assert.IsNotNull(s);
        Assert.IsTrue(s.Length > 0);
    }

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
    public void CloneTest1()
    {
        var dto = TimeOnly.FromDateTime(DateTime.Now);
        const string group = "gr1";

        var prop = new DateAndOrTimeProperty(dto, group);

        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value.TimeOnly);

        var clone = (DateAndOrTimeProperty)prop.Clone();
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value.TimeOnly);

        Assert.AreNotSame(clone, prop);
    }

    [TestMethod]
    public void CloneTest2()
    {
        var prop1 = new DateAndOrTimeProperty(DateAndOrTime.Create(new DateOnly(4, 3, 15)));

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreEqual(prop1.Value.DateOnly, prop2.Value.DateOnly);
        Assert.AreNotSame(prop1, prop2);
    }

    [TestMethod]
    public void CloneTest3()
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

    [TestMethod]
    public void CloneTest4()
    {
        var prop1 = new DateAndOrTimeProperty("text");

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreSame(prop1.Value.String, prop2.Value.String);
        Assert.AreNotSame(prop1, prop2);
    }

    [TestMethod]
    public void PrepareForVcfSerializationTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOpts.Default, null);

        var dto = TimeOnly.FromDateTime(DateTime.Now);
        var prop = new DateAndOrTimeProperty(dto);

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(Data.Time, prop.Parameters.DataType);
    }

    [TestMethod]
    public void PrepareForVcfSerializationTest2()
    {
        var vc = VCardBuilder.Create().BirthDayViews.Add(DateAndOrTime.Empty, p => p.DataType = Data.Text).VCard;
        string vcf = vc.ToVcfString(options: VcfOpts.Default.Set(VcfOpts.WriteEmptyProperties));

        StringAssert.Contains(vcf, "BDAY:\r\n");
    }

    [TestMethod]
    public void AppendValueTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOpts.Default, null);

        var dto = TimeOnly.FromDateTime(DateTime.Now);
        var prop = new DateAndOrTimeProperty(dto);

        prop.AppendValue(serializer);
        Assert.AreNotEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void DateOnlyPropertyTest1()
    {
        var dateOnly = DateOnly.FromDateTime(DateTime.Now);
        var prop = new DateAndOrTimeProperty(dateOnly);

        Assert.IsNotNull(prop);
        Assert.IsTrue(prop.Value.DateOnly.HasValue);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(dateOnly, prop.Value.DateOnly);
    }

    [TestMethod]
    public void DateTimeOffsetPropertyTest1()
    {
        var prop = new DateAndOrTimeProperty(DateTimeOffset.UtcNow, GROUP);

        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value.DateTimeOffset);
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
        Assert.AreEqual(now, prop2.Value);
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
}
