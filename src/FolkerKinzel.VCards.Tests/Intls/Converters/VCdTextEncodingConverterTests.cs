using System.Text;

#if !NET45
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class VCdTextEncodingConverterTests
{
    private const int UTF8 = 65001;

    [TestMethod]
    public void ParseTest1()
    {
        Encoding enc = TextEncodingConverter.GetEncoding("blödelidödel");
        Assert.AreEqual(UTF8, enc.CodePage);
    }

    [TestMethod]
    public void ParseTest2()
    {
        Encoding enc = TextEncodingConverter.GetEncoding("ISO-8859-1");
        Assert.AreEqual(28591, enc.CodePage);
    }

    [DataTestMethod]
    [DataRow(null, 65001)]
    [DataRow("iso-8859-1", 28591)]
    [DataRow("ISO-8859-1", 28591)]
    [DataRow("unBekannt", 65001)]
    public void GetEncodingTest1(string? input, int codePage)
    {
        Encoding enc = TextEncodingConverter.GetEncoding(input);
        Assert.AreEqual(codePage, enc.CodePage);
        Assert.IsNotInstanceOfType(enc.EncoderFallback, EncoderFallback.ExceptionFallback.GetType());
        Assert.IsNotInstanceOfType(enc.DecoderFallback.GetType(), DecoderFallback.ExceptionFallback.GetType());
    }

    [DataTestMethod]
    [DataRow(65001)]
    [DataRow(28591)]
    public void GetEncodingTest3(int codePage)
    {
        Encoding enc = TextEncodingConverter.GetEncoding(codePage);
        Assert.AreEqual(codePage, enc.CodePage);
        Assert.IsNotInstanceOfType(enc.EncoderFallback, EncoderFallback.ExceptionFallback.GetType());
        Assert.IsNotInstanceOfType(enc.DecoderFallback.GetType(), DecoderFallback.ExceptionFallback.GetType());
    }

    [DataTestMethod]
    [DataRow(-17)]
    [DataRow(int.MaxValue)]
    [DataRow(42)]
    public void GetEncodingTest7(int codePage)
    {
        Encoding enc = TextEncodingConverter.GetEncoding(codePage);
        Assert.AreEqual(65001, enc.CodePage);
        Assert.IsNotInstanceOfType(enc.EncoderFallback, EncoderFallback.ExceptionFallback.GetType());
        Assert.IsNotInstanceOfType(enc.DecoderFallback.GetType(), DecoderFallback.ExceptionFallback.GetType());
    }

    [TestMethod]
    public void GetEncodingTest5()
    {
        const int defaultCodePage = 65001;
        Encoding enc = TextEncodingConverter.GetEncoding(0);
        Assert.AreEqual(defaultCodePage, enc.CodePage);
        Assert.IsNotInstanceOfType(enc.EncoderFallback, EncoderFallback.ExceptionFallback.GetType());
        Assert.IsNotInstanceOfType(enc.DecoderFallback.GetType(), DecoderFallback.ExceptionFallback.GetType());
    }
}
