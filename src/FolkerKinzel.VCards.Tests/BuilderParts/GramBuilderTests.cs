using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GramBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new GramBuilder().SetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new GramBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new GramBuilder().SetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new GramBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new GramBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().GramGenders.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new GramBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().GramGenders.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GramBuilder().Add(Gram.Feminine);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GramBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GramBuilder().Remove(p => true);

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
