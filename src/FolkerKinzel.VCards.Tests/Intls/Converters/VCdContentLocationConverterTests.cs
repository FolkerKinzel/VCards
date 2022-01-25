using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class VCdContentLocationConverterTests
{
    [TestMethod()]
    public void Roundtrip1()
    {
        foreach (VCdContentLocation kind in (VCdContentLocation[])Enum.GetValues(typeof(VCdContentLocation)))
        {
            string cIdString = kind.ToVcfString();
            VCdContentLocation kind2 = VCdContentLocationConverter.Parse(cIdString);
            Assert.AreEqual(kind, kind2);

            cIdString = kind.ToVcfString();
            cIdString = cIdString == "CID" ? "ContentId" : cIdString;

            object kind3 = Enum.Parse(typeof(VCdContentLocation), cIdString, true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(VCdContentLocation.Inline, VCdContentLocationConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(VCdContentLocation.Inline.ToVcfString(), ((VCdContentLocation)4711).ToVcfString());
    }

    [TestMethod()]
    public void Roundtrip2()
    {
        string cIdString = "CONTENT-ID";
        VCdContentLocation kind = VCdContentLocationConverter.Parse(cIdString);
        Assert.AreEqual(VCdContentLocation.ContentID, kind);
    }
}
