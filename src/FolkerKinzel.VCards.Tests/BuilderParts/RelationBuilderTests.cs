using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class RelationBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().SetPreferences());

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Relations.Add("")
            .Relations.Add("Goofy")
            .Relations.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.HasCount(2, vc.Relations);
        Assert.AreEqual(100, vc.Relations.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Relations.ElementAt(1)!.Parameters.Preference);

        builder.Relations.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Relations.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Relations.ElementAt(1)!.Parameters.Preference);

        builder.Relations.UnsetPreferences();
        Assert.IsTrue(vc.Relations.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().SetIndexes());

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Relations.Add("")
            .Relations.Add("Goofy")
            .Relations.SetIndexes();

        VCard vc = builder.VCard;

        var property = vc.Relations;

        Assert.IsNotNull(property);
        Assert.HasCount(2, property);
        Assert.IsNull(property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.Relations.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.Relations.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Relations.Edit(null!));

    [TestMethod]
    public void EditTest3() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Relations.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Edit(
                (props, bl) => new RelationProperty(Relation.Create(ContactID.Create("Susi"))),
                                                    true)
            .VCard;

        Assert.IsNotNull(vc.Relations);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Add(Guid.Empty));

    [TestMethod]
    public void AddTest2()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Add(""));

    [TestMethod]
    public void AddTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Add((Uri?)null));

    [TestMethod]
    public void AddTest4() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Add((VCard?)null));

    [TestMethod]
    public void AddTest5()
    {
        var vc = VCardBuilder.Create().Relations.Add((Relation?)null).VCard;

        var relation = vc.Relations.FirstOrNull(skipEmptyItems: false);
        Assert.IsNotNull(relation);
        Assert.IsTrue(relation.IsEmpty);
    }

    [TestMethod]
    public void AddTest6()
    {
        var vc = VCardBuilder.Create().Relations.Add((ContactID?)null).VCard;

        var relation = vc.Relations.FirstOrNull(skipEmptyItems: false);
        Assert.IsNotNull(relation);
        Assert.IsTrue(relation.IsEmpty);
    }

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new RelationBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new RelationBuilder().Equals((RelationBuilder?)null));

        var builder = new RelationBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new RelationBuilder().ToString());
}
