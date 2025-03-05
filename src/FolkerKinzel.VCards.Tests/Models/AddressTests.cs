using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;

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
        var serializer = new Vcf_4_0Serializer(writer, VcfOpts.Default);
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

    //[TestMethod]
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
    //    "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
    //    "CA1829:Use Length/Count property instead of Count() when available", Justification = "Test")]
    //public void IEnumerableTest1()
    //{
    //    IReadOnlyList<IReadOnlyList<string>> adr = new Address(AddressBuilder.Create());
    //    Assert.AreEqual(adr.Count, adr.Count());
    //}

    //[TestMethod]
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
    //    "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    //public void IEnumerableTest2()
    //{
    //    IReadOnlyList<IReadOnlyList<string>> adr = new Address(AddressBuilder.Create());
    //    Assert.AreEqual(adr.Count, adr.AsWeakEnumerable().Count());
    //}

    [TestMethod]
    public void EqualsTest1()
    {
        var address1 = Address.Empty;
        var address2 = address1;
        Assert.IsTrue(address1 == address2);
        Assert.IsFalse(address1 != address2);
        object o1 = address1;
        object o2 = address2;
        Assert.IsTrue(o1.Equals(o2));
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());

        Assert.IsFalse(o1.Equals(42));
    }

    [TestMethod]
    public void EqualsTest2()
    {
        var address1 = AddressBuilder.Create().AddLocality("Berlin").Build();
        
        Address? address2 = null;
        Assert.IsFalse(address1.Equals(address2));
        Assert.IsFalse(address1 == address2);
        Assert.IsFalse(address2 == address1);

        var address3 = AddressBuilder.Create().AddLocality("Berlin").Build();

        Assert.IsTrue(address1.Equals(address3));
        Assert.AreEqual(address1.GetHashCode(), address3.GetHashCode());
    }
}
