using System;
using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass]
public class StringExtensionTests
{
    //[TestMethod]
    //public void NeedsToBeQpEncodedTest1()
    //{
    //    string? s = null;
    //    Assert.IsFalse(s.NeedsToBeQpEncoded());
    //}

    [TestMethod]
    public void UnMaskTest1()
    {
        const string s = @"a\nb";

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
Nach:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0));
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
Nach:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0));
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
Nach:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0));
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
Nach:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0));
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0));
Nach:
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0));
*/
        Assert.AreEqual("a" + Environment.NewLine + "b", s.UnMask(new StringBuilder(), VCdVersion.V4_0));
    }

    [TestMethod]
    public void UnMaskTest2()
    {
        const string s = @"a\nb";

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V3_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V3_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V3_0);
*/
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V3_0);
        Assert.AreEqual("a" + Environment.NewLine + "b", unMasked);
    }

    [TestMethod]
    public void UnMaskTest4()
    {
        const string s = @"\\\\\n";

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest5()
    {
        const string s = @"\\\\\N";

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\" + Environment.NewLine, unMasked);
    }

    [TestMethod]
    public void UnMaskTest6()
    {
        const string s = @"\\\\N";

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net5.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net6.0)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp3.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (net461)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/

/* Nicht gemergte Änderung aus Projekt "FolkerKinzel.VCards.Tests (netcoreapp2.1)"
Vor:
        string unMasked = s.UnMask(new StringBuilder(), Models.Enums.VCdVersion.V4_0);
Nach:
        string unMasked = s.UnMask(new StringBuilder(), VCards.VCdVersion.V4_0);
*/
        string unMasked = s.UnMask(new StringBuilder(), VCdVersion.V4_0);
        Assert.AreEqual(@"\\N", unMasked);
    }
}
