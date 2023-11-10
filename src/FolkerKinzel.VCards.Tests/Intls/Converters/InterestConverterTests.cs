using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class InterestConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Interest kind in (Interest[])Enum.GetValues(typeof(Interest)))
        {
            Interest? kind2 = InterestConverter.Parse(kind.ToString().ToLowerInvariant());
            Assert.AreEqual(kind, kind2);
            object kind3 = Enum.Parse(typeof(Interest), ((Interest?)kind).ToVCardString() ?? "", true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        //Assert.AreEqual(null, InterestLevelConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Interest?)4711).ToVCardString());
    }

    [TestMethod]
    public void ParseTest() => Assert.IsNull(InterestConverter.Parse("nichtvorhanden"));
}
