using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GramBuilderTests
{
    [TestMethod]
    public void SetPreferencesTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().SetPreferences());

    [TestMethod]
    public void UnsetPreferencesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().UnsetPreferences());

    [TestMethod]
    public void SetIndexesTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().SetIndexes());

    [TestMethod]
    public void UnsetIndexesTest1() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().UnsetIndexes());

    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GramGenders.Edit(null!));

    [TestMethod]
    public void EditTest3() 
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().GramGenders.Edit(null!, true));

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().Add(Gram.Feminine));

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new GramBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GramBuilder().Equals((TextBuilder?)null));

        var builder = new GramBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GramBuilder().ToString());
}
