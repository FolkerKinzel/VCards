﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class PropertyClassTypesCollectorTests

    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            PropertyClassTypes? adr = PropertyClassTypes.Home | PropertyClassTypes.Work;

            var list = new List<string>();


            var collector = new PropertyClassTypesCollector();

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
            var arr = (PropertyClassTypes[])Enum.GetValues(typeof(PropertyClassTypes));
            var collector = new PropertyClassTypesCollector();

            var list = new List<string>(1);

            foreach (PropertyClassTypes item in arr)
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
            var arr = (PropertyClassTypes[])Enum.GetValues(typeof(PropertyClassTypes));
            var collector = new PropertyClassTypesCollector();

            var list = new List<string>(1);

            foreach (PropertyClassTypes item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                PropertyClassTypes comp;

                comp = (PropertyClassTypes)Enum.Parse(typeof(PropertyClassTypes), list[0], true);

                Assert.AreEqual(comp, item);
            }
        }
    }
}