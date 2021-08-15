using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class NonStandardPropertyTests
    {
        [TestMethod()]
        public void NonStandardPropertyTest1()
        {
            const string GROUP = "theGroup";
            const string KEY = "X-Test";
            const string VALUE = "The value";

            var prop = new NonStandardProperty(KEY, VALUE, GROUP);

            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreEqual(VALUE, prop.Value);
            Assert.AreEqual(KEY, prop.PropertyKey);
            Assert.IsFalse(prop.IsEmpty);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void NonStandardPropertyTest2() => _ = new NonStandardProperty("aaa", "ddd");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonStandardPropertyTest3() => _ = new NonStandardProperty(null!, "ddd");

        [TestMethod()]
        public void ToStringTest()
        {
            const string GROUP = "theGroup";
            const string KEY = "X-Test";
            const string VALUE = "The value";

            var prop = new NonStandardProperty(KEY, VALUE, GROUP);

            Assert.IsNotNull(prop.ToString());

        }
    }
}