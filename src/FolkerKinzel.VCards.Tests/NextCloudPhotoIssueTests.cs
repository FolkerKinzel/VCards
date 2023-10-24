using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class NextCloudPhotoIssueTests
{
    [TestMethod]
    public void NextCloudPhotoIssueTest1()
    {
        string contentsVCFFile = File.ReadAllText(TestFiles.NextCloudPhotoIssueTxt);
        VCard? convertedItem = VCard.ParseVcf(contentsVCFFile).FirstOrDefault();
        Assert.IsNotNull(convertedItem);
        Assert.IsNotNull(convertedItem!.Photos);
        Models.DataProperty? photo = convertedItem!.Photos!.FirstOrDefault();
        Assert.IsFalse(photo!.IsEmpty);
    }
}
