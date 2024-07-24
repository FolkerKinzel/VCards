using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class NameTests
{
    [DataTestMethod]
    [DataRow("a;;;;")]
    [DataRow(";a;;;")]
    [DataRow(";;a;;")]
    [DataRow(";;;a;")]
    [DataRow(";;;;a")]
    public void IsEmptyTest1(string input)
        => Assert.IsFalse(new Name(input.AsMemory(), VCdVersion.V4_0).IsEmpty);

    [TestMethod]
    public void AppendVCardStringTest1()
    {
        const string input = ";;Heinrich,August;;";
        var name = new Name(input.AsMemory(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, Opts.Default);
        name.AppendVcfString(serializer);

        Assert.AreEqual(input, serializer.Builder.ToString());
    }

    [DataTestMethod]
    [DataRow("ä;;;;")]
    [DataRow(";ä;;;")]
    [DataRow(";;ä;;")]
    [DataRow(";;;ä;")]
    [DataRow(";;;;ä")]
    [DataRow("ä;ä;ä;ä;ä")]
    public void NeedsToBeQPEncodedTest1(string input)
        => Assert.IsTrue(new Name(input.AsMemory(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow("a,ä;;;;")]
    [DataRow(";a,ä;;;")]
    [DataRow(";;a,ä;;")]
    [DataRow(";;;a,ä;")]
    [DataRow(";;;;a,ä")]
    public void NeedsToBeQPEncodedTest1b(string input)
        => Assert.IsTrue(new Name(input.AsMemory(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow("a,a;a;a;a;a")]
    [DataRow(";;;;")]
    public void NeedsToBeQPEncodedTest2(string input)
        => Assert.IsFalse(new Name(input.AsMemory(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Heinrich,August;   ;;;;;;;;;;";
        var name = new Name(input.AsMemory(), VCdVersion.V4_0);

        string s = name.ToString();
        StringAssert.Contains(s, "August");
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var name = new Name();
        Assert.IsTrue(name.IsEmpty);
        Assert.IsNotNull(name.ToString());
    }

    [TestMethod]
    public void Rfc9554Test1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddToFamilyNames("1")
                .AddToGivenNames("2")
                .AddToAdditionalNames("3")
                .AddToPrefixes("4")
                .AddToSuffixes("5")
                .AddToSurname2("6")
                .AddToGeneration("7"))
            .VCard;

        string vcf2 = vc.ToVcfString(VCdVersion.V2_1);
        string vcf3 = vc.ToVcfString(VCdVersion.V3_0);
        string vcfRfc9554 = vc.ToVcfString(VCdVersion.V4_0);
        string vcf4 = vc.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Unset(Opts.WriteRfc9554Extensions));

        const string standard = "1;2;3;4;5\r\n";
        const string rfc9554 = "1;2;3;4;5;6;7\r\n";

        Assert.IsTrue(vcf2.Contains(standard));
        Assert.IsTrue(vcf3.Contains(standard));
        Assert.IsTrue(vcf4.Contains(standard));
        Assert.IsTrue(vcfRfc9554.Contains(rfc9554));
    }

    [TestMethod]
    public void Rfc9554Test2()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddToSurname2("surname2")
                .AddToGeneration("generation"))
            .VCard;

        string vcfRfc9554 = vc.ToVcfString(VCdVersion.V4_0);

        vc = Vcf.Parse(vcfRfc9554)[0];

        Assert.IsNotNull(vc.NameViews);
        Name name = vc.NameViews.First()!.Value;

        Assert.AreEqual("surname2", name.Surname2.First());
        Assert.AreEqual("generation", name.Generation.First());
    }
}
