using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class VCardAddressTests
    {
        private const string STREET =  "Priorauer Str. 32";
        private const string LOCALITY = "Raguhn-Jeßnitz";
        private const string POSTAL_CODE = "06779";
        private const string REGION = "Sachsen-Anhalt";
        private const string COUNTRY = "BRD";
        

        [TestMethod()]
        public void VCardAddressTest()
        {
            var adr = new VCardAddress(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY);

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Country[0]);
        }

        [TestMethod()]
        public void VCardAddressTest1()
        {
            var adr = new VCardAddress(
                new string[] { STREET },
                new string[] { LOCALITY },
                new string[] { POSTAL_CODE },
                new string[] { REGION },
                new string[] { COUNTRY });

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Country[0]);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var adr = new VCardAddress(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY);

            string s = adr.ToString();

            Assert.IsNotNull(s);
            Assert.AreNotEqual(0, s.Length);

        }
    }
}