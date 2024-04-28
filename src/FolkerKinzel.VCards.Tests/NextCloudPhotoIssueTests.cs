using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class NextCloudPhotoIssueTests
{
    [TestMethod]
    public void NextCloudPhotoIssueTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        string contentsVCFFile = File.ReadAllText(TestFiles.NextCloudPhotoIssueTxt);
        VCard? convertedItem = Vcf.Parse(contentsVCFFile).FirstOrDefault();
        Assert.IsNotNull(convertedItem);
        Assert.IsNotNull(convertedItem!.Photos);
        DataProperty? photo = convertedItem!.Photos!.FirstOrDefault();
        Assert.IsFalse(photo!.IsEmpty);
    }
}
