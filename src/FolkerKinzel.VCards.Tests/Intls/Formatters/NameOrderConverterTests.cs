namespace FolkerKinzel.VCards.Intls.Formatters.Tests;

[TestClass]
public class NameOrderConverterTests
{
    [TestMethod]
    [DataRow(null, (int)NameOrder.Default)]
    [DataRow("", (int)NameOrder.Default)]
    [DataRow("  ", (int)NameOrder.Default)]
    [DataRow("de", (int)NameOrder.Default)]
    [DataRow("de-DE", (int)NameOrder.Default)]
    [DataRow("ko", (int)NameOrder.Hungarian)]
    [DataRow("kok", (int)NameOrder.Default)]
    [DataRow("zh", (int)NameOrder.Hungarian)]
    [DataRow("hr", (int)NameOrder.Hungarian)]
    [DataRow("sr", (int)NameOrder.Hungarian)]
    [DataRow("bs", (int)NameOrder.Hungarian)]
    [DataRow("hu", (int)NameOrder.Hungarian)]
    [DataRow("es", (int)NameOrder.Spanish)]
    [DataRow("vi", (int)NameOrder.Vietnamese)]
    public void ParseIetfLanguageTagTest1(string? input, int expected)
        => Assert.AreEqual((NameOrder)expected, NameOrderConverter.ParseIetfLanguageTag(input));
}
