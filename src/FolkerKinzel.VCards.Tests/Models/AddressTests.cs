namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class AddressTests
{
    [DataTestMethod]
    [DataRow(null, false)]
    [DataRow("Germany", false)]
    [DataRow("USA", true)]
    [DataRow("U.S.A.", true)]
    [DataRow("U. S. A.", true)]
    [DataRow("United States Of America", true)]
    [DataRow("UNITED STATES OF AMERICA", true)]
    [DataRow("United States", true)]
    public void IsUSAddressTest(string? input, bool expected)
    {
        var addr = new AddressProperty(null, null, null, country: input, autoLabel:false);
        Assert.AreEqual(expected, addr.Value.IsUSAddress());
    }
}
