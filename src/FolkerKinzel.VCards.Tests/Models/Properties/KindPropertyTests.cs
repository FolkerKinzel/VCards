using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass()]
public class KindPropertyTests
{
    [TestMethod()]
    public void KindPropertyTest1()
    {
        const Kind kind = Kind.Application;

        var prop = new KindProperty(kind);

        Assert.AreEqual(kind, prop.Value);
        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void KindPropertyTest2()
    {
        const Kind kind = Kind.Application;

        var prop = new KindProperty(kind);

        var vcard = new VCard
        {
            Kind = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Kind);

        prop = vcard.Kind;

        Assert.AreEqual(kind, prop!.Value);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    public void GetVCardPropertyValueTest1()
    {
        VCardProperty prop = new KindProperty(Kind.Group);
        Assert.AreEqual(Kind.Group, prop.Value);
    }
}
