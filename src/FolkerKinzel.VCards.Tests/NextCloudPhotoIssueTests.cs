using FolkerKinzel.VCards.Extensions;
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

    [TestMethod]
    public void NextCloudPhotoIssue3Test()
    {
        VCard card = Vcf.Load(TestFiles.NextCloudPhotoIssue3Txt)[0];

        DataProperty? photo = card.Photos.PrefOrNull();

        Assert.IsNotNull(photo);
        Assert.IsNotNull(photo.Value);
        Assert.IsNotNull(photo.Value.Bytes);
        CollectionAssert.AreEqual(File.ReadAllBytes(TestFiles.NextCloudPhoto3), photo.Value.Bytes);

        //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"example{photo.GetFileTypeExtension()}");
        //File.WriteAllBytes(filePath, photo.Value.Bytes);
    }
}
