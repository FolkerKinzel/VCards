using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class AccessBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new AccessBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Access.Edit(null!));

    [TestMethod]
    public void EditTest3()
         => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new AccessBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Access.Edit((p, d) => new AccessProperty(Access.Confidential), true)
            .VCard;

        Assert.IsNotNull(vc.Access);
    }

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Access.Edit(null!, true));

    [TestMethod]
    public void SetTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new AccessBuilder().Set(Access.Public));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new AccessBuilder().Clear());

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new AccessBuilder().Equals((AccessBuilder?)null));

        var builder = new AccessBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new AccessBuilder().ToString());
}
