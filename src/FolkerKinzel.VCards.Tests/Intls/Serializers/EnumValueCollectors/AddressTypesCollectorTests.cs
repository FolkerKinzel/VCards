﻿using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class AddressTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        AddressTypes? adr = AddressTypes.Dom | AddressTypes.Parcel;

        var list = new List<string>();

        EnumValueCollector.Collect(adr, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains(AddressTypesConverter.AddressTypesValue.PARCEL));

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(adr, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        adr = null;
        list.Clear();

        EnumValueCollector.Collect(adr, list);
        Assert.AreEqual(0, list.Count);
    }


    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (AddressTypes[])Enum.GetValues(typeof(AddressTypes));
        var list = new List<string>(1);

        foreach (AddressTypes item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);
        }
    }


    [TestMethod()]
    public void RoundTrip()
    {
        var arr = (AddressTypes[])Enum.GetValues(typeof(AddressTypes));
        var list = new List<string>(1);

        foreach (AddressTypes item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

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
