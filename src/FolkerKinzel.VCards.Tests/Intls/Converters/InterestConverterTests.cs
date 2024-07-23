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
            Interest? kind2 = InterestConverter.Parse(kind.ToString().AsSpan());
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
    public void ParseTest() => Assert.IsNull(InterestConverter.Parse("nichtvorhanden".AsSpan()));
}

[TestClass]
public class GramConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Gram gram in (Gram[])Enum.GetValues(typeof(Gram)))
        {
            Gram? gram2 = GramConverter.Parse(gram.ToString().AsSpan());
            Assert.AreEqual(gram, gram2);
            object gram3 = Enum.Parse(typeof(Gram), ((Gram?)gram).ToVcfString() ?? "", true);
            Assert.AreEqual(gram, gram3);
        }

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Gram?)4711).ToVcfString());
    }

    [TestMethod]
    public void ParseTest() => Assert.IsNull(GramConverter.Parse("nichtvorhanden".AsSpan()));
}
