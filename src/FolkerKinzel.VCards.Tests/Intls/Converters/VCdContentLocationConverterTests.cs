using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass()]
public class VCdContentLocationConverterTests
{
    [TestMethod()]
    public void Roundtrip1()
    {
        foreach (ContentLocation kind in (ContentLocation[])Enum.GetValues(typeof(ContentLocation)))
        {
            string cIdString = kind.ToVcfString();
            ContentLocation kind2 = ContentLocationConverter.Parse(cIdString);
            Assert.AreEqual(kind, kind2);

            cIdString = kind.ToVcfString();
            cIdString = cIdString == "CID" ? "ContentId" : cIdString;

            object kind3 = Enum.Parse(typeof(ContentLocation), cIdString, true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(ContentLocation.Inline, ContentLocationConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(ContentLocation.Inline.ToVcfString(), ((ContentLocation)4711).ToVcfString());
    }

    [TestMethod()]
    public void Roundtrip2()
    {
        string cIdString = "CONTENT-ID";
        ContentLocation kind = ContentLocationConverter.Parse(cIdString);
        Assert.AreEqual(ContentLocation.ContentID, kind);
    }
}
