using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class EmbeddedBytesPropertyTests
{
    [TestMethod]
    public void EmbeddedBytesPropertyTest1()
    {
        var prop = new DataProperty(RawData.FromBytes([], "image/png"));

        Assert.IsInstanceOfType<byte[]>(prop.Value.Object);
        Assert.IsNotNull(prop.Value);
        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.ToString());

        Assert.AreEqual(".png", prop.Value.GetFileTypeExtension());

        var vc = new VCard() { Photos = prop };
        string vcf = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default | VcfOpts.WriteEmptyProperties);
        Assert.IsNotNull(Vcf.Parse(vcf)[0].Photos);
    }

    

    [TestMethod]
    public void EmbeddedBytesPropertyTest3()
    {
        var prop = new DataProperty(RawData.FromBytes( [1, 2, 3]));
        Assert.IsFalse(prop.IsEmpty);

        byte[]? val1 = prop.Value.Bytes;
        Assert.IsNotNull(val1);
        byte[]? val2 = prop.Value.Bytes;
        Assert.AreSame(val1, val2);
    }


    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new DataProperty(RawData.FromBytes([1, 2, 3]));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType<byte[]>(prop1.Value.Object);
        Assert.IsInstanceOfType<byte[]>(prop2.Value.Object);
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value!.Bytes);
        Assert.IsNotNull(prop2.Value!.Bytes);

        CollectionAssert.AreEqual(prop1.Value!.Bytes, prop2.Value!.Bytes);
    }
}

