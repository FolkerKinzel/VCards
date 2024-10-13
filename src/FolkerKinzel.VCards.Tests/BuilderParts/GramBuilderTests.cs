using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GramBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new GramBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .GramGenders.Add(null)
            .GramGenders.Add(Gram.Feminine)
            .GramGenders.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.GramGenders);
        Assert.AreEqual(2, vc.GramGenders.Count());
        Assert.AreEqual(100, vc.GramGenders.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.GramGenders.ElementAt(1)!.Parameters.Preference);

        builder.GramGenders.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.GramGenders.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.GramGenders.ElementAt(1)!.Parameters.Preference);

        builder.GramGenders.UnsetPreferences();
        Assert.IsTrue(vc.GramGenders.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new GramBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new GramBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .GramGenders.Add(null)
            .GramGenders.Add(Gram.Neuter)
            .GramGenders.SetIndexes();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.GramGenders);
        Assert.AreEqual(2, vc.GramGenders.Count());
        Assert.AreEqual(null, vc.GramGenders.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.GramGenders.ElementAt(1)!.Parameters.Index);

        builder.GramGenders.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.GramGenders.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.GramGenders.ElementAt(1)!.Parameters.Index);

        builder.GramGenders.UnsetIndexes();
        Assert.IsTrue(vc.GramGenders.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new GramBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new GramBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().GramGenders.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new GramBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().GramGenders.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GramBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GramBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GramBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GramBuilder().Equals((TextBuilder?)null));

        var builder = new GramBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GramBuilder().ToString());
}
