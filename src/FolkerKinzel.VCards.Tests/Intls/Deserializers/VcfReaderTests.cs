using System.Collections;
using System.Text;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass()]
public class VcfReaderTests
{
    [TestMethod()]
    public void VcfReaderCtorTest()
    {
        using var reader = new StringReader("");
        _ = new VcfReader(reader, new VcfDeserializationInfo());
    }


    [TestMethod()]
    public void SequenceOfTwoV3_0VcardsTest()
    {
        using StreamReader reader = File.OpenText(TestFiles.V3vcf);
        var info = new VcfDeserializationInfo();

        var list = new List<VcfRow>();

        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreNotEqual(0, list.Count);
        Assert.IsFalse(vcReader.EOF);

        list.Clear();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreNotEqual(0, list.Count);
        Assert.IsFalse(vcReader.EOF);

        list.Clear();


        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(0, list.Count);
        Assert.IsTrue(vcReader.EOF);
    }


    [TestMethod]
    public void V2NestedAgentVcardTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("AGENT:")
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("N:Friday;Fred")
            .AppendLine("TEL;WORK;VOICE:+1-213-555-1234")
            .AppendLine("END:VCARD")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("AGENT", list[1].Key);
        StringAssert.Contains(list[1].Value, "Friday;Fred");
        StringAssert.EndsWith(list[1].Value, "END:VCARD");

        Assert.IsFalse(vcReader.EOF);
    }


    #region v2.1_Base64

    [TestMethod]
    public void Base64v2SingleLineTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAMCAgICAgM")
            .AppendLine()
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("PHOTO", list[1].Key);
        StringAssert.EndsWith(list[1].Value, "ICAgM");

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void Base64v2TwoLinesTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAMCAgICAgM")
            .AppendLine("LastLine==")
            .AppendLine()
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("PHOTO", list[1].Key);
        StringAssert.EndsWith(list[1].Value, "LastLine==");
    }

    [TestMethod]
    public void Base64v2ThreeLinesTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAMCAgICAgM")
            .AppendLine("SecondLine")
            .AppendLine("LastLine==")
            .AppendLine()
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("PHOTO", list[1].Key);
        StringAssert.Contains(list[1].Value, "SecondLine");
        StringAssert.EndsWith(list[1].Value, "LastLine==");

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void Base64v2OutlookTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:")
            .AppendLine(" /9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAMCAgICAgM")
            .AppendLine(" SecondLine")
            .AppendLine(" LastLine==")
            .AppendLine()
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("PHOTO", list[1].Key);
        StringAssert.Contains(list[1].Value, "SecondLine");
        StringAssert.EndsWith(list[1].Value, "LastLine==");

        Assert.IsFalse(vcReader.EOF);
    }

    #endregion


    #region QuotedPrintable

    [TestMethod]
    public void QuotedPrintableSingleLineTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("TEL;WORK;VOICE;CHARSET=utf-8;ENCODING=QUOTED-PRINTABLE:Tel gesch=C3=A4ftlich")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);

        Assert.IsFalse(vcReader.EOF);
    }



    [TestMethod]
    public void QuotedPrintableTwoLinesTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("LABEL;WORK;CHARSET=utf-8;ENCODING=QUOTED-PRINTABLE:1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand=")
            .AppendLine(" Firma")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("LABEL", list[1].Key);
        Assert.IsTrue(list[1].Value?.EndsWith(" Firma") ?? false);

        Assert.IsFalse(vcReader.EOF);
    }

    [TestMethod]
    public void QuotedPrintableThreeLinesTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("LABEL;WORK;CHARSET=utf-8;ENCODING=QUOTED-PRINTABLE:1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand=")
            .AppendLine("Line 2=")
            .AppendLine(" Firma")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("LABEL", list[1].Key);
        StringAssert.EndsWith(list[1].Value, " Firma");
        StringAssert.Contains(list[1].Value, "Line 2");

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void QuotedPrintableThreeLines_B_Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("LABEL;WORK;CHARSET=utf-8;ENCODING=QUOTED-PRINTABLE:1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand=")
            .AppendLine(" Line 2=")
            .AppendLine("Firma")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("LABEL", list[1].Key);
        StringAssert.EndsWith(list[1].Value, "Firma");
        StringAssert.Contains(list[1].Value, " Line 2");

        Assert.IsFalse(vcReader.EOF);
    }

    #endregion


    #region LineWrapping

    [TestMethod]
    public void LineWrappingV4Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:4.0")
            .AppendLine("FN:1234")
            .AppendLine(" 5678")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("12345678", list[1].Value);

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void LineWrappingV3Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:3.0")
            .AppendLine("FN:1234")
            .AppendLine(" 5678")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("12345678", list[1].Value);

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void LineWrappingV2Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("FN:1234")
            .AppendLine(" 5678")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("1234 5678", list[1].Value);

        Assert.IsFalse(vcReader.EOF);
    }

    #endregion


    #region PropertiesWithoutWrapping

    [TestMethod]
    public void SimplePropsV4Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:4.0")
            .AppendLine("FN:KMS WSF")
            .AppendLine("N:KMS;WSF;;;")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);

        Assert.IsFalse(vcReader.EOF);
    }

    [TestMethod]
    public void SimplePropsV3Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:3.0")
            .AppendLine("FN:KMS WSF")
            .AppendLine("N:KMS;WSF;;;")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);

        Assert.IsFalse(vcReader.EOF);
    }


    [TestMethod]
    public void SimplePropsV2Test()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("FN:KMS WSF")
            .AppendLine("N:KMS;WSF;;;")
            .AppendLine("END:VCARD");

        using var reader = new StringReader(sb.ToString());
        var vcReader = new VcfReader(reader, new VcfDeserializationInfo());

        var list = new List<VcfRow>();

        foreach (VcfRow vcfRow in vcReader)
        {
            list.Add(vcfRow);
        }

        Assert.AreEqual(3, list.Count);

        Assert.IsFalse(vcReader.EOF);
    }

    #endregion


    [TestMethod]
    public void IEnumerableTest()
    {
        StringBuilder? sb = new StringBuilder()
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("AGENT:")
            .AppendLine("BEGIN:VCARD")
            .AppendLine("VERSION:2.1")
            .AppendLine("N:Friday;Fred")
            .AppendLine("TEL;WORK;VOICE:+1-213-555-1234")
            .AppendLine("END:VCARD")
            .AppendLine("FN:1234")
            .AppendLine("END:VCARD");

        string vCard = sb.ToString();
        var info = new VcfDeserializationInfo();
        int count1;
        int count2;

        using (var reader1 = new StringReader(vCard))
        {
            var vcfReader1 = new VcfReader(reader1, info);
            count1 = vcfReader1.AsWeakEnumerable().Count();
        }

        using (var reader2 = new StringReader(vCard))
        {
            var vcfReader2 = new VcfReader(reader2, info);
            count2 = vcfReader2.Count();
        }

        Assert.AreEqual(count1, count2);
    }


    [TestMethod]
    [ExpectedException(typeof(DecoderFallbackException))]
    public void HandleExceptionTest()
        => _ = Vcf.LoadVcf(TestFiles.AnsiIssueVcf, new UTF8Encoding(false, true));
}
