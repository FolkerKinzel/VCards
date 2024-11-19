using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class Rfc8605Tests
{
    [TestMethod]
    public void ContactUriTest1()
    {
        const string propKey = "CONTACT-URI";
        const string contactUri = "http://folker.de/contact";

        VCard vc = VCardBuilder
            .Create()
            .ContactUris.Add(contactUri)
            .ContactUris.Add("http://www.contoso.com")
            .ContactUris.SetPreferences()
            .VCard;

        Assert.IsNotNull(vc);
        Assert.IsNotNull (vc.ContactUris);
        Assert.IsTrue(vc.ContactUris.Any());

        string vcfStr1 = vc.ToVcfString(VCdVersion.V4_0);
        string vcfStr2 = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Unset(VcfOpts.WriteRfc8605Extensions));
        string vcfStr3 = vc.ToVcfString(VCdVersion.V3_0);
        string vcfStr4 = vc.ToVcfString(VCdVersion.V2_1);

        StringAssert.Contains(vcfStr1, contactUri);
        StringAssert.Contains(vcfStr1, propKey);
        Assert.IsFalse(vcfStr2.Contains(propKey, StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcfStr3.Contains(propKey, StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcfStr4.Contains(propKey, StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(vcfStr1)[0];

        Assert.IsNotNull(vc);
        Assert.IsNotNull(vc.ContactUris);
        Assert.AreEqual(2, vc.ContactUris.Count());
        Assert.AreEqual(contactUri, vc.ContactUris.PrefOrNull()?.Value);
    }

    [TestMethod]
    public void CcTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add(AddressBuilder.Create()
                                         .AddStreet("Blümchenweg 42")
                                         .AddLocality("Holzhausen")
                                         .AddPostalCode("01234")
                                         .AddCountry("Germany")
                                         .Build(),
                            parameters: p => p.CountryCode = "DE"
                          )
            .VCard;

        string vcfStr1 = vc.ToVcfString(VCdVersion.V4_0);
        string vcfStr2 = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Unset(VcfOpts.WriteRfc8605Extensions));
        string vcfStr3 = vc.ToVcfString(VCdVersion.V3_0);
        string vcfStr4 = vc.ToVcfString(VCdVersion.V2_1);

        StringAssert.Contains(vcfStr1, "CC=DE");
        Assert.IsFalse(vcfStr2.Contains("CC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcfStr3.Contains("CC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(vcfStr4.Contains("CC=", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(vcfStr1)[0];

        Assert.IsNotNull(vc);
        Assert.IsNotNull(vc.Addresses);

        AddressProperty? adr = vc.Addresses.First();
        Assert.IsNotNull(adr);

        Assert.AreEqual("DE", adr.Parameters.CountryCode);

        adr.Parameters.CountryCode = "  ";
        Assert.IsNull(adr.Parameters.CountryCode);
    }
}

