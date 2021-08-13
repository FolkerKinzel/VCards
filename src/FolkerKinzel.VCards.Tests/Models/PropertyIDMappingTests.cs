using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass]
    public class PropertyIDMappingTests
    {
        [TestMethod]
        public void CtorTest()
        {
            var pidMap = new PropertyIDMapping(5, new Uri("http://folkerkinzel.de/"));
            Assert.AreEqual(5, pidMap.ID);
        }

        [DataTestMethod()]
        [DataRow(-1)]
        [DataRow(10)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PropertyIDMappingTest2(int mappingNumber)
        {
            var uri = new Uri("http://folker.de/");
            _ = new PropertyIDMapping(mappingNumber, uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
        public void CtorExceptionTest() => _ = new PropertyIDMapping(0, new Uri("http://folkerkinzel.de/"));


        [TestMethod]
        public void ParseTest1()
        {
            string pidMap = "2;urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            var mapping = PropertyIDMapping.Parse(pidMap);

            Assert.AreEqual(2, mapping.ID);
            Assert.AreEqual(Guid.Parse("d89c9c7a-2e1b-4832-82de-7e992d95faa5"), mapping.Mapping);
        }

        [TestMethod]
        public void ParseTest2()
        {
            string pidMap = "  2 ; urn:uuid:d89c9c7a-2e1b-4832-82de-7e992d95faa5";

            var mapping = PropertyIDMapping.Parse(pidMap);

            Assert.AreEqual(2, mapping.ID);
            Assert.AreEqual(Guid.Parse("d89c9c7a-2e1b-4832-82de-7e992d95faa5"), mapping.Mapping);
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

            var pidmap = new PropertyIDMapping(i, new Uri("http://folkerkinzel.de/"));

            string s = pidmap.ToString();

            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(";"));
            Assert.IsTrue(5 < s.Length);
        }




    }
}
