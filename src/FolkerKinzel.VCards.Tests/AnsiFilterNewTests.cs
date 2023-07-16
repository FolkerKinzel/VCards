using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass()]
    public class AnsiFilterNewTests
    {
        private const string WIN1251 = "Віталій Володимирович Кличко";
        private const string WIN1252 = "Sören Täve Nüßlebaum";
        private const string WIN1253 = "Βαγγέλης";
        private const string WIN1255 = "אפרים קישון";
        private const string UTF8 = "孔夫子";

        [TestMethod()]
        public void AnsiFilterTest()
        {
            var filter = new AnsiFilter();
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
        }

        [TestMethod()]
        public void AnsiFilterTest1()
        {
            var filter = new AnsiFilter("windows-1251");
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1251", filter.FallbackEncodingWebName, true);
        }

        [TestMethod()]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnötige Zuweisung eines Werts.", Justification = "<Ausstehend>")]
        public void LoadVcfTest1()
        {
            var filter = new AnsiFilter();
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
            IList<VCard> vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_Utf8Vcf, out string enc);
            Assert.IsNotNull(vCards);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual("utf-8", enc);
            Assert.AreEqual(UTF8, vCards[0]!.DisplayNames!.First()!.Value);

            enc = "";
            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_HebrewVcf, out enc);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual("windows-1252", enc, true);
            Assert.AreNotEqual(WIN1255, vCards[0]!.DisplayNames!.First()!.Value);

            enc = "";
            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_GreekVcf, out enc);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual("windows-1253", enc, true);
            Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

            enc = "";
            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_UkrainianVcf, out enc);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual("windows-1251", enc, true);
            Assert.AreEqual(WIN1251, vCards[0]!.DisplayNames!.First()!.Value);

            enc = "";
            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_MurksVcf, out enc);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual("windows-1252", enc, true);
            Assert.AreEqual(WIN1252, vCards[0]!.DisplayNames!.First()!.Value);


        }

        [TestMethod]
        public void LoadVcfTest2()
        {
            var filter = new AnsiFilter();
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
            IList<VCard> vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_Utf8Vcf);
            Assert.IsNotNull(vCards);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual(UTF8, vCards[0]!.DisplayNames!.First()!.Value);

            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_HebrewVcf);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreNotEqual(WIN1255, vCards[0]!.DisplayNames!.First()!.Value);

            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_GreekVcf);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_GreekVcf);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual(WIN1253, vCards[0]!.DisplayNames!.First()!.Value);

            vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_MurksVcf);
            Assert.AreEqual(1, vCards.Count);
            Assert.AreEqual(WIN1252, vCards[0]!.DisplayNames!.First()!.Value);
        }

        [TestMethod]
        public void LoadVcfTest3()
        {
            var filter = new AnsiFilter();
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName, true);
            IList<VCard> vCards = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_v3AnsiVcf, out string encName);
            Assert.IsNotNull(vCards);
            Assert.AreEqual(2, vCards.Count);
            Assert.AreEqual("windows-1252", encName, true);

        }


            [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AnsiFilterTest2() => _ = new AnsiFilter(4711);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AnsiFilterTest4() => _ = new AnsiFilter("Nixda");


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AnsiFilterTest5() => _ = new AnsiFilter(null!);

        [TestMethod]
        public void AnsiFilterTest6()
        {
            var bt = File.ReadAllBytes(TestFiles.MultiAnsiFilterTests_v3Utf16Bom);
            var filter = new AnsiFilter();
            _ = filter.LoadVcf(TestFiles.MultiAnsiFilterTests_v3Utf16Bom, out string enc);
            Assert.AreEqual("utf-8", enc, false);
        }
    }
}