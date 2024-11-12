using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass]
public class GenderPropertyTests
{
    private const string GROUP = "myGroup";
    private const string IDENTITY = "identity";

    [DataTestMethod()]
    [DataRow(GROUP, Sex.Male, null)]
    [DataRow(GROUP, Sex.Female, null)]
    [DataRow(GROUP, Sex.Other, null)]
    [DataRow(GROUP, Sex.Unknown, null)]
    [DataRow(GROUP, Sex.NonOrNotApplicable, null)]
    [DataRow(GROUP, null, null)]
    [DataRow(null, null, IDENTITY)]
    [DataRow(null, Sex.Female, IDENTITY)]
    [DataRow(GROUP, Sex.Female, IDENTITY)]
    public void GenderPropertyTest1(string? expectedGroup, Sex? expectedSex, string? expectedGenderIdentity)
    {
        var genderProp = new GenderProperty(new Gender(expectedSex, expectedGenderIdentity), expectedGroup);

        Assert.IsNotNull(genderProp.Value);
        Assert.AreEqual(expectedGroup, genderProp.Group);
        Assert.AreEqual(expectedSex, genderProp.Value.Sex);
        Assert.AreEqual(expectedGenderIdentity, genderProp.Value.Identity);
    }


    [DataTestMethod()]
    [DataRow(GROUP + ".GENDER:M", GROUP, Sex.Male, null)]
    [DataRow(GROUP + ".GENDER:F", GROUP, Sex.Female, null)]
    [DataRow(GROUP + ".GENDER:O", GROUP, Sex.Other, null)]
    [DataRow(GROUP + ".GENDER:U", GROUP, Sex.Unknown, null)]
    [DataRow(GROUP + ".GENDER:N", GROUP, Sex.NonOrNotApplicable, null)]
    [DataRow(GROUP + ".GENDER:", GROUP, null, null)]
    [DataRow(GROUP + ".GENDER:;", GROUP, null, null)]
    [DataRow(GROUP + ".GENDER: ; ", GROUP, null, null)]
    [DataRow("GENDER: ;" + IDENTITY, null, null, IDENTITY)]
    [DataRow("GENDER:F;" + IDENTITY, null, Sex.Female, IDENTITY)]
    [DataRow(GROUP + ".GENDER:F;" + IDENTITY, GROUP, Sex.Female, IDENTITY)]
    [DataRow(GROUP + ".GENDER:F;", GROUP, Sex.Female, null)]
    public void GenderPropertyTest2(string s, string? expectedGroup, Sex? expectedSex, string? expectedGenderIdentity)
    {
        var vcfRow = VcfRow.Parse(s.AsMemory(), new VcfDeserializationInfo());

        Assert.IsNotNull(vcfRow);

        var genderProp = new GenderProperty(vcfRow!, VCdVersion.V4_0);

        Assert.IsNotNull(genderProp.Value);
        Assert.AreEqual(expectedGroup, genderProp.Group);
        Assert.AreEqual(expectedSex, genderProp.Value.Sex);
        Assert.AreEqual(expectedGenderIdentity, genderProp.Value.Identity);
    }


    [TestMethod]
    public void GenderPropertyTest3()
    {
        var prop = new GenderProperty(new Gender(Sex.Female, IDENTITY), GROUP);

        var vcard = new VCard
        {
            GenderViews = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.GenderViews);

        prop = vcard.GenderViews!.First() as GenderProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(Sex.Female, prop!.Value.Sex);
        Assert.AreEqual(IDENTITY, prop!.Value.Identity);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new GenderProperty(new Gender(Sex.Other));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new GenderProperty(new Gender(Sex.Other, "difficult"));

        var prop2 = (GenderProperty)prop1.Clone();

        Assert.AreSame(prop1.Value, prop2.Value);
        Assert.AreNotSame(prop1, prop2);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop1 = new GenderProperty(new Gender(Sex.Other, "difficult"));
        Assert.IsNotNull(prop1.ToString());
    }
}
