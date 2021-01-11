using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class TelTypesCollectorTests
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            TelTypes? tel = TelTypes.Voice | TelTypes.Msg;

            var list = new List<string>();


            var collector = new TelTypesCollector();

            collector.CollectValueStrings(tel, list);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("MSG"));

            // collector darf die Liste nicht löschen!:
            collector.CollectValueStrings(tel, list);
            Assert.AreEqual(4, list.Count);

            // auf null testen:
            tel = null;
            list.Clear();

            collector.CollectValueStrings(tel, list);
            Assert.AreEqual(0, list.Count);
        }



        [TestMethod()]
        public void DetectAllEnumValues()
        {
            var arr = (TelTypes[])Enum.GetValues(typeof(TelTypes));
            var collector = new TelTypesCollector();

            var list = new List<string>(1);

            foreach (TelTypes item in arr)
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
            var arr = (TelTypes[])Enum.GetValues(typeof(TelTypes));
            var collector = new TelTypesCollector();

            var list = new List<string>(1);

            foreach (TelTypes item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                TelTypes? comp = (TelTypes)Enum.Parse(typeof(TelTypes), list[0], true);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp.Value, item);
            }
        }
    }
}