﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class AddressTests
{
    [TestMethod]
    public void AddressTest1()
    {
        var adr = new Address(";;;;;;;bla".AsMemory(), VCdVersion.V4_0);
        Assert.IsNotNull(adr);
    }

    [TestMethod]
    public void AddressTest2()
    {
        var adr = new Address("".AsMemory(), VCdVersion.V4_0);
        Assert.IsNotNull(adr);
    }

    [DataTestMethod]
    [DataRow("a;;;;;;")]
    [DataRow(";a;;;;;")]
    [DataRow(";;a;;;;")]
    [DataRow(";;;a;;;")]
    [DataRow(";;;;a;;")]
    [DataRow(";;;;;a;")]
    [DataRow(";;;;;;a")]
    public void IsEmptyTest1(string input)
        => Assert.IsFalse(new Address(input.AsMemory(), VCdVersion.V4_0).IsEmpty);

    [TestMethod]
    public void AppendVCardStringTest1()
    {
        const string input = ";;Parkstr.,7a;;;;";
        var adr = new Address(input.AsMemory(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, Opts.Default);
        adr.AppendVCardString(serializer);

        Assert.AreEqual(input, serializer.Builder.ToString());
    }

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Parkstr.,7a;;;;";
        var adr = new Address(input.AsMemory(), VCdVersion.V4_0);

        string s = adr.ToString();
        StringAssert.Contains(s, "7a");
    }

    [DataTestMethod]
    [DataRow("ä;;;;;;")]
    [DataRow(";ä;;;;;")]
    [DataRow(";;ä;;;;")]
    [DataRow(";;;ä;;;")]
    [DataRow(";;;;ä;;")]
    [DataRow(";;;;;ä;")]
    [DataRow(";;;;;;ä")]
    public void NeedsToBeQPEncodedTest1(string input)
        => Assert.IsTrue(new Address(input.AsMemory(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow(";;;;;;")]
    [DataRow("a;;;;;;")]
    [DataRow(";a;;;;;")]
    [DataRow(";;a;;;;")]
    [DataRow(";;;a;;;")]
    [DataRow(";;;;a;;")]
    [DataRow(";;;;;a;")]
    [DataRow(";;;;;;a")]
    public void NeedsToBeQPEncodedTest2(string input)
        => Assert.IsFalse(new Address(input.AsMemory(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    

    //[DataTestMethod]
    //[DataRow(";;;;;;", false)]
    ////[DataRow(";;;a;;;", false)]
    //[DataRow(";;;ä;;;", true)]
    //[DataRow(";;ä;;;;", true)]
    //[DataRow(";;a;;;;", false)]
    ////[DataRow(";;a;a;;;", false)]
    ////[DataRow(";;ä;a;;;", true)]
    ////[DataRow(";;ä;ä;;;", true)]

    ////[DataRow(";;ä;a;;;", true)]
    ////[DataRow(";;ä;;;;", true)]
    ////[DataRow(";;a;a;;;", false)]
    ////[DataRow(";;a;;;;", false)]
    ////[DataRow(";;ä;ä;;;", true)]
    ////[DataRow(";;a;ä;;;", true)]
    //public void TestTest2(string input, bool expected)
    //    => Assert.AreEqual(expected, new Address(input.AsMemory(), VCdVersion.V2_1).CoverageTest());
}
