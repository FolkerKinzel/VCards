using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class ImppTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        Impp? adr = Impp.Mobile | Impp.Personal;

        var list = new List<string>();

        EnumValueCollector.Collect(adr, list);

        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Contains(ImppConverter.TypeValue.PERSONAL));

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
        var arr = (Impp[])Enum.GetValues(typeof(Impp));
        var list = new List<string>(1);

        foreach (Impp item in arr)
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
        var arr = (Impp[])Enum.GetValues(typeof(Impp));
        var list = new List<string>(1);

        foreach (Impp item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list[0]);

            Impp? comp = ImppConverter.Parse(list[0]);


            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp!.Value, item);

            var comp2 = (Impp)Enum.Parse(
                typeof(Impp), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
