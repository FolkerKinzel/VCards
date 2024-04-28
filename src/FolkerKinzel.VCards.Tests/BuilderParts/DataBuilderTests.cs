namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class DataBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new DataBuilder().SetPreferences();

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new DataBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new DataBuilder().SetIndexes();

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new DataBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new DataBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Photos.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new DataBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Photos.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddFileTest1() => new DataBuilder().AddFile("file");

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddUriTest1() => new DataBuilder().AddUri(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddBytesTest1() => new DataBuilder().AddBytes(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTextTest1() => new DataBuilder().AddText(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new DataBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new DataBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new DataBuilder().Equals((DataBuilder?)null));

        var builder = new DataBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DataBuilder().ToString());
}
