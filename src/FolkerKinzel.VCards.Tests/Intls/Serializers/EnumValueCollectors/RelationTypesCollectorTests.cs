using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class RelationTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        RelationTypes? rel = RelationTypes.Spouse | RelationTypes.CoResident;

        var list = new List<string>();

        EnumValueCollector.Collect(rel, list);

        Assert.AreEqual(2, list.Count);

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(rel, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        rel = null;
        list.Clear();

        EnumValueCollector.Collect(rel, list);
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
            EnumValueCollector.Collect(item, list);

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
            EnumValueCollector.Collect(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            RelationTypes? comp = RelationTypesConverter.Parse(list[0]);

            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp!.Value, item);

            var comp2 = (RelationTypes)Enum.Parse(
                typeof(RelationTypes), list[0].Replace("-", ""), true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
