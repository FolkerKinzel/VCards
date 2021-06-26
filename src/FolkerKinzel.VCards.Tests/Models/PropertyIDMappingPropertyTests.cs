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
    public class PropertyIDMappingPropertyTests
    {
        [TestMethod()]
        public void PropertyIDMappingPropertyTest1()
        {
            var prop = new PropertyIDMappingProperty(7, Guid.NewGuid());

            PropertyParts.PropertyIDMapping pidMap = prop.Value;

            var vcard = new VCard
            {
                PropertyIDMappings = prop
            };

            string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);

            List<VCard> list = VCard.ParseVcf(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.PropertyIDMappings);

            prop = vcard.PropertyIDMappings!.First() as PropertyIDMappingProperty;

            Assert.IsNotNull(prop);
            Assert.AreEqual(pidMap, prop!.Value);
            Assert.IsNull(prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }


        [DataTestMethod()]
        [DataRow(-1)]
        [DataRow(10)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PropertyIDMappingPropertyTest2(int mappingNumber) => _ = new PropertyIDMappingProperty(mappingNumber, Guid.NewGuid());
    }
}