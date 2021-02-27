using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class TimeStampPropertyTests
    {
        private const string GROUP = "myGroup";

        [TestMethod()]
        public void TimeStampPropertyTest1a()
        {
            var prop = new TimeStampProperty();

            Assert.IsNull(prop.Group);
            Assert.AreNotEqual(default, prop.Value);
        }

        [TestMethod()]
        public void TimeStampPropertyTest1b()
        {
            var prop = new TimeStampProperty(GROUP);

            Assert.AreEqual(GROUP,prop.Group);
            Assert.AreNotEqual(default, prop.Value);
        }

        [TestMethod()]
        public void TimeStampPropertyTest2a()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            var prop = new TimeStampProperty(now);

            Assert.IsNull(prop.Group);
            Assert.AreEqual(now, prop.Value);
        }

        [TestMethod()]
        public void TimeStampPropertyTest2b()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            var prop = new TimeStampProperty(now, GROUP);

            Assert.AreEqual(GROUP,prop.Group);
            Assert.AreEqual(now, prop.Value);
        }
    }
}