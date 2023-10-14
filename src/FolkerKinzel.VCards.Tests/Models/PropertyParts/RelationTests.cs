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
        rel.Switch(s => rel = null, null, null, null);
        Assert.IsNull(rel);
    }

    [TestMethod]
    public void MatchTest1()
    {
        const int expected = 42;
        var rel = new Relation("Hi");

        int result = rel.Match(s => expected, null, null, null);
        Assert.AreEqual(expected, result);
    }

}
