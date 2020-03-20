using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class PropertyClassTypesCollectorTest
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            PropertyClassTypes? adr = PropertyClassTypes.Home | PropertyClassTypes.Work;

            List<string> list = new List<string>();


            PropertyClassTypesCollector collector = new PropertyClassTypesCollector();

            collector.CollectValueStrings(adr, list);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("WORK"));

            // collector darf die Liste nicht löschen!:
            collector.CollectValueStrings(adr, list);
            Assert.AreEqual(4, list.Count);

            // auf null testen:
            adr = null;
            list.Clear();

            collector.CollectValueStrings(adr, list);
            Assert.AreEqual(0, list.Count);
        }



        [TestMethod()]
        public void DetectAllEnumValues()
        {
            PropertyClassTypes[] arr = (PropertyClassTypes[])Enum.GetValues(typeof(PropertyClassTypes));
            var collector = new PropertyClassTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);
            }
        }


        [TestMethod()]
        public void RoundTrip()
        {
            PropertyClassTypes[] arr = (PropertyClassTypes[])Enum.GetValues(typeof(PropertyClassTypes));
            var collector = new PropertyClassTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                PropertyClassTypes? comp = null;

                comp = (PropertyClassTypes)Enum.Parse(typeof(PropertyClassTypes), list[0], true);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp.Value, item);
            }
        }
    }
}