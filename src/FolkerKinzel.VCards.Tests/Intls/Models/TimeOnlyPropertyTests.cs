using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class TimeOnlyPropertyTests
{
    [TestMethod]
    public void CloneTest1()
    {
        var dto = TimeOnly.FromDateTime(DateTime.Now);
        const string group = "gr1";

        var prop = DateAndOrTimeProperty.FromTime(dto, group);

        Assert.IsInstanceOfType(prop, typeof(TimeOnlyProperty));
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value?.TimeOnly);
        Assert.AreEqual(Data.Time, prop.Parameters.DataType);

        var clone = (DateAndOrTimeProperty)prop.Clone();
        Assert.IsInstanceOfType(prop, typeof(TimeOnlyProperty));
        Assert.AreEqual(group, prop.Group);
        Assert.AreEqual(dto, prop.Value?.TimeOnly);
        Assert.AreEqual(Data.Time, prop.Parameters.DataType);

        Assert.AreNotSame(clone, prop);
    }

    [TestMethod]
    public void PrepareForVcfSerializationTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, Opts.Default, null);

        var dto = TimeOnly.FromDateTime(DateTime.Now);
        var prop = DateAndOrTimeProperty.FromTime(dto);

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(Data.Time, prop.Parameters.DataType);
    }

    [TestMethod]
    public void AppendValueTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, Opts.Default, null);

        var dto = TimeOnly.FromDateTime(DateTime.Now);
        var prop = DateAndOrTimeProperty.FromTime(dto);

        prop.AppendValue(serializer);
        Assert.AreNotEqual(0, serializer.Builder.Length);
    }
}

