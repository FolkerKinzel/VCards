namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VcfReaderTests
{
    [TestMethod]
    public void VcfReaderTest()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => new VcfReader(null!));

    [TestMethod]
    public void ReadToEndTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        using var textReader = new StreamReader(TestFiles.LargeFileVcf);
        using var reader = new VcfReader(textReader);
        IEnumerable<VCard> result = reader.ReadToEnd();

        Assert.HasCount(1000, result);
    }
}
