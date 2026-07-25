using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().SetPreferences());

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
        Assert.HasCount(2, vc.Notes);
        Assert.AreEqual(100, vc.Notes.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Notes.ElementAt(1)!.Parameters.Preference);

        builder.Notes.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Notes.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Notes.ElementAt(1)!.Parameters.Preference);

        builder.Notes.UnsetPreferences();
        Assert.IsTrue(vc.Notes.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().SetIndexes());

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
        Assert.HasCount(2, vc.Notes);
        Assert.IsNull(vc.Notes.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.Notes.ElementAt(1)!.Parameters.Index);

        builder.Notes.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Notes.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.Notes.ElementAt(1)!.Parameters.Index);

        builder.Notes.UnsetIndexes();
        Assert.IsTrue(vc.Notes.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Notes.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Notes.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .DisplayNames.Edit((props, bl) => new TextProperty("Duffy"), true)
            .VCard;

        Assert.IsNotNull(vc.DisplayNames);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().Add(null));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextBuilder().Remove(p => true));

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
