using System;
using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass]
    public class PropertyIDMappingTests
    {
        [TestMethod]
        public void CtorTest()
        {
            var pidMap = new PropertyIDMapping();

            Assert.IsTrue(pidMap.IsEmpty);
            Assert.AreEqual(0, pidMap.MappingNumber);

            pidMap = new PropertyIDMapping(5, Guid.NewGuid());

            Assert.IsFalse(pidMap.IsEmpty);
            Assert.AreEqual(5, pidMap.MappingNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes=false)]
        public void CtorExceptionTest()
        {
            new PropertyIDMapping(0, Guid.Empty);
        }

        [TestMethod]
        public void ToStringTests()
        {
            int i = 47;
            var guid = Guid.NewGuid();

            var pidmap = new PropertyIDMapping(i, guid);

            Assert.AreEqual(i, pidmap.MappingNumber);
            Assert.AreEqual(guid, pidmap.Uuid);

            string s = pidmap.ToString();

            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(";"));
            Assert.AreEqual(48, s.Length);
        }
    }
}
