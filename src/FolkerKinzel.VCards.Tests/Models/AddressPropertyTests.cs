using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
#pragma warning disable CS0618 // Typ oder Element ist veraltet

    [TestClass()]
    public class AddressPropertyTests
    {
        private const string STREET = "Schierauer Str. 24";
        private const string LOCALITY = "Dessau";
        private const string POSTAL_CODE = "06789";
        private const string REGION = "Sachsen-Anhalt";
        private const string COUNTRY = "BRD";
        private const string PO_BOX = "PO Box";
        private const string EXTENDED_ADDRESS = "extended";
        private const string GROUP = "myGroup";


        [TestMethod()]
        public void AddressPropertyTest()
        {
            var adr = new AddressProperty(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY, PO_BOX, EXTENDED_ADDRESS, propertyGroup: GROUP);

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Value.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Value.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
            Assert.AreEqual(PO_BOX, adr.Value.PostOfficeBox[0]);
            Assert.AreEqual(EXTENDED_ADDRESS, adr.Value.ExtendedAddress[0]);
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
                new string[] { COUNTRY },
                new string[] { PO_BOX },
                new string[] { EXTENDED_ADDRESS },

                propertyGroup: GROUP);

            Assert.IsNotNull(adr);
            Assert.AreEqual(STREET, adr.Value.Street[0]);
            Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
            Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
            Assert.AreEqual(REGION, adr.Value.Region[0]);
            Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
            Assert.AreEqual(PO_BOX, adr.Value.PostOfficeBox[0]);
            Assert.AreEqual(EXTENDED_ADDRESS, adr.Value.ExtendedAddress[0]);
            Assert.AreEqual(GROUP, adr.Group);
            Assert.IsFalse(adr.IsEmpty);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var adr = new AddressProperty(STREET, LOCALITY, POSTAL_CODE, REGION, COUNTRY, PO_BOX, EXTENDED_ADDRESS);

            string s = adr.ToString();

            Assert.IsNotNull(s);
            Assert.AreNotEqual(0, s.Length);

        }


    }
#pragma warning restore CS0618 // Typ oder Element ist veraltet

}