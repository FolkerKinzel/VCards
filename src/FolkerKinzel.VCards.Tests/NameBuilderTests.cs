﻿using FolkerKinzel.VCards.Models;

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
        Assert.IsInstanceOfType(bldr2, typeof(NameBuilder));
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
    public void AddFamilyNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddFamilyName("1").AddFamilyName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.FamilyNames);
    }

    [TestMethod()]
    public void AddFamilyNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddFamilyName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.FamilyNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddFamilyNameTest3() => NameBuilder.Create().AddFamilyName((string[]?)null!);


    [TestMethod()]
    public void AddGivenNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGivenName("1").AddGivenName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.GivenNames);
    }

    [TestMethod()]
    public void AddGivenNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGivenName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.GivenNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddGivenNameTest3() => NameBuilder.Create().AddGivenName((string[]?)null!);

    [TestMethod()]
    public void AddAdditionalNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddAdditionalName("1").AddAdditionalName("2"));

        CollectionAssert.AreEqual(expected, prop.Value.AdditionalNames);
    }

    [TestMethod()]
    public void AddAdditionalNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddAdditionalName(expected));

        CollectionAssert.AreEqual(expected, prop.Value.AdditionalNames);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddAdditionalNameTest3() => NameBuilder.Create().AddAdditionalName((string[]?)null!);

    [TestMethod()]
    public void AddPrefixTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddPrefix("1").AddPrefix("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes);
    }

    [TestMethod()]
    public void AddPrefixTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddPrefix(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPrefixTest3() => NameBuilder.Create().AddPrefix((string[]?)null!);

    [TestMethod()]
    public void AddSuffixTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSuffix("1").AddSuffix("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes);
    }

    [TestMethod()]
    public void AddSuffixTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSuffix(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddSuffixTest3() => NameBuilder.Create().AddSuffix((string[]?)null!);

    [TestMethod()]
    public void AddSurname2Test1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname2("1").AddSurname2("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Surnames2);
    }

    [TestMethod()]
    public void AddSurname2Test2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname2(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Surnames2);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddSurname2Test3() => NameBuilder.Create().AddSurname2((string[]?)null!);

    [TestMethod()]
    public void AddGenerationTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGeneration("1").AddGeneration("2"));

        CollectionAssert.AreEqual(expected, prop.Value.Generations);
    }

    [TestMethod()]
    public void AddGenerationTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGeneration(expected));

        CollectionAssert.AreEqual(expected, prop.Value.Generations);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddGenerationTest3() => NameBuilder.Create().AddGeneration((string[]?)null!);

    [TestMethod()]
    public void ClearTest()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddGeneration("1")
            .AddAdditionalName("2")
            .AddFamilyName("3")
            .AddGivenName("4")
            .AddPrefix("5")
            .AddSuffix("6")
            .AddSurname2("7")
            .Clear();

        Assert.IsInstanceOfType(bldr, typeof(NameBuilder));
        Assert.IsTrue(bldr.Data.All(x => x.Value.Count == 0));
    }

    [TestMethod()]
    public void AddTest1()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddFamilyName("")
            .AddGivenName((string?)null)
            .AddFamilyName("   ");

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest1()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddFamilyName(["", "    "]);

        Assert.IsFalse(bldr.Data.Any());
    }

    [TestMethod()]
    public void AddRangeTest2()
    {
        var prop = new NameProperty(NameBuilder
            .Create()
            .AddFamilyName("1")
            .AddFamilyName(["2", "3"]));

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, prop.Value.FamilyNames);
    }
}