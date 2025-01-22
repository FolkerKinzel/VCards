using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class ContactIDBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new ContactIDBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().ContactID.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new ContactIDBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().ContactID.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create(setContactID: false)
            .ContactID.Edit((p, d) => new ContactIDProperty(d), ContactID.Create())
            .VCard;

        Assert.IsNotNull(vc.ContactID);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new ContactIDBuilder().Set(ContactID.Empty);

    [TestMethod]
    public void SetTest2()
    {
        var vc = VCardBuilder.Create().ContactID.Set(new Uri("text", UriKind.Relative)).VCard;
        Assert.IsNotNull(vc.ContactID);
        Assert.IsNotNull(vc.ContactID.Value.String);
    }

    [TestMethod]
    public void SetTest3()
    {
        var vc = VCardBuilder.Create().ContactID.Set((Uri?)null).VCard;
        Assert.IsNotNull(vc.ContactID);
        Assert.IsTrue(vc.ContactID.Value.IsEmpty);
    }

    [TestMethod]
    public void SetTest4()
    {
        var vc = VCardBuilder.Create().ContactID.Set("   ").VCard;
        Assert.IsNotNull(vc.ContactID);
        Assert.IsNotNull(vc.ContactID.Value.IsEmpty);
    }

    [TestMethod]
    public void SetTest5()
    {
        var vc = VCardBuilder.Create().ContactID.Set("id").VCard;
        Assert.IsNotNull(vc.ContactID);
        Assert.IsNotNull(vc.ContactID.Value.String);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new ContactIDBuilder().Clear();

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new ContactIDBuilder().Equals((ContactIDBuilder?)null));

        var builder = new ContactIDBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new ContactIDBuilder().ToString());
}
