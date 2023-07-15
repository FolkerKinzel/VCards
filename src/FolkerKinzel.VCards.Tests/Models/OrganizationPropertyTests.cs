using FolkerKinzel.VCards.Intls.Deserializers;

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
        var orgProp = new OrganizationProperty(expectedOrganizationName, expectedOrganizationalUnits, expectedGroup);

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


/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        var orgProp = new OrganizationProperty(vcfRow!, Enums.VCdVersion.V4_0);
Nach:
        var orgProp = new OrganizationProperty(vcfRow!, VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        var orgProp = new OrganizationProperty(vcfRow!, Enums.VCdVersion.V4_0);
Nach:
        var orgProp = new OrganizationProperty(vcfRow!, VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        var orgProp = new OrganizationProperty(vcfRow!, Enums.VCdVersion.V4_0);
Nach:
        var orgProp = new OrganizationProperty(vcfRow!, VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        var orgProp = new OrganizationProperty(vcfRow!, Enums.VCdVersion.V4_0);
Nach:
        var orgProp = new OrganizationProperty(vcfRow!, VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        var orgProp = new OrganizationProperty(vcfRow!, Enums.VCdVersion.V4_0);
Nach:
        var orgProp = new OrganizationProperty(vcfRow!, VCards.VCdVersion.V4_0);
*/
        var orgProp = new OrganizationProperty(vcfRow!, VCdVersion.V4_0);

        Assert.IsNotNull(orgProp.Value);
        Assert.AreEqual(expectedGroup, orgProp.Group);
        Assert.AreEqual(expectedOrganizationName, orgProp.Value.OrganizationName);
        CollectionAssert.AreEqual(expectedOrganizationalUnits, orgProp.Value.OrganizationalUnits);
    }


    [TestMethod]
    public void OrganizationPropertyTest3()
    {
        VcfRow row = VcfRow.Parse("ORG:", new VcfDeserializationInfo())!;

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        var prop = new OrganizationProperty(row, Enums.VCdVersion.V3_0);
Nach:
        var prop = new OrganizationProperty(row, VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        var prop = new OrganizationProperty(row, Enums.VCdVersion.V3_0);
Nach:
        var prop = new OrganizationProperty(row, VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        var prop = new OrganizationProperty(row, Enums.VCdVersion.V3_0);
Nach:
        var prop = new OrganizationProperty(row, VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        var prop = new OrganizationProperty(row, Enums.VCdVersion.V3_0);
Nach:
        var prop = new OrganizationProperty(row, VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        var prop = new OrganizationProperty(row, Enums.VCdVersion.V3_0);
Nach:
        var prop = new OrganizationProperty(row, VCards.VCdVersion.V3_0);
*/
        var prop = new OrganizationProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }
}
