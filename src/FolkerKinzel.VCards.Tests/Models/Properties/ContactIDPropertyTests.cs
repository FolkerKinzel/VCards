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


    }


    [TestMethod]
    public void AppendValueTest1()
    {
        var vc = VCardBuilder.Create().ContactID.Set(new Uri("http://folker.com/")).VCard;
        string vcf = vc.ToVcfString();
        StringAssert.Contains(vcf, "http://folker.com/");
    }


}
