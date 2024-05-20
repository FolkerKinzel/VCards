using System.Text;
using FolkerKinzel.VCards.Enums;
namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class StringExtensionTests
{
    [TestMethod]
    public void UnMaskTest1()
    {
        const string s = @"a\nb";
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest4b()
    {
        const string s = @"\\\\\N";
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\\N";
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\\N", unMasked);
    }

    [DataTestMethod]
    [DataRow("", VCdVersion.V3_0, "")]
    [DataRow("a\\bc", VCdVersion.V2_1, "a\\bc")]
    [DataRow("a\\nb", VCdVersion.V3_0, "a\r\nb")]
    public void UnMaskTest6(string input, VCdVersion version, string? expected)
        => Assert.AreEqual(expected, input.UnMask(new StringBuilder(), version), false);


    //[DataTestMethod]
    //[DataRow("", VCdVersion.V3_0, "")]
    //[DataRow(null, VCdVersion.V3_0, null)]
    //[DataRow("a\\bc", VCdVersion.V2_1, "a\\bc")]
    //[DataRow("a\r\nb", VCdVersion.V2_1, "a\r\nb")]
    //[DataRow("a\r\nb", VCdVersion.V4_0, "a\\nb")]
    //[DataRow("a,b", VCdVersion.V2_1, "a,b")]
    //[DataRow("a,b", VCdVersion.V3_0, "a\\,b")]
    //[DataRow("a,b", VCdVersion.V4_0, "a\\,b")]
    //[DataRow("a;b", VCdVersion.V2_1, "a\\;b")]
    //[DataRow("a;b", VCdVersion.V3_0, "a\\;b")]
    //[DataRow("a;b", VCdVersion.V4_0, "a\\;b")]
    //[DataRow("a\\b", VCdVersion.V2_1, "a\\b")]
    //[DataRow("a\\b", VCdVersion.V3_0, "a\\b")]
    //[DataRow("a\\b", VCdVersion.V4_0, "a\\\\b")]
    //public void MaskTest1(string? input, VCdVersion version, string? expected)
    //    => Assert.AreEqual(expected, input.Mask(new StringBuilder(), version), false);
}
