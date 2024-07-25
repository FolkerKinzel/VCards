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

}
