using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class NameTests
{
    [DataTestMethod]
    [DataRow("a;;;;")]
    [DataRow(";a;;;")]
    [DataRow(";;a;;")]
    [DataRow(";;;a;")]
    [DataRow(";;;;a")]
    public void IsEmptyTest1(string input)
        => Assert.IsFalse(new Name(input.AsMemory(), VCdVersion.V4_0).IsEmpty);

    [TestMethod]
    public void AppendVCardStringTest1()
    {
        const string input = ";;Heinrich,August;;";
        var name = new Name(input.AsMemory(), VCdVersion.V4_0);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, VcfOpts.Default);
        name.AppendVcfString(serializer);

        Assert.AreEqual(input, serializer.Builder.ToString());
    }

    [TestMethod]
    public void ToStringTest1()
    {
        const string input = ";;Heinrich,August;   ;;;;;;;;;;";
        var name = new Name(input.AsMemory(), VCdVersion.V4_0);

        string s = name.ToString();
        StringAssert.Contains(s, "August");
    }

    [TestMethod]
    public void ToStringTest2()
    {
        Name name = Name.Empty;
        Assert.IsTrue(name.IsEmpty);
        Assert.IsNotNull(name.ToString());
    }

    [TestMethod]
    public void Rfc9554Test1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddSurname("1")
                .AddGiven("2")
                .AddGiven2("3")
                .AddPrefix("4")
                .AddSuffix("5")
                .AddSurname2("6")
                .AddGeneration("7")
                .Build())
            .VCard;

        string vcf2 = vc.ToVcfString(VCdVersion.V2_1);
        string vcf3 = vc.ToVcfString(VCdVersion.V3_0);
        string vcfRfc9554 = vc.ToVcfString(VCdVersion.V4_0);
        string vcf4 = vc.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Unset(VcfOpts.WriteRfc9554Extensions));

        const string standard = "1 6;2;3;4;5 7\r\n";
        const string standardV4 = "1,6;2;3;4;5,7\r\n";
        const string rfc9554 = "1,6;2;3;4;5,7;6;7\r\n";

        Assert.IsTrue(vcf2.Contains(standard));
        Assert.IsTrue(vcf3.Contains(standard));
        Assert.IsTrue(vcf4.Contains(standardV4));
        Assert.IsTrue(vcfRfc9554.Contains(rfc9554));
    }

    [TestMethod]
    public void Rfc9554Test2()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddSurname2("surname2")
                .AddGeneration("generation")
                .Build())
            .VCard;

        string vcfRfc9554 = vc.ToVcfString(VCdVersion.V4_0);

        vc = Vcf.Parse(vcfRfc9554)[0];

        Assert.IsNotNull(vc.NameViews);
        Name name = vc.NameViews.First()!.Value;

        Assert.AreEqual("surname2", name.Surnames2[0]);
        Assert.AreEqual("generation", name.Generations[0]);
    }

    [TestMethod]
    public void CtorTest1()
    {
        var prop = new NameProperty(
            NameBuilder
            .Create()
            .AddSurname(["1", "2"])
            .AddSurname2("2")
            .Build());

        CollectionAssert.AreEqual(new string[] { "1" }, prop.Value.Surnames.ToArray());
    }

    [TestMethod]
    public void CtorTest2()
    {
        var prop = new NameProperty(
            NameBuilder
            .Create()
            .AddSurname(["1", "2"])
            .AddSurname2("3")
            .Build());

        CollectionAssert.AreEqual(new string[] { "1", "2" }, prop.Value.Surnames.ToArray());
    }

    [TestMethod]
    public void CtorTest3()
    {
        string[] arr = ["1", "2"];

        var prop = new NameProperty(
            NameBuilder
            .Create()
            .AddSurname(arr)
            .AddSurname2(arr)
            .Build());

        Assert.AreEqual(0, prop.Value.Surnames.Count);
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1829:Use Length/Count property instead of Count() when available", Justification = "Test")]
    public void IEnumerableTest1()
    {
        IReadOnlyList<IReadOnlyList<string>> name = new Name(NameBuilder.Create());
        Assert.AreEqual(name.Count, name.Count());
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
        "CA1826:Do not use Enumerable methods on indexable collections", Justification = "Test")]
    public void IEnumerableTest2()
    {
        IReadOnlyList<IReadOnlyList<string>> name = new Name(NameBuilder.Create());
        Assert.AreEqual(name.Count, name.AsWeakEnumerable().Count());
    }
}
