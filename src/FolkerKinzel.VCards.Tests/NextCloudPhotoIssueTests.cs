using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class NextCloudPhotoIssueTests
    {
        [TestMethod]
        public void NextCloudPhotoIssueTest1()
        {
            string contentsVCFFile = File.ReadAllText(TestFiles.NextCloudPhotoIssueTxt);
            var convertedItem = VCard.ParseVcf(contentsVCFFile);
        }
    }
}
