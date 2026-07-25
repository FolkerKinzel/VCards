using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

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

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.HasCount(1, list);

        vcard = list[0];

        Assert.IsNotNull(vcard.GeoCoordinates);

        prop = vcard.GeoCoordinates.FirstOrNull();
        Assert.IsNotNull(prop);
        Assert.AreEqual(geo, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        const string GROUP = "group";
        var prop = new GeoProperty(new GeoCoordinate(42, 42), GROUP);
        Assert.HasCount(1, prop.AsWeakEnumerable());
        Assert.AreEqual(GROUP, prop.Group, true);

    }

    [TestMethod()]
    public void GeoPropertyTest3()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => new GeoProperty(null!));

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new GeoProperty(new GeoCoordinate(15.7, 14.8));

        var prop2 = (GeoProperty)prop1.Clone();

        Assert.AreSame(prop1.Value, prop2.Value);
        Assert.AreNotSame(prop1, prop2);
    }

    [TestMethod]
    public void TryParseTest1()
        => Assert.IsFalse(GeoProperty.TryParse(VcfRow.Parse("GEO:1000;-1000".AsMemory(),
                                                             new VcfDeserializationInfo())!,
                          out _));
}
