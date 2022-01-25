using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class ImppTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        ImppTypes? adr = ImppTypes.Mobile | ImppTypes.Personal;

        var list = new List<string>();

        ImppTypesCollector.CollectValueStrings(adr, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains(ImppTypesConverter.TypeValue.Personal));

        // collector darf die Liste nicht löschen!:
        ImppTypesCollector.CollectValueStrings(adr, list);
        Assert.AreEqual(4, list.Count);

        // auf null testen:
        adr = null;
        list.Clear();

        ImppTypesCollector.CollectValueStrings(adr, list);
        Assert.AreEqual(0, list.Count);
    }


    [TestMethod()]
    public void DetectAllEnumValues()
    {
        var arr = (ImppTypes[])Enum.GetValues(typeof(ImppTypes));
        var list = new List<string>(1);

        foreach (ImppTypes item in arr)
        {
            list.Clear();
            ImppTypesCollector.CollectValueStrings(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);
        }
    }


    [TestMethod()]
    public void RoundTrip()
    {
        var arr = (ImppTypes[])Enum.GetValues(typeof(ImppTypes));
        var list = new List<string>(1);

        foreach (ImppTypes item in arr)
        {
            list.Clear();
            ImppTypesCollector.CollectValueStrings(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            ImppTypes? comp = ImppTypesConverter.Parse(list[0]);


            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp!.Value, item);

            var comp2 = (ImppTypes)Enum.Parse(
                typeof(ImppTypes), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
