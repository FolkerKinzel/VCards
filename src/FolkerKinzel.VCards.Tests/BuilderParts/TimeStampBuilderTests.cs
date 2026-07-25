using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TimeStampBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TimeStampBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Updated.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TimeStampBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Updated.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Updated.Edit((p, d) => new TimeStampProperty(d), DateTimeOffset.UtcNow)
            .VCard;

        Assert.IsNotNull(vc.Updated);
    }

    [TestMethod]
    public void SetTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TimeStampBuilder().Set(DateTimeOffset.UtcNow));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new TimeStampBuilder().Clear());

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TimeStampBuilder().Equals((TimeStampBuilder?)null));

        var builder = new TimeStampBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TimeStampBuilder().ToString());
}
