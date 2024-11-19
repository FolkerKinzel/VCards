using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class ReferencedDataPropertyTests
{
    [TestMethod]
    public void CloneTest1()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/index.htm", UriKind.Absolute, out Uri? uri));
        var prop1 = new DataProperty(RawData.FromUri(uri));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsInstanceOfType<Uri>(prop1.Value.Object);
        Assert.IsInstanceOfType<Uri>(prop2.Value.Object);
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value!.Uri);
        Assert.IsNotNull(prop2.Value!.Uri);

        Assert.AreSame(prop1.Value!.Uri, prop2.Value!.Uri);
    }


    [TestMethod]
    public void GetFileTypeExtensionTest1()
        => Assert.AreEqual(".htm", RawData.FromUri(new Uri("http://folker.de/", UriKind.Absolute)).GetFileTypeExtension(), false);


    [TestMethod]
    public void GetFileTypeExtensionTest2()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/test.txt", UriKind.Absolute, out Uri? uri));

        var prop = RawData.FromUri(uri, "image/png");
        Assert.AreEqual(".png", prop.GetFileTypeExtension());

        prop = RawData.FromUri(uri);
        Assert.AreEqual(".txt", prop.GetFileTypeExtension());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DataProperty(RawData.FromText("")).ToString());


    [TestMethod]
    public void AppendToTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOpts.Default, null);
        new DataProperty(RawData.FromText("")).AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }


    [TestMethod]
    public void PrepareForVcfSerializationTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOpts.Default, null);

        var prop = new DataProperty(RawData.FromText(""));
        prop.Parameters.ContentLocation = Loc.Url;
        prop.Parameters.DataType = Data.Uri;

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(Loc.Inline, prop.Parameters.ContentLocation);
    }


    [TestMethod]
    public void PrepareForVcfSerializationTest2()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_2_1Serializer(writer, VcfOpts.Default, null);

        Assert.IsTrue(Uri.TryCreate("cid:something", UriKind.Absolute, out Uri? uri));
        var prop = new DataProperty(RawData.FromUri(uri));

        prop.PrepareForVcfSerialization(serializer);
        Assert.AreEqual(Loc.Cid, prop.Parameters.ContentLocation);
        Assert.AreEqual(Data.Uri, prop.Parameters.DataType);
    }
}

