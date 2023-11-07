using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class GeoPropertyTests
{
    private const string GROUP = "MyGroup";

    [TestMethod()]
    public void GeoPropertyTest1()
    {
        var geo = new GeoCoordinate(17.44, 8.33);

        var prop = new GeoProperty(geo, GROUP);

        Assert.AreEqual(geo, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    public void GeoPropertyTest2()
    {
        var geo = new GeoCoordinate(17.44, 8.33);

        var prop = new GeoProperty(geo, GROUP);

        var vcard = new VCard
        {
            GeoCoordinates = prop
        };

        string s = vcard.ToVcfString();

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.GeoCoordinates);

        prop = vcard.GeoCoordinates!.First();

        Assert.AreEqual(geo, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        const string GROUP = "group";
        var prop = new GeoProperty(42, 42, GROUP);
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
        Assert.AreEqual(GROUP, prop.Group, true);

    }
}
