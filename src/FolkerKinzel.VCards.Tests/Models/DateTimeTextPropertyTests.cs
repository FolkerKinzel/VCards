using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Models;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        string now = "Früh morgens";

        var prop = DateAndOrTimeProperty.Create(now, GROUP);

        var vcard = new VCard
        {
            BirthDayViews = prop
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

        Assert.IsNotNull(vcard.BirthDayViews);

        prop = vcard.BirthDayViews!.First() as DateTimeTextProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(now, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(Enums.VCdDataType.Text, prop.Parameters.DataType);
    }

    [TestMethod]
    public void DateTimeTextPropertyTest2()
    {
        var prop = DateAndOrTimeProperty.Create("   ");
        Assert.IsNull(prop.Value);
    }
}
