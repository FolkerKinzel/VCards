using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class NonStandardBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new NonStandardBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().NonStandards.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new NonStandardBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().NonStandards.Edit(null!, true);

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new NonStandardBuilder().Add("X-TEST", null);

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
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new NonStandardBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new NonStandardBuilder().Remove(p => true);

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
