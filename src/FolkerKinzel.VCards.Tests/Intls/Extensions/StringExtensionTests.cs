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
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";
        string unMasked = s.UnMask(VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";
        string unMasked = s.UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest4b()
    {
        const string s = @"\\\\\N";
        string unMasked = s.UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\\N";
        string unMasked = s.UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\\N", unMasked);
    }

    [DataTestMethod]
    [DataRow("", VCdVersion.V3_0, "")]
    [DataRow("a\\bc", VCdVersion.V2_1, "a\\bc")]
    [DataRow("a\\;bc", VCdVersion.V2_1, "a;bc")]
    [DataRow("a\\nb", VCdVersion.V3_0, "a\r\nb")]
    [DataRow("a\\nb", VCdVersion.V2_1, "a\\nb")]
    [DataRow("a\\b", VCdVersion.V3_0, "a\\b")]
    [DataRow("a\\n", VCdVersion.V3_0, "a\r\n")]
    [DataRow("ab\\", VCdVersion.V4_0, "ab\\")]
    [DataRow("a\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\\\", VCdVersion.V4_0, "a\\")]
    [DataRow("a\\\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\Nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\;\\,b", VCdVersion.V4_0, "a;,b")]
    public void UnMaskTest6(string input, VCdVersion version, string? expected)
        => Assert.AreEqual(expected, input.UnMask(version), false);

    [TestMethod]
    public void UnmaskTest7()
    {
        string input = "\\n" + new string('a', 500);
        Assert.AreEqual(input, input.UnMask(VCdVersion.V2_1));
        Assert.AreNotEqual(input, input.UnMask(VCdVersion.V3_0));
    }


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
