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
    public class GeoPropertyTests
    {
        private const string GROUP = "MyGroup";


        [TestMethod()]
        public void GeoPropertyTest1()
        {
            var geo = new PropertyParts.GeoCoordinate(17.44, 8.33);

            var prop = new GeoProperty(geo, GROUP);

            Assert.AreEqual(geo, prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }

        [TestMethod()]
        public void GeoPropertyTest2()
        {
            var geo = new PropertyParts.GeoCoordinate(17.44, 8.33);

            var prop = new GeoProperty(geo, GROUP);

            var vcard = new VCard
            {
                GeoCoordinates = prop
            };

            string s = vcard.ToVcfString();

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.GeoCoordinates);

            prop = vcard.GeoCoordinates!.First();

            Assert.AreEqual(geo, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }
    }
}