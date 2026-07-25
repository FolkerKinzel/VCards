using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class KindBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new KindBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Kind.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new KindBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Kind.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Kind.Edit((p, d) => new KindProperty(d), Enums.Kind.Individual)
            .VCard;

        Assert.IsNotNull(vc.Kind);
    }

    [TestMethod]
    public void SetTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new KindBuilder().Set(Enums.Kind.Individual));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new KindBuilder().Clear());

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new KindBuilder().Equals((KindBuilder?)null));

        var builder = new KindBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new KindBuilder().ToString());
}
