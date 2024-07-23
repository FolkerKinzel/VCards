using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class PhoneticConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Phonetic p in (Phonetic[])Enum.GetValues(typeof(Phonetic)))
        {
            Phonetic? p2 = PhoneticConverter.Parse(p.ToString().AsSpan());
            Assert.AreEqual(p, p2);
            object p3 = Enum.Parse(typeof(Phonetic), ((Phonetic?)p).ToVcfString() ?? "", true);
            Assert.AreEqual(p, p3);
        }

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Phonetic?)4711).ToVcfString());
    }

    [TestMethod]
    public void ParseTest() => Assert.IsNull(PhoneticConverter.Parse("nichtvorhanden".AsSpan()));
}
