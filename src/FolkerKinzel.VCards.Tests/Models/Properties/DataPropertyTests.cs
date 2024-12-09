using System.Text;
using FolkerKinzel.DataUrls;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class DataPropertyTests
{
    [TestMethod]
    public void DataPropertyTest3()
    {
        var row = VcfRow.Parse("PHOTO:".AsMemory(), new VcfDeserializationInfo());
        Assert.IsNotNull(row);
        var prop = new DataProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }

    [TestMethod]
    public void ValueTest1()
    {
        var prop = new DataProperty(RawData.FromText("abc"));
        RawData? val1 = prop.Value;
        RawData? val2 = prop.Value;
        Assert.IsNotNull(val1);
        Assert.AreEqual(val1, val2);
        Assert.AreSame(val1, val2);
    }

    [TestMethod]
    public void ValueTest2()
    {
        VCardProperty prop = new DataProperty(RawData.FromText("abc"));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<RawData>(prop.Value);
    }

    [TestMethod]
    public void ValueTest3()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/", UriKind.Absolute, out Uri? uri));
        VCardProperty prop = new DataProperty(RawData.FromUri(uri));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<RawData>(prop.Value);
    }

    [TestMethod]
    public void ValueTest4()
    {
        VCardProperty prop = new DataProperty(RawData.FromBytes([1, 2, 3]));
        Assert.IsFalse(prop.IsEmpty);
        Assert.IsInstanceOfType<RawData>(prop.Value);
    }

    [TestMethod]
    public void EmbeddedBytesPropertyTest1()
    {
        var prop = new DataProperty(RawData.FromBytes([], "image/png"));

        Assert.IsNotNull(prop.Value.Bytes);
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
        var prop = new DataProperty(RawData.FromBytes([1, 2, 3]));
        Assert.IsFalse(prop.IsEmpty);

        byte[]? val1 = prop.Value.Bytes;
        Assert.IsNotNull(val1);
        byte[]? val2 = prop.Value.Bytes;
        Assert.AreSame(val1, val2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromUriTest1()
    {
        Assert.IsTrue(Uri.TryCreate("../relative", UriKind.Relative, out Uri? uri));
        _ = RawData.FromUri(uri);
    }

    [TestMethod]
    public void FromBytesTest1()
    {
        var prop = new DataProperty(RawData.FromBytes([1,2], "blabla"));
        Assert.AreEqual("application/octet-stream", prop.Parameters.MediaType);
    }

    [TestMethod]
    public void FromFileTest1()
    {
        var prop = new DataProperty(RawData.FromFile(TestFiles.NextCloudPhotoIssueTxt));
        Assert.AreEqual("text/plain", prop.Parameters.MediaType);
    }

    [TestMethod]
    public void FromFileTest1b()
    {
        var prop = new DataProperty(RawData.FromFile(TestFiles.NextCloudPhotoIssueTxt, "blabla"));
        Assert.AreEqual("application/octet-stream", prop.Parameters.MediaType);
    }

    [TestMethod]
    public void FromFileTest2()
    {
        const string mime = "image/png";
        var prop = new DataProperty(RawData.FromFile(TestFiles.NextCloudPhotoIssueTxt, mime));
        Assert.AreEqual(mime, prop.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest1()
    {
        string textUri = DataUrl.FromText("The password");
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:{textUri}
        PHOTO:data:blabla;base64,ABCD
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Keys);
        DataProperty? key = vcard.Keys!.First();
        Assert.IsNotNull(key);
        Assert.IsNotNull(key.Value.String);
        Assert.IsNotNull(key.Value);
        Assert.AreEqual("The password", key.Value.String);
        Assert.AreEqual("text/plain", key.Parameters.MediaType);

        Assert.IsNotNull(vcard.Photos);
        DataProperty? photo = vcard.Photos!.First();
        Assert.IsNotNull(photo);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("blabla", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest1b()
    {
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:\,The%20password
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Keys);
        DataProperty? key = vcard.Keys.First();
        Assert.IsNotNull(key);
        Assert.IsNotNull(key.Value.String);
        Assert.IsNotNull(key.Value);
        Assert.AreEqual("The password", key.Value.String);
        Assert.AreEqual("text/plain", key.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest1c()
    {
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:;charset=utf-8\,The%20password
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Keys);
        DataProperty? key = vcard.Keys.First();
        Assert.IsNotNull(key);
        Assert.IsNotNull(key.Value.String);
        Assert.IsNotNull(key.Value);
        Assert.AreEqual("The password", key.Value.String);
        Assert.AreEqual("text/plain;charset=utf-8", key.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest1d()
    {
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:\;charset=utf-8\,The%20password
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Keys);
        DataProperty? key = vcard.Keys.First();
        Assert.IsNotNull(key);
        Assert.IsNotNull(key.Value.String);
        Assert.IsNotNull(key.Value);
        Assert.AreEqual("The password", key.Value.String);
        Assert.AreEqual("text/plain;charset=utf-8", key.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest2()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png;base64,äöü
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty? photo = vcard.Photos!.First();
        Assert.IsNotNull(photo);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest3()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:2.1
        PHOTO;GIF;ENCODING=QUOTED-PRINTABLE:ABCD
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty? photo = vcard.Photos!.First();
        Assert.IsNotNull(photo);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/gif", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest4()
    {
        const string vcf = """
        BEGIN:VCARD
        VERSION:2.1
        PHOTO;GIF;ENCODING=QUOTED-PRINTABLE:   
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsTrue(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/gif", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest5()
    {
        byte[] arr = [1, 2, 3];
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png\;base64\,{Convert.ToBase64String(arr)}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsFalse(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
        CollectionAssert.AreEqual(arr, photo.Value.Bytes);
    }

    [TestMethod]
    public void ParseTest5b()
    {
        byte[] arr = [1, 2, 3];
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png;base64\,{Convert.ToBase64String(arr)}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsFalse(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
        CollectionAssert.AreEqual(arr, photo.Value.Bytes);
    }

    [TestMethod]
    public void ParseTest5c()
    {
        byte[] arr = [1, 2, 3];
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png\;base64\,{Convert.ToBase64String(arr)}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsFalse(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
        CollectionAssert.AreEqual(arr, photo.Value.Bytes);
    }

    [TestMethod]
    public void ParseTest5d()
    {
        byte[] arr = [1, 2, 3];
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png\,{Uri.EscapeDataString(Encoding.UTF8.GetString(arr))}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsFalse(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
        CollectionAssert.AreEqual(arr, photo.Value.Bytes);
    }

    [TestMethod]
    public void ParseTest7()
    {
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        PHOTO:data:image/png\;parameter=value\;base64\,{Convert.ToBase64String([1, 2, 3])}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);
        Assert.IsNotNull(vcard.Photos);

        DataProperty photo = vcard.Photos!.First()!;
        Assert.IsFalse(photo.IsEmpty);
        Assert.IsNotNull(photo.Value.Bytes);
        Assert.AreEqual("image/png;parameter=value", photo.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest8()
    {
        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:text/plain;base64\,
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);

        Assert.IsNotNull(vcard.Keys);

        DataProperty key = vcard.Keys!.First()!;
        Assert.IsTrue(key.IsEmpty);
        Assert.IsNotNull(key.Value.String);
        Assert.AreEqual("text/plain", key.Parameters.MediaType);
    }

    [TestMethod]
    public void ParseTest9()
    {
        const string PASSWORD = "password";

        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:text/plain;base64\,{Base64.Encode(Encoding.UTF8.GetBytes(PASSWORD))}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);

        Assert.IsNotNull(vcard.Keys);

        DataProperty key = vcard.Keys!.First()!;
        Assert.IsFalse(key.IsEmpty);
        Assert.IsNotNull(key.Value.String);
        Assert.AreEqual("text/plain", key.Parameters.MediaType);
        Assert.AreEqual(PASSWORD, key.Value.String);
    }

    [TestMethod]
    public void ParseTest10()
    {
        const string PASSWORD = "password";

        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:;charset=utf-8;base64\,{Base64.Encode(Encoding.UTF8.GetBytes(PASSWORD))}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);

        Assert.IsNotNull(vcard.Keys);

        DataProperty key = vcard.Keys!.First()!;
        Assert.IsFalse(key.IsEmpty);
        Assert.IsNotNull(key.Value.String);
        Assert.AreEqual("text/plain;charset=utf-8", key.Parameters.MediaType);
        Assert.AreEqual(PASSWORD, key.Value.String);
    }

    [TestMethod]
    public void ParseTest11()
    {
        const string PASSWORD = "password";

        string vcf = $"""
        BEGIN:VCARD
        VERSION:4.0
        KEY:data:\;charset=utf-8\;base64\,{Base64.Encode(Encoding.UTF8.GetBytes(PASSWORD))}
        END:VCARD
        """;

        VCard vcard = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vcard);

        Assert.IsNotNull(vcard.Keys);

        DataProperty key = vcard.Keys!.First()!;
        Assert.IsFalse(key.IsEmpty);
        Assert.IsNotNull(key.Value.String);
        Assert.AreEqual("text/plain;charset=utf-8", key.Parameters.MediaType);
        Assert.AreEqual(PASSWORD, key.Value.String);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new DataProperty(RawData.FromText("Hi"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new DataProperty(RawData.FromBytes([1, 2, 3]));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value.Bytes);
        Assert.IsNotNull(prop2.Value.Bytes);

        CollectionAssert.AreEqual(prop1.Value.Bytes, prop2.Value.Bytes);
    }

    [TestMethod]
    public void CloneTest2()
    {
        var prop1 = new DataProperty(RawData.FromText("abc"));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value.String);
        Assert.IsNotNull(prop2.Value.String);

        Assert.AreSame(prop1.Value.String, prop2.Value.String);
    }

    [TestMethod]
    public void CloneTest3()
    {
        Assert.IsTrue(Uri.TryCreate("http://folker.de/index.htm", UriKind.Absolute, out Uri? uri));
        var prop1 = new DataProperty(RawData.FromUri(uri));
        var prop2 = (DataProperty)prop1.Clone();

        Assert.IsNotNull(prop2);
        Assert.IsNotNull(prop1.Value);
        Assert.IsNotNull(prop2.Value);
        Assert.IsNotNull(prop1.Value.Uri);
        Assert.IsNotNull(prop2.Value.Uri);

        Assert.AreSame(prop1.Value.Uri, prop2.Value.Uri);
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

    [TestMethod]
    public void AppendToTest1()
    {
        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOpts.Default, null);
        new DataProperty(RawData.FromText("")).AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }
}