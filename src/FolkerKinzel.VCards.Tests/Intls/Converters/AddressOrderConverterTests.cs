using System.Globalization;
using FolkerKinzel.VCards.Models;

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
        var addr = new AddressProperty(null, null, null, postalCode: null, country: input, appendLabel: false);
        AddressOrder? order = addr.Value.GetAddressOrder();
        Assert.AreEqual((AddressOrder?)expected, order);
    }


    [DataTestMethod]
    [DataRow("en-US", AddressOrder.Usa)]
    [DataRow("de-DE", AddressOrder.Din)]
    [DataRow("es-VE-Z", AddressOrder.Venezuela)]
    [DataRow("en-AU", AddressOrder.Usa)]
    [DataRow("en-CA", AddressOrder.Usa)]
    [DataRow("af-ZA", AddressOrder.Usa)]
    [DataRow("ar-BH", AddressOrder.Usa)]
    [DataRow("ar-EG", AddressOrder.Usa)]
    [DataRow("ar-JO", AddressOrder.Usa)]
    [DataRow("ar-SA", AddressOrder.Usa)]
    [DataRow("zh-CN", AddressOrder.Usa)]
    [DataRow("zh-SG", AddressOrder.Usa)]
    [DataRow("zh-TW", AddressOrder.Usa)]
    [DataRow("div-MV", AddressOrder.Usa)]
    [DataRow("en-NZ", AddressOrder.Usa)]
    [DataRow("en-GB", AddressOrder.Usa)]
    [DataRow("hi-IN", AddressOrder.Usa)]
    [DataRow("id-ID", AddressOrder.Usa)]
    [DataRow("ja-JP", AddressOrder.Usa)]
    [DataRow("ko-KR", AddressOrder.Usa)]
    [DataRow("kk-KZ", AddressOrder.Usa)]
    [DataRow("lv-LV", AddressOrder.Usa)]
    [DataRow("ms-BN", AddressOrder.Usa)]
    [DataRow("pt-BR", AddressOrder.Usa)]
    [DataRow("ru-RU", AddressOrder.Usa)]
    [DataRow("es-CO", AddressOrder.Usa)]
    [DataRow("es-DO", AddressOrder.Usa)]
    [DataRow("sw-KE", AddressOrder.Usa)]
    [DataRow("uk-UA", AddressOrder.Usa)]
    [DataRow("vi-VN", AddressOrder.Usa)]
    public void ToAddressOrderTest1(string input, object? expected)
    {
        try
        {
            var culture = CultureInfo.CreateSpecificCulture(input);
            Assert.AreEqual(expected, culture.ToAddressOrder());
        }
        catch (CultureNotFoundException) { }
    }

    [TestMethod]
    public void ToAddressOrderTest2()
    {
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            Assert.IsInstanceOfType(culture.ToAddressOrder(), typeof(AddressOrder));
        }
    }
}

