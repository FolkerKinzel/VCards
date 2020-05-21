using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;



namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass]
    public class DateAndOrTimeConverterTest
    {
        private readonly DateAndOrTimeConverter conv = new DateAndOrTimeConverter();

        [TestMethod]
        public void DateTest()
        {
            string s = "1963-08-17";

            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            DateTime reference = new DateTime(1963, 8, 17);
            Assert.AreEqual(reference, dt.DateTime);

        }




        [TestMethod]
        public void RountripsTest()
        {
            Roundtrip("19720131");
            Roundtrip("19720131T15-07", false);
            Roundtrip("19720131T15+04", false);

            RoundtripTimestamp("19961022T140000", false);
            RoundtripTimestamp("19961022T140000Z", true);
            RoundtripTimestamp("19961022T140000-05", false);
            RoundtripTimestamp("19961022T140000-0500", false);
            RoundtripTimestamp("19961022T140000+0500", false);

            RoundtripTimestamp("19961022T140000", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000Z", false, VCdVersion.V2_1 );
            RoundtripTimestamp("19961022T140000-05", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000-0500", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000+0500", false, VCdVersion.V2_1);
        }


        void Roundtrip(
            string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
        {
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            string s2 = DateAndOrTimeConverter.ToDateTimeString(dt, version);

            if (stringRoundTrip)
            {
                Assert.AreEqual(s, s2);
            }

            Assert.IsTrue(conv.TryParse(s2, out DateTimeOffset dt2));

            Assert.AreEqual(dt, dt2);
        }

        void RoundtripTimestamp(
            string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
        {
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            string s2 = DateAndOrTimeConverter.ToTimestamp(dt, version);

            if (stringRoundTrip)
            {
                Assert.AreEqual(s, s2);
            }

            Assert.IsTrue(conv.TryParse(s2, out DateTimeOffset dt2));

            Assert.AreEqual(dt, dt2);
        }
    }
}
