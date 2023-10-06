using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class ReferencedDataPropertyTests
{
    [TestMethod]
    public void CloneTest1()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/index.htm", UriKind.Absolute, out Uri? uri));
        var prop1 = DataProperty.FromUri(uri);
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType(prop1, typeof(ReferencedDataProperty));
        Assert.IsInstanceOfType(prop2, typeof(ReferencedDataProperty));
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value!.Uri);
        Assert.IsNotNull(prop2.Value!.Uri);

        Assert.AreSame(prop1.Value!.Uri, prop2.Value!.Uri);
    }


    [TestMethod]
    public void GetFileTypeExtensionTest1() => Assert.AreEqual(".bin", DataProperty.FromUri(null).GetFileTypeExtension(), false);


    [TestMethod]
    public void GetFileTypeExtensionTest2()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/test.txt", UriKind.Absolute, out Uri? uri));

        var prop = DataProperty.FromUri(uri, "image/png");
        Assert.AreEqual(".png", prop.GetFileTypeExtension());

        prop = DataProperty.FromUri(uri);
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());

    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(DataProperty.FromUri(null).ToString());


    [TestMethod]
    public void AppendToTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOptions.Default, null);
        DataProperty.FromUri(null).AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void PrepareForVcfSerializationTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOptions.Default, null);

        var prop = DataProperty.FromUri(null);
        prop.Parameters.ContentLocation = ContentLocation.Url;
        prop.Parameters.DataType = VCdDataType.Uri;

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(ContentLocation.Inline, prop.Parameters.ContentLocation);
        Assert.IsNull(prop.Parameters.DataType);
    }

    [TestMethod]
    public void PrepareForVcfSerializationTest2()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOptions.Default, null);

        Assert.IsTrue(Uri.TryCreate("cid:something", UriKind.Absolute, out Uri? uri));
        var prop = DataProperty.FromUri(uri);

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(ContentLocation.ContentID, prop.Parameters.ContentLocation);
        Assert.AreEqual(VCdDataType.Uri, prop.Parameters.DataType);
    }
}

