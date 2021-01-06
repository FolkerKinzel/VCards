using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class AddressPropertyTests
    {
        private const string STREET = "Schierauer Str. 24";
        private const string LOCALITY = "Dessau";
        private const string POSTAL_CODE = "06789";
        private const string REGION = "Sachsen-Anhalt";
        private const string COUNTRY = "BRD";
        private const string GROUP = "myGroup";

        [TestMethod()]
        public void AddressPropertyTest()
        {
            var adr = new AddressProperty(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY);

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Value.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Value.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
            Assert.AreEqual(GROUP, adr.Group);
            Assert.IsFalse(adr.IsEmpty);
        }

        [TestMethod()]
        public void AddressPropertyTest1()
        {
            var adr = new AddressProperty(
                new string[] { STREET },
                new string[] { LOCALITY },
                new string[] { POSTAL_CODE },
                new string[] { REGION },
                new string[] { COUNTRY });

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Value.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Value.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
            Assert.AreEqual(GROUP, adr.Group);
            Assert.IsFalse(adr.IsEmpty);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var adr = new AddressProperty(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY);

            string s = adr.ToString();

            Assert.IsNotNull(s);
            Assert.AreNotEqual(0, s.Length);

        }

    }
}