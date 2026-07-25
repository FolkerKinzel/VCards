using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class OrgBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().SetPreferences());

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
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().SetIndexes());

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
    public void UnsetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Organizations.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Organizations.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Organizations.Edit((props, bl) => new OrgProperty(new Organization("Org")), true)
            .VCard;

        Assert.IsNotNull(vc.Organizations);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().Add("Contoso"));

    [TestMethod]
    public void AddTest2()
    {
        VCard vc = VCardBuilder.Create().Organizations.Add(new Organization("Contoso")).VCard;

        Assert.IsNotNull(vc.Organizations);
        OrgProperty? org = vc.Organizations.FirstOrDefault();
        Assert.IsNotNull(org);
        Assert.IsFalse(org.IsEmpty);
    }

    [TestMethod]
    public void AddTest3()
    => VCardBuilder.Create().Organizations.Add((Organization?)null!);

    [TestMethod]
    public void AddTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .Organizations.Add(new Organization("Contoso"),
                               group: vc => vc.NewGroup(),
                               displayName: (dn, org) => dn.Add(org.Value.Name))
            .Organizations.Add("The Bad Ones",
                                displayName: (dn, org) => dn.Add(org.Value.Name))
            .VCard;

        Assert.IsNotNull(vc.DisplayNames);
        Assert.AreEqual(2, vc.DisplayNames.Count());
    }

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().Clear());

    [TestMethod]
    public void RemoveTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new OrgBuilder().Remove(p => true));

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
