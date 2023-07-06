using System;
using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass]
public class StringExtensionTests
{
    [TestMethod]
    public void NeedsToBeQpEncodedTest1()
    {
        string? s = null;
        Assert.IsFalse(s.NeedsToBeQpEncoded());
    }

    [TestMethod]
    public void UnMaskTest1()
    {
        const string s = @"a\nb";
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\N";
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest6()
    {
        const string s = @"\\\\N";
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
        Assert.AreEqual(@"\\N", unMasked);
    }
}
