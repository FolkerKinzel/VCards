using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Formatters.Tests;

[TestClass]
public class DefaultAddressFormatterTests
{
    [TestMethod]
    public void DefaultAddressFormatterTest1()
    {
        var prop = new AddressProperty(AddressBuilder
            .Create()
            .AddStreet("Friedrichstr. 1")
            .AddLocality("Berlin")
            );

        prop.Parameters.CountryCode = "DE";
        prop.Parameters.ComponentOrder = ";4711";

        Assert.AreEqual($"Friedrichstr. 1{Environment.NewLine}Berlin", AddressFormatter.Default.ToLabel(prop));
    }

    [TestMethod]
    public void DefaultAddressFormatterTest2()
    {
        var prop = new AddressProperty(AddressBuilder
            .Create()
            .AddStreet("Friedrichstr. 1")
            .AddLocality("Berlin")
            );

        prop.Parameters.CountryCode = "DE";
        prop.Parameters.ComponentOrder = "s,\\, ;2;3";

        Assert.AreEqual("Friedrichstr. 1, Berlin", AddressFormatter.Default.ToLabel(prop));
    }

    [TestMethod]
    public void DefaultAddressFormatterTest3()
    {
        var prop = new AddressProperty(AddressBuilder
           .Create()
           );

        Assert.IsNull(AddressFormatter.Default.ToLabel(prop));
    }
}
