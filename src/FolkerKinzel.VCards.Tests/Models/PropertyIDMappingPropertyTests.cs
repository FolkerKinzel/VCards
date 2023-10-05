using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
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

}
