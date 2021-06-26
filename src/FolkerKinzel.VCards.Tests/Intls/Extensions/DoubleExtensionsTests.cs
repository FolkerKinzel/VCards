using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests
{
    [TestClass()]
    public class DoubleExtensionsTests
    {
        [DataTestMethod()]
        [DataRow(double.NaN, double.NaN, true)]
        [DataRow(5.123456, 5.1234561, true)]
        [DataRow(5.1234568, 5.1234561, true)]
        [DataRow(0, 0, true)]
        [DataRow(5.123456, 5.123457, false)]
        [DataRow(double.NegativeInfinity, double.PositiveInfinity, false)]
        [DataRow(double.NegativeInfinity, double.NegativeInfinity, true)]
        [DataRow(double.PositiveInfinity, double.PositiveInfinity, true)]
        public void Equals6DigitPrecisionTest(double d1, double d2, bool expected)
        {
            Assert.AreEqual(expected, d1.Equals6DigitPrecision(d2));

            if(expected)
            {
                Assert.AreEqual(d1.GetHashcode6DigitPrecision(), d2.GetHashcode6DigitPrecision());
            }
        }

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest1() => Assert.IsTrue(double.NaN.Equals6DigitPrecision(double.NaN));

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest2() => Assert.IsTrue(5.123456.Equals6DigitPrecision(5.1234561));

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest3() => Assert.IsFalse(5.02.Equals6DigitPrecision(5.03));

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest4() => Assert.IsFalse(double.NegativeInfinity.Equals6DigitPrecision(double.PositiveInfinity));

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest5() => Assert.IsTrue(double.NegativeInfinity.Equals6DigitPrecision(double.NegativeInfinity));

        //[TestMethod()]
        //public void Equals6DigitPrecisionTest6() => Assert.IsTrue(double.PositiveInfinity.Equals6DigitPrecision(double.PositiveInfinity));
    }
}