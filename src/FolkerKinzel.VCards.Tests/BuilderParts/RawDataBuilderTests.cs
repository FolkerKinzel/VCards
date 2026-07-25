using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class RawDataBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().SetPreferences());

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Photos.AddBytes(null)
            .Photos.AddBytes([1, 2, 3])
            .Photos.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Photos);
        Assert.AreEqual(2, vc.Photos.Count());
        Assert.AreEqual(100, vc.Photos.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Photos.ElementAt(1)!.Parameters.Preference);

        builder.Photos.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Photos.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Photos.ElementAt(1)!.Parameters.Preference);

        builder.Photos.UnsetPreferences();
        Assert.IsTrue(vc.Photos.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Photos.AddBytes(null)
            .Photos.AddBytes([1, 2, 3])
            .Photos.SetIndexes();

        VCard vc = builder.VCard;

        var property = vc.Photos;

        Assert.IsNotNull(property);
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.Photos.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.Photos.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Photos.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Photos.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Keys.Edit((props, bl) => new DataProperty(RawData.FromText("Passw")), true)
            .VCard;

        Assert.IsNotNull(vc.Keys);
    }

    [TestMethod]
    public void AddFileTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().AddFile("file"));

    [TestMethod]
    public void AddUriTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().AddUri(null));

    [TestMethod]
    public void AddBytesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().AddBytes(null));

    [TestMethod]
    public void AddTextTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().AddText(null));

    [TestMethod]
    public void ClearTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new RawDataBuilder().Equals((RawDataBuilder?)null));

        var builder = new RawDataBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new RawDataBuilder().ToString());

    [TestMethod]
    public void AddRawDataTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RawDataBuilder().AddRawData(RawData.FromText("text")));

    [TestMethod]
    public void AddRawDataTest2()
    {
        var vc = VCardBuilder.Create().Keys.AddRawData(null).VCard;
        var key = vc.Keys?.FirstOrDefault();
        Assert.IsNotNull(key);
        Assert.IsTrue(key.IsEmpty);
    }

    [TestMethod]
    public void AddUriTest2()
    {
        var vc = VCardBuilder.Create().Keys.AddUri(null).VCard;
        var key = vc.Keys?.FirstOrDefault();
        Assert.IsNotNull(key);
        Assert.IsTrue(key.IsEmpty);
    }

    [TestMethod]
    public void AddUriTest3()
    {
        var vc = VCardBuilder.Create().Keys.AddUri(new Uri("relative", UriKind.Relative)).VCard;
        var key = vc.Keys?.FirstOrDefault();
        Assert.IsNotNull(key);
        Assert.IsNotNull(key.Value.String);
    }
}
