using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class RelationTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        Rel? rel = Rel.Spouse | Rel.CoResident;

        var list = new List<string>();

        EnumValueCollector.Collect(rel, list);

        Assert.HasCount(2, list);

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(rel, list);
        Assert.HasCount(4, list);

        // auf null testen:
        rel = null;
        list.Clear();

        EnumValueCollector.Collect(rel, list);
        Assert.IsEmpty(list);
    }


    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (Rel[])Enum.GetValues(typeof(Rel));

        var list = new List<string>(1);

        foreach (Rel item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.HasCount(1, list);
        }
    }


    [TestMethod()]
    public void RoundTrip()
    {
        var arr = (Rel[])Enum.GetValues(typeof(Rel));

        var list = new List<string>(1);

        foreach (Rel item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.HasCount(1, list);
            Assert.IsNotNull(list[0]);

            Rel? comp = RelConverter.Parse(list[0].AsSpan());

            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp.Value, item);

            var comp2 = (Rel)Enum.Parse(
                typeof(Rel), list[0].Replace("-", ""), true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
