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
    public void AddPostOfficeBoxTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostOfficeBox("1").AddPostOfficeBox("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    public void AddPostOfficeBoxTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostOfficeBox(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostOfficeBox);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPostOfficeBoxTest3() => AddressBuilder.Create().AddPostOfficeBox((string[]?)null!);

    [TestMethod()]
    public void AddExtendedAddressTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtendedAddress("1").AddExtendedAddress("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Extended);
    }

    [TestMethod()]
    public void AddExtendedAddressTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtendedAddress(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Extended);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddExtendedAddressTest3() => AddressBuilder.Create().AddExtendedAddress((string[]?)null!);

    [TestMethod()]
    public void AddExtendedAddressTest4()
    {
        var prop = new AddressProperty(
            AddressBuilder.Create()
                          .AddExtendedAddress("1")
                          .AddExtendedAddress("2")
                          .AddApartment("a"));

        Assert.AreEqual(0, prop.Value.Extended.Count);
        Assert.AreEqual("a", prop.Value.Apartment.Single());
    }

    [TestMethod()]
    public void AddStreetTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet("1").AddStreet("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    public void AddStreetTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Street);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStreetTest3() => AddressBuilder.Create().AddStreet((string[]?)null!);

    [TestMethod()]
    public void AddStreetTest4()
    {
        var prop = new AddressProperty(AddressBuilder.Create()
                                                     .AddStreet("1")
                                                     .AddStreet("2")
                                                     .AddStreetName("s"));

        Assert.AreEqual(0, prop.Value.Street.Count);
        Assert.AreEqual("s", prop.Value.StreetName.Single());
    }

    [TestMethod()]
    public void AddLocalityTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality("1").AddLocality("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    public void AddLocalityTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Locality);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddLocalityTest3() => AddressBuilder.Create().AddLocality((string[]?)null!);

    [TestMethod()]
    public void AddRegionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion("1").AddRegion("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    public void AddRegionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Region);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddRegionTest3() => AddressBuilder.Create().AddRegion((string[]?)null!);

    [TestMethod()]
    public void AddPostalCodeTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode("1").AddPostalCode("2"));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    public void AddPostalCodeTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode(expected));

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPostalCodeTest3() => AddressBuilder.Create().AddPostalCode((string[]?)null!);

    [TestMethod()]
    public void AddCountryTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry("1").AddCountry("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    public void AddCountryTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Country);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddCountryTest3() => AddressBuilder.Create().AddCountry((string[]?)null!);

    [TestMethod()]
    public void AddRoomTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom("1").AddRoom("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    public void AddRoomTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Room);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddRoomTest3() => AddressBuilder.Create().AddRoom((string[]?)null!);

    [TestMethod()]
    public void AddApartmentTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment("1").AddApartment("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    public void AddApartmentTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Apartment);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddApartmentTest3() => AddressBuilder.Create().AddApartment((string[]?)null!);

    [TestMethod()]
    public void AddFloorTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor("1").AddFloor("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    public void AddFloorTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Floor);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddFloorTest3() => AddressBuilder.Create().AddFloor((string[]?)null!);

    [TestMethod()]
    public void AddStreetNumberTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber("1").AddStreetNumber("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    public void AddStreetNumberTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStreetNumberTest3() => AddressBuilder.Create().AddStreetNumber((string[]?)null!);

    [TestMethod()]
    public void AddStreetNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName("1").AddStreetName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    public void AddStreetNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.StreetName);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStreetNameTest3() => AddressBuilder.Create().AddStreetName((string[]?)null!);

    [TestMethod()]
    public void AddBuildingTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding("1").AddBuilding("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    public void AddBuildingTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Building);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddBuildingTest3() => AddressBuilder.Create().AddBuilding((string[]?)null!);

    [TestMethod()]
    public void AddBlockTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock("1").AddBlock("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    public void AddBlockTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Block);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddBlockTest3() => AddressBuilder.Create().AddBlock((string[]?)null!);

    [TestMethod()]
    public void AddSubDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict("1").AddSubDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    public void AddSubDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddSubDistrictTest3() => AddressBuilder.Create().AddSubDistrict((string[]?)null!);

    [TestMethod()]
    public void AddDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict("1").AddDistrict("2"));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    public void AddDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict(expected));

        CollectionAssert.AreEqual(expected, prop.Value.District);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddDistrictTest3() => AddressBuilder.Create().AddDistrict((string[]?)null!);

    [TestMethod()]
    public void AddLandmarkTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark("1").AddLandmark("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    public void AddLandmarkTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Landmark);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddLandmarkTest3() => AddressBuilder.Create().AddLandmark((string[]?)null!);

    [TestMethod()]
    public void AddDirectionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection("1").AddDirection("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    public void AddDirectionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Direction);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddDirectionTest3() => AddressBuilder.Create().AddDirection((string[]?)null!);

}
