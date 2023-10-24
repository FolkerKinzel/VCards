using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests
{
    [TestClass()]
    public class IEnumerableExtensionTests
    {
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext? TestContext { get; set; }


        private static List<VCard?> GenerateVCardList()
        {
            var agent = new VCard()
            {
                DisplayNames = new TextProperty?[]
                {
                    null,
                    new TextProperty("The Agent", "myGroup")
                }
            };


            return new List<VCard?>
            {
                null,
                new VCard()
                {
                    Relations = new RelationProperty?[]
                    {
                        null,
                        new RelationVCardProperty(agent, RelationTypes.Agent | RelationTypes.CoWorker, "otherGroup" )
                    }
                }
            };
        }


        [TestMethod]
        public void ReferenceVCardsTest()
        {
            List<VCard?>? list = GenerateVCardList();

            list = list.ReferenceVCards().ToList()!;

            Assert.AreEqual(2, list.Count);

            VCard? vc1 = list[0];

            Assert.IsInstanceOfType(vc1, typeof(VCard));
            Assert.IsNotNull(vc1?.Relations);

            var uuidProp = vc1?.Relations?.FirstOrDefault(x => x is RelationUuidProperty) as RelationUuidProperty;
            Assert.IsNotNull(uuidProp);
            Guid o1 = uuidProp!.Value;

            Assert.IsFalse(uuidProp.IsEmpty);

            VCard? vc2 = list[1];

            Assert.IsInstanceOfType(vc2, typeof(VCard));
            Assert.IsNotNull(vc2?.UniqueIdentifier);

            Guid? o2 = vc2?.UniqueIdentifier?.Value;

            Assert.IsTrue(o2.HasValue);
            Assert.AreEqual((Guid)o1!, o2!.Value);
        }

        [TestMethod]
        public void DereferenceVCardsTest()
        {
            List<VCard?>? list = GenerateVCardList();

            list = list.ReferenceVCards().ToList()!;

            Assert.AreEqual(2, list.Count);
            Assert.IsNull(list[0]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));

            list = list.DereferenceVCards().ToList()!;

            Assert.AreEqual(2, list.Count);
            Assert.IsNotNull(list[0]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveVcfTest_InvalidFilename(VCdVersion version)
        {
            var list = new List<VCard?>() { new VCard() };

            string path = "   ";

            list.SaveVcf(path, version);

            Assert.IsFalse(File.Exists(path));
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void SaveVcfTest_EmptyList(VCdVersion version)
        {
            var list = new List<VCard?>();

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_Empty.vcf");

            list.SaveVcf(path, version);

            Assert.IsFalse(File.Exists(path));
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveVcfTest_ListNull(VCdVersion version)
        {
            List<VCard?>? list = null;

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_Empty.vcf");

            list!.SaveVcf(path, version);
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveVcfTest_fileNameNull(VCdVersion version)
        {
            var list = new List<VCard?>() { new VCard() };

            list.SaveVcf(null!, version);
        }


        [TestMethod]
        public void SaveVcfTest_vCard2_1()
        {
            List<VCard?> list = GenerateVCardList();

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v2.1.vcf");

            list.SaveVcf(path, VCdVersion.V2_1);

            IList<VCard> list2 = VCard.LoadVcf(path);

            Assert.AreNotEqual(list.Count, list2.Count);
            Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
        }

        [TestMethod]
        public void SaveVcfTest_vCard3_0()
        {
            List<VCard?> list = GenerateVCardList();

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v3.0.vcf");

            list.SaveVcf(path, VCdVersion.V3_0);

            IList<VCard> list2 = VCard.LoadVcf(path);

            Assert.AreNotEqual(list.Count, list2.Count);
            Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);

        }

        [TestMethod]
        public void SaveVcfTest_vCard4_0()
        {
            List<VCard?> list = GenerateVCardList();

            string path = Path.Combine(TestContext!.TestRunResultsDirectory, "SaveVcfTest_v4.0.vcf");

            list.SaveVcf(path, VCdVersion.V4_0);

            IList<VCard> list2 = VCard.LoadVcf(path);

            Assert.AreEqual(2, list2.Count);
            Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SerializeVcfTest1() => new List<VCard?>().SerializeVcf(null!, VCdVersion.V3_0);



        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void SerializeVcfTest2(VCdVersion version)
        {
            List<VCard?> list = GenerateVCardList();

            using var ms = new MemoryStream();

            list.SerializeVcf(ms, version, leaveStreamOpen: true);

            Assert.AreNotEqual(0, ms.Length);

            ms.Position = 0;

            Assert.AreNotEqual(-1, ms.ReadByte());
        }


        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void SerializeVcfTest3(VCdVersion version)
        {
            List<VCard?> list = GenerateVCardList();

            using var ms = new MemoryStream();

            list.SerializeVcf(ms, version, leaveStreamOpen: false);

            _ = ms.Length;

        }

        [DataTestMethod()]
        [DataRow(VCdVersion.V2_1)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V4_0)]
        public void ToVcfStringTest(VCdVersion version)
        {
            List<VCard?> list = GenerateVCardList();

            string s = list.ToVcfString(version);

            IList<VCard> list2 = VCard.ParseVcf(s);

            Assert.AreNotEqual(0, list2.Count);
            Assert.IsNotNull(list2.FirstOrDefault()?.Relations?.FirstOrDefault()?.Value?.VCard);
            Assert.AreEqual(version, list2[0].Version);
        }
    }
}