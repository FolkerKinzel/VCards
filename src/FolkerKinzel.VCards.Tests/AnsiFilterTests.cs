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
        Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
        IList<VCard> vCards = filter.LoadVcf(TestFiles.AnsiIssueVcf, out string encodingWebName);
        Assert.AreEqual(filter.FallbackEncodingWebName, encodingWebName, true);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("Lämmerweg 12", vCards[0].Addresses!.First()!.Value.Street.First());
    }

    [TestMethod]
    public void AnsiFilterTest10()
    {
        IList<VCard> vCards = new AnsiFilter().LoadVcf(TestFiles.AnsiIssueVcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("Lämmerweg 12", vCards[0].Addresses!.First()!.Value.Street.First());
    }

    [TestMethod]
    public void AnsiFilterTest2()
    {
        var filter = new AnsiFilter();
        IList<VCard> vCards = filter.LoadVcf(TestFiles.V4vcf, out string encodingWebName);
        Assert.AreEqual("utf-8", encodingWebName);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(2, vCards.Count);
    }

    [TestMethod]
    public void AnsiFilterTest4()
    {
        var filter = new AnsiFilter();
        Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
        IList<VCard> vCards = filter.LoadVcf(TestFiles.LabelIssueVcf, out string encodingWebName);
        Assert.AreEqual(filter.FallbackEncodingWebName, encodingWebName);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    public void AnsiFilterTest5()
    {
        var filter = new AnsiFilter(1253);
        Assert.AreEqual("windows-1253", filter.FallbackEncodingWebName, true);
        IList<VCard> vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_GreekVcf, out string encodingWebName);
        Assert.AreEqual(filter.FallbackEncodingWebName, encodingWebName);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest6() => _ = new AnsiFilter(4711);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest7() => _ = new AnsiFilter("Nixda");

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AnsiFilterTest8() => _ = new AnsiFilter(null!);

    [TestMethod()]
    public void AnsiFilterTest9()
    {
        var filter = new MultiAnsiFilter("windows-1252");
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
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
