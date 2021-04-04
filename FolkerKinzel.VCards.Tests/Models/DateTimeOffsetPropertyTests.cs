using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class DateTimeOffsetPropertyTests
    {
        private const string GROUP = "myGroup";

        [TestMethod()]
        public void DateTimeOffsetPropertyTest1()
        {

            var prop = new DateTimeOffsetProperty(DateTimeOffset.UtcNow, GROUP);

            Assert.IsNotNull(prop);
            Assert.IsNotNull(prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreNotEqual(DateTimeOffset.MinValue, prop.Value);

            DateTimeOffset dto = prop.Value!.Value;

            VCardProperty vcProp = prop;

            Assert.AreEqual(vcProp.Value, dto);
        }


        [TestMethod()]
        public void DateTimeOffsetPropertyTest2()
        {
            var now = new DateTimeOffset(2021,4,4,12,41,2,TimeSpan.FromHours(2));

            var prop = new DateTimeOffsetProperty(now, GROUP);

            var vcard = new VCard
            {
                BirthDayViews = prop
            };

            string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.BirthDayViews);

            prop = vcard.BirthDayViews!.First() as DateTimeOffsetProperty;

            Assert.IsNotNull(prop);
            Assert.AreEqual(now, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);

            Assert.AreEqual(Enums.VCdDataType.DateAndOrTime, prop.Parameters.DataType);
        }
    }
}