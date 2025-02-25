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
            Assert.IsTrue(KindConverter.TryParse(kind.ToString().AsSpan(), out Kind kind2));
            Assert.AreEqual(kind, kind2);

            object kind3 = Enum.Parse(typeof(Kind), kind.ToVcfString(), true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf nicht definiert
        Assert.AreEqual(Kind.Individual.ToVcfString(), ((Kind)4711).ToVcfString());
    }


    [TestMethod]
    public void TryParseTest1() => Assert.IsFalse(KindConverter.TryParse("blabla".AsSpan(), out _));
}
