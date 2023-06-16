using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.Strings;
using FolkerKinzel.VCards;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class AnsiFilterTests
{
    [TestMethod]
    public void AnsiFilterTest1()
    {
        var filter = new AnsiFilter();
        Assert.AreEqual(1252, filter.FallbackEncoding.CodePage);
        Assert.IsTrue(filter.LoadVcf(TestFiles.AnsiIssueVcf, out IList<VCard> vCards));
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    public void AnsiFilterTest2()
    {
        var filter = new AnsiFilter();
        Assert.IsFalse(filter.LoadVcf(TestFiles.V4vcf, out IList<VCard> vCards));
        Assert.IsNotNull(vCards);
        Assert.AreEqual(2, vCards.Count);
    }

    [TestMethod]
    public void AnsiFilterTest3()
    {
        var filter = new AnsiFilter("Nixda");
        Assert.AreEqual("utf-8", filter.FallbackEncoding.WebName);
    }

    [TestMethod]
    public void AnsiFilterTest4()
    {
        var filter = new AnsiFilter();
        Assert.AreEqual(1252, filter.FallbackEncoding.CodePage);
        Assert.IsTrue(filter.LoadVcf(TestFiles.LabelIssueVcf, out IList<VCard> vCards));
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    public void AnsiFilterTest5()
    {
        var filter = new AnsiFilter(1253);
        Assert.AreEqual(1253, filter.FallbackEncoding.CodePage);
        Assert.IsTrue(filter.LoadVcf(TestFiles.MultiAnsiFilterTests_GreekVcf, out IList<VCard> vCards));
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest6()
    {
        var filter = new AnsiFilter(4711);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest7()
    {
        var filter = new AnsiFilter("Nixda");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AnsiFilterTest8()
    {
        var filter = new AnsiFilter(null!);
    }


    //[TestMethod]
    //public void AnsiTest1() 
    //{
    //    const string original = "ÄÖÜßäöü";

    //    var ansi = TextEncodingConverter.GetEncoding("Windows-1252");

    //    var ansiBytes = ansi.GetBytes(original);

    //    var utf8 = new UTF8Encoding(false, true);

    //    string utf8String = utf8.GetString(ansiBytes);
    //    var utf8Bytes = utf8.GetBytes(utf8String);

    //    string original2 = ansi.GetString(utf8Bytes);
    //}

    //[TestMethod]
    //public void AnsiTest2()
    //{
    //    const string original = "ÄÖÜßäöü";

    //    var ansi = TextEncodingConverter.GetEncoding(1252);

    //    var ansiBytes = ansi.GetBytes(original);

    //    var utf8 = TextEncodingConverter.GetEncoding(1251);

    //    string utf8String = utf8.GetString(ansiBytes);
    //    var utf8Bytes = utf8.GetBytes(utf8String);

    //    string original2 = ansi.GetString(utf8Bytes);
    //}



}
