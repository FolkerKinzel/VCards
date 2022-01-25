using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class VCdVersionConverterTests
{
    [DataTestMethod()]
    [DataRow(null, VCdVersion.V2_1)]
    [DataRow("bla", VCdVersion.V2_1)]
    [DataRow("", VCdVersion.V2_1)]
    [DataRow("2.1", VCdVersion.V2_1)]
    [DataRow("3.0", VCdVersion.V3_0)]
    [DataRow("4.0", VCdVersion.V4_0)]
    public void ParseTest(string? input, VCdVersion expected)
        => Assert.AreEqual(expected, VCdVersionConverter.Parse(input));
}
