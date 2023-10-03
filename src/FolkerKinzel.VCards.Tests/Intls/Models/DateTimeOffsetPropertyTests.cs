using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateTimeOffsetPropertyTests
{
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

