using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class OrgBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new OrgBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Organizations.Add((string?)null)
            .Organizations.Add("Contoso")
            .Organizations.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Organizations);
        Assert.AreEqual(2, vc.Organizations.Count());
        Assert.AreEqual(100, vc.Organizations.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Organizations.ElementAt(1)!.Parameters.Preference);

        builder.Organizations.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Organizations.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Organizations.ElementAt(1)!.Parameters.Preference);

        builder.Organizations.UnsetPreferences();
        Assert.IsTrue(vc.Organizations.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new OrgBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new OrgBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Organizations.Add((string?)null)
            .Organizations.Add("Contoso")
            .Organizations.SetIndexes();

        VCard vc = builder.VCard;

        IEnumerable<OrgProperty?>? property = vc.Organizations;

        Assert.IsNotNull(property);
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.Organizations.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.Organizations.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new OrgBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new OrgBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Organizations.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new OrgBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Organizations.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new OrgBuilder().Add("Contoso");

    [TestMethod]
    public void AddTest2()
    {
        VCard vc = VCardBuilder.Create().Organizations.Add(new Organization("Contoso")).VCard;

        Assert.IsNotNull(vc.Organizations);
        var org = vc.Organizations.FirstOrDefault();
        Assert.IsNotNull(org);
        Assert.IsFalse(org.IsEmpty);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new OrgBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new OrgBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new OrgBuilder().Equals((OrgBuilder?)null));

        var builder = new OrgBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new OrgBuilder().ToString());
}
