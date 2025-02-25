using System.Text;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    [DataTestMethod]
    [DataRow("", VCdVersion.V2_1, "")]
    [DataRow("", VCdVersion.V3_0, "")]
    [DataRow("", VCdVersion.V4_0, "")]
    [DataRow(",;\\", VCdVersion.V2_1, ",\\;\\")]
    [DataRow(",;\r\n\n\\", VCdVersion.V3_0, "\\,\\;\\n\\n\\")]
    [DataRow(",;\r\n\n\\", VCdVersion.V4_0, "\\,\\;\\n\\n\\\\")]
    public void AppendMaskedTest1(string input, VCdVersion version, string expected)
    {
        string s = new StringBuilder().AppendValueMasked(input, version).ToString();
        Assert.AreEqual(expected, s);
    }

    [DataTestMethod]
    [DataRow(",;\"\"\r\n\n\\", true, VCdVersion.V4_0, "\",;^\'^\'\\n\\n\\\"")]
    [DataRow(",;\"\"\r\n\n\\", false, VCdVersion.V4_0, "\",;^\'^\'^n^n\\\"")]
    [DataRow("a\r\n\"b", true, VCdVersion.V3_0, "ab")]
    [DataRow("a\r\n\"b", true, VCdVersion.V4_0, "a\\n^'b")]
    [DataRow("a\r\n\"b", false, VCdVersion.V4_0, "a^n^'b")]
    [DataRow("a,b", false, VCdVersion.V4_0, "\"a,b\"")]
    [DataRow("a,b", false, VCdVersion.V3_0, "\"a,b\"")]
    [DataRow("a;b", false, VCdVersion.V4_0, "\"a;b\"")]
    [DataRow("a;b", false, VCdVersion.V3_0, "\"a;b\"")]
    [DataRow("a:b", false, VCdVersion.V4_0, "\"a:b\"")]
    [DataRow("a:b", false, VCdVersion.V3_0, "\"a:b\"")]
    [DataRow("a^b", false, VCdVersion.V4_0, "a^^b")]
    [DataRow("a^b", false, VCdVersion.V3_0, "a^b")]
    public void AppendParameterValueEscapedAndQuotedTest1(string input, bool isLabel, VCdVersion version, string expected)
        => Assert.AreEqual(expected,
            new StringBuilder().AppendParameterValueEscapedAndQuoted(input, version, isLabel: isLabel).ToString());

    [DataTestMethod]
    [DataRow(VCdVersion.V4_0)]
    [DataRow(VCdVersion.V3_0)]
    public void AppendParameterValueEscapedAndQuotedTest2(VCdVersion version)
    => Assert.AreEqual("\"\"",
            new StringBuilder().AppendParameterValueEscapedAndQuoted(null, version).ToString());
}
