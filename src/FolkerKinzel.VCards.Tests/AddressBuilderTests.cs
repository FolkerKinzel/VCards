using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

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
            .AddExtended("g")
            .AddFloor("h")
            .AddLandmark("i")
            .AddLocality("k")
            .AddPostalCode("l")
            .AddPOBox("m")
            .AddRegion("n")
            .AddRoom("o")
            .AddStreet("p")
            .AddStreetName("q")
            .AddStreetNumber("r")
            .AddSubDistrict("s");

        _ = bldr.Build();

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
            .AddPostalCode(["2", "3"])
            .Build());

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, prop.Value.PostalCode.ToArray());
    }

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(AddressBuilder.Create().Equals((AddressBuilder?)null));

        var builder = AddressBuilder.Create();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(AddressBuilder.Create().ToString());

    [TestMethod()]
    public void AddPostOfficeBoxTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPOBox("1").AddPOBox("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.POBox.ToArray());
    }

    [TestMethod()]
    public void AddPostOfficeBoxTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPOBox(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.POBox.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPostOfficeBoxTest3() => AddressBuilder.Create().AddPOBox((string[]?)null!);

    [TestMethod()]
    public void AddExtendedAddressTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtended("1").AddExtended("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Extended.ToArray());
    }

    [TestMethod()]
    public void AddExtendedAddressTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddExtended(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Extended.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddExtendedAddressTest3() => AddressBuilder.Create().AddExtended((string[]?)null!);

    [TestMethod()]
    public void AddExtendedAddressTest4()
    {
        var prop = new AddressProperty(
            AddressBuilder.Create()
                          .AddExtended("1")
                          .AddExtended("2")
                          .AddApartment("a")
                          .Build());

        Assert.AreEqual(0, prop.Value.Extended.Count);
        Assert.AreEqual("a", prop.Value.Apartment.Single());     
    }

    [TestMethod()]
    public void AddStreetTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet("1").AddStreet("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Street.ToArray());
    }

    [TestMethod()]
    public void AddStreetTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreet(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Street.ToArray());
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
                                                     .AddStreetName("s")
                                                     .Build());

        Assert.AreEqual(0, prop.Value.Street.Count);
        Assert.AreEqual("s", prop.Value.StreetName.Single());
    }

    [TestMethod()]
    public void AddLocalityTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality("1").AddLocality("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Locality.ToArray());
    }

    [TestMethod()]
    public void AddLocalityTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLocality(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Locality.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddLocalityTest3() => AddressBuilder.Create().AddLocality((string[]?)null!);

    [TestMethod()]
    public void AddRegionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion("1").AddRegion("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Region.ToArray());
    }

    [TestMethod()]
    public void AddRegionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRegion(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Region.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddRegionTest3() => AddressBuilder.Create().AddRegion((string[]?)null!);

    [TestMethod()]
    public void AddPostalCodeTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode("1").AddPostalCode("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode.ToArray());
    }

    [TestMethod()]
    public void AddPostalCodeTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddPostalCode(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.PostalCode.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPostalCodeTest3() => AddressBuilder.Create().AddPostalCode((string[]?)null!);

    [TestMethod()]
    public void AddCountryTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry("1").AddCountry("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Country.ToArray());
    }

    [TestMethod()]
    public void AddCountryTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddCountry(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Country.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddCountryTest3() => AddressBuilder.Create().AddCountry((string[]?)null!);

    [TestMethod()]
    public void AddRoomTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom("1").AddRoom("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Room.ToArray());
    }

    [TestMethod()]
    public void AddRoomTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddRoom(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Room.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddRoomTest3() => AddressBuilder.Create().AddRoom((string[]?)null!);

    [TestMethod()]
    public void AddApartmentTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment("1").AddApartment("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Apartment.ToArray());
    }

    [TestMethod()]
    public void AddApartmentTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddApartment(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Apartment.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddApartmentTest3() => AddressBuilder.Create().AddApartment((string[]?)null!);

    [TestMethod()]
    public void AddFloorTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor("1").AddFloor("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Floor.ToArray());
    }

    [TestMethod()]
    public void AddFloorTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddFloor(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Floor.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddFloorTest3() => AddressBuilder.Create().AddFloor((string[]?)null!);

    [TestMethod()]
    public void AddStreetNumberTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber("1").AddStreetNumber("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber.ToArray());
    }

    [TestMethod()]
    public void AddStreetNumberTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetNumber(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.StreetNumber.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStreetNumberTest3() => AddressBuilder.Create().AddStreetNumber((string[]?)null!);

    [TestMethod()]
    public void AddStreetNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName("1").AddStreetName("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.StreetName.ToArray());
    }

    [TestMethod()]
    public void AddStreetNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddStreetName(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.StreetName.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStreetNameTest3() => AddressBuilder.Create().AddStreetName((string[]?)null!);

    [TestMethod()]
    public void AddBuildingTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding("1").AddBuilding("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Building.ToArray());
    }

    [TestMethod()]
    public void AddBuildingTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBuilding(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Building.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddBuildingTest3() => AddressBuilder.Create().AddBuilding((string[]?)null!);

    [TestMethod()]
    public void AddBlockTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock("1").AddBlock("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Block.ToArray());
    }

    [TestMethod()]
    public void AddBlockTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddBlock(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Block.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddBlockTest3() => AddressBuilder.Create().AddBlock((string[]?)null!);

    [TestMethod()]
    public void AddSubDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict("1").AddSubDistrict("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict.ToArray());
    }

    [TestMethod()]
    public void AddSubDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddSubDistrict(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.SubDistrict.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddSubDistrictTest3() => AddressBuilder.Create().AddSubDistrict((string[]?)null!);

    [TestMethod()]
    public void AddDistrictTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict("1").AddDistrict("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.District.ToArray());
    }

    [TestMethod()]
    public void AddDistrictTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDistrict(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.District.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddDistrictTest3() => AddressBuilder.Create().AddDistrict((string[]?)null!);

    [TestMethod()]
    public void AddLandmarkTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark("1").AddLandmark("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Landmark.ToArray());
    }

    [TestMethod()]
    public void AddLandmarkTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddLandmark(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Landmark.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddLandmarkTest3() => AddressBuilder.Create().AddLandmark((string[]?)null!);

    [TestMethod()]
    public void AddDirectionTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection("1").AddDirection("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Direction.ToArray());
    }

    [TestMethod()]
    public void AddDirectionTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new AddressProperty(AddressBuilder.Create().AddDirection(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Direction.ToArray());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddDirectionTest3() => AddressBuilder.Create().AddDirection((string[]?)null!);

}
