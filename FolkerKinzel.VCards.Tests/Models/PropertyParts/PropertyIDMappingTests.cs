using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests
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
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
        public void CtorExceptionTest() => _ = new PropertyIDMapping(0, Guid.Empty);


        [TestMethod]
        public void ParseTest1()
        {
            string pidMap = "2;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            var mapping = PropertyIDMapping.Parse(pidMap);

            Assert.AreEqual(2, mapping.MappingNumber);
            Assert.AreEqual(Guid.Parse("d89c9c7a-2e1b-4832-82de-7e992d95faa5"), mapping.Uuid);
        }

        [TestMethod]
        public void ParseTest2()
        {
            string pidMap = "  2 ; urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            var mapping = PropertyIDMapping.Parse(pidMap);

            Assert.AreEqual(2, mapping.MappingNumber);
            Assert.AreEqual(Guid.Parse("d89c9c7a-2e1b-4832-82de-7e992d95faa5"), mapping.Uuid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParseTest3()
        {
            string pidMap = "22;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            _ = PropertyIDMapping.Parse(pidMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParseTest4()
        {
            string pidMap = "2;http://d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            _ = PropertyIDMapping.Parse(pidMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParseTest5()
        {
            string pidMap = "2";

            _ = PropertyIDMapping.Parse(pidMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParseTest6()
        {
            string pidMap = "";

            _ = PropertyIDMapping.Parse(pidMap);
        }

        [TestMethod]
        public void ToStringTest1()
        {
            int i = 4;
            var guid = Guid.NewGuid();

            var pidmap = new PropertyIDMapping(i, guid);

            string s = pidmap.ToString();

            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(";"));
            Assert.AreEqual(47, s.Length);
        }


        [TestMethod]
        public void ToStringTest2()
        {
            var pidmap = new PropertyIDMapping();

            string s = pidmap.ToString();

            Assert.IsNotNull(s);
            Assert.IsFalse(s.Contains(";"));
            Assert.AreEqual(0, s.Length);
        }
        
    }
}
