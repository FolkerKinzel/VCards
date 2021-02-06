using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class WhatsAppIssueTests
    {
        [TestMethod]
        public void WhatsAppIssueTest1()
        {
            List<VCard> list = VCard.Load(TestFiles.WhatsAppIssueVcf);

            Assert.AreNotEqual(0, list.Count);
        }
    }

   
}
