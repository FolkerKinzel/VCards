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
    public class MultiAnsiFilterTests
    {
        [TestMethod()]
        public void MultiAnsiFilterTest()
        {
            var filter = new MultiAnsiFilter();
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName);
        }

        [TestMethod()]
        public void MultiAnsiFilterTest1()
        {
            var filter = new MultiAnsiFilter("windows-1252");
            Assert.IsNotNull(filter);
            Assert.AreEqual("windows-1252", filter.FallbackEncodingWebName);
        }

        [TestMethod()]
        public void LoadVcfTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MultiAnsiFilterTest2() => _ = new AnsiFilter(4711);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MultiAnsiFilterTest4() => _ = new AnsiFilter("Nixda");


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MultiAnsiFilterTest5() => _ = new AnsiFilter(null!);
    }
}