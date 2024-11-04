using FolkerKinzel.VCards.Models;
namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class RelationUuidPropertyTests
{
    [TestMethod]
    public void IsEmptyTest1()
    {
        var prop = RelationProperty.FromGuid(Guid.Empty);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNull(prop.Value);
    }

    [TestMethod]
    public void IsEmptyTest2()
    {
        var prop = RelationProperty.FromGuid(Guid.NewGuid());
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsNotNull(prop.Value!.ContactID);
    }
}
