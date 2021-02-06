using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class AddressTypesCollectorTests
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            AddressTypes? adr = AddressTypes.Dom | AddressTypes.Parcel;

            var list = new List<string>();


            var collector = new AddressTypesCollector();

            collector.CollectValueStrings(adr, list);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains(AddressTypesConverter.AdrTypeValue.PARCEL));

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
            var arr = (AddressTypes[])Enum.GetValues(typeof(AddressTypes));
            var collector = new AddressTypesCollector();

            var list = new List<string>(1);

            foreach (AddressTypes item in arr)
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
            var arr = (AddressTypes[])Enum.GetValues(typeof(AddressTypes));
            var collector = new AddressTypesCollector();

            var list = new List<string>(1);

            foreach (AddressTypes item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                AddressTypes? comp = AddressTypesConverter.Parse(list[0]);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp!.Value, item);

                var comp2 = (AddressTypes)Enum.Parse(
                    typeof(AddressTypes), list[0], true);

                Assert.AreEqual(comp, comp2);
            }
        }

    }
}