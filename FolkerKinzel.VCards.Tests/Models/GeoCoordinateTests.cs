using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class GeoCoordinateTests
    {
        [TestMethod()]
        public void GeoCoordinateTest()
        {
            double latitude = 17.5;
            double longitude = 4.2;

            var geo = new GeoCoordinate(latitude, longitude);

            Assert.AreEqual(latitude, geo.Latitude);
            Assert.AreEqual(longitude, geo.Longitude);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EqualsTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.Fail();
        }
    }
}