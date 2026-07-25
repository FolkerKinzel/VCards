using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class StringCollectionBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().SetPreferences());

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .NickNames.Add("")
            .NickNames.Add("Goofy")
            .NickNames.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.NickNames);
        Assert.AreEqual(2, vc.NickNames.Count());
        Assert.AreEqual(100, vc.NickNames.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.NickNames.ElementAt(1)!.Parameters.Preference);

        builder.NickNames.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.NickNames.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.NickNames.ElementAt(1)!.Parameters.Preference);

        builder.NickNames.UnsetPreferences();
        Assert.IsTrue(vc.NickNames.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    public void UnsetPreferencesTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .NickNames.Add("")
            .NickNames.Add("Goofy")
            .NickNames.SetIndexes();

        VCard vc = builder.VCard;

        IEnumerable<StringCollectionProperty?>? property = vc.NickNames;

        Assert.IsNotNull(property);
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.NickNames.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.NickNames.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().NickNames.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().NickNames.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .NickNames.Edit((props, bl) => new StringCollectionProperty("Goofy"), true)
            .VCard;

        Assert.IsNotNull(vc.NickNames);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Add((string?)null));

    [TestMethod]
    public void AddTest2()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Add([]));

    [TestMethod]
    public void AddTest3()
    {
        var vc = VCardBuilder.Create().NickNames.Add((string[]?)null).VCard;
        var nickName = vc.NickNames.FirstOrNull(skipEmptyItems: false);
        Assert.IsNotNull(nickName);
        Assert.IsTrue(nickName.IsEmpty);
    }

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new StringCollectionBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new StringCollectionBuilder().Equals((StringCollectionBuilder?)null));

        var builder = new StringCollectionBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new StringCollectionBuilder().ToString());
}
