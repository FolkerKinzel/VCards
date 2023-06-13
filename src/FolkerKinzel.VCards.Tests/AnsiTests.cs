using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.Strings;
using FolkerKinzel.VCards.Utilities;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class AnsiTests
{
    [TestMethod]
    public void AnsiTest1() 
    {
        const string original = "ÄÖÜßäöü";

        var ansi = TextEncodingConverter.GetEncoding("Windows-1252");

        var ansiBytes = ansi.GetBytes(original);

        var utf8 = new UTF8Encoding(false, true);

        string utf8String = utf8.GetString(ansiBytes);
        var utf8Bytes = utf8.GetBytes(utf8String);

        string original2 = ansi.GetString(utf8Bytes);
    }

    [TestMethod]
    public void AnsiTest2()
    {
        const string original = "ÄÖÜßäöü";

        var ansi = TextEncodingConverter.GetEncoding(1252);

        var ansiBytes = ansi.GetBytes(original);

        var utf8 = TextEncodingConverter.GetEncoding(1251);

        string utf8String = utf8.GetString(ansiBytes);
        var utf8Bytes = utf8.GetBytes(utf8String);

        string original2 = ansi.GetString(utf8Bytes);
    }


    [TestMethod]
    public void AnsiFilterTest1() 
    {
        var filter = new AnsiFilter();
        var vCards = filter.LoadVcf(TestFiles.AnsiIssuevcf);
    }
}
