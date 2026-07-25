using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;

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

        Assert.HasCount(2, list);
        Assert.Contains(ImppConverter.TypeValue.PERSONAL, list);

        // collector darf die Liste nicht löschen!:
        EnumValueCollector.Collect(adr, list);
        Assert.HasCount(4, list);

        // auf null testen:
        adr = null;
        list.Clear();

        EnumValueCollector.Collect(adr, list);
        Assert.IsEmpty(list);
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

            Assert.HasCount(1, list);
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

            Assert.HasCount(1, list);
            Assert.IsNotNull(list[0]);

            Impp? comp = ImppConverter.Parse(list[0].AsSpan());


            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp.Value, item);

            var comp2 = (Impp)Enum.Parse(
                typeof(Impp), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }
}
