using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class GenderPropertyTests
{
    private const string GROUP = "myGroup";
    private const string IDENTITY = "identity";

    [DataTestMethod()]
    [DataRow(GROUP, Gender.Male, null)]
    [DataRow(GROUP, Gender.Female, null)]
    [DataRow(GROUP, Gender.Other, null)]
    [DataRow(GROUP, Gender.Unknown, null)]
    [DataRow(GROUP, Gender.NonOrNotApplicable, null)]
    [DataRow(GROUP, null, null)]
    [DataRow(null, null, IDENTITY)]
    [DataRow(null, Gender.Female, IDENTITY)]
    [DataRow(GROUP, Gender.Female, IDENTITY)]
    public void GenderPropertyTest1(string? expectedGroup, Gender? expectedSex, string? expectedGenderIdentity)
    {
        var genderProp = new GenderProperty(expectedSex, expectedGenderIdentity, expectedGroup);

        Assert.IsNotNull(genderProp.Value);
        Assert.AreEqual(expectedGroup, genderProp.Group);
        Assert.AreEqual(expectedSex, genderProp.Value.Gender);
        Assert.AreEqual(expectedGenderIdentity, genderProp.Value.GenderIdentity);
    }


    [DataTestMethod()]
    [DataRow(GROUP + ".GENDER:M", GROUP, Gender.Male, null)]
    [DataRow(GROUP + ".GENDER:F", GROUP, Gender.Female, null)]
    [DataRow(GROUP + ".GENDER:O", GROUP, Gender.Other, null)]
    [DataRow(GROUP + ".GENDER:U", GROUP, Gender.Unknown, null)]
    [DataRow(GROUP + ".GENDER:N", GROUP, Gender.NonOrNotApplicable, null)]
    [DataRow(GROUP + ".GENDER:", GROUP, null, null)]
    [DataRow(GROUP + ".GENDER:;", GROUP, null, null)]
    [DataRow(GROUP + ".GENDER: ; ", GROUP, null, null)]
    [DataRow("GENDER: ;" + IDENTITY, null, null, IDENTITY)]
    [DataRow("GENDER:F;" + IDENTITY, null, Gender.Female, IDENTITY)]
    [DataRow(GROUP + ".GENDER:F;" + IDENTITY, GROUP, Gender.Female, IDENTITY)]
    [DataRow(GROUP + ".GENDER:F;", GROUP, Gender.Female, null)]
    public void GenderPropertyTest2(string s, string? expectedGroup, Gender? expectedSex, string? expectedGenderIdentity)
    {
        var vcfRow = VcfRow.Parse(s, new VcfDeserializationInfo());

        Assert.IsNotNull(vcfRow);

        var genderProp = new GenderProperty(vcfRow!, VCdVersion.V4_0);

        Assert.IsNotNull(genderProp.Value);
        Assert.AreEqual(expectedGroup, genderProp.Group);
        Assert.AreEqual(expectedSex, genderProp.Value.Gender);
        Assert.AreEqual(expectedGenderIdentity, genderProp.Value.GenderIdentity);
    }


    [TestMethod]
    public void GenderPropertyTest3()
    {
        var prop = new GenderProperty(Gender.Female, IDENTITY, GROUP);

        var vcard = new VCard
        {
            GenderViews = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.GenderViews);

        prop = vcard.GenderViews!.First() as GenderProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(Gender.Female, prop!.Value.Gender);
        Assert.AreEqual(IDENTITY, prop!.Value.GenderIdentity);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }
}
