using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class RelationTests
{
    [TestMethod]
    public void ValueTest1()
    {
        var rel = new Relation("Hi");
        Assert.IsNotNull(rel.Value);
        Assert.IsNotNull(rel.String);
        Assert.IsNull(rel.Guid);
        Assert.IsNull(rel.VCard);
        Assert.IsNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = new Relation(Guid.NewGuid());
        Assert.IsNotNull(rel.Value);
        Assert.IsNull(rel.String);
        Assert.IsNotNull(rel.Guid);
        Assert.IsNull(rel.VCard);
        Assert.IsNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var rel = new Relation(new VCard());
        Assert.IsNotNull(rel.Value);
        Assert.IsNull(rel.String);
        Assert.IsNull(rel.Guid);
        Assert.IsNotNull(rel.VCard);
        Assert.IsNull(rel.Uri);
    }

    [TestMethod]
    public void ValueTest4()
    {
        var rel = new Relation(new Uri("http://folker.de/"));
        Assert.IsNotNull(rel.Value);
        Assert.IsNull(rel.String);
        Assert.IsNull(rel.Guid);
        Assert.IsNull(rel.VCard);
        Assert.IsNotNull(rel.Uri);
    }

    [TestMethod]
    public void SwitchTest1()
    {
        var rel = new Relation("Hi");
        rel.Switch(s => rel = null, null!, null!, null!);
        Assert.IsNull(rel);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SwitchTest2()
    {
        var rel = new Relation("Hi");
        rel.Switch(null!, null!, null!, null!);
    }

    [TestMethod]
    public void ConvertTest1()
    {
        const int expected = 42;
        var rel = new Relation("Hi");

        int result = rel.Convert(s => expected, null!, null!, null!);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ConvertTest2()
    {
        const int expected = 42;
        var rel = new Relation("Hi");

        int result = rel.Convert<int>(null!, null!, null!, null!);
        Assert.AreEqual(expected, result);
    }


    [TestMethod]
    public void TryAsStringTest1() => Assert.IsFalse(RelationProperty.FromGuid(Guid.NewGuid()).Value!.TryAsString(out _));

    [TestMethod]
    public void TryAsStringTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsFalse(RelationProperty.FromVCard(new VCard()).Value!.TryAsString(out _));
    }

    [TestMethod]
    public void TryAsStringTest3() => Assert.IsTrue(RelationProperty.FromText("Hi").Value!.TryAsString(out _));

    [TestMethod]
    public void TryAsStringTest4() => Assert.IsTrue(RelationProperty.FromUri(new Uri("http://folker.de/")).Value!.TryAsString(out _));

    [TestMethod]
    public void TryAsStringTest5()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsTrue(RelationProperty.FromVCard(new VCard() { Organizations = new OrgProperty("Org") }).Value!.TryAsString(out _));
    }

    [TestMethod]
    public void TryAsStringTest6()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsTrue(RelationProperty.FromVCard(new VCard() { NameViews = new NameProperty("Folker") }).Value!.TryAsString(out _));
    }

    [TestMethod]
    public void TryAsStringTest7()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsTrue(RelationProperty.FromVCard(new VCard() { DisplayNames = new TextProperty("Folker") }).Value!.TryAsString(out _));
    }

    [TestMethod]
    public void TryAsStringTest8()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsTrue(RelationProperty.FromVCard(
            new VCard()
            {
                Organizations = new OrgProperty("Org"),
                DisplayNames = Array.Empty<TextProperty>(),
                NameViews = Array.Empty<NameProperty>()
            }).Value!.TryAsString(out _));
    }

    [TestMethod]
    public void TryAsStringTest9()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsFalse(RelationProperty.FromVCard(
            new VCard()
            {
                Organizations = Array.Empty<OrgProperty>(),
                DisplayNames = Array.Empty<TextProperty>(),
                NameViews = Array.Empty<NameProperty>()
            }).Value!.TryAsString(out _));
    }
}
