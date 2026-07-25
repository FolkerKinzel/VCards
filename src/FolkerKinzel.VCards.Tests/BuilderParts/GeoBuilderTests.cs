using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GeoBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().SetPreferences());

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .GeoCoordinates.Add(null)
            .GeoCoordinates.Add(42, 42)
            .GeoCoordinates.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.HasCount(2, vc.GeoCoordinates);
        Assert.AreEqual(100, vc.GeoCoordinates.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.GeoCoordinates.ElementAt(1)!.Parameters.Preference);

        builder.GeoCoordinates.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.GeoCoordinates.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.GeoCoordinates.ElementAt(1)!.Parameters.Preference);

        builder.GeoCoordinates.UnsetPreferences();
        Assert.IsTrue(vc.GeoCoordinates.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .GeoCoordinates.Add(null)
            .GeoCoordinates.Add(42, 42)
            .GeoCoordinates.SetIndexes();

        VCard vc = builder.VCard;

        var property = vc.GeoCoordinates;

        Assert.IsNotNull(property);
        Assert.HasCount(2, property);
        Assert.IsNull(property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.GeoCoordinates.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.GeoCoordinates.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GeoCoordinates.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GeoCoordinates.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Edit((props, bl) => new GeoProperty(new GeoCoordinate(42, 42)), true)
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().Add(null));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GeoBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GeoBuilder().Equals((GeoBuilder?)null));

        var builder = new GeoBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GeoBuilder().ToString());
}
