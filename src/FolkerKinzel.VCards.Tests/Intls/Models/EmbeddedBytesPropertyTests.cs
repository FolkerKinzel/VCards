using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

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

    [DataTestMethod]
    [DataRow(null)]
    [DataRow(new byte[0])]
    public void EmbeddedBytesPropertyTest2(byte[]? input)
        => Assert.IsTrue(new EmbeddedBytesProperty(input, "application/octet-stream", null, new ParameterSection()).IsEmpty);

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = DataProperty.FromBytes(new byte[] { 1, 2, 3 });
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType(prop1, typeof(EmbeddedBytesProperty));
        Assert.IsInstanceOfType(prop2, typeof(EmbeddedBytesProperty));
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value!.Bytes);
        Assert.IsNotNull(prop2.Value!.Bytes);

        CollectionAssert.AreEqual(prop1.Value!.Bytes, prop2.Value!.Bytes);
    }
}

