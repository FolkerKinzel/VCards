using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class Rfc9555Tests
{
    [TestMethod]
    public void Rfc9555Test1()
    {
        string expectedDisplayName = $"6,5 4;3{Environment.NewLine}2,1b,1a,0";

        VCard vc = VCardBuilder
            .Create()
            .EMails.Add("mailto:dagobert@entenhausen.us", group: v => v.NewGroup())
            .ABLabels.Add("uncle", group: v => v.EMails?.Last()?.Group)
            .JSContactProps.Add("{\"bar\":1234}", parameters: p => p.JSContactPointer = "example.com:foo")
            .NameViews.Add(NameBuilder
                .Create()
                .AddSurname("0")
                .AddGiven("1a")
                .AddGiven("1b")
                .AddGiven2("2")
                .AddPrefix("3")
                .AddSuffix("4")
                .AddSurname2("5")
                .AddGeneration("6")
                .Build(),
                parameters: p => p.ComponentOrder = "s,\\,;6;5;s, ;4;s,\\;;3;s,\r\n;2;1,1;1;0"
                )
            .NameViews.ToDisplayNames(NameFormatter.Default)
            .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9555, out string v3, out string v2);

        string v4Conserved = v4;

        v4 = v4.ReplaceWhiteSpaceWith("");
        v4WithoutRfc9555 = v4WithoutRfc9555.ReplaceWhiteSpaceWith("");
        v3 = v3.ReplaceWhiteSpaceWith("");
        v2 = v2.ReplaceWhiteSpaceWith("");

        StringComparison comp = StringComparison.OrdinalIgnoreCase;

        Assert.IsTrue(v4.Contains("JSPROP", comp));
        Assert.IsTrue(v4.Contains("JSCOMPS", comp));
        Assert.IsTrue(v4.Contains("JSPTR", comp));
        Assert.IsTrue(v4.Contains("X-ABLabel", comp));

        Assert.IsFalse(v4WithoutRfc9555.Contains("JSPROP", comp));
        Assert.IsFalse(v4WithoutRfc9555.Contains("JSCOMPS", comp));
        Assert.IsFalse(v4WithoutRfc9555.Contains("JSPTR", comp));
        Assert.IsTrue(v4WithoutRfc9555.Contains("X-ABLabel", comp));

        Assert.IsFalse(v3.Contains("JSPROP", comp));
        Assert.IsFalse(v3.Contains("JSCOMPS", comp));
        Assert.IsFalse(v3.Contains("JSPTR", comp));
        Assert.IsTrue(v3.Contains("X-ABLabel", comp));

        Assert.IsFalse(v2.Contains("JSPROP", comp));
        Assert.IsFalse(v2.Contains("JSCOMPS", comp));
        Assert.IsFalse(v2.Contains("JSPTR", comp));
        Assert.IsTrue(v2.Contains("X-ABLabel", comp));

        vc = Vcf.Parse(v4Conserved)[0];
        Assert.AreEqual("uncle", vc.ABLabels?.First()?.Value);

        TextProperty? jsprop = vc.JSContactProps?.FirstOrDefault();
        Assert.IsNotNull(jsprop);
        Assert.AreEqual("{\"bar\":1234}", jsprop.Value);
        Assert.AreEqual("example.com:foo", jsprop.Parameters.JSContactPointer);
        Assert.AreEqual(Data.Text, jsprop.Parameters.DataType);

        Assert.AreEqual("uncle", vc.ABLabels?.First()?.Value);

        Assert.AreEqual(expectedDisplayName, vc.DisplayNames?.First()?.Value);
    }

    [TestMethod]
    public void ABLabelsTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .EMails.Add("mailto:dagobert@entenhausen.us", group: v => v.NewGroup())
            .ABLabels.Add("uncle", group: v => v.EMails?.Last()?.Group)
            .VCard;

        VcfOpts opts = VcfOpts.Default.Unset(VcfOpts.WriteXExtensions);
        string v4 = vc.ToVcfString(VCdVersion.V4_0, options: opts).ReplaceWhiteSpaceWith("");
        string v3 = vc.ToVcfString(VCdVersion.V3_0, options: opts).ReplaceWhiteSpaceWith("");
        string v2 = vc.ToVcfString(VCdVersion.V2_1, options: opts).ReplaceWhiteSpaceWith("");

        StringComparison comp = StringComparison.OrdinalIgnoreCase;
        Assert.IsFalse(v4.Contains("X-ABLabel", comp));
        Assert.IsFalse(v3.Contains("X-ABLabel", comp));
        Assert.IsFalse(v2.Contains("X-ABLabel", comp));
    }

    private static void Serialize(VCard vc, out string v4, out string v4WithoutRfc9555, out string v3, out string v2)
    {
        v4 = vc.ToVcfString(VCdVersion.V4_0);
        v4WithoutRfc9555 = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Unset(VcfOpts.WriteRfc9555Extensions));
        v3 = vc.ToVcfString(VCdVersion.V3_0);
        v2 = vc.ToVcfString(VCdVersion.V2_1);
    }
}

