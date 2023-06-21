using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class LabelIssueTests
    {
        [TestMethod]
        public void LabelIssueTest1()
        {
            var filter = new AnsiFilter();
            IList<VCard> vCards = filter.LoadVcf(TestFiles.LabelIssueVcf, out string enc);
            Assert.IsNotNull(vCards);
            Assert.AreEqual(1, vCards.Count);
            Assert.IsNotNull(vCards[0]);
            IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
            Assert.IsNotNull(addresses);
            Assert.AreEqual(3, addresses!.Count());
            const string street = "Business-Straße 19";
            Assert.IsNotNull(addresses!.FirstOrDefault(
                x => x!.Parameters!.Label!.Contains(street) &&
                     x.Value.Street.Contains(street)));
        }
    }
}
