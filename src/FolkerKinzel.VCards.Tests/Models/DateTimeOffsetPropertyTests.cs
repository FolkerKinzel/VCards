using System.Collections;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class DateTimeOffsetPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeOffsetPropertyTest1()
    {

        var prop = DateAndOrTimeProperty.Create(DateTimeOffset.UtcNow, GROUP) as DateTimeOffsetProperty;

        Assert.IsNotNull(prop);
        Assert.IsNotNull(prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.AreNotEqual(DateTimeOffset.MinValue, prop.Value);

        DateTimeOffset dto = prop.Value;

        VCardProperty vcProp = prop;
        Assert.AreEqual(vcProp.Value, dto);
    }


    [TestMethod()]
    public void DateTimeOffsetPropertyTest2()
    {
        var now = new DateTimeOffset(2021, 4, 4, 12, 41, 2, TimeSpan.FromHours(2));

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

        prop = vcard.BirthDayViews!.First() as DateTimeOffsetProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(now, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);

        Assert.AreEqual(Enums.VCdDataType.DateAndOrTime, prop.Parameters.DataType);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest3()
    {
        var prop = DateAndOrTimeProperty.Create(DateTimeOffset.MinValue, GROUP);
        Assert.IsNull(prop.Value);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest4()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.First();
        Assert.AreSame(first, prop);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest5()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        DateAndOrTimeProperty first = prop.AsWeakEnumerable().First();
        Assert.AreSame(first, prop);
    }

    [TestMethod()]
    public void DateTimeOffsetPropertyTest6()
    {
        IEnumerable<DateAndOrTimeProperty> prop = DateAndOrTimeProperty.Create(DateTimeOffset.Now, GROUP);
        foreach (DateAndOrTimeProperty item in prop)
        {
            Assert.IsNotNull(item.Value);
        }
    }
}
