namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class DataBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new DataBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Photos.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddFileTest1() => new DataBuilder().AddFile("file");

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddUriTest1() => new DataBuilder().AddUri(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddBytesTest1() => new DataBuilder().AddBytes(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTextTest1() => new DataBuilder().AddText(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new DataBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new DataBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new DataBuilder().Equals((DataBuilder?)null));

        var builder = new DataBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DataBuilder().ToString());
}
