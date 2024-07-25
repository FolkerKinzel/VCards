using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Tests;
namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AddressToLabelConverterTests
{
    [TestMethod]
    public void ToLabelTest1()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        var address = new AddressProperty(street, city, null, zip, country);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest1b()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        const string poBox = "Postfach 4711";
        var address = new AddressProperty(street, city, null, zip, country, postOfficeBox: poBox, extendedAddress: null);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        Assert.IsFalse(label!.Contains(street));
        Assert.IsTrue(label!.Contains(poBox));
        StringAssert.Contains(label, country.ToUpperInvariant());
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest2()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string state = "Sachsen-Anhalt";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        var address = new AddressProperty(street, city, state, zip, country);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest3()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string state = "Sachsen-Anhalt";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        const string extended = "3. Stock";
        var address = new AddressProperty(street, city, state, zip, country, postOfficeBox: null, extendedAddress: extended);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }


    [TestMethod]
    public void ToLabelTest4()
    {
        const string street = "2101 MASSACHUSETTS AVE NW";
        const string city = "WASHINGTON";
        const string state = "DC";
        const string zip = "2008";
        const string country = "United States";
        const string extended = "";
        var address = new AddressProperty(street, city, state, zip, country, postOfficeBox: null, extendedAddress: extended);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {state} {zip}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest5()
    {
        const string street = "2101 MASSACHUSETTS AVE NW";
        const string city = "WASHINGTON";
        const string state = "DC";
        const string zip = "2008";
        const string country = "United States";
        const string extended = "";
        var address = new AddressProperty(street, city, state, zip, country, postOfficeBox: null, extendedAddress: extended);
#pragma warning disable CS0618 // Type or member is obsolete
        string? label = address.Value.ToLabel();
#pragma warning restore CS0618 // Type or member is obsolete
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {state} {zip}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest6()
    {
        const string street = "AV. FRANCISCO DE MIRANDA";
        const string sector = "LOS PALOS GRANDES";
        const string city = "CARACAS";
        const string state = "DISTRITO CAPITAL";
        const string zip = "1060";
        const string country = "Venezuela";
        var address = new AddressProperty([street, sector], [city], [state], [zip], [country]);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {zip} {state}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest7()
    {
        const string street = "2101 MASSACHUSETTS AVE NW";
        const string city = "WASHINGTON";
        const string state = "DC";
        const string zip = "2008";
        const string country = "United States";
        const string extended = "";
        var address = new AddressProperty(street, city, state, zip, country, postOfficeBox: null, extendedAddress: extended);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {state} {zip}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());

        address.Parameters.CountryCode = "DE";
        label = address.ToLabel();
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());

    }
}

