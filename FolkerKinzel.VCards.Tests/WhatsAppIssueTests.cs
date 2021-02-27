using System;
using System.Collections.Generic;
using System.Linq;
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

            IEnumerable<Models.TextProperty?>? phoneNumbers = list[0].PhoneNumbers;

            Assert.IsNotNull(phoneNumbers);

            Models.TextProperty? whatsAppNumber = phoneNumbers!.ElementAtOrDefault(1);

            Assert.IsNotNull(whatsAppNumber);

            KeyValuePair<string, string>? parameter = whatsAppNumber!.Parameters.NonStandardParameters?.FirstOrDefault();

            Assert.IsTrue(parameter.HasValue);

            Assert.AreEqual("TYPE", parameter!.Value.Key);
            Assert.AreEqual("WhatsApp", parameter!.Value.Value);

        }
    }

   
}
