namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AddressOrderConverterTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("Germany", AddressOrder.Din)]
    [DataRow("USA", AddressOrder.Usa)]
    [DataRow("U.S.A.", AddressOrder.Usa)]
    [DataRow("U. S. A.", AddressOrder.Usa)]
    [DataRow("United States Of America", AddressOrder.Usa)]
    [DataRow("UNITED STATES OF AMERICA", AddressOrder.Usa)]
    [DataRow("United States", AddressOrder.Usa)]
    [DataRow("Venezuela", AddressOrder.Venezuela)]
    public void GetAddressOrderTest(string? input, object? expected)
    {
        var addr = new Models.AddressProperty(null, null, null, country: input, appendLabel: false);
        AddressOrder? order = addr.Value.GetAddressOrder();
        Assert.AreEqual((AddressOrder?)expected, order);
    }
}

