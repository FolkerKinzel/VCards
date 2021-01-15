using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests
{
    [TestClass()]
    public class VCardReaderTests
    {
        [TestMethod()]
        public void VCardReaderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            using StreamReader reader = File.OpenText(TestFiles.V3vcf);
            var info = new VCardDeserializationInfo();

            foreach (VcfRow item in new VCardReader(reader, info))
            {

            }
        }
    }
}