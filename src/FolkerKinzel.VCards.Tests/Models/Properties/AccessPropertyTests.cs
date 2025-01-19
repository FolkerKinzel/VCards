using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass()]
public class AccessPropertyTests
{
    private const string GROUP = "MyGroup";

    [TestMethod()]
    public void AccessPropertyTest1()
    {
        var prop = new AccessProperty(Access.Confidential);

        Assert.AreEqual(Access.Confidential, prop.Value);
        Assert.AreEqual(Access.Confidential, ((VCardProperty)prop).Value);

        Assert.IsFalse(prop.IsEmpty);
    }


    [TestMethod()]
    public void AccessPropertyTest2()
    {
        var row = VcfRow.Parse($"{GROUP}.{VCard.PropKeys.CLASS}:private".AsMemory(), new VcfDeserializationInfo());
        Assert.IsNotNull(row);

        Assert.IsTrue(AccessProperty.TryParse(row, out AccessProperty? prop));

        Assert.AreEqual(GROUP, prop.Group);

        Assert.AreEqual(Access.Private, prop.Value);
        Assert.AreEqual(Access.Private, ((VCardProperty)prop).Value);

        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    public void AppendValueTest()
    {
        var vcard = new VCard
        {
            Access = new AccessProperty(Access.Private)
        };

        string serialized = vcard.ToVcfString();

        IReadOnlyList<VCard> list = Vcf.Parse(serialized);

        Assert.AreEqual(1, list.Count);

        vcard = list[0];
        Assert.IsNotNull(vcard.Access);

        Assert.AreEqual(Access.Private, vcard.Access.Value);
    }

    [TestMethod]
    public void CloneTest1()
    {
        var prop1 = new AccessProperty(Access.Private);

        var prop2 = (AccessProperty)prop1.Clone();

        Assert.AreEqual(prop1.Value, prop2.Value);
        Assert.AreNotSame(prop1, prop2);
    }

    [TestMethod]
    public void TryParseTest1() 
        => Assert.IsFalse(AccessProperty.TryParse(VcfRow.Parse("CLASS:blabla".AsMemory(), new VcfDeserializationInfo())!, out _));

}
