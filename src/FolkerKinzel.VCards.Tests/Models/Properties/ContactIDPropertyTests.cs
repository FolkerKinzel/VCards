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
    public void AppendValueTest1()
    {
        var vc = VCardBuilder.Create().ContactID.Set(new Uri("http://folker.com/")).VCard;
        string vcf = vc.ToVcfString();
        StringAssert.Contains(vcf, "http://folker.com/");
    }


}
