using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class VCdEncodingConverterTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("Base64", VCdEncoding.Base64)]
    [DataRow("B", VCdEncoding.Base64)]
    [DataRow("b", VCdEncoding.Base64)]
    [DataRow("QuotedPrintable", VCdEncoding.QuotedPrintable)]
    [DataRow("Q", VCdEncoding.QuotedPrintable)]
    [DataRow("q", VCdEncoding.QuotedPrintable)]
    [DataRow("8Bit", VCdEncoding.Ansi)]
    [DataRow("gluck", null)]
    [DataRow(".", null)]
    [DataRow(" ", null)]
    [DataRow("", null)]
    public void ParseTest(string? input, VCdEncoding? expected)
    {
        VCdEncoding? enc = VCdEncodingConverter.Parse(input);
        Assert.AreEqual(expected, enc);
    }
}
