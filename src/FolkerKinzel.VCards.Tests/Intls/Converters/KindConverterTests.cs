using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class KindConverterTests
{
    [TestMethod()]
    public void Roundtrip()
    {
        foreach (Kind kind in (Kind[])Enum.GetValues(typeof(Kind)))
        {
            Kind kind2 = KindConverter.Parse(kind.ToString());
            Assert.AreEqual(kind, kind2);

            object kind3 = Enum.Parse(typeof(Kind), kind.ToVcfString(), true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(Kind.Individual, KindConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(Kind.Individual.ToVcfString(), ((Kind)4711).ToVcfString());
    }
}
