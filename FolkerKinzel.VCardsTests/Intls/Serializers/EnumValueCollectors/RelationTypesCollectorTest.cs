using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class RelationTypesCollectorTest
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            RelationTypes? adr = RelationTypes.Spouse | RelationTypes.CoResident;

            List<string> list = new List<string>();


            var collector = new RelationTypesCollector();

            collector.CollectValueStrings(adr, list);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains(RelationTypesConverter.RelationTypeValue.CO_RESIDENT));

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
            RelationTypes[] arr = (RelationTypes[])Enum.GetValues(typeof(RelationTypes));
            var collector = new RelationTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
            }
        }


        [TestMethod()]
        public void RoundTrip()
        {
            RelationTypes[] arr = (RelationTypes[])Enum.GetValues(typeof(RelationTypes));
            var collector = new RelationTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                RelationTypes? comp = null;

                comp = RelationTypesConverter.Parse(list[0], comp);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp!.Value, item);

                var comp2 = (RelationTypes)Enum.Parse(
                    typeof(RelationTypes), list[0].Replace("-",""), true);

                Assert.AreEqual(comp, comp2);
            }
        }
    }
}