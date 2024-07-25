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
        AddressBuilder bldr = AddressBuilder
            .Create()
            .AddToApartment("a")
            .AddToBlock("b")
            .AddToBuilding("c")
            .AddToCountry("d")
            .AddToDirection("e")
            .AddToDistrict("f")
            .AddToExtendedAddress("g")
            .AddToFloor("h")
            .AddToLandmark("i")
            .AddToLocality("k")
            .AddToPostalCode("l")
            .AddToPostOfficeBox("m")
            .AddToRegion("n")
            .AddToRoom("o")
            .AddToStreet("p")
            .AddToStreetName("q")
            .AddToStreetNumber("r")
            .AddToSubDistrict("s")
            .Clear();

        Assert.IsInstanceOfType(bldr, typeof(AddressBuilder));
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

    [TestMethod()]
    public void AddToRoomTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToRoom("1").AddToRoom("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    public void AddToRoomTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToRoom(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToRoomTest3() => AddressBuilder.Create().AddToRoom((string[]?)null!);

    [TestMethod()]
    public void AddToApartmentTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToApartment("1").AddToApartment("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    public void AddToApartmentTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToApartment(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToApartmentTest3() => AddressBuilder.Create().AddToApartment((string[]?)null!);

    [TestMethod()]
    public void AddToFloorTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToFloor("1").AddToFloor("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    public void AddToFloorTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToFloor(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToFloorTest3() => AddressBuilder.Create().AddToFloor((string[]?)null!);

    [TestMethod()]
    public void AddToStreetNumberTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreetNumber("1").AddToStreetNumber("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    public void AddToStreetNumberTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreetNumber(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetNumberTest3() => AddressBuilder.Create().AddToStreetNumber((string[]?)null!);

    [TestMethod()]
    public void AddToStreetNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreetName("1").AddToStreetName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    public void AddToStreetNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToStreetName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetNameTest3() => AddressBuilder.Create().AddToStreetName((string[]?)null!);

    [TestMethod()]
    public void AddToBuildingTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToBuilding("1").AddToBuilding("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    public void AddToBuildingTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToBuilding(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToBuildingTest3() => AddressBuilder.Create().AddToBuilding((string[]?)null!);

    [TestMethod()]
    public void AddToBlockTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToBlock("1").AddToBlock("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    public void AddToBlockTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToBlock(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToBlockTest3() => AddressBuilder.Create().AddToBlock((string[]?)null!);

    [TestMethod()]
    public void AddToSubDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToSubDistrict("1").AddToSubDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    public void AddToSubDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToSubDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToSubDistrictTest3() => AddressBuilder.Create().AddToSubDistrict((string[]?)null!);

    [TestMethod()]
    public void AddToDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToDistrict("1").AddToDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    public void AddToDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToDistrictTest3() => AddressBuilder.Create().AddToDistrict((string[]?)null!);

    [TestMethod()]
    public void AddToLandmarkTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToLandmark("1").AddToLandmark("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    public void AddToLandmarkTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToLandmark(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToLandmarkTest3() => AddressBuilder.Create().AddToLandmark((string[]?)null!);

    [TestMethod()]
    public void AddToDirectionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToDirection("1").AddToDirection("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    public void AddToDirectionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddToDirection(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToDirectionTest3() => AddressBuilder.Create().AddToDirection((string[]?)null!);

}
