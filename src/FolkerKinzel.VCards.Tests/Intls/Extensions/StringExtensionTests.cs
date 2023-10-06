namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class StringExtensionTests
{
    [DataTestMethod]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void UnMaskTest1(string? input, string? expected)
        => Assert.AreEqual(expected, input.UnMask(new System.Text.StringBuilder(), VCdVersion.V3_0), false);
}
