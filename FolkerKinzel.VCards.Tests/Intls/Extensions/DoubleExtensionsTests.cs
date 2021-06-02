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
        [TestMethod()]
        public void Equals2DigitPrecisionTest1() => Assert.IsTrue(double.NaN.Equals2DigitPrecision(double.NaN));

        [TestMethod()]
        public void Equals2DigitPrecisionTest2() => Assert.IsTrue(5.2.Equals2DigitPrecision(5.201));

        [TestMethod()]
        public void Equals2DigitPrecisionTest3() => Assert.IsFalse(5.02.Equals2DigitPrecision(5.03));

        [TestMethod()]
        public void Equals2DigitPrecisionTest4() => Assert.IsFalse(double.NegativeInfinity.Equals2DigitPrecision(double.PositiveInfinity));

        [TestMethod()]
        public void Equals2DigitPrecisionTest5() => Assert.IsTrue(double.NegativeInfinity.Equals2DigitPrecision(double.NegativeInfinity));

        [TestMethod()]
        public void Equals2DigitPrecisionTest6() => Assert.IsTrue(double.PositiveInfinity.Equals2DigitPrecision(double.PositiveInfinity));
    }
}