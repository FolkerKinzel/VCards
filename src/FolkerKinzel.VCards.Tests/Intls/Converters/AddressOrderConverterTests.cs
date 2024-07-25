using System.Globalization;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AddressOrderConverterTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("Germany", AddressOrder.Din)]
    [DataRow("Deutschland", AddressOrder.Din)]
    [DataRow("USA", AddressOrder.Usa)]
    [DataRow("U.S.A.", AddressOrder.Usa)]
    [DataRow("U. S. A.", AddressOrder.Usa)]
    [DataRow("United States Of America", AddressOrder.Usa)]
    [DataRow("UNITED STATES OF AMERICA", AddressOrder.Usa)]
    [DataRow("United States", AddressOrder.Usa)]
    [DataRow("Venezuela", AddressOrder.Venezuela)]
    public void ParseAddressTest1(string? input, object? expected)
    {
        var addr = new AddressProperty(null, null, null, postalCode: null, country: input, autoLabel: false);
        AddressOrder? order = AddressOrderConverter.ParseAddress(addr.Value);
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
    public void ParseCultureInfoTest1(string input, object? expected)
    {
        try
        {
            var culture = CultureInfo.CreateSpecificCulture(input);
            Assert.AreEqual(expected, AddressOrderConverter.ParseCultureInfo(culture));
        }
        catch (CultureNotFoundException) { }
    }

    [TestMethod]
    public void ParseCultureInfoTest2()
    {
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            Assert.IsInstanceOfType(AddressOrderConverter.ParseCultureInfo(culture), typeof(AddressOrder));
        }
    }

    [DataTestMethod]
    [DataRow("AU", 1)] // AUSTRALIA												
    [DataRow("BH", 1)] // BAHRAIN												
    [DataRow("BD", 1)] // BANGLADESH												
    [DataRow("BM", 1)] // BERMUDA												
    [DataRow("BT", 1)] // BHUTAN													
    [DataRow("BR", 1)] // BRAZIL													
    [DataRow("BN", 1)] // BRUNEI DARUSSALAM										
    [DataRow("KH", 1)] // CAMBODIA												
    [DataRow("CA", 1)] // CANADA													
    [DataRow("KY", 1)] // CAYMAN ISLANDS											
    [DataRow("CN", 1)] // CHINA											
    [DataRow("CX", 1)] // Christmas Island										
    [DataRow("CC", 1)] // Cocos Islands											
    [DataRow("CO", 1)] // COLOMBIA												
    [DataRow("DO", 1)] // DOMINICAN REPUBLIC										
    [DataRow("EG", 1)] // EGYPT													
    [DataRow("FK", 1)] // Falkland Islands										
    [DataRow("GU", 1)] // Guam													
    [DataRow("HM", 1)] // Heard Island and McDonald Islands						
    [DataRow("IN", 1)] // INDIA													
    [DataRow("ID", 1)] // INDONESIA												
    [DataRow("IE", 1)] // IRELAND												
    [DataRow("IM", 1)] // Isle of Man											
    [DataRow("JP", 1)] // JAPAN													
    [DataRow("JO", 1)] // JORDAN													
    [DataRow("KZ", 1)] // KAZAKHSTAN												
    [DataRow("KE", 1)] // KENYA													
    [DataRow("KR", 1)] // REPUBLIC OF KOREA										
    [DataRow("LV", 1)] // LATVIA													
    [DataRow("LS", 1)] // LESOTHO												
    [DataRow("MV", 1)] // MALDIVES (State before Locality)						
    [DataRow("MT", 1)] // MALTA													
    [DataRow("MH", 1)] // Marshall Islands										
    [DataRow("MU", 1)] // MAURITIUS												
    [DataRow("FM", 1)] // Micronesia, Federated States of						
    [DataRow("MM", 1)] // MYANMAR												
    [DataRow("NR", 1)] // REPUBLIC OF NAURU										
    [DataRow("NP", 1)] // NEPAL													
    [DataRow("NZ", 1)] // NEW ZEALAND											
    [DataRow("NG", 1)] // NIGERIA												
    [DataRow("NF", 1)] // NORFOLK ISLAND											
    [DataRow("MP", 1)] // Northern Mariana Islands								
    [DataRow("PK", 1)] // PAKISTAN												
    [DataRow("PW", 1)] // Palau													
    [DataRow("PN", 1)] // PITCAIRN												
    [DataRow("PR", 1)] // Puerto Rico											
    [DataRow("RU", 1)] // RUSSIAN FEDERATION										
    [DataRow("SH", 1)] // SAINT HELENA											
    [DataRow("SA", 1)] // SAUDI ARABIA											
    [DataRow("SG", 1)] // SINGAPORE												
    [DataRow("SO", 1)] // SOMALIA												
    [DataRow("ZA", 1)] // SOUTH AFRICA											
    [DataRow("GS", 1)] // South Georgia and the South Sandwich Islands			
    [DataRow("LK", 1)] // SRI LANKA												
    [DataRow("SZ", 1)] // SWAZILAND, ESWATINI
    [DataRow("TW", 1)] // TAIWAN													
    [DataRow("TH", 1)] // THAILAND												
    [DataRow("TC", 1)] // TURKS AND CAICOS ISLANDS								
    [DataRow("UA", 1)] // UKRAINE												
    [DataRow("GB", 1)] // UNITED KINGDOM											
    [DataRow("US", 1)] // UNITED STATES											
    [DataRow("UM", 1)] // United States Minor Outlying Islands (UNITED STATES)	
    [DataRow("UZ", 1)] // Uzbekistan												
    [DataRow("VN", 1)] // VIET NAM												
    [DataRow("VG", 1)] // Virgin Islands (U.K.)									
    [DataRow("VI", 1)] // Virgin Islands (U.S.)	

    [DataRow("VE", 2)] // VENEZUELA (LOCALITY POSTAL_CODE, PROVINCE)				
    [DataRow("PG", 2)] // PAPUA NEW GUINEA (LOCALITY POSTAL_CODE PROVINCE)
    public void ParseAddressPropertyTest1(string countryCode, int adrOrderValue)
    {
        var expected = (AddressOrder)adrOrderValue;

        var prop = new AddressProperty(AddressBuilder.Create());
        prop.Parameters.CountryCode = countryCode;

        Assert.AreEqual(expected, AddressOrderConverter.ParseAddressProperty(prop));
    }

}

