namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class RelationUriPropertyTests
{
    private const string GROUP = "myGroup";


    [TestMethod()]
    public void RelationUriPropertyTest1()
    {
        const Enums.RelationTypes relation = Enums.RelationTypes.Acquaintance;
        var uri = new Uri("http://test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(uri, relation, GROUP);

        Assert.AreEqual(uri, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Enums.VCdDataType.Uri, prop.Parameters.DataType);
    }


    [TestMethod()]
    public void RelationUriPropertyTest2()
    {
        const Enums.RelationTypes relation = Enums.RelationTypes.Acquaintance;
        var uri = new Uri("http://test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(uri, relation, GROUP);

        var vcard = new VCard
        {
            Relations = prop
        };


/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V4_0);
*/
        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Enums.VCdDataType.Uri, prop.Parameters.DataType);
    }


    [TestMethod()]
    public void RelationUriPropertyTest3()
    {
        const Enums.RelationTypes relation = Enums.RelationTypes.Agent;
        var uri = new Uri("http://test.ääh.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(uri, relation, GROUP);

        var vcard = new VCard
        {
            Relations = prop
        };


/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/
        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Enums.VCdDataType.Uri, prop.Parameters.DataType);
    }

    [TestMethod()]
    public void RelationUriPropertyTest4()
    {
        const Enums.RelationTypes relation = Enums.RelationTypes.Agent;
        var uri = new Uri("cid:test.com/", UriKind.Absolute);

        var prop = new RelationUriProperty(uri, relation, GROUP);

        var vcard = new VCard
        {
            Relations = prop
        };


/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);
Nach:
        string s = vcard.ToVcfString(VCards.VCdVersion.V2_1);
*/
        string s = vcard.ToVcfString(VCdVersion.V2_1);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Relations);

        prop = vcard.Relations!.First() as RelationUriProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(uri, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(relation, prop.Parameters.RelationType);
        Assert.AreEqual(Enums.VCdDataType.Uri, prop.Parameters.DataType);
    }

}
