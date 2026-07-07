using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class NonStandardParameterIssueTests
{
    [TestMethod]
    public void NonStandardParameterIssueTest1()
    {
        const string input = """
    BEGIN:VCARD
    VERSION:3.0
    FN:Test
    EMAIL;TYPE=work:a@b.com
    TEL;TYPE=work,voice,x-1st:123
    ADR;TYPE=work:;;Street;City;;;
    END:VCARD
    """;

        var card = Vcf.Parse(input)[0];
        var opts = VcfOpts.Default | VcfOpts.WriteNonStandardParameters;

        string vcf30 = Vcf.AsString(card, VCdVersion.V3_0, options: opts);
        string vcf40 = Vcf.AsString(card, VCdVersion.V4_0, options: opts);
        string vcf21 = Vcf.AsString(card, VCdVersion.V2_1, options: opts);

        Assert.IsFalse(vcf30.Contains(",:"));
        Assert.IsTrue(vcf30.Contains("x-1st", StringComparison.Ordinal));
        Assert.IsTrue(vcf30.Contains("TYPE=WORK", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(vcf40.Contains("TYPE=WORK", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(vcf40.Contains("x-1st", StringComparison.Ordinal));
        Assert.IsFalse(vcf40.Contains(",:"));


        Assert.IsTrue(vcf21.Contains("x-1st", StringComparison.Ordinal));
        Assert.IsTrue(vcf21.Contains("WORK", StringComparison.OrdinalIgnoreCase));
    }
}
