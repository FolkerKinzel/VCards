using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass]
    public class TimeZoneInfoConverterTests
    {
        [DataTestMethod]
        [DataRow("-0700")]
        [DataRow("-07:00")]
        [DataRow("-07")]
        [DataRow("+0400")]
        [DataRow("+04:00")]
        [DataRow("+04")]
        [DataRow("+09")]
        [DataRow("-12")]
        public void ParseTest1(string? s)
        {
            TimeZoneInfo? tzInfo = TimeZoneInfoConverter.Parse(s);

            Assert.IsNotNull(tzInfo);
        }


        [TestMethod]
        public void ParseTest2()
        {
            TimeZoneInfo tz1 = TimeZoneInfo.Local;


            var sb = new StringBuilder();
            TimeZoneInfoConverter.AppendTo(sb, tz1, Models.Enums.VCdVersion.V4_0);

            string s = sb.ToString();

            TimeZoneInfo? tz2 = TimeZoneInfoConverter.Parse(s);

            Assert.AreEqual(tz1, tz2);
        }
    }
}
