using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class RelationBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new RelationBuilder().SetPreferences();

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
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual(100, vc.Relations.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Relations.ElementAt(1)!.Parameters.Preference);

        builder.Relations.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Relations.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Relations.ElementAt(1)!.Parameters.Preference);

        builder.Relations.UnsetPreferences();
        Assert.IsTrue(vc.Relations.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new RelationBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new RelationBuilder().SetIndexes();

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
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.Relations.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.Relations.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new RelationBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new RelationBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Relations.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new RelationBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Relations.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Edit((props, bl) => new RelationProperty(Relation.Create(ContactID.Create("Susi"))), true)
            .VCard;

        Assert.IsNotNull(vc.Relations);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new RelationBuilder().Add(Guid.Empty);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new RelationBuilder().Add("");

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new RelationBuilder().Add((Uri?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest4() => new RelationBuilder().Add((VCard?)null);

    [TestMethod]
    public void AddTest5()
    {
        var vc = VCardBuilder.Create().Relations.Add((Relation?)null).VCard;

        var relation = vc.Relations.FirstOrNull(skipEmptyItems:false);
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
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new RelationBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new RelationBuilder().Remove(p => true);

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
