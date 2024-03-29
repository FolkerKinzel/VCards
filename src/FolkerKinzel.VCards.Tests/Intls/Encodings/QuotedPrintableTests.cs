﻿using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Intls.Encodings.Tests;

[TestClass]
public class QuotedPrintableTests
{
    [TestMethod]
    public void DecodeStringTest1()
    {
        string quoted = $"1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand={Environment.NewLine} Firma";

        string? s = QuotedPrintable.Decode(quoted, null);

        Assert.IsNotNull(s);
        StringAssert.Contains(s, "Firmenstraße");
        StringAssert.EndsWith(s, " Firma");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void DecodeStringTest2(string? quoted)
        => Assert.AreEqual(0, QuotedPrintable.Decode(quoted, null).Length);


    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void DecodeDataTest1(string? quoted)
    {
        byte[] data = QuotedPrintable.DecodeData(quoted);
        Assert.IsNotNull(data);
        Assert.AreEqual(0, data.Length);
    }


    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void EncodeStringTest1(string? quoted)
        => Assert.AreEqual(string.Empty, QuotedPrintable.Encode(quoted, 0));


    [TestMethod]
    public void EncodeDataTest1()
    {
        var s = new string(' ', 100);
        string quoted = QuotedPrintable.Encode(s, 0);
        Assert.IsNotNull(quoted);
        Assert.AreNotEqual(quoted[quoted.Length - 1], ' ');
        Assert.AreEqual(2, quoted.GetLinesCount());
    }

    [TestMethod]
    public void TruncatedTextTest1() => Assert.AreEqual("abc", QuotedPrintable.Decode("abc=C", null));
}
