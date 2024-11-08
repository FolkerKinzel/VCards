namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class RelationTests
{
    [TestMethod]
    public void ValueTest1()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNotNull(rel.ContactID.String);
        Assert.IsNull(rel.ContactID.Uri);
        Assert.IsNull(rel.VCard);
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = Relation.Create(ContactID.Create());
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNull(rel.VCard);
    }


    [TestMethod]
    public void ValueTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var rel = Relation.Create(new VCard());
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.ContactID);
        Assert.IsNotNull(rel.VCard);
    }

    [TestMethod]
    public void ValueTest4()
    {
        var rel = Relation.Create(ContactID.Create(new Uri("http://folker.de/")));
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNotNull(rel.ContactID.Uri);
        Assert.IsNull(rel.VCard);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SwitchTest2()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        rel.Switch(null!, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ConvertTest2()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        _ = rel.Convert<int>(null!, null!);
    }

    //[TestMethod]
    //public void TryAsStringTest1() => Assert.IsFalse(new RelationProperty(ContactID.Create()).Value.TryAsString(out _));

    //[TestMethod]
    //public void TryAsStringTest2()
    //    => Assert.IsFalse(new RelationProperty(new VCard()).Value.TryAsString(out _));

    //[TestMethod]
    //public void TryAsStringTest3() => Assert.IsTrue(new RelationProperty(ContactID.Create("Hi")).Value.TryAsString(out _));

    //[TestMethod]
    //public void TryAsStringTest4() => Assert.IsTrue(new RelationProperty(ContactID.Create(new Uri("http://folker.de/"))).Value.TryAsString(out _));

    //[TestMethod]
    //public void TryAsStringTest5()
    //    => Assert.IsTrue(new RelationProperty(new VCard() { Organizations = new OrgProperty("Org") }).Value.TryAsString(out _));

    //[TestMethod]
    //public void TryAsStringTest6()
    //{
    //    VCard.SyncTestReset();
    //    VCard.RegisterApp(null);

    //    Assert.IsTrue(new RelationProperty(new VCard() { NameViews = new NameProperty("Folker") }).Value.TryAsString(out _));
    //}

    //[TestMethod]
    //public void TryAsStringTest7()
    //{
    //    Assert.IsTrue(new RelationProperty(
    //            new VCard()
    //            {
    //                DisplayNames = new TextProperty("Folker")
    //            }).Value.TryAsString(out _));
    //}

    //[TestMethod]
    //public void TryAsStringTest8()
    //{
    //    Assert.IsTrue(new RelationProperty(
    //        new VCard()
    //        {
    //            Organizations = new OrgProperty("Org"),
    //            DisplayNames = [],
    //            NameViews = []
    //        }).Value.TryAsString(out _));
    //}

    //[TestMethod]
    //public void TryAsStringTest9()
    //{
    //    Assert.IsFalse(new RelationProperty(
    //        new VCard()
    //        {
    //            Organizations = [],
    //            DisplayNames = [],
    //            NameViews = []
    //        }).Value.TryAsString(out _));
    //}
}
