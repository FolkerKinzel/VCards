using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GenderBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new GenderBuilder().SetIndexes();

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
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.GenderViews.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.GenderViews.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new GenderBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new GenderBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().GenderViews.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new GenderBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().GenderViews.Edit(null!, true);

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GenderBuilder().Add(Gender.Male);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GenderBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GenderBuilder().Remove(p => true);

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
