namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class UuidBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new UuidBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new UuidBuilder().Clear();

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new UuidBuilder().Equals((UuidBuilder?)null));

        var builder = new UuidBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new UuidBuilder().ToString());
}
