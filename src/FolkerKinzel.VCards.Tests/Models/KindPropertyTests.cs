namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class KindPropertyTests
{
    private const string GROUP = "myGroup";


    [TestMethod()]
    public void KindPropertyTest1()
    {
        const Enums.VCdKind kind = Enums.VCdKind.Application;

        var prop = new KindProperty(kind, GROUP);

        Assert.AreEqual(kind, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void KindPropertyTest2()
    {
        const Enums.VCdKind kind = Enums.VCdKind.Application;

        var prop = new KindProperty(kind, GROUP);

        var vcard = new VCard
        {
            Kind = prop
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

        Assert.IsNotNull(vcard.Kind);

        prop = vcard.Kind;

        Assert.AreEqual(kind, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }
}
