using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class NameTests
{
    [DataTestMethod]
    [DataRow("a;;;;")]
    [DataRow(";a;;;")]
    [DataRow(";;a;;")]
    [DataRow(";;;a;")]
    [DataRow(";;;;a")]
    public void IsEmptyTest1(string input)
        => Assert.IsFalse(new Name(input, new VcfDeserializationInfo(), VCdVersion.V4_0).IsEmpty);

    [TestMethod]
    public void AppendVCardStringTest1()
    {
        const string input = ";;Heinrich,August;;";
        var name = new Name(input, new VcfDeserializationInfo(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, VcfOptions.Default);
        name.AppendVCardString(serializer);

        Assert.AreEqual(input, serializer.Builder.ToString());
    }

    [DataTestMethod]
    [DataRow("ä;;;;")]
    [DataRow(";ä;;;")]
    [DataRow(";;ä;;")]
    [DataRow(";;;ä;")]
    [DataRow(";;;;ä")]
    public void NeedsToBeQPEncodedTest1(string input)
        => Assert.IsTrue(new Name(input, new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow("a,ä;;;;")]
    [DataRow(";a,ä;;;")]
    [DataRow(";;a,ä;;")]
    [DataRow(";;;a,ä;")]
    [DataRow(";;;;a,ä")]
    public void NeedsToBeQPEncodedTest1b(string input)
        => Assert.IsTrue(new Name(input, new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [DataTestMethod]
    [DataRow("a,a;a;a;a;a")]
    [DataRow(";;;;")]
    public void NeedsToBeQPEncodedTest2(string input)
        => Assert.IsFalse(new Name(input, new VcfDeserializationInfo(), VCdVersion.V2_1).NeedsToBeQpEncoded());

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Heinrich,August;;;;";
        var name = new Name(input, new VcfDeserializationInfo(), VCdVersion.V4_0);

        string s = name.ToString();
        StringAssert.Contains(s, "August");
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var name = new Name();
        Assert.IsTrue(name.IsEmpty);
        Assert.IsNotNull(name.ToString());
    }
}
