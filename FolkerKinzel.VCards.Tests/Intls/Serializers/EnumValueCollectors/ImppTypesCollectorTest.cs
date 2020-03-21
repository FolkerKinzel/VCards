using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests
{
    [TestClass()]
    public class ImppTypesCollectorTest
    {
        [TestMethod()]
        public void CollectValueStringsTest()
        {
            ImppTypes? adr = ImppTypes.Mobile | ImppTypes.Personal;

            List<string> list = new List<string>();


            ImppTypesCollector collector = new ImppTypesCollector();

            collector.CollectValueStrings(adr, list);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains(ImppTypesConverter.TypeValue.Personal));

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
            ImppTypes[] arr = (ImppTypes[])Enum.GetValues(typeof(ImppTypes));
            var collector = new ImppTypesCollector();

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
            ImppTypes[] arr = (ImppTypes[])Enum.GetValues(typeof(ImppTypes));
            var collector = new ImppTypesCollector();

            var list = new List<string>(1);

            foreach (var item in arr)
            {
                list.Clear();
                collector.CollectValueStrings(item, list);

                Assert.AreEqual(1, list.Count);
                Assert.IsNotNull(list[0]);

                ImppTypes? comp = null;

                comp = ImppTypesConverter.Parse(list[0], comp);

                Assert.IsTrue(comp.HasValue);
                Assert.AreEqual(comp!.Value, item);

                var comp2 = (ImppTypes)Enum.Parse(
                    typeof(ImppTypes), list[0], true);

                Assert.AreEqual(comp, comp2);
            }
        }
    }
}