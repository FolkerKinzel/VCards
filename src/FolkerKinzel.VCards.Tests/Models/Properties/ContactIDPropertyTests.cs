using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class ContactIDPropertyTests
{
    [TestMethod]
    public void ContactIDPropertyTest1()
    {
        var prop =
            new ContactIDProperty(VcfRow.Parse("UID;VALUE=TEXT:   ".AsMemory(), new VcfDeserializationInfo())!, Enums.VCdVersion.V4_0);
        Assert.IsTrue(prop.IsEmpty);
    }

    [TestMethod]
    public void ContactIDPropertyTest2()
    {
        var prop =
            new ContactIDProperty(VcfRow.Parse("UID:   ".AsMemory(), new VcfDeserializationInfo())!, Enums.VCdVersion.V4_0);
        Assert.IsTrue(prop.IsEmpty);
    }

    [TestMethod]
    public void ContactIDPropertyTest3()
    {
        var prop =
            new ContactIDProperty(VcfRow.Parse("UID;VALUE=TEXT:abc".AsMemory(), new VcfDeserializationInfo())!, Enums.VCdVersion.V4_0);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual("abc", prop.Value.String);
    }

    [TestMethod]
    public void ContactIDPropertyTest4()
    {
        var prop =
            new ContactIDProperty(VcfRow.Parse("UID:abc".AsMemory(), new VcfDeserializationInfo())!, Enums.VCdVersion.V4_0);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual("abc", prop.Value.String);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ContactIDPropertyTest5() => _ = new ContactIDProperty((ContactID?)null!);

    [TestMethod]
    public void ContactIDPropertyTest6()
    {
        const string id = "Äöü";
        VCard vc = VCardBuilder
            .Create(false)
            .ContactID.Set(id)
            .VCard;

        Assert.IsNotNull(vc.ContactID);
        Assert.IsNull(vc.ContactID.OriginalString);

        string vcf21 = Vcf.AsString(vc, Enums.VCdVersion.V2_1);
        string vcf30 = Vcf.AsString(vc, Enums.VCdVersion.V3_0);
        string vcf40 = Vcf.AsString(vc, Enums.VCdVersion.V4_0);

        VCard vc21 = Vcf.Parse(vcf21)[0];
        VCard vc30 = Vcf.Parse(vcf30)[0];
        VCard vc40 = Vcf.Parse(vcf40)[0];

        Assert.IsNotNull(vc21.ContactID);
        Assert.AreEqual(id, vc21.ContactID.OriginalString);
        Assert.AreEqual(id, vc21.ContactID.Value.String);

        Assert.IsNotNull(vc30.ContactID);
        Assert.AreEqual(id, vc30.ContactID.OriginalString);
        Assert.AreEqual(id, vc30.ContactID.Value.String);


        Assert.IsNotNull(vc40.ContactID);
        Assert.AreEqual(id, vc40.ContactID.OriginalString);
        Assert.AreEqual(id, vc40.ContactID.Value.String);
    }

    [TestMethod]
    public void ContactIDPropertyTest7()
    {
        const string id = "789c03ec-b707-4e79-97ae-89ee061d861b";
        var guid = Guid.Parse(id);
        string guidUpperCase = guid.ToString().ToUpperInvariant();

        VCard vc1 = VCardBuilder
            .Create(false)
            .ContactID.Set(guid)
            .VCard;

        VCard vc2 = VCardBuilder
            .Create(false)
            .ContactID.Set(guidUpperCase)
            .VCard;

        Assert.IsNotNull (vc1.ContactID?.Value);
        Assert.IsNull(vc1.ContactID.Value.String);
        Assert.IsNull(vc1.ContactID.OriginalString);

        Assert.AreEqual(vc1.ContactID.Value, vc2.ContactID?.Value);
        Assert.IsNotNull(vc2.ContactID?.Value.String);
        Assert.IsNull(vc2.ContactID.OriginalString);

        string vcf1 = vc1.ToVcfString(Enums.VCdVersion.V3_0);
        string vcf2 = vc2.ToVcfString(Enums.VCdVersion.V3_0);

        StringAssert.Contains(vcf1, id);
        StringAssert.Contains(vcf2, guidUpperCase);

        VCard vc1a = Vcf.Parse(vcf1)[0];
        VCard vc2a = Vcf.Parse(vcf2)[0];


        Assert.IsNotNull(vc1a.ContactID?.Value);
        Assert.IsNotNull(vc1a.ContactID.Value.String);
        Assert.IsNotNull(vc1a.ContactID.OriginalString);

        Assert.AreEqual(vc1a.ContactID.Value, vc2a.ContactID?.Value);
        Assert.IsNotNull(vc2a.ContactID?.Value.String);
        Assert.IsNotNull(vc2a.ContactID.OriginalString);

        string vcf1a = vc1.ToVcfString(Enums.VCdVersion.V3_0);
        string vcf2a = vc2.ToVcfString(Enums.VCdVersion.V3_0);

        StringAssert.Contains(vcf1a, id);
        StringAssert.Contains(vcf2a, guidUpperCase);
    }

    [TestMethod]
    public void ContactIDPropertyTest8()
    {
        const string id = "789c03ec-b707-4e79-97ae-89ee061d861b";
        var guid = Guid.Parse(id);
        string guidUpperCase = guid.ToString().ToUpperInvariant();

        VCard vc1 = VCardBuilder
            .Create(false)
            .ContactID.Set(guid)
            .VCard;

        VCard vc2 = VCardBuilder
            .Create(false)
            .ContactID.Set(guidUpperCase)
            .VCard;

        Assert.IsNotNull(vc1.ContactID?.Value);
        Assert.IsNull(vc1.ContactID.Value.String);
        Assert.IsNull(vc1.ContactID.OriginalString);

        Assert.AreEqual(vc1.ContactID.Value, vc2.ContactID?.Value);
        Assert.IsNotNull(vc2.ContactID?.Value.String);
        Assert.IsNull(vc2.ContactID.OriginalString);

        string vcf1 = vc1.ToVcfString(Enums.VCdVersion.V4_0);
        string vcf2 = vc2.ToVcfString(Enums.VCdVersion.V4_0);

        StringAssert.Contains(vcf1, id);
        StringAssert.Contains(vcf1, "urn:uuid:");
        StringAssert.Contains(vcf2, guidUpperCase);
        StringAssert.Contains(vcf2, "VALUE=TEXT");

        VCard vc1a = Vcf.Parse(vcf1)[0];
        VCard vc2a = Vcf.Parse(vcf2)[0];


        Assert.IsNotNull(vc1a.ContactID?.Value);
        Assert.IsTrue(vc1a.ContactID.Value.Guid.HasValue);
        Assert.IsNotNull(vc1a.ContactID.OriginalString);

        Assert.AreEqual(vc1a.ContactID.Value, vc2a.ContactID?.Value);
        Assert.IsNotNull(vc2a.ContactID?.Value.String);
        Assert.IsNotNull(vc2a.ContactID.OriginalString);

        string vcf1a = vc1.ToVcfString(Enums.VCdVersion.V4_0);
        string vcf2a = vc2.ToVcfString(Enums.VCdVersion.V4_0);

        StringAssert.Contains(vcf1a, id);
        StringAssert.Contains(vcf1a, "urn:uuid:");

        StringAssert.Contains(vcf2a, guidUpperCase);
        StringAssert.Contains(vcf2a, "VALUE=TEXT");
    }

    [TestMethod]
    public void ContactIDPropertyTest9()
    {
        const string id = "789c03ec-b707-4e79-97ae-89ee061d861b";
        var guid = Guid.Parse(id);
        string guidUpperCase = guid.ToString().ToUpperInvariant();
        Uri uri = new Uri("urn:uuid:" + guidUpperCase);

        VCard vc1 = VCardBuilder
            .Create(false)
            .ContactID.Set(uri)
            .VCard;

        VCard vc2 = VCardBuilder
            .Create(false)
            .ContactID.Set(guidUpperCase)
            .VCard;

        Assert.IsNotNull(vc1.ContactID?.Value);
        Assert.IsNotNull(vc1.ContactID.Value.Uri);
        Assert.IsNull(vc1.ContactID.OriginalString);

        Assert.AreEqual(vc1.ContactID.Value, vc2.ContactID?.Value);
        Assert.IsNotNull(vc2.ContactID?.Value.String);
        Assert.IsNull(vc2.ContactID.OriginalString);

        string vcf1 = vc1.ToVcfString(Enums.VCdVersion.V4_0);
        string vcf2 = vc2.ToVcfString(Enums.VCdVersion.V4_0);

        StringAssert.Contains(vcf1, uri.OriginalString);
        StringAssert.Contains(vcf2, guidUpperCase);
        StringAssert.Contains(vcf2, "VALUE=TEXT");

        VCard vc1a = Vcf.Parse(vcf1)[0];
        VCard vc2a = Vcf.Parse(vcf2)[0];

        Assert.IsNotNull(vc1a.ContactID?.Value);
        Assert.IsTrue(vc1a.ContactID.Value.Guid.HasValue);
        Assert.IsNotNull(vc1a.ContactID.OriginalString);

        Assert.AreEqual(vc1a.ContactID.Value, vc2a.ContactID?.Value);
        Assert.IsNotNull(vc2a.ContactID?.Value.String);
        Assert.IsNotNull(vc2a.ContactID.OriginalString);

        string vcf1a = vc1.ToVcfString(Enums.VCdVersion.V4_0);
        string vcf2a = vc2.ToVcfString(Enums.VCdVersion.V4_0);

        StringAssert.Contains(vcf1a, uri.OriginalString);

        StringAssert.Contains(vcf2a, guidUpperCase);
        StringAssert.Contains(vcf2a, "VALUE=TEXT");
    }

    [TestMethod]
    public void AppendValueTest1()
    {
        var vc = VCardBuilder.Create().ContactID.Set(new Uri("http://folker.com/")).VCard;
        string vcf = vc.ToVcfString();
        StringAssert.Contains(vcf, "http://folker.com/");
    }


}
