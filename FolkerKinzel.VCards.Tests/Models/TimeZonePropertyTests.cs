using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class TimeZonePropertyTests
    {
        private const string GROUP = "MyGroup";

        [TestMethod()]
        public void TimeZonePropertyTest1()
        {
            TimeZoneInfo tz = TimeZoneInfo.GetSystemTimeZones()[7];

            var prop = new TimeZoneProperty(tz, GROUP);

            Assert.AreEqual(tz, prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }

        [TestMethod()]
        public void TimeZonePropertyTest2()
        {
            TimeZoneInfo tz = TimeZoneInfo.GetSystemTimeZones()[4];

            var prop = new TimeZoneProperty(tz, GROUP);

            var vcard = new VCard
            {
                TimeZones = prop
            };

            string s = vcard.ToVcfString();

            List<VCard> list = VCard.ParseVcf(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.TimeZones);

            prop = vcard.TimeZones!.First();

            Assert.AreEqual(tz, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }

    }
}