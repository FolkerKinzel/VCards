using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class StringCollectionPropertyTests
{
    private const string GROUP = "myGroup";

    [DataTestMethod]
    [DataRow(new string?[] { "Bodo, der Blöde", "Dumbo" }, GROUP, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "Bodo, der Blöde", "  ", "Dumbo" }, GROUP, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "  " }, GROUP, null)]
    [DataRow(new string?[] { }, GROUP, null)]
    [DataRow(new string?[] { "Bodo, der Blöde", "Dumbo" }, null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "Bodo, der Blöde", "  ", "Dumbo" }, null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "  " }, null, null)]
    [DataRow(new string?[] { }, null, null)]
    public void StringCollectionPropertyTest1(string?[]? inputNickNames, string? expectedGroup, string[]? expectedNickNames)
    {
        var nickNameProp = new StringCollectionProperty(inputNickNames, expectedGroup);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value);
    }


    [DataTestMethod]
    [DataRow("Dumbo", GROUP, new string[] { "Dumbo" })]
    [DataRow(null, GROUP, null)]
    [DataRow("", GROUP, null)]
    [DataRow("  ", GROUP, null)]
    [DataRow("Dumbo", null, new string[] { "Dumbo" })]
    [DataRow(null, null, null)]
    [DataRow("", null, null)]
    [DataRow("  ", null, null)]
    public void StringCollectionPropertyTest2(string? s, string? expectedGroup, string[]? expectedNickNames)
    {
        var nickNameProp = new StringCollectionProperty(s, expectedGroup);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value);
    }


    [DataTestMethod]
    [DataRow(GROUP + ".NICKNAME:", GROUP, null)]
    [DataRow(GROUP + @".NICKNAME:Bodo\, der Blöde,Dumbo", GROUP, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(@"NICKNAME:Bodo\, der Blöde,Dumbo", null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(@"NICKNAME:,Bodo\, der Blöde,  ,Dumbo, ", null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(@"NICKNAME: , ,,", null, null)]
    [DataRow(@"NICKNAME:Dumbo, ", null, new string[] { "Dumbo" })]
    public void StringCollectionPropertyTest3(string s, string? expectedGroup, string[]? expectedNickNames)
    {
        var vcfRow = VcfRow.Parse(s, new VcfDeserializationInfo());
        Assert.IsNotNull(vcfRow);

        var nickNameProp = new StringCollectionProperty(vcfRow!, VCdVersion.V4_0);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value);
    }


    [TestMethod]
    public void ToStringTest1()
    {
        string s = new StringCollectionProperty(new string[] { "Bla", "Blub" }).ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
    }


    [TestMethod]
    public void ToStringTest2()
    {
        string? s = null;
        s = new StringCollectionProperty(s).ToString();

        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);
    }

    [TestMethod]
    public void AppendValueTest1()
    {
        var prop = new StringCollectionProperty((string?)null);
        Assert.IsTrue(prop.IsEmpty);

        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOptions.Default, null);

        prop.AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new StringCollectionProperty("");
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
