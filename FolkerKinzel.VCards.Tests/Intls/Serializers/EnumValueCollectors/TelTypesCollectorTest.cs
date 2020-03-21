using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class TelTypesCollectorTest
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            TelTypes? tel = TelTypes.Voice | TelTypes.Msg;

            List<string> list = new List<string>();


            TelTypesCollector collector = new TelTypesCollector();

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
            TelTypes[] arr = (TelTypes[])Enum.GetValues(typeof(TelTypes));
            var collector = new TelTypesCollector();

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
            TelTypes[] arr = (TelTypes[])Enum.GetValues(typeof(TelTypes));
            var collector = new TelTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                TelTypes? comp = null;

                comp = (TelTypes)Enum.Parse(typeof(TelTypes), list[0], true);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp.Value, item);
            }
        }
    }
}