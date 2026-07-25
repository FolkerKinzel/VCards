using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextSingletonBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextSingletonBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().DirectoryName.Edit(null!));

    [TestMethod]
    public void EditTest3() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextSingletonBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().DirectoryName.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create(setContactID: false)
            .DirectoryName.Edit((p, d) => new TextProperty(d), "The directory")
            .VCard;

        Assert.IsNotNull(vc.DirectoryName);
    }

    [TestMethod]
    public void SetTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextSingletonBuilder().Set(null));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TextSingletonBuilder().Clear());

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextSingletonBuilder().Equals((TextSingletonBuilder?)null));

        var builder = new TextSingletonBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextSingletonBuilder().ToString());
}
