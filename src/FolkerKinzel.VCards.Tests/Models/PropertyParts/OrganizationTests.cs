namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class OrganizationTests
{
    [TestMethod]
    public void ToStringTest1()
    {
        var org = new Organization(new List<string> { "  " });
        string s = org.ToString();

        Assert.IsTrue(org.IsEmpty);
        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);

    }
}
