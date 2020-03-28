using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass]
    public class TimeConverterTest
    {
        private readonly TimeConverter conv = new TimeConverter();

        [TestMethod]
        public void TestTimeConverter()
        {
            Roundtrip("16:58:00", false);
            Roundtrip("165800Z");
            Roundtrip("16:58:00Z", true, VCdVersion.V2_1);
            Roundtrip("16:58:00-04:00", false);
            Roundtrip("16:58:00+04", false);
        }


        void Roundtrip(
            string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
        {
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            string s2 = TimeConverter.ToTimeString(dt, version);

            if (stringRoundTrip)
            {
                Assert.AreEqual(s, s2);
            }

            Assert.IsTrue(conv.TryParse(s2, out DateTimeOffset dt2));

            Assert.AreEqual(dt, dt2);
        }

        
    }
}
