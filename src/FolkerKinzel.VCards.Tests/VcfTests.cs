using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VcfTests
{

    [TestMethod]
    public void LoadTest_fileNameNull()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Load(null!));

    [TestMethod]
    public void LoadTest_invalidFileName()
        => _ = Assert.ThrowsExactly<ArgumentException>(
            () => Vcf.Load("  "));

    [TestMethod]
    public void ParseTest_contentNull() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Parse(null!));

    [TestMethod]
    public void ParseTest_contentEmpty()
    {
        IReadOnlyList<VCard> list = Vcf.Parse("");
        Assert.IsEmpty(list);
    }

    [TestMethod]
    public void ParseTest1()
    {
        IReadOnlyList<VCard> list = Vcf.Parse("BEGIN:VCARD\r\nFN:Folker\r\nEND:VCARD");
        Assert.HasCount(1, list);

        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
    }

    [TestMethod]
    public void DeserializeTest1()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Deserialize(() => new MemoryStream(), null!));

    [TestMethod]
    public void DeserializeTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Deserialize(null!, new AnsiFilter()));

    [TestMethod]
    public void DeserializeTest_readerNull()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Deserialize(null!));

    [TestMethod]
    public void DeserializeTest3()
    {
        IReadOnlyList<VCard> vc = Vcf.Deserialize(() => null, new AnsiFilter());
        Assert.IsEmpty(vc);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest1()
        => await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => _ = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(new MemoryStream()), (AnsiFilter)null!,
                                                       TestContext.CancellationToken));
    [TestMethod]
    public async Task DeserializeAsyncTest2()
        => await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => _ = await Vcf.DeserializeAsync(null!, new AnsiFilter(), TestContext.CancellationToken));

    [TestMethod]
    public async Task DeserializeAsyncTest3()
    {
        IReadOnlyList<VCard> vc = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(null!), new AnsiFilter());
        Assert.IsEmpty(vc);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest4()
        => _ = await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => Vcf.DeserializeAsync(null!));

    [TestMethod]
    public async Task DeserializeAsyncTest5()
    {
        IReadOnlyList<VCard> vc = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(null!));
        Assert.IsEmpty(vc);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest6()
    {
        IReadOnlyList<VCard> vc =
            await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(new MemoryStream(File.ReadAllBytes(TestFiles.V4vcf))));
        Assert.HasCount(2, vc);
    }


    [TestMethod]
    public void DeserializeManyTest1() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.DeserializeMany(null!).Count());

    [TestMethod]
    public void DeserializeManyTest2()
    {
        VCard vc = Vcf.DeserializeMany([null, () => File.OpenRead(TestFiles.AnsiIssueVcf)], new AnsiFilter()).First();

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void DeserializeManyTest3()
    {
        VCard[] vc = Vcf.DeserializeMany([null, () => File.OpenRead(TestFiles.AnsiIssueVcf), () => null, () => File.OpenRead(TestFiles.OutlookV2vcf)]).ToArray();
        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void DeserializeManyTest4()
    {
        VCard[] vc = Vcf.DeserializeMany([null,
            () => new StreamDummy(File.OpenRead(TestFiles.AnsiIssueVcf), canSeek: false),
            () => null,
            () => File.OpenRead(TestFiles.OutlookV2vcf)],
                                          new AnsiFilter()).ToArray();

        Assert.AreEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void DeserializeManyTest5() => Assert.IsNull(Vcf.DeserializeMany([]).FirstOrDefault());

    [TestMethod]
    public void DeserializeManyTest6()
        => Assert.IsNull(Vcf.DeserializeMany([() => File.OpenRead(TestFiles.EmptyVcf)]).FirstOrDefault());


    [TestMethod]
    public async Task DeserializeManyAsyncTest1() 
        => await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => _ = await Vcf.DeserializeManyAsync(null!, token: TestContext.CancellationToken)
                                     .CountAsync(TestContext.CancellationToken));

    [TestMethod]
    public async Task DeserializeManyAsyncTest2()
    {
        VCard vc = await Vcf.DeserializeManyAsync([null, t => Task.FromResult<Stream>(File.OpenRead(TestFiles.AnsiIssueVcf))],
                                                   new AnsiFilter(), TestContext.CancellationToken)
                            .FirstAsync(TestContext.CancellationToken);

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest3()
    {
        Func<CancellationToken, Task<Stream>>?[] factories = [ null,
            t => Task.FromResult<Stream>( File.OpenRead(TestFiles.AnsiIssueVcf)),
            t => Task.FromResult<Stream>(null!),
            t => Task.FromResult<Stream>(File.OpenRead(TestFiles.OutlookV2vcf))];

        VCard[] vc = await Vcf.DeserializeManyAsync(factories, token: TestContext.CancellationToken)
                              .ToArrayAsync(TestContext.CancellationToken);

        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest4()
    {
        VCard[] vc = await Vcf.DeserializeManyAsync(
            [null,
            t => Task.FromResult<Stream>( new StreamDummy(File.OpenRead(TestFiles.AnsiIssueVcf), canSeek: false)),
            t => Task.FromResult<Stream>( null!),
            //t => throw new Exception(),
            t => Task.FromResult<Stream>( File.OpenRead(TestFiles.OutlookV2vcf))],
                                          new AnsiFilter()).ToArrayAsync();

        Assert.AreEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest5()
        => Assert.IsNull(await Vcf.DeserializeManyAsync([], token: TestContext.CancellationToken)
                                  .FirstOrDefaultAsync(TestContext.CancellationToken));

    [TestMethod]
    public async Task DeserializeManyAsyncTest6()
        => Assert.IsNull(await Vcf.DeserializeManyAsync([t => Task.FromResult<Stream>(File.OpenRead(TestFiles.EmptyVcf))], token: TestContext.CancellationToken)
                                                                  .FirstOrDefaultAsync(TestContext.CancellationToken));

    [TestMethod]
    public void LoadTest1()
    {
        VCard vc = Vcf.Load(TestFiles.AnsiIssueVcf, new AnsiFilter())[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void LoadTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Load(null!, new AnsiFilter()));

    [TestMethod]
    public void LoadTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.Load(TestFiles.AnsiIssueVcf, (AnsiFilter)null!));

    [TestMethod]
    public void LoadManyTest1()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.LoadMany(null!).Count());

    [TestMethod]
    public void LoadManyTest2()
    {
        VCard vc = Vcf.LoadMany([null, TestFiles.AnsiIssueVcf], new AnsiFilter()).First();

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void LoadManyTest3()
    {
        VCard[] vc = Vcf.LoadMany([null, TestFiles.AnsiIssueVcf, TestFiles.OutlookV2vcf]).ToArray();

        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void LoadManyTest4() => Assert.IsNull(Vcf.LoadMany([]).FirstOrDefault());

    [TestMethod]
    public void LoadManyTest5() => Assert.IsNull(Vcf.LoadMany([TestFiles.EmptyVcf]).FirstOrDefault());

    [TestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void ToVcfStringTest_vcardListNull1(VCdVersion version) 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => Vcf.AsString(null!, version));

    public TestContext TestContext { get; set; }
}
