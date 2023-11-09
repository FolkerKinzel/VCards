using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class DataConverterTests
{
    [TestMethod]
    public void Roundtrip()
    {
        foreach (Data kind in (Data[])Enum.GetValues(typeof(Data)))
        {
            string? s = DataConverter.ToVcfString(kind);
            Data? kind2 = DataConverter.Parse(s);
            Assert.AreEqual(kind, kind2);
        }

        // Test auf null
        Assert.AreEqual(null, DataConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(null, ((Data?)4711).ToVcfString());
    }
}
