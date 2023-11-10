using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class AddressTests
{
    [TestMethod]
    public void AddressTest1()
    {
        var adr = new Address(";;;;;;;bla", new VcfDeserializationInfo(), VCdVersion.V4_0);
        Assert.IsNotNull(adr);
    }

    [TestMethod]
    public void AddressTest2()
    {
        var adr = new Address("", new VcfDeserializationInfo(), VCdVersion.V4_0);
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
        => Assert.IsFalse(new Address(input, new VcfDeserializationInfo(), VCdVersion.V4_0).IsEmpty);

    [TestMethod]
    public void AppendVCardStringTest1()
    {
        const string input = ";;Parkstr.,7a;;;;";
        var adr = new Address(input, new VcfDeserializationInfo(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, VcfOptions.Default);
        adr.AppendVCardString(serializer);

        Assert.AreEqual(input, serializer.Builder.ToString());
    }

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Parkstr.,7a;;;;";
        var adr = new Address(input, new VcfDeserializationInfo(), VCdVersion.V4_0);

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
        => Assert.IsTrue(new Address(input, new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow("a;;;;;;")]
    [DataRow(";a;;;;;")]
    [DataRow(";;a;;;;")]
    [DataRow(";;;a;;;")]
    [DataRow(";;;;a;;")]
    [DataRow(";;;;;a;")]
    [DataRow(";;;;;;a")]
    public void NeedsToBeQPEncodedTest2(string input)
        => Assert.IsFalse(new Address(input, new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [TestMethod]
    public void NeedsToBeQPEncodedTest3()
        => Assert.IsFalse(new Address(";;;;;;", new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());
}
