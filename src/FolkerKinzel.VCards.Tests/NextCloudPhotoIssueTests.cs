using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        Assert.IsNotNull(convertedItem!.Photos);
        DataProperty? photo = convertedItem!.Photos!.FirstOrDefault();
        Assert.IsFalse(photo!.IsEmpty);
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
