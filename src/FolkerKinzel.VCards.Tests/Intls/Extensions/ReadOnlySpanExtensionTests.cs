using System.Text;
using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class ReadOnlySpanExtensionTests
{
    [TestMethod]
    public void UnMaskTest1()
    {
        const string s = @"a\nb";
        Assert.AreEqual("a" + Environment.NewLine + "b", s.AsSpan().UnMask(VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";
        string unMasked = s.AsSpan().UnMask(VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";
        string unMasked = s.AsSpan().UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest4b()
    {
        const string s = @"\\\\\N";
        string unMasked = s.AsSpan().UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\\N";
        string unMasked = s.AsSpan().UnMask(VCdVersion.V4_0);
        Assert.AreEqual(@"\\\N", unMasked);
    }

    [DataTestMethod]
    [DataRow("", VCdVersion.V3_0, "")]
    [DataRow("a\\bc", VCdVersion.V2_1, "a\\bc")]
    [DataRow("a\\;bc", VCdVersion.V2_1, "a;bc")]
    [DataRow("a\\nb", VCdVersion.V3_0, "a\r\nb")]
    [DataRow("a\\nb", VCdVersion.V2_1, "a\r\nb")]
    [DataRow("a\\b", VCdVersion.V3_0, "a\\b")]
    [DataRow("a\\n", VCdVersion.V4_0, "a\r\n")]
    [DataRow("a\\n", VCdVersion.V3_0, "a\r\n")]
    [DataRow("a\\n", VCdVersion.V2_1, "a\r\n")]
    [DataRow("ab\\", VCdVersion.V4_0, "ab\\")]
    [DataRow("a\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\\\", VCdVersion.V4_0, "a\\")]
    [DataRow("a\\\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\Nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\;\\,b", VCdVersion.V4_0, "a;,b")]
    public void UnMaskTest6(string input, VCdVersion version, string expected)
        => Assert.AreEqual(expected.Replace("\r\n", Environment.NewLine), input.AsSpan().UnMask(version), false);

    [TestMethod]
    public void UnmaskTest7()
    {
        string input = "\\n" + new string('a', 500);
        Assert.AreNotEqual(input, input.AsSpan().UnMask(VCdVersion.V2_1));
        Assert.AreNotEqual(input, input.AsSpan().UnMask(VCdVersion.V3_0));
    }

    [TestMethod]
    public void UnMaskAndDecodeTest1()
    {
        const string input = "abc\\,\\;\\n\\\\=\r\n=C3=A4";

        string decoded = input.AsSpan().UnMaskAndDecode("UTF-8");
        Assert.AreEqual("abc\\,;\r\n\\\\ä".Replace("\r\n", Environment.NewLine), decoded);
    }
    
}
