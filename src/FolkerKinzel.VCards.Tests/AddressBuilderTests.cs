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
            .AddApartment("a")
            .AddBlock("b")
            .AddBuilding("c")
            .AddCountry("d")
            .AddDirection("e")
            .AddDistrict("f")
            .AddExtendedAddress("g")
            .AddFloor("h")
            .AddLandmark("i")
            .AddLocality("k")
            .AddPostalCode("l")
            .AddPostOfficeBox("m")
            .AddRegion("n")
            .AddRoom("o")
            .AddStreet("p")
            .AddStreetName("q")
            .AddStreetNumber("r")
            .AddSubDistrict("s")
            .Clear();

        Assert.IsInstanceOfType(bldr, typeof(AddressBuilder));
        Assert.IsTrue(bldr.Data.All(x => x.Value.Count == 0));
    }

    [TestMethod()]
    public void AddTest1()
    {
        AddressBuilder bldr = AddressBuilder
            .Create()
            .AddStreet("")
            .AddStreetName((string?)null)
            .AddStreet("   ");

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest1()
    {
        AddressBuilder bldr = AddressBuilder
            .Create()
            .AddLocality(["", "    "]);

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest2()
    {
        var prop = new AddressProperty(AddressBuilder
            .Create()
            .AddPostalCode("1")
            .AddPostalCode(["2", "3"]));

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
        var prop = new AddressProperty(AddressBuilder.Create().AddPostOfficeBox("1").AddPostOfficeBox("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    public void AddToPostOfficeBoxTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostOfficeBox(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToPostOfficeBoxTest3() => AddressBuilder.Create().AddPostOfficeBox((string[]?)null!);

    [TestMethod()]
    public void AddToExtendedAddressTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtendedAddress("1").AddExtendedAddress("2"));

        CollectionAssert.AreEqual(expected, prop.Value.ExtendedAddress);
    }

    [TestMethod()]
    public void AddToExtendedAddressTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtendedAddress(expected));

        CollectionAssert.AreEqual(expected, prop.Value.ExtendedAddress);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToExtendedAddressTest3() => AddressBuilder.Create().AddExtendedAddress((string[]?)null!);

    [TestMethod()]
    public void AddToStreetTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet("1").AddStreet("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    public void AddToStreetTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetTest3() => AddressBuilder.Create().AddStreet((string[]?)null!);

    [TestMethod()]
    public void AddToLocalityTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality("1").AddLocality("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    public void AddToLocalityTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToLocalityTest3() => AddressBuilder.Create().AddLocality((string[]?)null!);

    [TestMethod()]
    public void AddToRegionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion("1").AddRegion("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    public void AddToRegionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToRegionTest3() => AddressBuilder.Create().AddRegion((string[]?)null!);

    [TestMethod()]
    public void AddToPostalCodeTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode("1").AddPostalCode("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    public void AddToPostalCodeTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToPostalCodeTest3() => AddressBuilder.Create().AddPostalCode((string[]?)null!);

    [TestMethod()]
    public void AddToCountryTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry("1").AddCountry("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    public void AddToCountryTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToCountryTest3() => AddressBuilder.Create().AddCountry((string[]?)null!);

    [TestMethod()]
    public void AddToRoomTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom("1").AddRoom("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    public void AddToRoomTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToRoomTest3() => AddressBuilder.Create().AddRoom((string[]?)null!);

    [TestMethod()]
    public void AddToApartmentTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment("1").AddApartment("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    public void AddToApartmentTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToApartmentTest3() => AddressBuilder.Create().AddApartment((string[]?)null!);

    [TestMethod()]
    public void AddToFloorTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor("1").AddFloor("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    public void AddToFloorTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToFloorTest3() => AddressBuilder.Create().AddFloor((string[]?)null!);

    [TestMethod()]
    public void AddToStreetNumberTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber("1").AddStreetNumber("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    public void AddToStreetNumberTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetNumberTest3() => AddressBuilder.Create().AddStreetNumber((string[]?)null!);

    [TestMethod()]
    public void AddToStreetNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName("1").AddStreetName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    public void AddToStreetNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToStreetNameTest3() => AddressBuilder.Create().AddStreetName((string[]?)null!);

    [TestMethod()]
    public void AddToBuildingTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding("1").AddBuilding("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    public void AddToBuildingTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToBuildingTest3() => AddressBuilder.Create().AddBuilding((string[]?)null!);

    [TestMethod()]
    public void AddToBlockTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock("1").AddBlock("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    public void AddToBlockTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToBlockTest3() => AddressBuilder.Create().AddBlock((string[]?)null!);

    [TestMethod()]
    public void AddToSubDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict("1").AddSubDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    public void AddToSubDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToSubDistrictTest3() => AddressBuilder.Create().AddSubDistrict((string[]?)null!);

    [TestMethod()]
    public void AddToDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict("1").AddDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    public void AddToDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToDistrictTest3() => AddressBuilder.Create().AddDistrict((string[]?)null!);

    [TestMethod()]
    public void AddToLandmarkTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark("1").AddLandmark("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    public void AddToLandmarkTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToLandmarkTest3() => AddressBuilder.Create().AddLandmark((string[]?)null!);

    [TestMethod()]
    public void AddToDirectionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection("1").AddDirection("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    public void AddToDirectionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToDirectionTest3() => AddressBuilder.Create().AddDirection((string[]?)null!);

}
