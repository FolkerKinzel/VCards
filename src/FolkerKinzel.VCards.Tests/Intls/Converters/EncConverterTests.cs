using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class EncConverterTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("Base64", Enc.Base64)]
    [DataRow("B", Enc.Base64)]
    [DataRow("b", Enc.Base64)]
    [DataRow("QuotedPrintable", Enc.QuotedPrintable)]
    [DataRow("Q", Enc.QuotedPrintable)]
    [DataRow("q", Enc.QuotedPrintable)]
    [DataRow("8Bit", Enc.Ansi)]
    [DataRow("gluck", null)]
    [DataRow(".", null)]
    [DataRow(" ", null)]
    [DataRow("", null)]
    public void ParseTest(string? input, Enc? expected)
    {
        Enc? enc = EncConverter.Parse(input);
        Assert.AreEqual(expected, enc);
    }
}
