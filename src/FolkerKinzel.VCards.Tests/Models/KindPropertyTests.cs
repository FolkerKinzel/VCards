using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class KindPropertyTests
{
    [TestMethod()]
    public void KindPropertyTest1()
    {
        const VCards.Enums.Kind kind = VCards.Enums.Kind.Application;

        var prop = new KindProperty(kind);

        Assert.AreEqual(kind, prop.Value);
        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void KindPropertyTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        const VCards.Enums.Kind kind = VCards.Enums.Kind.Application;

        var prop = new KindProperty(kind);

        var vcard = new VCard
        {
            Kind = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = Vcf.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Kind);

        prop = vcard.Kind;

        Assert.AreEqual(kind, prop!.Value);
        Assert.IsFalse(prop.IsEmpty);
    }
}
