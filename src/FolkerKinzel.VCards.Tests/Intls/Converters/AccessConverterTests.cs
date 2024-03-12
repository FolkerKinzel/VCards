using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AccessConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Acs kind in (Acs[])Enum.GetValues(typeof(Acs)))
        {
            Acs kind2 = AccessConverter.Parse(kind.ToString());
            Assert.AreEqual(kind, kind2);
            object kind3 = Enum.Parse(typeof(Acs), kind.ToVCardString(), true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(Acs.Public, AccessConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(Acs.Public.ToVCardString(), ((Acs)4711).ToVCardString());
    }
}
