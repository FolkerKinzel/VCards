using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

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

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1829:Use Length/Count property instead of Count() when available", Justification = "Test")]
    public void IEnumerableTest1()
    {
        IReadOnlyList<IReadOnlyList<string>> adr = new Address(AddressBuilder.Create());
        Assert.AreEqual(adr.Count, adr.Count());
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    public void IEnumerableTest2()
    {
        IReadOnlyList<IReadOnlyList<string>> adr = new Address(AddressBuilder.Create());
        Assert.AreEqual(adr.Count, adr.AsWeakEnumerable().Count());
    }
}
