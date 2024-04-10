using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class ExpertiseConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Expertise kind in (Expertise[])Enum.GetValues(typeof(Expertise)))
        {
            Expertise? kind2 = ExpertiseConverter.Parse(kind.ToString());
            Assert.AreEqual(kind, kind2);
            object kind3 = Enum.Parse(typeof(Expertise), ((Expertise?)kind).ToVcfString() ?? "", true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        //Assert.AreEqual(null, ExpertiseLevelConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Expertise?)4711).ToVcfString());
    }

    [TestMethod]
    public void ParseTest() => Assert.IsNull(ExpertiseConverter.Parse("nichtvorhanden"));

}
