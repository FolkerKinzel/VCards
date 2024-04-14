using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Tests.Models;

[TestClass]
public class OrganizationTests
{
    [TestMethod]
    public void ToStringTest1()
    {
        var org = new Organization(["  "]);
        string s = org.ToString();

        Assert.IsTrue(org.IsEmpty);
        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);
    }

    [DataTestMethod]
    [DataRow("org", "äöü", true)]
    [DataRow("äöü", "", true)]
    [DataRow("", "äöü", true)]
    [DataRow("", "", false)]
    [DataRow("äöü", "äöü", true)]
    [DataRow("äöü", "unit", true)]
    [DataRow("org", "unit", false)]
    public void NeedsToBeQpEncodedTest1(string org, string units, bool expected)
    {
        var list = new List<string>() { org, units };
        var organization = new Organization(list);
        Assert.AreEqual(expected, organization.NeedsToBeQpEncoded());
    }

    [TestMethod]
    public void CtorTest()
    {
        var org = new Organization([]);
        Assert.IsNotNull(org);
        Assert.IsTrue(org.IsEmpty);
    }
}
