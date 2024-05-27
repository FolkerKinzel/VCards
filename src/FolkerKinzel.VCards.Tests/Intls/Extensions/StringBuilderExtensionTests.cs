using System.Text;
using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class StringBuilderExtensionTests
{
    [DataTestMethod]
    [DataRow("", VCdVersion.V2_1, "")]
    [DataRow("", VCdVersion.V3_0, "")]
    [DataRow("", VCdVersion.V4_0, "")]
    [DataRow(",;\r\n\n\\", VCdVersion.V2_1, ",\\;\r\n\n\\")]
    [DataRow(",;\r\n\n\\", VCdVersion.V3_0, "\\,\\;\\n\\n\\")]
    [DataRow(",;\r\n\n\\", VCdVersion.V4_0, "\\,\\;\\n\\n\\\\")]
    public void AppendMaskedTest1(string input, VCdVersion version, string expected)
    {
        string s = new StringBuilder().AppendMasked(input, version).ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void AppendEscapedAndQuotedTest1()
        => Assert.AreEqual("\",;\\n\\n\\\\\"", new StringBuilder().AppendEscapedAndQuoted(",;\"\"\r\n\n\\").ToString());
}
