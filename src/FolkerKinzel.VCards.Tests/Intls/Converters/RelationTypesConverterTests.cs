using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class RelationTypesConverterTests
{
    [TestMethod]
    public void ParseTest1()
    {
        foreach (Rel rel in (Rel[])Enum.GetValues(typeof(Rel)))
        {
            Rel? rel2 = RelConverter.Parse(RelConverter.ToVcfString(rel)!.ToUpperInvariant());
            Assert.AreEqual(rel, rel2);
        }

        Assert.IsNull(RelConverter.Parse("NICHT_VORHANDEN"));
    }

    [TestMethod]
    public void ParseTest2() => Assert.IsNull(RelConverter.Parse(null));

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = RelConverter.ToVcfString((Rel)4711);
}
