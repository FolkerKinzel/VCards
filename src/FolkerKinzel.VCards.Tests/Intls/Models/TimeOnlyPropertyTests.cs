using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class TimeOnlyPropertyTests
{
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
    public void AppendValueTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOpts.Default, null);

        var dto = TimeOnly.FromDateTime(DateTime.Now);
        var prop = new DateAndOrTimeProperty(dto);

        prop.AppendValue(serializer);
        Assert.AreNotEqual(0, serializer.Builder.Length);
    }
}

