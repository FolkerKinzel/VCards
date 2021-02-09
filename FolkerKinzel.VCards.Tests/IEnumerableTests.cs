using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class IEnumerableTests
    {
        [TestMethod]
        public void IEnumerableTest1()
        {
            XNamespace f = "f";

            var vc = new VCard
            {
                XmlProperties = new XmlProperty(new XElement(f + "Test"))
            };

            IEnumerable enumerable = vc.XmlProperties;

            foreach (var item in enumerable)
            {
                Assert.IsNotNull(item);
            }

            Assert.IsNotNull(vc.XmlProperties.FirstOrDefault());
        }


        [TestMethod]
        public void IEnumerableTest2()
        {
            var vc = new VCard
            {
                BirthPlaceViews = new TextProperty("Lummerland")
            };

            IEnumerable enumerable = vc.BirthPlaceViews;

            foreach (var item in enumerable)
            {
                Assert.IsNotNull(item);
            }

            Assert.IsNotNull(vc.XmlProperties.FirstOrDefault());
        }

        [TestMethod]
        public void IEnumerableTest3()
        {
            var vc = new VCard
            {
                BirthDayViews = new DateTimeOffsetProperty(DateTime.Now)
            };

            IEnumerable enumerable = vc.BirthDayViews;

            foreach (var item in enumerable)
            {
                Assert.IsNotNull(item);
            }

            IEnumerable<VCardProperty?> props = vc.BirthDayViews;

            Assert.IsNotNull(vc.BirthDayViews.FirstOrDefault());
        }



    }
}
