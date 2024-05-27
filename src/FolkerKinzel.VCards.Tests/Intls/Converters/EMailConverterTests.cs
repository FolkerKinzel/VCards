namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class EMailConverterTests
{
    [DataTestMethod]
    [DataRow("internet")]
    [DataRow("x400")]
    [DataRow("OTHER")]
    public void ParseTest1(string input) => Assert.AreEqual(input.ToUpperInvariant(), EMailConverter.ToString(input.AsSpan()));
}
