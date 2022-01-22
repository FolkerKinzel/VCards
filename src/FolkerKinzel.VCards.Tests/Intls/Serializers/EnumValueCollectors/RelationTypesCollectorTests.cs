using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class RelationTypesCollectorTests
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            RelationTypes? adr = RelationTypes.Spouse | RelationTypes.CoResident;

            var list = new List<string>();

            RelationTypesCollector.CollectValueStrings(adr, list);

            Assert.AreEqual(2, list.Count);

            // collector darf die Liste nicht löschen!:
            RelationTypesCollector.CollectValueStrings(adr, list);
            Assert.AreEqual(4, list.Count);

            // auf null testen:
            adr = null;
            list.Clear();

            RelationTypesCollector.CollectValueStrings(adr, list);
            Assert.AreEqual(0, list.Count);
        }


        [TestMethod()]
        public void DetectAllEnumValues()
        {
            var arr = (RelationTypes[])Enum.GetValues(typeof(RelationTypes));

            var list = new List<string>(1);

            foreach (RelationTypes item in arr)
            {
                list.Clear();
                RelationTypesCollector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
            }
        }


        [TestMethod()]
        public void RoundTrip()
        {
            var arr = (RelationTypes[])Enum.GetValues(typeof(RelationTypes));

            var list = new List<string>(1);

            foreach (RelationTypes item in arr)
            {
                list.Clear();
                RelationTypesCollector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                RelationTypes? comp = RelationTypesConverter.Parse(list[0]);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp!.Value, item);

                var comp2 = (RelationTypes)Enum.Parse(
                    typeof(RelationTypes), list[0].Replace("-",""), true);

                Assert.AreEqual(comp, comp2);
            }
        }
    }
}