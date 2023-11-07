using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;


#pragma warning disable CS0618 // Typ oder Element ist veraltet

[TestClass()]
public class AddressPropertyTests
{
    private const string STREET = "Schierauer Str. 24";
    private const string LOCALITY = "Dessau";
    private const string POSTAL_CODE = "06789";
    private const string REGION = "Sachsen-Anhalt";
    private const string COUNTRY = "BRD";
    private const string PO_BOX = "PO Box";
    private const string EXTENDED_ADDRESS = "extended";
    private const string GROUP = "myGroup";


    [TestMethod()]
    public void AddressPropertyTest()
    {
        var adr = new AddressProperty(STREET, LOCALITY, REGION, POSTAL_CODE, COUNTRY, PO_BOX, EXTENDED_ADDRESS, group: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(STREET, adr.Value.Street[0]);
        Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
        Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
        Assert.AreEqual(REGION, adr.Value.Region[0]);
        Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
        Assert.AreEqual(PO_BOX, adr.Value.PostOfficeBox[0]);
        Assert.AreEqual(EXTENDED_ADDRESS, adr.Value.ExtendedAddress[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    [TestMethod()]
    public void AddressPropertyTest1()
    {
        var adr = new AddressProperty(
            new string[] { STREET },
            new string[] { LOCALITY },
            new string[] { REGION },
            new string[] { POSTAL_CODE },
            new string[] { COUNTRY },
            new string[] { PO_BOX },
            new string[] { EXTENDED_ADDRESS },

            group: GROUP);

        Assert.IsNotNull(adr);
        Assert.AreEqual(STREET, adr.Value.Street[0]);
        Assert.AreEqual(LOCALITY, adr.Value.Locality[0]);
        Assert.AreEqual(POSTAL_CODE, adr.Value.PostalCode[0]);
        Assert.AreEqual(REGION, adr.Value.Region[0]);
        Assert.AreEqual(COUNTRY, adr.Value.Country[0]);
        Assert.AreEqual(PO_BOX, adr.Value.PostOfficeBox[0]);
        Assert.AreEqual(EXTENDED_ADDRESS, adr.Value.ExtendedAddress[0]);
        Assert.AreEqual(GROUP, adr.Group);
        Assert.IsFalse(adr.IsEmpty);
    }

    [TestMethod()]
    public void ToStringTest()
    {
        var adr = new AddressProperty(STREET, LOCALITY, REGION, POSTAL_CODE, COUNTRY, PO_BOX, EXTENDED_ADDRESS);

        string s = adr.ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);

    }

    [TestMethod]
    public void AddressPropertyTest3()
    {
        VcfRow row = VcfRow.Parse("ADR:", new VcfDeserializationInfo())!;
        var prop = new AddressProperty(row, VCdVersion.V3_0);

        Assert.IsNotNull(prop.Value);
    }


    [TestMethod]
    public void IsEmptyTest_v2_1()
    {
        const VCdVersion version = VCdVersion.V2_1;
        const string labelText = "Nice Label";

        AddressProperty adr = new("", null, null, null, autoLabel: false);
        Assert.IsTrue(adr.IsEmpty);

        var vc = new VCard
        {
            Addresses = adr
        };

        Assert.IsTrue(vc.IsEmpty());

        string vcf = vc.ToVcfString(version);
        IList<VCard> cards = VCard.ParseVcf(vcf);
        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNull(vc.Addresses);

        adr.Parameters.Label = labelText;
        Assert.IsFalse(adr.IsEmpty);

        vc.Addresses = adr;

        vcf = vc.ToVcfString(version);
        cards = VCard.ParseVcf(vcf);

        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNotNull(vc.Addresses);

        adr = vc.Addresses!.First()!;
        Assert.IsTrue(adr.Value.IsEmpty);
        Assert.IsFalse(adr.IsEmpty);
        Assert.AreEqual(labelText, adr.Parameters.Label);
    }

    [TestMethod]
    public void IsEmptyTest_v3_0()
    {
        const VCdVersion version = VCdVersion.V3_0;
        const string labelText = "Nice Label";

        AddressProperty adr = new("", null, null, null, autoLabel: false);
        Assert.IsTrue(adr.IsEmpty);

        var vc = new VCard
        {
            Addresses = adr
        };

        Assert.IsTrue(vc.IsEmpty());

        string vcf = vc.ToVcfString(version);
        IList<VCard> cards = VCard.ParseVcf(vcf);
        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNull(vc.Addresses);

        adr.Parameters.Label = labelText;
        Assert.IsFalse(adr.IsEmpty);

        vc.Addresses = adr;

        vcf = vc.ToVcfString(version);
        cards = VCard.ParseVcf(vcf);

        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNotNull(vc.Addresses);

        adr = vc.Addresses!.First()!;
        Assert.IsTrue(adr.Value.IsEmpty);
        Assert.IsFalse(adr.IsEmpty);
        Assert.AreEqual(labelText, adr.Parameters.Label);
    }

    [TestMethod]
    public void IsEmptyTest_v4_0()
    {
        const VCdVersion version = VCdVersion.V4_0;
        const string labelText = "Nice Label";

        AddressProperty adr = new("", null, null, null);
        Assert.IsTrue(adr.IsEmpty);

        var vc = new VCard
        {
            Addresses = adr
        };

        Assert.IsTrue(vc.IsEmpty());

        string vcf = vc.ToVcfString(version);
        IList<VCard> cards = VCard.ParseVcf(vcf);
        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNull(vc.Addresses);

        adr.Parameters.Label = labelText;
        Assert.IsFalse(adr.IsEmpty);

        vc.Addresses = adr;

        vcf = vc.ToVcfString(version);
        cards = VCard.ParseVcf(vcf);

        Assert.IsNotNull(cards);
        Assert.AreEqual(1, cards.Count);
        vc = cards[0];
        Assert.IsNotNull(vc.Addresses);

        adr = vc.Addresses!.First()!;
        Assert.IsTrue(adr.Value.IsEmpty);
        Assert.IsFalse(adr.IsEmpty);
        Assert.AreEqual(labelText, adr.Parameters.Label);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new AddressProperty("Elm Street", null, null, null);
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
