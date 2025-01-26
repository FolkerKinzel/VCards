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
    public void AppendValueTest1()
    {
        var vc = VCardBuilder.Create().ContactID.Set(new Uri("http://folker.com/")).VCard;
        string vcf = vc.ToVcfString();
        StringAssert.Contains(vcf, "http://folker.com/");
    }


}
