using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class VCdEncodingConverterTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("Base64", ValueEncoding.Base64)]
    [DataRow("B", ValueEncoding.Base64)]
    [DataRow("b", ValueEncoding.Base64)]
    [DataRow("QuotedPrintable", ValueEncoding.QuotedPrintable)]
    [DataRow("Q", ValueEncoding.QuotedPrintable)]
    [DataRow("q", ValueEncoding.QuotedPrintable)]
    [DataRow("8Bit", ValueEncoding.Ansi)]
    [DataRow("gluck", null)]
    [DataRow(".", null)]
    [DataRow(" ", null)]
    [DataRow("", null)]
    public void ParseTest(string? input, ValueEncoding? expected)
    {
        ValueEncoding? enc = ValueEncodingConverter.Parse(input);
        Assert.AreEqual(expected, enc);
    }
}
