namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class XmlBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new XmlBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new XmlBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new XmlBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new XmlBuilder().Equals((XmlBuilder?)null));

        var builder = new XmlBuilder();
        Assert.AreEqual(builder.GetHashCode(),((object) builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new XmlBuilder().ToString());
}
