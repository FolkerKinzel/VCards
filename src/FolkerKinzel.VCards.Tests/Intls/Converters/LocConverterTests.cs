using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class LocConverterTests
{
    [TestMethod]
    public void Roundtrip1()
    {
        foreach (Loc kind in (Loc[])Enum.GetValues(typeof(Loc)))
        {
            string cIdString = kind.ToVcfString();
            Loc kind2 = LocConverter.Parse(cIdString.AsSpan());
            Assert.AreEqual(kind, kind2);

            cIdString = kind.ToVcfString();
            cIdString = cIdString == "CID" ? "Cid" : cIdString;

            object kind3 = Enum.Parse(typeof(Loc), cIdString, true);
            Assert.AreEqual(kind, kind3);
        }

        // Test auf null
        Assert.AreEqual(Loc.Inline, LocConverter.Parse(null));

        // Test auf nicht definiert
        Assert.AreEqual(Loc.Inline.ToVcfString(), ((Loc)4711).ToVcfString());
    }

    [TestMethod]
    public void Roundtrip2()
    {
        string cIdString = "CONTENT-ID";
        Loc kind = LocConverter.Parse(cIdString.AsSpan());
        Assert.AreEqual(Loc.Cid, kind);
    }
}
