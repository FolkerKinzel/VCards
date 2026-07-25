using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextViewBuilderTests
{
    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .BirthPlaceViews.Add(null)
            .BirthPlaceViews.Add("Berlin")
            .BirthPlaceViews.SetIndexes();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.AreEqual(2, vc.BirthPlaceViews.Count());
        Assert.AreEqual(null, vc.BirthPlaceViews.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.BirthPlaceViews.ElementAt(1)!.Parameters.Index);

        builder.BirthPlaceViews.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.BirthPlaceViews.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.BirthPlaceViews.ElementAt(1)!.Parameters.Index);

        builder.BirthPlaceViews.UnsetIndexes();
        Assert.IsTrue(vc.BirthPlaceViews.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().BirthPlaceViews.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().BirthPlaceViews.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthPlaceViews.Edit((props, bl) => new TextProperty("Entenhausen"), true)
            .VCard;

        Assert.IsNotNull(vc.BirthPlaceViews);
    }

    [TestMethod]
    public void AddTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().Add(null));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().Clear());

    [TestMethod]
    public void RemoveTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextViewBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextViewBuilder().Equals((TextViewBuilder?)null));

        var builder = new TextViewBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextViewBuilder().ToString());
}
