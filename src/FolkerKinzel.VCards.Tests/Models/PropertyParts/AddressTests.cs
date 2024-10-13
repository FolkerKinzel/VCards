using FolkerKinzel.VCards.Enums;
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
        const string input = ";;Parkstr.,7a;   ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;";
        var adr = new Address(input.AsMemory(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, Opts.Default);
        adr.AppendVcfString(serializer);

        Assert.AreEqual(";;Parkstr.,7a;;;;", serializer.Builder.ToString());
    }

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Parkstr.,7a;;;;";
        var adr = new Address(input.AsMemory(), VCdVersion.V4_0);

        string s = adr.ToString();
        StringAssert.Contains(s, "7a");
    }

}
