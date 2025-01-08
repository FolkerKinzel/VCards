using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class StringCollectionPropertyTests
{
    private const string GROUP = "myGroup";

    [DataTestMethod]
    [DataRow(new string?[] { "Bodo, der Blöde", "Dumbo" }, GROUP, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "Bodo, der Blöde", "  ", "Dumbo" }, GROUP, new string[] { "", "", "Bodo, der Blöde", "  ",  "Dumbo" })]
    [DataRow(new string?[] { "", null, "  " }, GROUP, new string[] { "", "", "  " })]
    [DataRow(new string[] { }, GROUP, new string[] { })]
    [DataRow(new string?[] { "Bodo, der Blöde", "Dumbo" }, null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(new string?[] { "", null, "Bodo, der Blöde", "  ", "Dumbo" }, null, new string[] { "", "", "Bodo, der Blöde",  "  ", "Dumbo" })]
    [DataRow(new string?[] { "", null, "  " }, null, new string[] { "", "", "  " })]
    [DataRow(new string[] { }, null, new string[] { })]
    public void StringCollectionPropertyTest1(string?[] inputNickNames, string? expectedGroup, string[]? expectedNickNames)
    {
        var nickNameProp = new StringCollectionProperty(inputNickNames, expectedGroup);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value.ToArray());
    }


    [DataTestMethod]
    [DataRow("Dumbo", GROUP, new string[] { "Dumbo" })]
    [DataRow(null, GROUP, new string[0])]
    [DataRow("", GROUP, new string[0])]
    [DataRow("  ", GROUP, new string[] { "  " })]
    [DataRow("Dumbo", null, new string[] { "Dumbo" })]
    [DataRow(null, null, new string[0])]
    [DataRow("", null, new string[0])]
    [DataRow("  ", null, new string[] { "  " })]
    public void StringCollectionPropertyTest2(string? s, string? expectedGroup, string[]? expectedNickNames)
    {
        var nickNameProp = new StringCollectionProperty(s, expectedGroup);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value.ToArray());
    }


    [DataTestMethod]
    [DataRow(GROUP + ".NICKNAME:", GROUP, new string[0])]
    [DataRow(GROUP + @".NICKNAME:Bodo\, der Blöde,Dumbo", GROUP, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(@"NICKNAME:Bodo\, der Blöde,Dumbo", null, new string[] { "Bodo, der Blöde", "Dumbo" })]
    [DataRow(@"NICKNAME:,Bodo\, der Blöde,  ,Dumbo, ", null, new string[] { "", "Bodo, der Blöde", "  ", "Dumbo", " " })]
    [DataRow(@"NICKNAME: , ,,", null, new string[] { " ", " ", "", ""})]
    [DataRow(@"NICKNAME:Dumbo, ", null, new string[] { "Dumbo", " " })]
    public void StringCollectionPropertyTest3(string s, string? expectedGroup, string[]? expectedNickNames)
    {
        var vcfRow = VcfRow.Parse(s.AsMemory(), new VcfDeserializationInfo());
        Assert.IsNotNull(vcfRow);

        var nickNameProp = new StringCollectionProperty(vcfRow!, VCdVersion.V4_0);

        Assert.AreEqual(expectedGroup, nickNameProp.Group);
        CollectionAssert.AreEqual(expectedNickNames, nickNameProp.Value.ToArray());
    }


    [TestMethod]
    public void ToStringTest1()
    {
        string s = new StringCollectionProperty(["Bla", "Blub"]).ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);
    }


    [TestMethod]
    public void ToStringTest2()
    {
        string? s = null;
        var prop = new StringCollectionProperty(s);

        Assert.IsTrue(prop.IsEmpty);
        Assert.IsNotNull(prop.ToString());
    }

    [TestMethod]
    public void AppendValueTest1()
    {
        var prop = new StringCollectionProperty((string?)null);
        Assert.IsTrue(prop.IsEmpty);

        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOpts.Default, null);

        prop.AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new StringCollectionProperty("");
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new StringCollectionProperty("text");

        var prop2 = (StringCollectionProperty)prop1.Clone();

        Assert.AreSame(prop1.Value, prop2.Value);
        Assert.AreNotSame(prop1, prop2);
    }
}
