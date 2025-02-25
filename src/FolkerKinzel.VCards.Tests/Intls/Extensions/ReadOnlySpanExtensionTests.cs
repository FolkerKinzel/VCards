using FolkerKinzel.VCards.Enums;
namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class ReadOnlySpanExtensionTests
{
    [TestMethod]
    public void UnMaskTest1()
    {
        const string s = @"a\nb";
        Assert.AreEqual("a" + Environment.NewLine + "b", s.AsSpan().UnMaskValue(VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";
        string unMasked = s.AsSpan().UnMaskValue(VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";
        string unMasked = s.AsSpan().UnMaskValue(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest4b()
    {
        const string s = @"\\\\\N";
        string unMasked = s.AsSpan().UnMaskValue(VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\\N";
        string unMasked = s.AsSpan().UnMaskValue(VCdVersion.V4_0);
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
    [DataRow("a\\N", VCdVersion.V3_0, "a\r\n")]
    [DataRow("a\\n", VCdVersion.V2_1, "a\r\n")]
    [DataRow("ab\\", VCdVersion.V4_0, "ab\\")]
    [DataRow("a\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\\\", VCdVersion.V4_0, "a\\")]
    [DataRow("a\\\\b", VCdVersion.V4_0, "a\\b")]
    [DataRow("a\\nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\Nb", VCdVersion.V4_0, "a\r\nb")]
    [DataRow("a\\;\\,\\:b", VCdVersion.V4_0, "a;,\\:b")]
    [DataRow("a\\;\\,\\:b", VCdVersion.V3_0, "a;,:b")]
    [DataRow("a\\;\\,\\:b", VCdVersion.V2_1, "a;\\,\\:b")]
    public void UnMaskTest6(string input, VCdVersion version, string expected)
        => Assert.AreEqual(expected.Replace("\r\n", Environment.NewLine), input.AsSpan().UnMaskValue(version), false);

    [TestMethod]
    public void UnmaskTest7()
    {
        string input = "\\n" + new string('a', 500);
        Assert.AreNotEqual(input, input.AsSpan().UnMaskValue(VCdVersion.V2_1));
        Assert.AreNotEqual(input, input.AsSpan().UnMaskValue(VCdVersion.V3_0));
    }

    [TestMethod]
    public void UnMaskAndDecodeTest1()
    {
        const string input = "abc\\,\\;\\n\\\\=\r\n=C3=A4";

        string decoded = input.AsSpan().UnMaskAndDecodeValue("UTF-8");
        Assert.AreEqual("abc\\,;\r\n\\\\ä".Replace("\r\n", Environment.NewLine), decoded);
    }

    [DataTestMethod]
    [DataRow("a^'b\\n\\x^y", true, "a\"b\r\n\\x^y")]
    [DataRow("a^'b\\N", true, "a\"b\r\n")]
    [DataRow("a\\nb^^", true, "a\r\nb^")]
    [DataRow("a\\nb^\'", true, "a\r\nb\"")]
    [DataRow("a\\nb^n", true, "a\r\nb\r\n")]
    [DataRow("a^nb\\n", true, "a\r\nb\r\n")]
    [DataRow("a^'b\\n", false, "a\"b\\n")]
    [DataRow("a^'b\\N", false, "a\"b\\N")]
    [DataRow("a\\nb^^", false, "a\\nb^")]
    [DataRow("a^^b\\n", false, "a^b\\n")]
    [DataRow("a\\nb^n", false, "a\\nb\r\n")]
    [DataRow("^'a\\nb^n", false, "\"a\\nb\r\n")]
    public void UnMaskParameterValueTest1(string input, bool isLabel, string expected)
    {
        string decoded = input.AsSpan().UnMaskParameterValue(isLabel);
        Assert.AreEqual(expected.Replace("\r\n", Environment.NewLine), decoded);
    }

    [TestMethod]
    public void UnMaskParameterValueTest2()
    {
        string trailing = new('a', 500);
        string s = "^'" + trailing;

        Assert.AreEqual("\"" + trailing, s.AsSpan().UnMaskParameterValue(false));
    }

}
