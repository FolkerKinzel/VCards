using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public void GetAddressOrderTest1(string? input, object? expected)
    {
        var addr = new Models.AddressProperty(null, null, postalCode: null, country: input, appendLabel: false);
        AddressOrder? order = addr.Value.GetAddressOrder();
        Assert.AreEqual((AddressOrder?)expected, order);
    }


    [DataTestMethod]
    [DataRow("en-US", AddressOrder.Usa)]
    [DataRow("de-DE", AddressOrder.Din)]
    [DataRow ("es-VE-Z", AddressOrder.Venezuela)]
    public void ToAddressOrderTest1(string input, object? expected)
    {
        var culture = CultureInfo.CreateSpecificCulture(input);
        Assert.AreEqual(expected, culture.ToAddressOrder());
    }
}

