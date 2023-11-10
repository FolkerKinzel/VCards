using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class OrganizationPropertyTests
{
    private const string GROUP = "myGroup";

    [DataTestMethod()]
    [DataRow(GROUP, "ABC, Inc.", new string[] { "North American Division", "Marketing" })]
    [DataRow(null, "ABC, Inc.", new string[] { "North American Division", "Marketing" })]
    [DataRow(null, null, new string[] { "North American Division", "Marketing" })]
    [DataRow(null, "ABC, Inc.", new string[] { "Marketing" })]
    [DataRow(null, "ABC, Inc.", null)]
    public void OrganizationPropertyTest1(string? expectedGroup, string? expectedOrganizationName, string[] expectedOrganizationalUnits)
    {
        var orgProp = new OrgProperty(expectedOrganizationName, expectedOrganizationalUnits, expectedGroup);

        Assert.IsNotNull(orgProp.Value);
        Assert.AreEqual(expectedGroup, orgProp.Group);
        Assert.AreEqual(expectedOrganizationName, orgProp.Value.OrganizationName);
        CollectionAssert.AreEqual(expectedOrganizationalUnits, orgProp.Value.OrganizationalUnits);
    }


    [DataTestMethod()]
    [DataRow(GROUP + @".ORG:ABC\, Inc.;North American Division;Marketing",
        GROUP, "ABC, Inc.", new string[] { "North American Division", "Marketing" })]
    [DataRow(@"ORG:ABC\, Inc.;North American Division;Marketing",
        null, "ABC, Inc.", new string[] { "North American Division", "Marketing" })]
    [DataRow(@"ORG:;North American Division;Marketing",
        null, null, new string[] { "North American Division", "Marketing" })]
    [DataRow(@"ORG: ;  ;North American Division;;Marketing; ",
        null, null, new string[] { "North American Division", "Marketing" })]
    [DataRow(@"ORG:ABC\, Inc.;;Marketing",
        null, "ABC, Inc.", new string[] { "Marketing" })]
    [DataRow(@"ORG:ABC\, Inc.",
        null, "ABC, Inc.", null)]
    [DataRow(@"ORG:ABC\, Inc.;",
        null, "ABC, Inc.", null)]
    [DataRow(@"ORG:ABC\, Inc.;  ;",
        null, "ABC, Inc.", null)]
    public void OrganizationPropertyTest2(
        string s, string? expectedGroup, string? expectedOrganizationName, string[] expectedOrganizationalUnits)
    {
        var vcfRow = VcfRow.Parse(s, new VcfDeserializationInfo());
        Assert.IsNotNull(vcfRow);

        var orgProp = new OrgProperty(vcfRow!, VCdVersion.V4_0);

        Assert.IsNotNull(orgProp.Value);
        Assert.AreEqual(expectedGroup, orgProp.Group);
        Assert.AreEqual(expectedOrganizationName, orgProp.Value.OrganizationName);
        CollectionAssert.AreEqual(expectedOrganizationalUnits, orgProp.Value.OrganizationalUnits);
    }


    [TestMethod]
    public void OrganizationPropertyTest3()
    {
        VcfRow row = VcfRow.Parse("ORG:", new VcfDeserializationInfo())!;

        var prop = new OrgProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new OrgProperty("Contoso");
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
