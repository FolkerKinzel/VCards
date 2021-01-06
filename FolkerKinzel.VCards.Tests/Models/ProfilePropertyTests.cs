using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Models.Interfaces;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class ProfilePropertyTests
    {
        [TestMethod()]
        public void ProfilePropertyTest()
        {
            const string GROUP = "myGroup";

            var prop = new ProfileProperty(GROUP);

            Assert.IsNotNull(prop);
            Assert.IsNotNull(prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreEqual("VCARD", prop.Value);

            string s = prop.Value;

            VCardProperty vcProp = prop;

            Assert.AreEqual(vcProp.Value, s);
        }
    }
}