using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.TimeZoneConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.TimeZoneConverters.Tests
{
    [TestClass()]
    public class NamedTimeZoneConverterTests
    {
        private readonly NamedTimeZoneConverter _conv = new NamedTimeZoneConverter();

        [DataTestMethod()]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("blabla", false)]
        [DataRow("America/Sao_Paulo", true)]
        public void TryGetUtcOffsetFromTimeZoneNameTest1(string? input, bool expectedResult)
        {
            var result = _conv.TryGetUtcOffsetFromTimeZoneName(input, null!, out TimeSpan offset);
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryGetUtcOffsetFromTimeZoneNameTest2() => _ = _conv.TryGetUtcOffsetFromTimeZoneName(null!, null!, out TimeSpan _);


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryGetUtcOffsetFromTimeZoneNameTest3()
        {
            string id = TimeZoneInfo.Local.Id;

            bool result = _conv.TryGetUtcOffsetFromTimeZoneName(id, null!, out TimeSpan offset);

            Assert.IsTrue(result);
            Assert.AreEqual(TimeZoneInfo.Local.BaseUtcOffset, offset);
        }

    }
}