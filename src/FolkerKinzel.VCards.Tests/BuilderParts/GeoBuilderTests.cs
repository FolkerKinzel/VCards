using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GeoBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new GeoBuilder().SetPreferences();

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
        Assert.AreEqual(2, vc.GeoCoordinates.Count());
        Assert.AreEqual(100, vc.GeoCoordinates.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.GeoCoordinates.ElementAt(1)!.Parameters.Preference);

        builder.GeoCoordinates.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.GeoCoordinates.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.GeoCoordinates.ElementAt(1)!.Parameters.Preference);

        builder.GeoCoordinates.UnsetPreferences();
        Assert.IsTrue(vc.GeoCoordinates.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new GeoBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new GeoBuilder().SetIndexes();

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
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.GeoCoordinates.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.GeoCoordinates.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new GeoBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new GeoBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().GeoCoordinates.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new GeoBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().GeoCoordinates.Edit(null!, true);

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GeoBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GeoBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GeoBuilder().Remove(p => true);

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
