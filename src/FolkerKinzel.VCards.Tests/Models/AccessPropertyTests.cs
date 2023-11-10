using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class AccessPropertyTests
{
    private const string GROUP = "MyGroup";

    [TestMethod()]
    public void AccessPropertyTest1()
    {

/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
        var prop = new AccessProperty(Enums.Access.Confidential);
After:
        var prop = new AccessProperty(Access.Confidential);
*/
        var prop = new AccessProperty(VCards.Enums.Access.Confidential);


/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
        Assert.AreEqual(Enums.Access.Confidential, prop.Value);
        Assert.AreEqual(Enums.Access.Confidential, ((VCardProperty)prop).Value);
After:
        Assert.AreEqual(Access.Confidential, prop.Value);
        Assert.AreEqual(Access.Confidential, ((VCardProperty)prop).Value);
*/
        Assert.AreEqual(VCards.Enums.Access.Confidential, prop.Value);
        Assert.AreEqual(VCards.Enums.Access.Confidential, ((VCardProperty)prop).Value);

        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void AccessPropertyTest2()
    {
        var row = VcfRow.Parse($"{GROUP}.{VCard.PropKeys.CLASS}:private", new VcfDeserializationInfo());
        Assert.IsNotNull(row);

        var prop = new AccessProperty(row!);

        Assert.AreEqual(GROUP, prop.Group);

/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
        Assert.AreEqual(Enums.Access.Private, prop.Value);
        Assert.AreEqual(Enums.Access.Private, ((VCardProperty)prop).Value);
After:
        Assert.AreEqual(Access.Private, prop.Value);
        Assert.AreEqual(Access.Private, ((VCardProperty)prop).Value);
*/
        Assert.AreEqual(VCards.Enums.Access.Private, prop.Value);
        Assert.AreEqual(VCards.Enums.Access.Private, ((VCardProperty)prop).Value);


        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    public void AppendValueTest()
    {
        var vcard = new VCard
        {

/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
            Access = new AccessProperty(Enums.Access.Private)
After:
            Access = new AccessProperty(Access.Private)
*/
            Access = new AccessProperty(VCards.Enums.Access.Private)
        };

        string serialized = vcard.ToVcfString();

        IList<VCard> list = VCard.ParseVcf(serialized);

        Assert.AreEqual(1, list.Count);

        vcard = list[0];
        Assert.IsNotNull(vcard.Access);

/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
        Assert.AreEqual(Enums.Access.Private, vcard.Access.Value);
After:
        Assert.AreEqual(Access.Private, vcard.Access.Value);
*/
        Assert.AreEqual(VCards.Enums.Access.Private, vcard.Access.Value);
    }


}
