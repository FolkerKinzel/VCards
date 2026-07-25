using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class DateAndOrTimeBuilderTests
{
    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .BirthDayViews.Add(null)
            .BirthDayViews.Add(2, 20)
            .BirthDayViews.SetIndexes();

        VCard vc = builder.VCard;

        var property = vc.BirthDayViews;

        Assert.IsNotNull(property);
        Assert.HasCount(2, property);
        Assert.IsNull(property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.BirthDayViews.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.BirthDayViews.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().BirthDayViews.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().BirthDayViews.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Edit((props, bl) => new DateAndOrTimeProperty(DateTime.Now), true)
            .VCard;

        Assert.IsNotNull(vc.BirthDayViews);
    }

    [TestMethod]
    public void AddTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(null));

    [TestMethod]
    public void AddTest2()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(new DateOnly(2023, 12, 4)));

    [TestMethod]
    public void AddTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(2023, 12, 4));

    [TestMethod]
    public void AddTest4() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(12, 4));

    [TestMethod]
    public void AddTest5()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(DateTimeOffset.Now));

    [TestMethod]
    public void AddTest6()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(TimeOnly.FromDateTime(DateTime.Now)));

    [TestMethod]
    public void AddTest7()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Add(DateTime.Now));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Clear());

    [TestMethod]
    public void RemoveTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new DateAndOrTimeBuilder().Remove(p => true));

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
