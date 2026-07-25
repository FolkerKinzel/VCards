using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors.Tests;

[TestClass()]
public class AddressTypesCollectorTests
{
    [TestMethod()]
    public void CollectValueStringsTest()
    {
        Adr? adr = Adr.Dom | Adr.Parcel;

        var list = new List<string>();

        EnumValueCollector.Collect(adr, list);

        Assert.HasCount(2, list);
        Assert.Contains(AdrConverter.AddressTypesValue.PARCEL, list);

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
        var arr = (Adr[])Enum.GetValues(typeof(Adr));
        var list = new List<string>(1);

        foreach (Adr item in arr)
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
        var arr = (Adr[])Enum.GetValues(typeof(Adr));
        var list = new List<string>(1);

        foreach (Adr item in arr)
        {
            list.Clear();
            EnumValueCollector.Collect(item, list);

            Assert.HasCount(1, list);
            Assert.IsNotNull(list[0]);

            Adr? comp = AdrConverter.Parse(list[0].AsSpan());

            Assert.IsTrue(comp.HasValue);
            Assert.AreEqual(comp.Value, item);

            var comp2 = (Adr)Enum.Parse(
                typeof(Adr), list[0], true);

            Assert.AreEqual(comp, comp2);
        }
    }

}
