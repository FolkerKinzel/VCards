namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class ProfileBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new ProfileBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().Profile.Edit(null!));

    [TestMethod]
    public void SetTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new ProfileBuilder().Set(null));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new ProfileBuilder().Clear());

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new ProfileBuilder().Equals((ProfileBuilder?)null));

        var builder = new ProfileBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new ProfileBuilder().ToString());
}
