using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Models.Tests;


[TestClass]
public class EmbeddedBytesPropertyTests
{
    [TestMethod]
    public void EmbeddedBytesPropertyTest1()
    {
        var prop = DataProperty.FromBytes(Array.Empty<byte>(), "image/png");

        Assert.IsInstanceOfType(prop, typeof(EmbeddedBytesProperty));
        Assert.IsNull(prop.Value);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.ToString());

        Assert.AreEqual(".png", prop.GetFileTypeExtension());

        var vc = new VCard() { Photos = prop };
        string vcf = vc.ToVcfString(VCdVersion.V4_0, options: VcfOptions.Default | VcfOptions.WriteEmptyProperties);
        Assert.IsNotNull(VCard.ParseVcf(vcf)[0].Photos);
    }
}

