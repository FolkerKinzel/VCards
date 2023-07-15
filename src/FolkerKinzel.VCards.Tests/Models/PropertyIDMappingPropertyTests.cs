using FolkerKinzel.VCards.Intls.Deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class PropertyIDMappingPropertyTests
{
    [TestMethod()]
    public void PropertyIDMappingPropertyTest1()
    {
        var pidMap = new PropertyIDMapping(7, new Uri("http://folkerkinzel.de/"));
        var prop = new PropertyIDMappingProperty(pidMap);

        var vcard = new VCard
        {
            PropertyIDMappings = new PropertyIDMappingProperty?[]{prop, null, new PropertyIDMappingProperty((PropertyIDMapping?)null)}
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

        Assert.IsNotNull(vcard.PropertyIDMappings);

        prop = vcard.PropertyIDMappings!.First() as PropertyIDMappingProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(pidMap.ID, prop!.Value?.ID);
        Assert.AreEqual(pidMap.Mapping, prop!.Value?.Mapping);
        Assert.IsNull(prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    

    [TestMethod]
    public void GetValueTest()
    {
        VcfRow row = VcfRow.Parse("PidMap:", new VcfDeserializationInfo())!;
        var prop = new PropertyIDMappingProperty(row);

        Assert.IsNull(prop.Value);

        //using var writer = new StringWriter();
        //var serializer = new Vcf_3_0Serializer(writer, VcfOptions.WriteEmptyProperties, null);
        //serializer.
        //prop.BuildProperty(serializer);
        //Assert.AreEqual(0, writer.ToString().Length);
    }

}
