using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards;
using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass()]
    public class VCardTests
    {
        [TestMethod()]
        public void LoadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ParseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeserializeTest()
        {
            Assert.Fail();
        }

        //[TestMethod()]
        //public void SetReferencesTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DereferenceTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SerializeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SerializeTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToVcfStringTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var vc = new VCard()
            {
                DisplayNames = new TextProperty?[]
                {
                    null,
                    new TextProperty("Test")
                }
            };

            string s = vc.ToString();

            Assert.IsNotNull(s);
            Assert.IsFalse(string.IsNullOrWhiteSpace(s));
        }
    }
}