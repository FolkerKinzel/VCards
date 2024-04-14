using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VcfReaderTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VcfReaderTest()
    {
        _ = new VcfReader(null!);
    }

    [TestMethod]
    public void ReadToEndTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        using var textReader = new StreamReader(TestFiles.LargeFileVcf);
        using var reader = new VcfReader(textReader);
        IEnumerable<VCard> result = reader.ReadToEnd();

        Assert.AreEqual(1000, result.Count());
    }
}
