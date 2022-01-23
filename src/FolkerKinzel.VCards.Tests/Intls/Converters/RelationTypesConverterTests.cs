using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class RelationTypesConverterTests
{
    [TestMethod()]
    public void ParseTest1()
    {
        foreach (RelationTypes rel in (RelationTypes[])Enum.GetValues(typeof(RelationTypes)))
        {
            RelationTypes? rel2 = RelationTypesConverter.Parse(RelationTypesConverter.ToVcfString(rel)!.ToUpperInvariant());
            Assert.AreEqual(rel, rel2);
        }

        Assert.IsNull(RelationTypesConverter.Parse("NICHT_VORHANDEN"));
    }

    [TestMethod]
    public void ParseTest2() => Assert.IsNull(RelationTypesConverter.Parse(null));

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ToVcfStringTest() => _ = RelationTypesConverter.ToVcfString((RelationTypes)4711);
}
