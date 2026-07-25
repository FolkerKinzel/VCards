using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GenderBuilderTests
{
    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .GenderViews.Add((Gender?)null)
            .GenderViews.Add(Sex.Female)
            .GenderViews.SetIndexes();

        VCard vc = builder.VCard;

        IEnumerable<GenderProperty?>? property = vc.GenderViews;

        Assert.IsNotNull(property);
        Assert.HasCount(2, property);
        Assert.IsNull(property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.GenderViews.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.GenderViews.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GenderViews.Edit(null!));

    [TestMethod]
    public void EditTest3() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GenderViews.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Edit((props, bl) => new GenderProperty(Gender.Male), true)
            .VCard;

        Assert.IsNotNull(vc.GenderViews);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().Add(Gender.Male));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().Clear());

    [TestMethod]
    public void RemoveTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GenderBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GenderBuilder().Equals((GenderBuilder?)null));

        var builder = new GenderBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GenderBuilder().ToString());
}
