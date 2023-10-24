using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class TelTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        PhoneTypes? tel = PhoneTypes.Voice | PhoneTypes.Msg;

        var list = new List<string>();

        PhoneTypesCollector.CollectValueStrings(tel, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains("MSG"));

        // collector darf die Liste nicht löschen!:
        PhoneTypesCollector.CollectValueStrings(tel, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        tel = null;
        list.Clear();

        PhoneTypesCollector.CollectValueStrings(tel, list);
        Assert.AreEqual(0, list.Count);
    }



    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (PhoneTypes[])Enum.GetValues(typeof(PhoneTypes));

        var list = new List<string>(1);

        foreach (PhoneTypes item in arr)
        {
            list.Clear();
            PhoneTypesCollector.CollectValueStrings(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);
        }
    }


    [TestMethod()]
    public void RoundTrip()
    {
        var arr = (PhoneTypes[])Enum.GetValues(typeof(PhoneTypes));

        var list = new List<string>(1);

        foreach (PhoneTypes item in arr)
        {
            list.Clear();
            PhoneTypesCollector.CollectValueStrings(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            //TelTypes? comp = (TelTypes)Enum.Parse(typeof(TelTypes), list[0], true);

            //Assert.IsTrue(comp.HasValue);
            //Assert.AreEqual(comp.Value, item);


            PhoneTypes? comp = PhoneTypesConverter.Parse(list[0]);

            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp!.Value, item);

            var comp2 = (PhoneTypes)Enum.Parse(typeof(PhoneTypes), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
