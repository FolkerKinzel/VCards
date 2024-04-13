namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new TextBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Notes.Add(null)
            .Notes.Add("One note")
            .Notes.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Notes);
        Assert.AreEqual(2, vc.Notes.Count());
        Assert.AreEqual(100, vc.Notes.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Notes.ElementAt(1)!.Parameters.Preference);

        builder.Notes.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Notes.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Notes.ElementAt(1)!.Parameters.Preference);

        builder.Notes.UnsetPreferences();
        Assert.IsTrue(vc.Notes.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new TextBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new TextBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Notes.Add(null)
            .Notes.Add("One note")
            .Notes.SetIndexes();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Notes);
        Assert.AreEqual(2, vc.Notes.Count());
        Assert.AreEqual(null, vc.Notes.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.Notes.ElementAt(1)!.Parameters.Index);

        builder.Notes.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Notes.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.Notes.ElementAt(1)!.Parameters.Index);

        builder.Notes.UnsetIndexes();
        Assert.IsTrue(vc.Notes.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new TextBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new TextBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Notes.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new TextBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Notes.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TextBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TextBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextBuilder().Equals((TextBuilder?)null));

        var builder = new TextBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextBuilder().ToString());
}
