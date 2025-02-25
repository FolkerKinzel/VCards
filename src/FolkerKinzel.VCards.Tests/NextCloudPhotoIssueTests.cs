using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class NextCloudPhotoIssueTests
{
    [TestMethod]
    public void NextCloudPhotoIssueTest1()
    {
        string contentsVCFFile = File.ReadAllText(TestFiles.NextCloudPhotoIssueTxt);
        VCard? convertedItem = Vcf.Parse(contentsVCFFile).FirstOrDefault();
        Assert.IsNotNull(convertedItem);
        Assert.IsNotNull(convertedItem.Photos);
        DataProperty? photo = convertedItem.Photos.FirstOrDefault();
        Assert.IsNotNull(photo);
        Assert.IsFalse(photo.IsEmpty);
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


    //[TestMethod]
    //public void NextCloudPhotoIssue2Test()
    //{
    //    VCard card = Vcf.Load(TestFiles.NextCloudPhotoIssue2Txt)[0];

    //    DataProperty? photo = card.Photos.PrefOrNull();

    //    if (photo?.Value is DataPropertyValue dataUrl)
    //    {
    //        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"example{photo.GetFileTypeExtension()}");

    //        dataUrl.Switch(
    //            bytes => File.WriteAllBytes(filePath, bytes),
    //            url => { },
    //            text => { });
    //    }
    //}
}
