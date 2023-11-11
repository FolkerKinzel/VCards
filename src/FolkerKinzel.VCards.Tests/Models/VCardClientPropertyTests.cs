using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class VCardClientPropertyTests
{
    [TestMethod]
    public void VCardClientTest1()
    {
        var prop = new VCardClientProperty(7, "http://folkerkinzel.de/");

        var vcard = new VCard
        {
            VCardClients = new VCardClientProperty?[] { prop, null }
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.VCardClients);

        VCardClientProperty? prop2 = vcard.VCardClients!.First();

        Assert.IsNotNull(prop);
        Assert.AreEqual(prop2!.Value!.LocalID, prop!.Value?.LocalID);
        Assert.AreEqual(prop2!.Value!.GlobalID, prop!.Value?.GlobalID);
        Assert.IsNull(prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VCardClientPropertyTest2() => _ = new VCardClientProperty(null!);


    [TestMethod]
    public void TryParseTest1()
    {
        VcfRow row = VcfRow.Parse("PidMap:", new VcfDeserializationInfo())!;
        Assert.IsFalse(VCardClientProperty.TryParse(row, out _));
    }


    [TestMethod]
    public void TryParseTest2()
    {
        var row = VcfRow.Parse("CLIENTPIDMAP:", new VcfDeserializationInfo());
        Assert.IsFalse(VCardClientProperty.TryParse(row!, out _));
        
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new VCardClientProperty(4, "https://contoso.com");
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
