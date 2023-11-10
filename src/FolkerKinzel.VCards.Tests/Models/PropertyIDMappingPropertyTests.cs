using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Tests;

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
            PropertyIDMappings = new PropertyIDMappingProperty?[] { prop, null, new PropertyIDMappingProperty((PropertyIDMapping?)null) }
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.PropertyIDMappings);

        prop = vcard.PropertyIDMappings!.First() as PropertyIDMappingProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(pidMap.LocalID, prop!.Value?.LocalID);
        Assert.AreEqual(pidMap.GlobalID, prop!.Value?.GlobalID);
        Assert.IsNull(prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }



    [TestMethod]
    public void GetValueTest()
    {
        VcfRow row = VcfRow.Parse("PidMap:", new VcfDeserializationInfo())!;
        var prop = new PropertyIDMappingProperty(row);

        Assert.IsNull(prop.Value);
    }


    [TestMethod]
    public void AppendValueTest1()
    {
        var prop = new PropertyIDMappingProperty((PropertyIDMapping?)null);
        Assert.IsTrue(prop.IsEmpty);
        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, VcfOptions.Default);
        prop.AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new PropertyIDMappingProperty(4, new Uri("https://contoso.com"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
