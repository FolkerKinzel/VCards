namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextViewBuilderTests
{


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new TextViewBuilder().SetIndexes();

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new TextViewBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new TextViewBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().BirthPlaceViews.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new TextViewBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().BirthPlaceViews.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TextViewBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextViewBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TextViewBuilder().Remove(p => true);

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
