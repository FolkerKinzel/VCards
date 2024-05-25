using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class TelTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        Tel? tel = Tel.Voice | Tel.Msg;

        var list = new List<string>();

        EnumValueCollector.Collect(tel, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains("MSG"));

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(tel, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        tel = null;
        list.Clear();

        EnumValueCollector.Collect(tel, list);
        Assert.AreEqual(0, list.Count);
    }



    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (Tel[])Enum.GetValues(typeof(Tel));

        var list = new List<string>(1);

        foreach (Tel item in arr)
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
        var arr = (Tel[])Enum.GetValues(typeof(Tel));

        var list = new List<string>(1);

        foreach (Tel item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            //TelTypes? comp = (TelTypes)Enum.Parse(typeof(TelTypes), list[0], true);

            //Assert.IsTrue(comp.HasValue);
            //Assert.AreEqual(comp.Value, item);


            Tel? comp = TelConverter.Parse(list[0].AsSpan());

            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp!.Value, item);

            var comp2 = (Tel)Enum.Parse(typeof(Tel), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
