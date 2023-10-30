using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class AccessPropertyTests
{
    private const string GROUP = "MyGroup";

    [TestMethod()]
    public void AccessPropertyTest1()
    {
        var prop = new AccessProperty(Enums.Access.Confidential);

        Assert.AreEqual(Enums.Access.Confidential, prop.Value);
        Assert.AreEqual(Enums.Access.Confidential, ((VCardProperty)prop).Value);

        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void AccessPropertyTest2()
    {
        var row = VcfRow.Parse($"{GROUP}.{VCard.PropKeys.CLASS}:private", new VcfDeserializationInfo());
        Assert.IsNotNull(row);

        var prop = new AccessProperty(row!);

        Assert.AreEqual(GROUP, prop.Group);
        Assert.AreEqual(Enums.Access.Private, prop.Value);
        Assert.AreEqual(Enums.Access.Private, ((VCardProperty)prop).Value);


        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    public void AppendValueTest()
    {
        var vcard = new VCard
        {
            Access = new AccessProperty(Enums.Access.Private)
        };

        string serialized = vcard.ToVcfString();

        IList<VCard> list = VCard.ParseVcf(serialized);

        Assert.AreEqual(1, list.Count);

        vcard = list[0];
        Assert.IsNotNull(vcard.Access);
        Assert.AreEqual(Enums.Access.Private, vcard.Access.Value);
    }


}
