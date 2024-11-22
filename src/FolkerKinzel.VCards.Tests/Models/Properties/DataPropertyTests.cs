using System.Text;
using FolkerKinzel.DataUrls;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

//internal class DataPropertyDerived : DataProperty
//{
//    public DataPropertyDerived(DataProperty prop) : base(prop)
//    {
//    }

//    public DataPropertyDerived(ParameterSection parameters, string? group)
//        : base(parameters, group)
//    {
//    }

//    public override object Clone() => throw new NotImplementedException();
//    public override string GetFileTypeExtension() => throw new NotImplementedException();
//    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
//}


[TestClass]
public class DataPropertyTests
{
    [TestMethod]
    public void DataPropertyTest3()
    {
        VcfRow? row = VcfRow.Parse("PHOTO:".AsMemory(), new VcfDeserializationInfo());
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
        Assert.IsNotNull(key.Value);
        Assert.AreEqual("The password", key.Value.String);
        Assert.AreEqual("text/plain", key.Parameters.MediaType);

        Assert.IsNotNull(vcard.Photos);
        DataProperty? photo = vcard.Photos!.First();
        Assert.IsNotNull(photo);
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
        Assert.AreEqual("image/png", photo.Parameters.MediaType);
        CollectionAssert.AreEqual(arr, photo.Value.Bytes);
    }

    //[TestMethod]
    //public void ParseTest6()
    //{
    //    string vcf = $"""
    //    BEGIN:VCARD
    //    VERSION:4.0
    //    PHOTO:data:image/png\;base64\,{Convert.ToBase64String([1, 2, 3])}
    //    END:VCARD
    //    """;

    //    VCard vcard = Vcf.Parse(vcf)[0];

    //    Assert.IsNotNull(vcard);
    //    Assert.IsNotNull(vcard.Photos);

    //    DataProperty photo = vcard.Photos!.First()!;
    //    Assert.IsFalse(photo.IsEmpty);
    //    Assert.IsInstanceOfType(photo, typeof(EmbeddedBytesProperty));
    //    Assert.AreEqual("image/png", photo.Parameters.MediaType);
    //}

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
        Assert.IsInstanceOfType<byte[]>(photo.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
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
        Assert.IsInstanceOfType<string>(key.Value.Object);
        Assert.AreEqual("text/plain;charset=utf-8", key.Parameters.MediaType);
        Assert.AreEqual(PASSWORD, key.Value.String);
    }


    //[TestMethod]
    //public void IsEmptyTest1()
    //{
    //    DataProperty prop = new DataPropertyDerived(DataProperty.FromText("Hello"));
    //    Assert.IsTrue(prop.IsEmpty);
    //}

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new DataProperty(RawData.FromText("Hi"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}