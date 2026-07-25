using FolkerKinzel.VCards.Models.Properties;

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
        Assert.IsInstanceOfType<NameBuilder>(bldr1);
        Assert.IsInstanceOfType<NameBuilder>(bldr2);
    }

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(NameBuilder.Create().Equals((NameBuilder?)null));

        var builder = NameBuilder.Create();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(NameBuilder.Create().ToString());

    [TestMethod()]
    public void AddFamilyNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname("1").AddSurname("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Surnames.ToArray());
    }

    [TestMethod()]
    public void AddFamilyNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Surnames.ToArray());
    }

    [TestMethod()]
    public void AddFamilyNameTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddSurname((string[]?)null!));


    [TestMethod()]
    public void AddGivenNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGiven("1").AddGiven("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Given.ToArray());
    }

    [TestMethod()]
    public void AddGivenNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGiven(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Given.ToArray());
    }

    [TestMethod()]
    public void AddGivenNameTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddGiven((string[]?)null!));

    [TestMethod()]
    public void AddAdditionalNameTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGiven2("1").AddGiven2("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Given2.ToArray());
    }

    [TestMethod()]
    public void AddAdditionalNameTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGiven2(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Given2.ToArray());
    }

    [TestMethod()]
    public void AddAdditionalNameTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddGiven2((string[]?)null!));

    [TestMethod()]
    public void AddPrefixTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddPrefix("1").AddPrefix("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes.ToArray());
    }

    [TestMethod()]
    public void AddPrefixTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddPrefix(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Prefixes.ToArray());
    }

    [TestMethod()]
    public void AddPrefixTest3() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddPrefix((string[]?)null!));

    [TestMethod()]
    public void AddSuffixTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSuffix("1").AddSuffix("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes.ToArray());
    }

    [TestMethod()]
    public void AddSuffixTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSuffix(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Suffixes.ToArray());
    }

    [TestMethod()]
    public void AddSuffixTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddSuffix((string[]?)null!));

    [TestMethod()]
    public void AddSurname2Test1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname2("1").AddSurname2("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Surnames2.ToArray());
    }

    [TestMethod()]
    public void AddSurname2Test2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddSurname2(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Surnames2.ToArray());
    }

    [TestMethod()]
    public void AddSurname2Test3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddSurname2((string[]?)null!));

    [TestMethod()]
    public void AddGenerationTest1()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGeneration("1").AddGeneration("2").Build());

        CollectionAssert.AreEqual(expected, prop.Value.Generations.ToArray());
    }

    [TestMethod()]
    public void AddGenerationTest2()
    {
        string[] expected = ["1", "2"];
        var prop = new NameProperty(NameBuilder.Create().AddGeneration(expected).Build());

        CollectionAssert.AreEqual(expected, prop.Value.Generations.ToArray());
    }

    [TestMethod()]
    public void AddGenerationTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => NameBuilder.Create().AddGeneration((string[]?)null!));

    [TestMethod()]
    public void ClearTest()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddGeneration("1")
            .AddGiven2("2")
            .AddSurname("3")
            .AddGiven("4")
            .AddPrefix("5")
            .AddSuffix("6")
            .AddSurname2("7");

        _ = bldr.Build();

        Assert.IsInstanceOfType<NameBuilder>(bldr);
        Assert.IsTrue(bldr.Data.All(x => x.Value.Count == 0));
    }

    [TestMethod()]
    public void AddTest1()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddSurname("")
            .AddGiven((string?)null)
            .AddSurname("   ");

        Assert.IsEmpty(bldr.Data);
    }

    [TestMethod()]
    public void AddRangeTest1()
    {
        NameBuilder bldr = NameBuilder
            .Create()
            .AddSurname(["", "    "]);

        Assert.IsEmpty(bldr.Data);
    }

    [TestMethod()]
    public void AddRangeTest2()
    {
        var prop = new NameProperty(NameBuilder
            .Create()
            .AddSurname("1")
            .AddSurname(["2", "3"])
            .Build());

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, prop.Value.Surnames.ToArray());
    }
}