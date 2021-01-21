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
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadTest_fileNameNull() => _ = VCard.Load(null!);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadTest_invalidFileName() => _ = VCard.Load("  ");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseTest_contentNull() => _ = VCard.Parse(null!);

        [TestMethod()]
        public void ParseTest_contentEmpty()
        {
            List<VCard> list = VCard.Parse("");
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod()]
        public void ParseTest()
        {
            List<VCard> list = VCard.Parse("BEGIN:VCARD\r\nFN:Folker\r\nEND:VCARD");
            Assert.AreEqual(1, list.Count);
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