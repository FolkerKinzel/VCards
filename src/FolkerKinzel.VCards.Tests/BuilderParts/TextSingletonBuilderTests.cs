namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class TextSingletonBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new TextSingletonBuilder().Edit(p => { });

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().DirectoryName.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new TextSingletonBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextSingletonBuilder().Clear();

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
