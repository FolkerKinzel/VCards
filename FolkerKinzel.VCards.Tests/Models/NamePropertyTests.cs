using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class NamePropertyTests
    {
        private const string LAST_NAME = "Duck";
        private const string FIRST_NAME = "Donald";
        private const string MIDDLE_NAME = "Willie";
        private const string PREFIX = "Dr.";
        private const string SUFFIX = "Jr.";

        private const string GROUP = "myGroup";

        [TestMethod()]
        public void AddressPropertyTest()
        {
            var adr = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX, propertyGroup: GROUP);

            Assert.IsNotNull(adr);
            Assert.AreEqual(LAST_NAME, adr.Value.LastName[0]);
            Assert.AreEqual(FIRST_NAME, adr.Value.FirstName[0]);
            Assert.AreEqual(MIDDLE_NAME, adr.Value.MiddleName[0]);
            Assert.AreEqual(PREFIX, adr.Value.Prefix[0]);
            Assert.AreEqual(SUFFIX, adr.Value.Suffix[0]);
            Assert.AreEqual(GROUP, adr.Group);
            Assert.IsFalse(adr.IsEmpty);
        }

        [TestMethod()]
        public void AddressPropertyTest1()
        {
            var adr = new NameProperty(
                new string[] { LAST_NAME },
                new string[] { FIRST_NAME },
                new string[] { MIDDLE_NAME },
                new string[] { PREFIX },
                new string[] { SUFFIX },

                propertyGroup: GROUP);

            Assert.IsNotNull(adr);
            Assert.AreEqual(LAST_NAME, adr.Value.LastName[0]);
            Assert.AreEqual(FIRST_NAME, adr.Value.FirstName[0]);
            Assert.AreEqual(MIDDLE_NAME, adr.Value.MiddleName[0]);
            Assert.AreEqual(PREFIX, adr.Value.Prefix[0]);
            Assert.AreEqual(SUFFIX, adr.Value.Suffix[0]);
            Assert.AreEqual(GROUP, adr.Group);
            Assert.IsFalse(adr.IsEmpty);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var adr = new NameProperty(LAST_NAME, FIRST_NAME, MIDDLE_NAME, PREFIX, SUFFIX);

            string s = adr.ToString();

            Assert.IsNotNull(s);
            Assert.AreNotEqual(0, s.Length);
        }
    }
}