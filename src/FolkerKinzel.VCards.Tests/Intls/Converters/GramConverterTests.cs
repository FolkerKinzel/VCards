using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class GramConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Gram gram in (Gram[])Enum.GetValues(typeof(Gram)))
        {
            Assert.IsTrue(GramConverter.TryParse(gram.ToString().AsSpan(), out Gram gram2));
            Assert.AreEqual(gram, gram2);
            object gram3 = Enum.Parse(typeof(Gram), ((Gram)gram).ToVcfString() ?? "", true);
            Assert.AreEqual(gram, gram3);
        }

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Gram)4711).ToVcfString());
    }

    [TestMethod]
    public void ParseTest() => Assert.IsFalse(GramConverter.TryParse("nichtvorhanden".AsSpan(), out _));
}