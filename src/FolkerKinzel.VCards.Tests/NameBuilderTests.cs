using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass()]
public class NameBuilderTests
{
    [TestMethod()]
    public void CreateTest()
    {
        var bldr1 = NameBuilder.Create();
        var bldr2 = NameBuilder.Create();

        Assert.AreNotSame(bldr1, bldr2);
        Assert.IsInstanceOfType(bldr1, typeof(NameBuilder));
        Assert.IsInstanceOfType(bldr2 , typeof(NameBuilder));
    }

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new NameBuilder().Equals((NameBuilder?)null));

        var builder = new NameBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new NameBuilder().ToString());

    [TestMethod()]
    public void AddToFamilyNamesTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToFamilyNames("1").AddToFamilyNames("2"));

        CollectionAssert.AreEqual(expected, prop.Value.FamilyNames);
    }

    [TestMethod()]
    public void AddToFamilyNamesTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToFamilyNames(expected));

        CollectionAssert.AreEqual(expected, prop.Value.FamilyNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToFamilyNamesTest3() => NameBuilder.Create().AddToFamilyNames((string[]?)null!);
    

    [TestMethod()]
    public void AddToGivenNamesTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToGivenNames("1").AddToGivenNames("2"));

        CollectionAssert.AreEqual(expected, prop.Value.GivenNames);
    }

    [TestMethod()]
    public void AddToGivenNamesTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToGivenNames(expected));

        CollectionAssert.AreEqual(expected, prop.Value.GivenNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToGivenNamesTest3() => NameBuilder.Create().AddToGivenNames((string[]?)null!);

    [TestMethod()]
    public void AddToAdditionalNamesTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToAdditionalNames("1").AddToAdditionalNames("2"));

        CollectionAssert.AreEqual(expected, prop.Value.AdditionalNames);
    }

    [TestMethod()]
    public void AddToAdditionalNamesTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToAdditionalNames(expected));

        CollectionAssert.AreEqual(expected, prop.Value.AdditionalNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToAdditionalNamesTest3() => NameBuilder.Create().AddToAdditionalNames((string[]?)null!);

    [TestMethod()]
    public void AddToPrefixesTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToPrefixes("1").AddToPrefixes("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes);
    }

    [TestMethod()]
    public void AddToPrefixesTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToPrefixes(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToPrefixesTest3() => NameBuilder.Create().AddToPrefixes((string[]?)null!);

    [TestMethod()]
    public void AddToSuffixesTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToSuffixes("1").AddToSuffixes("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes);
    }

    [TestMethod()]
    public void AddToSuffixesTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToSuffixes(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToSuffixesTest3() => NameBuilder.Create().AddToSuffixes((string[]?)null!);

    [TestMethod()]
    public void AddToSurname2Test1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToSurname2("1").AddToSurname2("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Surname2);
    }

    [TestMethod()]
    public void AddToSurname2Test2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToSurname2(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Surname2);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToSurname2Test3() => NameBuilder.Create().AddToSurname2((string[]?)null!);

    [TestMethod()]
    public void AddToGenerationTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToGeneration("1").AddToGeneration("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Generation);
    }

    [TestMethod()]
    public void AddToGenerationTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddToGeneration(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Generation);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddToGenerationTest3() => NameBuilder.Create().AddToGeneration((string[]?)null!);

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
        NameBuilder bldr = NameBuilder
            .Create()
            .AddToFamilyNames("")
            .AddToGivenNames((string?)null)
            .AddToFamilyNames("   ");

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest1()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddToFamilyNames(["", "    "]);

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest2()
    {
        var prop = new NameProperty(NameBuilder
            .Create()
            .AddToFamilyNames("1")
            .AddToFamilyNames(["2", "3"]));

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, prop.Value.FamilyNames);
    }
}