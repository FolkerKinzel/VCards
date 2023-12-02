using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VcfTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest1()
    {
        _ = Vcf.Deserialize(() => new MemoryStream(), null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest2()
    {
        _ = Vcf.Deserialize(null!, new AnsiFilter());
    }

    [TestMethod]
    public void DeserializeTest3()
    {
        IList<VCard> vc = Vcf.Deserialize(() => null, new AnsiFilter());
        Assert.AreEqual(0, vc.Count);
    }

    

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeManyTest1()
    {
        _ = Vcf.DeserializeMany(null!).Count();
    }

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
    public void LoadTest2()
    {
        _ = Vcf.Load(null!, new AnsiFilter());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadTest3()
    {
        _ = Vcf.Load(TestFiles.AnsiIssueVcf, (AnsiFilter)null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadManyTest1()
    {
        _ = Vcf.LoadMany(null!).Count();
    }

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
