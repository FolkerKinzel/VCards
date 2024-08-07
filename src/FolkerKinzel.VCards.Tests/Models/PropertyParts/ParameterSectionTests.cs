﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Syncs;
using FolkerKinzel.VCards.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class ParameterSectionTests
{
    [DataTestMethod]
    [DataRow("Date")]
    [DataRow("DATE")]
    [DataRow("\"Date\"")]
    public void CleanParameterValueTest(string value)
    {
        var info = new VcfDeserializationInfo();
        var para = new ParameterSection("BDAY", $"VALUE={value}".AsMemory(), info);

        Assert.AreEqual(Data.Date, para.DataType);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AssignTest1()
    {
        var sec = new ParameterSection();
        sec.Assign(null!);
    }

    [TestMethod]
    public void AssignTest2()
    {
        var sec = new ParameterSection();
        sec.Assign(sec);
    }

    [TestMethod]
    public void AddressTypeTest1()
    {
        var sec = new ParameterSection
        {
            AddressType = default(Adr)
        };
        Assert.IsNull(sec.AddressType);
    }

    [TestMethod]
    public void IndexTest1()
    {
        var sec = new ParameterSection
        {
            Index = -7
        };
        Assert.AreEqual(1, sec.Index);

        sec.Index = 1;
        Assert.AreEqual(1, sec.Index);

        sec.Index = null;
        Assert.IsNull(sec.Index);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var sec = new ParameterSection
        {
            Label = $"""
            First line
            Second line
            """
        };

        string s = sec.ToString();

        Assert.IsTrue(s.GetLinesCount() > 1);
    }

    [TestMethod]
    public void SortAsTest()
    {
        List<string?> list = ["", "  ", " Contoso ", null, "IT"];

        var prop = new OrgProperty("Contoso", ["Computer", "Internet"]);
        prop.Parameters.SortAs = list!;

        IEnumerable<string> result = prop.Parameters.SortAs;

        Assert.IsNotNull(result);
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.IsTrue(result.All(x => StringComparer.Ordinal.Equals(x, x.Trim())));
        Assert.AreEqual(2, result.Count());

        list.Add(null);
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.AreEqual(2, result.Count());

        list.Add(" Web ");
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.All(x => StringComparer.Ordinal.Equals(x, x.Trim())));
    }

    [TestMethod]
    public void CloneTest1()
    {
        var sec = new ParameterSection
        {
            PropertyIDs = new PropertyID(1, null)
        };

        var sec2 = (ParameterSection)sec.Clone();
        Assert.AreNotSame(sec, sec2);
        Assert.AreSame(sec.PropertyIDs.First(), sec2.PropertyIDs!.First());
    }

    [TestMethod]
    public void ParseAttributeKeyFromValueTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            FN;8Bit;de-DE;ISO-8859-1:Test
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc);
        Assert.IsNotNull(vc.DisplayNames);
        TextProperty? prop = vc.DisplayNames.FirstOrDefault();
        Assert.IsNotNull(prop);
        Assert.AreEqual(Enc.Ansi, prop.Parameters.Encoding);
        Assert.AreEqual("ISO-8859-1", prop.Parameters.CharSet);
        Assert.AreEqual("de-DE", prop.Parameters.Language);
    }

    [TestMethod]
    public void ParseTypeParameterTest1()
    {
        const string vcf = """
            BEGIN:VCARD
            X-AIM;MSG:test1
            X-GADUGADU;MSG:test2
            X-GOOGLE-TALK;MSG:test3
            X-GROUPWISE;MSG:test4
            X-GTALK;MSG:test5
            X-ICQ;MSG:test6
            X-JABBER;MSG:test7
            X-KADDRESSBOOK-X-IMADDRESS;MSG:test8
            X-MSN;MSG:test9
            X-MS-IMADDRESS;MSG:test10
            X-SKYPE;MSG:test11
            X-SKYPE-USERNAME;MSG:test12
            X-TWITTER;MSG:test13
            X-YAHOO;MSG:test14
            END:VCARD
            """;

        VCard vc = Vcf.Parse(vcf)[0];

        Assert.IsNotNull(vc);
        Assert.IsNotNull(vc.Messengers);
        TextProperty?[] messengers = vc.Messengers.ToArray();
        Assert.AreEqual(14, messengers.Length);
        CollectionAssert.AllItemsAreNotNull(messengers);
        Assert.IsTrue(messengers.All(p => p!.Parameters.PhoneType.IsSet(Tel.Msg)));
    }
}
