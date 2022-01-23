using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass]
    public class TimeZoneIDTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeZoneIdTest() => _ = new TimeZoneID("  ");


        [DataTestMethod]
        [DataRow("-0700")]
        [DataRow("-07:00")]
        [DataRow("-07")]
        [DataRow("+0400")]
        [DataRow("+04:00")]
        [DataRow("+04")]
        [DataRow("+09")]
        [DataRow("0400")]
        [DataRow("04:00")]
        [DataRow("04")]
        [DataRow("09")]
        [DataRow("-12")]
        public void TryGetUtcOffsetTest1(string s)
        {
            var tzInfo = new TimeZoneID(s);
            Assert.IsTrue(tzInfo.TryGetUtcOffset(out _));
        }


        //[TestMethod]
        //public void ParseTest2()
        //{
        //    TimeZoneInfo tz1 = TimeZoneInfo.Local;

        //    var sb = new StringBuilder();
        //    TimeZoneConverter.AppendTo(sb, tz1, Models.Enums.VCdVersion.V4_0);

        //    string s = sb.ToString();

        //    TimeZoneInfo? tz2 = _timeZoneInfoConverter.Parse(s);

        //    Assert.AreEqual(tz1, tz2);
        //}


        [DataTestMethod]
        [DataRow("-0700", true)]
        [DataRow("-07:00", true)]
        [DataRow("-07", true)]
        [DataRow("+0400", true)]
        [DataRow("+04:00", true)]
        [DataRow("+04", true)]
        [DataRow("+09", true)]
        [DataRow("0400", true)]
        [DataRow("04:00", true)]
        [DataRow("04", true)]
        [DataRow("09", true)]
        [DataRow("-12", true)]
        [DataRow("-22", false)]
        [DataRow("Text-07:00Text", false)]
        public void TryGetUtcOffsetTest2(string input, bool expected)
        {
            var tz = new TimeZoneID(input);
            Assert.AreEqual(expected, tz.TryGetUtcOffset(out _));
        }
    }
}
