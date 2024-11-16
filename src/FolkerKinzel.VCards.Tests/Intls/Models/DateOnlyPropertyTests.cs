using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateOnlyPropertyTests
{
    [TestMethod]
    public void DateOnlyPropertyTest1()
    {
        var dateOnly = DateOnly.FromDateTime(DateTime.Now);
        var prop = new DateAndOrTimeProperty(dateOnly);

        Assert.IsNotNull(prop);
        Assert.IsTrue(prop.Value.DateOnly.HasValue);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(dateOnly, prop.Value!.DateOnly);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new DateAndOrTimeProperty(DateAndOrTime.Create(3, 15));

        var prop2 = (DateAndOrTimeProperty)prop1.Clone();

        Assert.AreEqual(prop1.Value.DateOnly, prop2.Value.DateOnly);
        Assert.AreNotSame(prop1, prop2);
    }
}

