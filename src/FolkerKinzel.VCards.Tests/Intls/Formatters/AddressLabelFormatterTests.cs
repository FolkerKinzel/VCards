using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Formatters.Tests;

[TestClass]
public class AddressLabelFormatterTests
{
    [TestMethod]
    public void ToLabelTest1()
    {
        const string zip = "67133";
        const string city = "Maxdorf";
        const string country = "Germany";
        const string street = "Röntgenstr. 9";
        var address = new AddressProperty(AddressBuilder
            .Create()
            .AddStreet(street)
            .AddLocality(city)
            .AddCountry(country)
            .AddPostalCode(zip));
        address.Parameters.Label = AddressFormatter.Default.ToLabel(address);
        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country);
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
        var address = new AddressProperty(AddressBuilder
            .Create()
            .AddCountry(country)
            .AddPostalCode(zip)
            .AddLocality(city)
            .AddStreet(street)
            .AddPostOfficeBox(poBox)
            );

        address.Parameters.Label = AddressFormatter.Default.ToLabel(address);

        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        Assert.IsFalse(label!.Contains(street));
        Assert.IsTrue(label!.Contains(poBox));
        StringAssert.Contains(label, country);
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
        var address = new AddressProperty(AddressBuilder
            .Create()
            .AddCountry(country)
            .AddPostalCode(zip)
            .AddLocality(city)
            .AddStreet(street)
            .AddRegion(state)
            );

        address.Parameters.Label = AddressFormatter.Default.ToLabel(address);

        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country);
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

        var address = new AddressProperty(AddressBuilder
            .Create()
            .AddCountry(country)
            .AddPostalCode(zip)
            .AddLocality(city)
            .AddStreet(street)
            .AddRegion(state)
            .AddExtendedAddress(extended)
            );

        address.Parameters.Label = AddressFormatter.Default.ToLabel(address);

        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country);
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

        var address = new AddressProperty(AddressBuilder
            .Create()
            .AddCountry(country)
            .AddPostalCode(zip)
            .AddLocality(city)
            .AddStreet(street)
            .AddRegion(state)
            .AddExtendedAddress(extended)
            );

        address.Parameters.Label = AddressFormatter.Default.ToLabel(address);

        string? label = address.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {state} {zip}");
        StringAssert.Contains(label, country);
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
        StringAssert.Contains(label, country);
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest5b()
    {
        const string street = "Elm Street";
        const string city = "Springwood";
        
        var address = new AddressProperty(AddressBuilder.Create().AddStreet(street).AddLocality(city));
#pragma warning disable CS0618 // Type or member is obsolete
        string? label = address.Value.ToLabel();
#pragma warning restore CS0618 // Type or member is obsolete
        Assert.IsNotNull(label);
        StringAssert.Contains(label, city);
        StringAssert.Contains(label, street);
        Assert.IsFalse(label.HasEmptyLine());
    }

    [TestMethod]
    public void ToLabelTest5c()
    { 
        var address = new AddressProperty(AddressBuilder.Create());
#pragma warning disable CS0618 // Type or member is obsolete
        string? label = address.Value.ToLabel();
#pragma warning restore CS0618 // Type or member is obsolete
        Assert.IsNull(label);
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
        StringAssert.Contains(label, country);
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

        var prop = new AddressProperty(AddressBuilder
            .Create()
            .AddCountry(country)
            .AddPostalCode(zip)
            .AddLocality(city)
            .AddStreet(street)
            .AddRegion(state)
            .AddExtendedAddress(extended)
            );

        prop.Parameters.Label = AddressFormatter.Default.ToLabel(prop);

        string? label = prop.Parameters.Label;
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{city} {state} {zip}");
        StringAssert.Contains(label, country);
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());

        prop.Parameters.CountryCode = "DE";
        label = prop.ToLabel();
        Assert.IsNotNull(label);
        StringAssert.Contains(label, $"{zip} {city}");
        StringAssert.Contains(label, country);
        StringAssert.Contains(label, state);
        Assert.IsFalse(label.HasEmptyLine());

    }
}

