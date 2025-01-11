using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class RelationTests
{
    [TestMethod]
    public void ValueTest1()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNotNull(rel.ContactID.String);
        Assert.IsNull(rel.ContactID.Uri);
        Assert.IsNull(rel.VCard);
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = Relation.Create(ContactID.Create());
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNull(rel.VCard);
    }


    [TestMethod]
    public void ValueTest3()
    {
        var rel = Relation.Create(new VCard());
        Assert.IsNull(rel.ContactID);
        Assert.IsNotNull(rel.VCard);
    }

    [TestMethod]
    public void ValueTest4()
    {
        var rel = Relation.Create(ContactID.Create(new Uri("http://folker.de/")));
        Assert.IsNotNull(rel.ContactID);
        Assert.IsNotNull(rel.ContactID.Uri);
        Assert.IsNull(rel.VCard);
    }

    [TestMethod]
    public void SwitchTest2()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        rel.Switch(null!, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTest2()
    {
        var rel = Relation.Create(ContactID.Create("Hi"));
        _ = rel.Convert<int>(null!, null!);
    }

    [TestMethod]
    public void EqualsTest1()
    {
        var rel1 = Relation.Create(new VCard(setContactID: false));
        var rel2 = Relation.Create(new VCard(setContactID: false));
        var rel3 = Relation.Create(rel1.VCard!);

        Assert.IsTrue(rel1.Equals(rel1));
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.IsTrue(rel1 == rel1);
        Assert.IsFalse(rel1 != rel1);
#pragma warning restore CS1718 // Comparison made to same variable

        Assert.IsFalse(rel1.Equals(null));
        Assert.IsFalse(rel1 == null);
        Assert.IsFalse(null == rel1);

        Assert.IsTrue(rel1 != null);

        Assert.IsFalse(rel1.Equals(rel2));
        Assert.IsFalse(rel1 == rel2);
        Assert.IsTrue(rel1 != rel2);

        Assert.IsTrue(rel1.Equals(rel3));
        Assert.IsTrue(((object)rel1).Equals(rel3));
        Assert.IsTrue(rel1 == rel3);
        Assert.IsFalse(rel1 != rel3);

        Assert.AreEqual(rel1.GetHashCode(), rel3.GetHashCode());
    }

    [TestMethod]
    public void EqualsTest2()
    {
        var rel1 = Relation.Create(new VCard(setContactID: true));
        var rel2 = Relation.Create(new VCard(setContactID: true));
        var rel3 = Relation.Create(VCardBuilder.Create(setContactID: false)
                                               .ContactID.Set(rel1.VCard!.ContactID!.Value)
                                               .VCard);
        var rel4 = Relation.Create(new VCard(setContactID: false));


        Assert.IsTrue(rel1.Equals(rel1));
        Assert.IsFalse(rel1.Equals(rel2));
        Assert.IsTrue(rel1.Equals(rel3));
        Assert.AreEqual(rel1.GetHashCode(), rel3.GetHashCode());

        Assert.IsFalse(rel1.Equals(rel4));
    }

    [TestMethod]
    public void EqualsTest3()
    {
        var rel1 = Relation.Create(new VCard(setContactID: true));
        var rel2 = Relation.Create(ContactID.Create());
        var rel3 = Relation.Create(rel1.VCard!.ContactID!.Value);
        var rel4 = Relation.Create(new VCard(setContactID: false));
        var rel5 = Relation.Create(new VCard(setContactID: true));

        Assert.IsTrue(rel1.Equals(rel1));
        Assert.IsFalse(rel1.Equals(rel2));
        Assert.IsFalse(rel2.Equals(rel1));

        Assert.IsTrue(rel1.Equals(rel3));
        Assert.IsTrue(rel3.Equals(rel1));

        Assert.AreEqual(rel1.GetHashCode(), rel3.GetHashCode());

        Assert.IsFalse (rel1.Equals(rel4));
        Assert.IsFalse(rel4.Equals(rel1));

        Assert.IsFalse(rel1.Equals(rel5));
        Assert.IsFalse(rel5.Equals(rel1));

        Assert.IsFalse(rel2.Equals(rel4));
        Assert.IsFalse(rel4.Equals(rel2));
    }

    [TestMethod]
    public void EqualsTest4()
    {
        var rel = Relation.Create(VCardBuilder.Create(setContactID: false)
                                               .ContactID.Set(ContactID.Empty)
                                               .VCard);

        Assert.IsFalse(Relation.Empty.Equals(rel));
        Assert.IsTrue(Relation.Empty.Equals(Relation.Empty));
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
