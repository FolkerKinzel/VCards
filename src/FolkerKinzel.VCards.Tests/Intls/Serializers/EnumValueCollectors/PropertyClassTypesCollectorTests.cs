using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class PropertyClassTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        PCl? type = PCl.Home | PCl.Work;

        var list = new List<string>();

        EnumValueCollector.Collect(type, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains("WORK"));

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(type, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        type = null;
        list.Clear();

        EnumValueCollector.Collect(type, list);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (PCl[])Enum.GetValues(typeof(PCl));
        var list = new List<string>(1);

        foreach (PCl item in arr)
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
        var arr = (PCl[])Enum.GetValues(typeof(PCl));
        var list = new List<string>(1);

        foreach (PCl item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            PCl comp;

            comp = (PCl)Enum.Parse(typeof(PCl), list[0], true);

            Assert.AreEqual(comp, item);
        }
    }
}
