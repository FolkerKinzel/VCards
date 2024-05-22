using System.Text;
using FolkerKinzel.VCards.Tests;
namespace FolkerKinzel.VCards.Intls.Encodings.Tests;

[TestClass]
public class QuotedPrintableTests
{
    [DataTestMethod]
    [DataRow("ä", "=C3=A4", 0)]
    [DataRow("ä", "=C3=A4", 69)]
    [DataRow("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaä", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa=C3=A4", 0)]
    [DataRow("äa", "=C3==\r\nA4a", 70)]
    [DataRow("ä", "=C3=\r\n=A4", 71)]
    [DataRow("ä", "=C=\r\n3=A4", 72)]
    [DataRow("ä", "==\r\nC3=A4", 73)]
    [DataRow("ä", "=\r\n=C3=A4", 74)]
    [DataRow("ä", "=\r\n=C3=A4", 75)]
    [DataRow("abc", "abc", 72)]
    [DataRow("", "", 0)]
    [DataRow(null, "", 0)]
    [DataRow("a", "a", 0)]
    [DataRow(" ", "=20", 0)]
    [DataRow("\t", "=09", 0)]
    [DataRow("aaa\t", "aaa==\r\n09", 70)]
    [DataRow("aa\t", "aa=09", 70)]
    public void AppendQuotedPrintableTest1(string? input, string expected, int firstLineOffset)
    {
        string output = new StringBuilder().AppendQuotedPrintable(input.AsSpan(), firstLineOffset).ToString();
        Assert.AreEqual(expected, output);
    }


    [TestMethod]
    public void DecodeStringTest1()
    {
        string quoted = $"1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand=\r\n Firma";

        string? s = QuotedPrintable.Decode(quoted.AsSpan(), null);

        Assert.IsNotNull(s);
        StringAssert.Contains(s, "Firmenstraße");
        StringAssert.EndsWith(s, " Firma");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void DecodeStringTest2(string? quoted)
        => Assert.AreEqual(0, QuotedPrintable.Decode(quoted.AsSpan(), null).Length);


    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void DecodeDataTest1(string? quoted)
    {
        byte[] data = QuotedPrintable.DecodeData(quoted.AsSpan());
        Assert.IsNotNull(data);
        Assert.AreEqual(0, data.Length);
    }


    //[DataTestMethod]
    //[DataRow(null)]
    //[DataRow("")]
    //public void EncodeStringTest1(string? quoted)
    //    => Assert.AreEqual(string.Empty, QuotedPrintable.Encode(quoted, 0));


    [TestMethod]
    public void EncodeDataTest1()
    {
        string s = new(' ', 100);
        string quoted = new StringBuilder().AppendQuotedPrintable(s.AsSpan(), 0).ToString();
        Assert.IsNotNull(quoted);
        Assert.AreNotEqual(quoted[quoted.Length - 1], ' ');
        Assert.AreEqual(2, quoted.GetLinesCount());
    }

    [TestMethod]
    public void TruncatedTextTest1() => Assert.AreEqual("abcC", QuotedPrintable.Decode("abc=C".AsSpan(), null));

    [DataTestMethod]
    [DataRow("abc==\r\nC3=A4","abcä")]
    [DataRow("abc=C=\r\n3=A4", "abcä")]
    [DataRow("abc=C3=\r\n=A4", "abcä")]
    [DataRow("abc==\r\nc3=a4", "abcä")]
    [DataRow("abc=c=\r\n3=a4", "abcä")]
    [DataRow("abc=c3=\r\n=a4", "abcä")]
    public void SoftLineBreaks(string input, string expected) => Assert.AreEqual(expected, QuotedPrintable.Decode(input.AsSpan(), null));

}
