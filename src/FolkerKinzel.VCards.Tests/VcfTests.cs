namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VcfTests
{


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest1() => _ = Vcf.Deserialize(() => new MemoryStream(), null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest2() => _ = Vcf.Deserialize(null!, new AnsiFilter());

    [TestMethod]
    public void DeserializeTest3()
    {
        IList<VCard> vc = Vcf.Deserialize(() => null, new AnsiFilter());
        Assert.AreEqual(0, vc.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task DeserializeAsyncTest1() => _ = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(new MemoryStream()), (AnsiFilter)null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task DeserializeAsyncTest2() => _ = await Vcf.DeserializeAsync(null!, new AnsiFilter());

    [TestMethod]
    public async Task DeserializeAsyncTest3()
    {
        IList<VCard> vc = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(null!), new AnsiFilter());
        Assert.AreEqual(0, vc.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task DeserializeAsyncTest4() => _ = await Vcf.DeserializeAsync(null!);

    [TestMethod]
    public async Task DeserializeAsyncTest5()
    {
        IList<VCard> vc = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(null!));
        Assert.AreEqual(0, vc.Count);
    }

    [TestMethod]
    public async Task DeserializeAsyncTest6()
    {
        IList<VCard> vc = await Vcf.DeserializeAsync(t => Task.FromResult<Stream>(new MemoryStream(File.ReadAllBytes(TestFiles.V4vcf))));
        Assert.AreEqual(2, vc.Count);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeManyTest1() => _ = Vcf.DeserializeMany(null!).Count();

    [TestMethod]
    public void DeserializeManyTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = Vcf.DeserializeMany([null, () => File.OpenRead(TestFiles.AnsiIssueVcf)], new AnsiFilter()).First();

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void DeserializeManyTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard[] vc = Vcf.DeserializeMany([null, () => File.OpenRead(TestFiles.AnsiIssueVcf), () => null, () => File.OpenRead(TestFiles.OutlookV2vcf)]).ToArray();

        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void DeserializeManyTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard[] vc = Vcf.DeserializeMany([null,
            () => new StreamDummy(File.OpenRead(TestFiles.AnsiIssueVcf), canSeek: false),
            () => null,
            () => File.OpenRead(TestFiles.OutlookV2vcf)],
                                          new AnsiFilter()).ToArray();

        Assert.AreEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void DeserializeManyTest5()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(Vcf.DeserializeMany([]).FirstOrDefault());
    }

    [TestMethod]
    public void DeserializeManyTest6()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(Vcf.DeserializeMany([() => File.OpenRead(TestFiles.EmptyVcf)]).FirstOrDefault());
    }

#if !NET48
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task DeserializeManyAsyncTest1() => _ = await Vcf.DeserializeManyAsync(null!).CountAsync();

    [TestMethod]
    public async Task DeserializeManyAsyncTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = await Vcf.DeserializeManyAsync([null, t => Task.FromResult<Stream>( File.OpenRead(TestFiles.AnsiIssueVcf))], 
                                                   new AnsiFilter()).FirstAsync();

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard[] vc = await Vcf.DeserializeManyAsync(
            [ null, 
            t => Task.FromResult<Stream>( File.OpenRead(TestFiles.AnsiIssueVcf)), 
            t => Task.FromResult<Stream>(null!),
            //t => throw new Exception(),
            t => Task.FromResult<Stream>(File.OpenRead(TestFiles.OutlookV2vcf))]).ToArrayAsync();

        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(await Vcf.DeserializeManyAsync([]).FirstOrDefaultAsync());
    }

    [TestMethod]
    public async Task DeserializeManyAsyncTest6()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(await Vcf.DeserializeManyAsync([t => Task.FromResult<Stream>( File.OpenRead(TestFiles.EmptyVcf))]).FirstOrDefaultAsync());
    }
#endif

    [TestMethod]
    public void LoadTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = Vcf.Load(TestFiles.AnsiIssueVcf, new AnsiFilter())[0];

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadTest2() => _ = Vcf.Load(null!, new AnsiFilter());

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadTest3() => _ = Vcf.Load(TestFiles.AnsiIssueVcf, (AnsiFilter)null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadManyTest1() => _ = Vcf.LoadMany(null!).Count();

    [TestMethod]
    public void LoadManyTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = Vcf.LoadMany([null, TestFiles.AnsiIssueVcf], new AnsiFilter()).First();

        Assert.AreEqual("Lämmerweg 12", vc.Addresses!.First()!.Value.Street[0]);
    }

    [TestMethod]
    public void LoadManyTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard[] vc = Vcf.LoadMany([null, TestFiles.AnsiIssueVcf, TestFiles.OutlookV2vcf]).ToArray();

        Assert.AreNotEqual("Lämmerweg 12", vc[0].Addresses!.First()!.Value.Street[0]);

    }

    [TestMethod]
    public void LoadManyTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(Vcf.LoadMany([]).FirstOrDefault());
    }

    [TestMethod]
    public void LoadManyTest5()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        Assert.IsNull(Vcf.LoadMany([TestFiles.EmptyVcf]).FirstOrDefault());
    }
}
