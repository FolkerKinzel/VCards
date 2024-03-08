namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class ProfileBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new ProfileBuilder().Edit(p => { });

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Profile.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new ProfileBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new ProfileBuilder().Clear();

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
