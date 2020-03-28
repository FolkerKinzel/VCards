using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass]
    public class PropertyIDTest
    {
        [TestMethod]
        public void CtorTest()
        {
            var pid = new PropertyID();

            Assert.IsTrue(pid.IsEmpty);
            Assert.AreEqual(0, pid.PropertyNumber);
            Assert.IsNull(pid.MappingNumber);

            pid = new PropertyID(5, 7);

            Assert.AreEqual(5, pid.PropertyNumber);
            Assert.AreEqual(7, pid.MappingNumber);

            pid = new PropertyID(5);

            Assert.AreEqual(5, pid.PropertyNumber);
            Assert.IsNull(pid.MappingNumber);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
        public void CtorExceptionTest1()
        {
            new PropertyID(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
        public void CtorExceptionTest2()
        {
            new PropertyID(5, 0);
        }

        [TestMethod]
        public void ToStringTests()
        {
            var pid = new PropertyID(5, 7);

            string s = pid.ToString();

            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains("."));
            Assert.AreEqual(3, s.Length);


            pid = new PropertyID(5);

            s = pid.ToString();

            Assert.IsNotNull(s);
            Assert.IsFalse(s.Contains("."));
            Assert.AreEqual(1, s.Length);
        }
    }
}
