using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TimeZoneBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new TimeZoneBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .TimeZones.Add((TimeZoneID?)null)
            .TimeZones.Add("Europe/Berlin")
            .TimeZones.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(2, vc.TimeZones.Count());
        Assert.AreEqual(100, vc.TimeZones.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.TimeZones.ElementAt(1)!.Parameters.Preference);

        builder.TimeZones.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.TimeZones.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.TimeZones.ElementAt(1)!.Parameters.Preference);

        builder.TimeZones.UnsetPreferences();
        Assert.IsTrue(vc.TimeZones.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new TimeZoneBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new TimeZoneBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .TimeZones.Add((TimeZoneID?)null)
            .TimeZones.Add("Europe/Berlin")
            .TimeZones.SetIndexes();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(2, vc.TimeZones.Count());
        Assert.AreEqual(null, vc.TimeZones.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.TimeZones.ElementAt(1)!.Parameters.Index);

        builder.TimeZones.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.TimeZones.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.TimeZones.ElementAt(1)!.Parameters.Index);

        builder.TimeZones.UnsetIndexes();
        Assert.IsTrue(vc.TimeZones.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new TimeZoneBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new TimeZoneBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().TimeZones.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new TimeZoneBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().TimeZones.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Edit((props, bl) => new TimeZoneProperty(TimeZoneID.Empty), true)
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TimeZoneBuilder().Add((TimeZoneID?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new TimeZoneBuilder().Add("Europe/Berlin");

    [TestMethod]
    public void AddTest3()
    {
        var vc = VCardBuilder.Create().TimeZones.Add((string?)null!).VCard;
        var timeZone = vc.TimeZones.FirstOrNull(skipEmptyItems: false);

        Assert.IsNotNull(timeZone);
        Assert.IsTrue(timeZone.IsEmpty);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TimeZoneBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TimeZoneBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TimeZoneBuilder().Equals((TimeZoneBuilder?)null));

        var builder = new TimeZoneBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TimeZoneBuilder().ToString());
}
