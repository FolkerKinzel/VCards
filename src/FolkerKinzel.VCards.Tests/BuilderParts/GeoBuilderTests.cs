namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class GeoBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new GeoBuilder().Edit(p => { });

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().GeoCoordinates.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GeoBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GeoBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GeoBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GeoBuilder().Equals((GeoBuilder?)null));

        var builder = new GeoBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GeoBuilder().ToString());
}
