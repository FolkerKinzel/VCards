using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards;
using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Models;
using System.IO;
using FolkerKinzel.VCards.Models.Enums;
using System.Linq;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass()]
    public class VCardTests
    {
        public TestContext? TestContext { get; set; }

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

            Assert.IsNotNull(list[0].DisplayNames);

            TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
            Assert.IsNotNull(dispNameProp);
            Assert.AreEqual("Folker", dispNameProp?.Value);
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void SaveTest(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, $"SaveTest_{version}.vcf");

            vcard.Save(path, version);

            List<VCard> list = VCard.Load(path);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0].DisplayNames);

            TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
            Assert.IsNotNull(dispNameProp);
            Assert.AreEqual("Folker", dispNameProp?.Value);

            Assert.AreEqual(version, list[0].Version);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveTest_fileNameNull()
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            vcard.Save(null!);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveTest_InvalidFileName()
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            vcard.Save("   ");
        }


        

        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SerializeTest_StreamNull(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            vcard.Serialize(null!, version);
        }



        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void SerializeTest(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            using var ms = new MemoryStream();

            vcard.Serialize(ms, version, leaveStreamOpen: true);

            Assert.AreNotEqual(0, ms.Length);

            ms.Position = 0;

            Assert.AreNotEqual(-1, ms.ReadByte());
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void SerializeTest_CloseStream(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            using var ms = new MemoryStream();

            vcard.Serialize(ms, version, leaveStreamOpen: false);

            _ = ms.Length;
        }

        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void SerializeTest_StreamClosed(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            using var ms = new MemoryStream();
            ms.Close();

            vcard.Serialize(ms, version);
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void DeserializeTest(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            using var ms = new MemoryStream();

            vcard.Serialize(ms, version, leaveStreamOpen: true);
            ms.Position = 0;

            using var reader = new StreamReader(ms);
            List<VCard> list = VCard.Deserialize(reader);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0].DisplayNames);

            TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
            Assert.IsNotNull(dispNameProp);
            Assert.AreEqual("Folker", dispNameProp?.Value);

            Assert.AreEqual(version, list[0].Version);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeTest_readerNull() => _ = VCard.Deserialize(null!);


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void ToVcfStringTest(VCdVersion version)
        {
            var vcard = new VCard
            {
                DisplayNames = new TextProperty[] { new TextProperty("Folker") }
            };

            string s = vcard.ToVcfString(version);

            List<VCard> list = VCard.Parse(s);

            Assert.AreEqual(1, list.Count);

            Assert.IsNotNull(list[0].DisplayNames);

            TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
            Assert.IsNotNull(dispNameProp);
            Assert.AreEqual("Folker", dispNameProp?.Value);

            Assert.AreEqual(version, list[0].Version);
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToVcfStringTest_vcardListNull(VCdVersion version) => _ = VCard.ToVcfString(null!, version);


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