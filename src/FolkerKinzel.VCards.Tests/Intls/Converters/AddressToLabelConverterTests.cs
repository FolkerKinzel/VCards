using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        var address = new AddressProperty(street, city, zip, null, country);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
    }

    [TestMethod]
    public void ToLabelTest2()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string state = "Sachsen-Anhalt";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        var address = new AddressProperty(street, city, zip, state, country);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
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
        var address = new AddressProperty(street, city, state, zip, country, extendedAddress: extended);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country.ToUpperInvariant());
        StringAssert.Contains(label, state);
    }
}

