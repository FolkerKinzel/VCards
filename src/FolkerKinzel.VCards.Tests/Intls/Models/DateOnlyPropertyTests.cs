using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class DateOnlyPropertyTests
{
    [TestMethod]
    public void DateOnlyPropertyTest1()
    {
        var dateOnly = DateOnly.FromDateTime(DateTime.Now);
        var prop = DateAndOrTimeProperty.FromDate(dateOnly);

        Assert.IsNotNull(prop);
        Assert.IsInstanceOfType(prop, typeof(DateOnlyProperty));
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(dateOnly, prop.Value!.DateOnly);
    }
}

