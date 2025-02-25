﻿namespace FolkerKinzel.VCards.Tests;

[TestClass()]
public class AnsiFilterTests
{
    private class Counter
    {
        private int _count;

        public int Count => _count++;
    }

    private const string WIN1251 = "Віталій Володимирович Кличко";
    private const string WIN1252 = "Sören Täve Nüßlebaum";
    private const string WIN1253 = "Βαγγέλης";
    private const string WIN1255 = "אפרים קישון";
    private const string UTF8 = "孔夫子";

    [TestMethod()]
    public void AnsiFilterTest()
    {
        var filter = new AnsiFilter();
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1252", filter.FallbackEncoding.WebName, true);
    }

    [TestMethod()]
    public void AnsiFilterTest1()
    {
        var filter = new AnsiFilter("windows-1251");
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1251", filter.FallbackEncoding.WebName, true);
    }

    [TestMethod()]
    public void LoadVcfTest1()
    {
        var filter = new AnsiFilter();
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1252", filter.FallbackEncoding.WebName, true);
        IReadOnlyList<VCard> vCards = filter.Load(TestFiles.MultiAnsiFilterTests_Utf8Vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("utf-8", filter.UsedEncoding?.WebName);
        Assert.AreEqual(UTF8, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_HebrewVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("windows-1252", filter.UsedEncoding?.WebName, true);
        Assert.AreNotEqual(WIN1255, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_GreekVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("windows-1253", filter.UsedEncoding?.WebName, true);
        Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_UkrainianVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("windows-1251", filter.UsedEncoding?.WebName, true);
        Assert.AreEqual(WIN1251, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_MurksVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual("windows-1252", filter.UsedEncoding?.WebName, true);
        Assert.AreEqual(WIN1252, vCards[0]!.DisplayNames!.First()!.Value);
    }

    [TestMethod]
    public void LoadVcfTest2()
    {
        var filter = new AnsiFilter();
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1252", filter.FallbackEncoding.WebName, true);
        IReadOnlyList<VCard> vCards = filter.Load(TestFiles.MultiAnsiFilterTests_Utf8Vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual(UTF8, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_HebrewVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreNotEqual(WIN1255, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_GreekVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_GreekVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

        vCards = filter.Load(TestFiles.MultiAnsiFilterTests_MurksVcf);
        Assert.AreEqual(1, vCards.Count);
        Assert.AreEqual(WIN1252, vCards[0]!.DisplayNames!.First()!.Value);
    }

    [TestMethod]
    public void LoadVcfTest3()
    {
        var filter = new AnsiFilter();
        Assert.IsNotNull(filter);
        Assert.AreEqual("windows-1252", filter.FallbackEncoding.WebName, true);
        IReadOnlyList<VCard> vCards = filter.Load(TestFiles.MultiAnsiFilterTests_v3AnsiVcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(2, vCards.Count);
        Assert.AreEqual("windows-1252", filter.UsedEncoding?.WebName, true);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest2() => _ = new AnsiFilter(4711);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnsiFilterTest4() => _ = new AnsiFilter("Nixda");

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AnsiFilterTest5() => _ = new AnsiFilter(null!);

    [TestMethod]
    public void AnsiFilterTest6()
    {
        var filter = new AnsiFilter();
        _ = filter.Load(TestFiles.MultiAnsiFilterTests_v3Utf16Bom);
        Assert.AreEqual("utf-8", filter.UsedEncoding?.WebName, false);
    }

    [TestMethod]
    public void DeserializeTest1()
    {
        var filter = new AnsiFilter();

        VCard vc = filter.Deserialize(() => File.OpenRead(TestFiles.AnsiIssueNoEncodingVcf))[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void DeserializeTest2()
    {
        var filter = new AnsiFilter();

        VCard vc = filter.Deserialize(() => File.OpenRead(TestFiles.AnsiIssueInvalidEncodingVcf))[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void DeserializeTest3()
    {
        var filter = new AnsiFilter();

        var cnt = new Counter();

        VCard vc = filter.Deserialize(() => cnt.Count == 0 ? new StreamDummy(File.OpenRead(TestFiles.AnsiIssueInvalidEncodingVcf), canSeek: false) : null)[0];

        Assert.AreNotEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest1()
    {
        var filter = new AnsiFilter();

        VCard vc = (await filter.DeserializeAsync(t => Task.FromResult<Stream>(File.OpenRead(TestFiles.AnsiIssueNoEncodingVcf)), default))[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest2()
    {
        var filter = new AnsiFilter();

        VCard vc = (await filter.DeserializeAsync(t => Task.FromResult<Stream>(File.OpenRead(TestFiles.AnsiIssueInvalidEncodingVcf)), default))[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest3()
    {
        var filter = new AnsiFilter();

        var cnt = new Counter();

        VCard vc = (await filter.DeserializeAsync(
            t => Task.FromResult<Stream>(cnt.Count == 0 ? new StreamDummy(File.OpenRead(TestFiles.AnsiIssueInvalidEncodingVcf), canSeek: false)
                                                        : null!), default))[0];

        Assert.AreNotEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest4()
    {
        var filter = new AnsiFilter();

        var cnt = new Counter();

        VCard vc = (await filter.DeserializeAsync(t => Task.FromResult<Stream>(File.OpenRead(TestFiles.V4vcf)), default))[0];

        Assert.AreEqual("Möhrke Gerda", vc.DisplayNames!.First()!.Value);
    }
}