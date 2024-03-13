using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class AccessConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Access kind in (Access[])Enum.GetValues(typeof(Access)))
        {
            Access kind2 = AccessConverter.Parse(kind.ToString());
            Assert.AreEqual(kind, kind2);
            object kind3 = Enum.Parse(typeof(Access), kind.ToVCardString(), true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(Access.Public, AccessConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(Access.Public.ToVCardString(), ((Access)4711).ToVCardString());
    }
}
