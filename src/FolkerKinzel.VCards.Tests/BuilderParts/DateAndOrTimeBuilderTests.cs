namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class DateAndOrTimeBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new DateAndOrTimeBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().BirthDayViews.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new DateAndOrTimeBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new DateAndOrTimeBuilder().Add(new DateOnly(2023, 12, 4));

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new DateAndOrTimeBuilder().Add(2023, 12, 4);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest4() => new DateAndOrTimeBuilder().Add(12, 4);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest5() => new DateAndOrTimeBuilder().Add(DateTimeOffset.Now);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest6() => new DateAndOrTimeBuilder().Add(TimeOnly.FromDateTime(DateTime.Now));

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new DateAndOrTimeBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new DateAndOrTimeBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new DateAndOrTimeBuilder().Equals((DateAndOrTimeBuilder?)null));

        var builder = new DateAndOrTimeBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DateAndOrTimeBuilder().ToString());
}
