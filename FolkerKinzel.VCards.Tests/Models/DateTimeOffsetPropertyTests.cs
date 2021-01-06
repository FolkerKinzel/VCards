using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class DateTimeOffsetPropertyTests
    {
        [TestMethod()]
        public void DateTimeOffsetPropertyTest()
        {
            const string GROUP = "myGroup";

            var prop = new DateTimeOffsetProperty(DateTimeOffset.UtcNow, GROUP);

            Assert.IsNotNull(prop);
            Assert.IsNotNull(prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreNotEqual(DateTimeOffset.MinValue, prop.Value);

            DateTimeOffset dto = prop.Value!.Value;

            VCardProperty vcProp = prop;

            Assert.AreEqual(vcProp.Value, dto);
        }
    }
}