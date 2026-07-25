using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class NonStandardBuilderTests
{
    [TestMethod]
    public void EditTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new NonStandardBuilder().Edit(p => p));

    [TestMethod]
    public void EditTest2()
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().NonStandards.Edit(null!));

    [TestMethod]
    public void EditTest3()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new NonStandardBuilder().Edit((p, d) => p, true));

    [TestMethod]
    public void EditTest4() 
        => _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => VCardBuilder.Create().NonStandards.Edit(null!, true));

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create(setContactID: false)
            .NonStandards.Edit((p, d) => d, new NonStandardProperty("X-TEST", "The value"))
            .VCard;

        Assert.IsNotNull(vc.NonStandards);
    }

    [TestMethod]
    public void AddTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new NonStandardBuilder().Add("X-TEST", null));

    [TestMethod]
    public void AddTest2()
    {
        var vc = VCardBuilder.Create().NonStandards.Add(null!, "the value").VCard;
        var nonStandard = vc.NonStandards?.FirstOrDefault();
        Assert.IsNotNull(nonStandard);
        Assert.IsTrue(nonStandard.IsEmpty);
    }

    [TestMethod]
    public void AddTest3()
    {
        var vc = VCardBuilder.Create().NonStandards.Add(null!, "the value", group: v => "G").VCard;
        var nonStandard = vc.NonStandards?.FirstOrDefault();
        Assert.IsNotNull(nonStandard);
        Assert.IsTrue(nonStandard.IsEmpty);
        Assert.AreEqual("G", nonStandard.Group);
    }

    [TestMethod]
    public void ClearTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new NonStandardBuilder().Clear());

    [TestMethod]
    public void RemoveTest1()
        => _ = Assert.ThrowsExactly<InvalidOperationException>(
            () => new NonStandardBuilder().Remove(p => true));

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new NonStandardBuilder().Equals((NonStandardBuilder?)null));

        var builder = new NonStandardBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new NonStandardBuilder().ToString());
}
