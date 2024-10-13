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