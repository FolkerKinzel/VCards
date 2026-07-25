namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class LanguageConverterTests
{
    [TestMethod]
    [DataRow("en")]
    [DataRow("de")]
    public void ToStringTest1(string input) => Assert.AreEqual(input, LanguageConverter.ToString(input.AsSpan()));
}
