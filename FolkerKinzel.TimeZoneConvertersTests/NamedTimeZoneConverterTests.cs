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
        private readonly NamedTimeZoneConverter _conv = new();

        [DataTestMethod()]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("blabla", false)]
        [DataRow("America/Sao_Paulo", true)]
        [DataRow("Asia/Ulan_Bator", true)]
        public void TryGetUtcOffsetFromTimeZoneNameTest1(string input)
        {
            var result = _conv.TryGetUtcOffsetFromTimeZoneName(input, null!, out TimeSpan offset);
            
            
            if (result)
            {
                Assert.AreNotEqual(default, offset);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryGetUtcOffsetFromTimeZoneNameTest2() => _ = _conv.TryGetUtcOffsetFromTimeZoneName(null!, null!, out TimeSpan _);


        [TestMethod]
        public void TryGetUtcOffsetFromTimeZoneNameTest3()
        {
            string id = TimeZoneInfo.Local.Id;

            bool result = _conv.TryGetUtcOffsetFromTimeZoneName(id, null!, out TimeSpan offset);

            Assert.IsTrue(result);
            Assert.AreEqual(TimeZoneInfo.Local.BaseUtcOffset, offset);
        }

    }
}