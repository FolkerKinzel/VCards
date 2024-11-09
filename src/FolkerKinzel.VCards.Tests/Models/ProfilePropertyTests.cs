using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class ProfilePropertyTests
{
    [TestMethod()]
    public void ProfilePropertyTest1()
    {
        var prop = new ProfileProperty();

        Assert.IsNotNull(prop);
        Assert.IsNotNull(prop.Value);
        Assert.AreEqual("VCARD", prop.Value);

        string s = prop.Value;

        VCardProperty vcProp = prop;
        Assert.AreEqual(vcProp.Value, s);

        TextProperty textProp = prop;
        Assert.AreEqual(textProp.Value, s);
    }


    [TestMethod()]
    public void ProfilePropertyTest2()
    {
        var prop = new ProfileProperty();

        var vcard = new VCard
        {
            Profile = prop
        };

        string s = vcard.ToVcfString();

        IReadOnlyList<VCard> list = Vcf.Parse(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.Profile);

        prop = vcard.Profile;

        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod]
    public void GetValueTest()
    {
        VcfRow row = VcfRow.Parse("Profile:".AsMemory(), new VcfDeserializationInfo())!;
        var prop = new ProfileProperty(row, VCdVersion.V3_0);

        Assert.AreEqual("VCARD", prop.Value);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new ProfileProperty();

        var prop2 = (ProfileProperty)prop1.Clone();

        Assert.AreSame(prop1.Value, prop2.Value);
        Assert.AreNotSame(prop1, prop2);
    }
}
