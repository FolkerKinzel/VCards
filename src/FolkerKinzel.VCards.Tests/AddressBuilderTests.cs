using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class AddressBuilderTests
{
    [TestMethod()]
    public void CreateTest()
    {
        var bldr1 = AddressBuilder.Create();
        var bldr2 = AddressBuilder.Create();

        Assert.AreNotSame(bldr1, bldr2);
        Assert.IsInstanceOfType(bldr1, typeof(AddressBuilder));
        Assert.IsInstanceOfType(bldr2, typeof(AddressBuilder));
    }

    [TestMethod()]
    public void ClearTest()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddToGeneration("1")
            .AddToAdditionalNames("2")
            .AddToFamilyNames("3")
            .AddToGivenNames("4")
            .AddToPrefixes("5")
            .AddToSuffixes("6")
            .AddToSurname2("7")
            .Clear();

        Assert.IsInstanceOfType(bldr, typeof(NameBuilder));
        Assert.IsTrue(bldr.Data.All(x => x.Value.Count == 0));
    }

    [TestMethod()]
    public void AddTest1()
    {
        AddressBuilder bldr = AddressBuilder
            .Create()
            .AddToStreet("")
            .AddToStreetName((string?)null)
            .AddToStreet("   ");

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest1()
    {
        AddressBuilder bldr = AddressBuilder
            .Create()
            .AddToLocality(["", "    "]);

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest2()
    {
        var prop = new AddressProperty(AddressBuilder
            .Create()
            .AddToPostalCode("1")
            .AddToPostalCode(["2", "3"]));

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, prop.Value.PostalCode);
    }

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new AddressBuilder().Equals((AddressBuilder?)null));

        var builder = new AddressBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new AddressBuilder().ToString());

    [TestMethod()]
    public void AddToPostOfficeBoxTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToPostOfficeBox("1").AddToPostOfficeBox("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    public void AddToPostOfficeBoxTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToPostOfficeBox(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToPostOfficeBoxTest3() => AddressBuilder.Create().AddToPostOfficeBox((string[]?)null!);

    [TestMethod()]
    public void AddToExtendedAddressTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToExtendedAddress("1").AddToExtendedAddress("2"));

        CollectionAssert.AreEqual(expected, prop.Value.ExtendedAddress);
    }

    [TestMethod()]
    public void AddToExtendedAddressTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToExtendedAddress(expected));

        CollectionAssert.AreEqual(expected, prop.Value.ExtendedAddress);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToExtendedAddressTest3() => AddressBuilder.Create().AddToExtendedAddress((string[]?)null!);

    [TestMethod()]
    public void AddToStreetTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreet("1").AddToStreet("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    public void AddToStreetTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreet(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetTest3() => AddressBuilder.Create().AddToStreet((string[]?)null!);

    [TestMethod()]
    public void AddToLocalityTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToLocality("1").AddToLocality("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    public void AddToLocalityTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToLocality(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToLocalityTest3() => AddressBuilder.Create().AddToLocality((string[]?)null!);

    [TestMethod()]
    public void AddToRegionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToRegion("1").AddToRegion("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    public void AddToRegionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToRegion(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToRegionTest3() => AddressBuilder.Create().AddToRegion((string[]?)null!);

    [TestMethod()]
    public void AddToPostalCodeTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToPostalCode("1").AddToPostalCode("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    public void AddToPostalCodeTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToPostalCode(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToPostalCodeTest3() => AddressBuilder.Create().AddToPostalCode((string[]?)null!);

    [TestMethod()]
    public void AddToCountryTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToCountry("1").AddToCountry("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    public void AddToCountryTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToCountry(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToCountryTest3() => AddressBuilder.Create().AddToCountry((string[]?)null!);

}
