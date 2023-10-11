using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

internal class DataPropertyDerived : DataProperty
{
    public DataPropertyDerived(DataProperty prop) : base(prop)
    {
    }

    public DataPropertyDerived(string? mimeType, ParameterSection parameterSection, string? propertyGroup) 
        : base(mimeType, parameterSection, propertyGroup)
    {
    }

    public override object Clone() => throw new NotImplementedException();
    public override string GetFileTypeExtension() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}


[TestClass]
public class DataPropertyTests
{
    [TestMethod]
    public void DataPropertyTest3()
    {
        VcfRow row = VcfRow.Parse("PHOTO:", new VcfDeserializationInfo())!;
        var prop = DataProperty.Create(row, VCdVersion.V3_0);

        Assert.IsNull(prop.Value);
    }


    [TestMethod]
    public void ValueTest1()
    {
        var prop = DataProperty.FromText("abc");
        DataPropertyValue? val1 = prop.Value;
        DataPropertyValue? val2 = prop.Value;
        Assert.IsNotNull(val1);
        Assert.AreEqual(val1, val2);
        Assert.AreSame(val1, val2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromUriTest1()
    {
        Assert.IsTrue(Uri.TryCreate("../relative", UriKind.Relative, out Uri? uri));
        _ = DataProperty.FromUri(uri);
    }

    [TestMethod]
    public void FromBytesTest1()
    {
        var prop = DataProperty.FromBytes(null, "blabla");
        Assert.AreEqual("application/octet-stream", prop.Parameters.MediaType);
    }

    [TestMethod]
    public void FromFileTest1()
    {
        var prop = DataProperty.FromFile(TestFiles.NextCloudPhotoIssueTxt, "blabla");
        Assert.AreEqual("text/plain", prop.Parameters.MediaType);
    }

    [TestMethod]
    public void FromFileTest2()
    {
        const string mime = "image/png";
        var prop = DataProperty.FromFile(TestFiles.NextCloudPhotoIssueTxt, mime);
        Assert.AreEqual(mime, prop.Parameters.MediaType);
    }

    [TestMethod]
    public void CreateTest1()
    {
        string textUri = DataUrl.FromText("The password");
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:{textUri}
        PHOTO:data:blabla;base64,ABCD
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Keys);
        Assert.IsNotNull(vcard.Photos);
        Assert.IsInstanceOfType(vcard.Keys!.First(), typeof(EmbeddedTextProperty));

        DataProperty? photo = vcard.Photos!.First();
        Assert.IsInstanceOfType(photo, typeof(EmbeddedBytesProperty));
        Assert.AreEqual("application/octet-stream", photo!.Parameters.MediaType);
    }

    [TestMethod]
    public void CreateTest2()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png;base64,äöü
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty? photo = vcard.Photos!.First();
        Assert.IsInstanceOfType(photo, typeof(EmbeddedTextProperty));
        Assert.AreEqual("image/png", photo!.Parameters.MediaType);
    }

    [TestMethod]
    public void CreateTest3()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:2.1
        PHOTO;GIF;ENCODING=QUOTED-PRINTABLE:ABCD
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty? photo = vcard.Photos!.First();
        Assert.IsInstanceOfType(photo, typeof(EmbeddedBytesProperty));
        Assert.AreEqual("image/gif", photo!.Parameters.MediaType);
    }

    [TestMethod]
    public void CreateTest4()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:2.1
        PHOTO;GIF;ENCODING=QUOTED-PRINTABLE:   
        END:VCARD
        """;

        VCard vcard = VCard.ParseVcf(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsTrue(photo.IsEmpty);
        Assert.IsInstanceOfType(photo, typeof(EmbeddedBytesProperty));
        Assert.AreEqual("image/gif", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void IsEmptyTest1()
    {
        DataProperty prop = new DataPropertyDerived(DataProperty.FromText("Hello"));
        Assert.IsTrue(prop.IsEmpty);
    }
}