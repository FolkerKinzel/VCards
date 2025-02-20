using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class StringCollectionBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new StringCollectionBuilder().SetPreferences();

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new StringCollectionBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new StringCollectionBuilder().SetIndexes();

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new StringCollectionBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new StringCollectionBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().NickNames.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new StringCollectionBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().NickNames.Edit(null!, true);

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new StringCollectionBuilder().Add((string?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new StringCollectionBuilder().Add([]);

    [TestMethod]
    public void AddTest3()
    {
        var vc = VCardBuilder.Create().NickNames.Add((string[]?)null).VCard;
        var nickName = vc.NickNames.FirstOrNull(skipEmptyItems:false);
        Assert.IsNotNull(nickName);
        Assert.IsTrue(nickName.IsEmpty);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new StringCollectionBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new StringCollectionBuilder().Remove(p => true);

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
