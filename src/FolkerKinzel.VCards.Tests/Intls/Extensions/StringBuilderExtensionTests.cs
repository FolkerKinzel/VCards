using System.Text;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    [DataTestMethod]
    [DataRow("")]
    [DataRow("\"")]
    [DataRow("\'")]
    [DataRow("\"\"")]
    [DataRow("\'\'")]
    public void RemoveQuotesTest1(string input)
        => Assert.AreEqual(0, new StringBuilder(input).RemoveQuotes().Length);
}
