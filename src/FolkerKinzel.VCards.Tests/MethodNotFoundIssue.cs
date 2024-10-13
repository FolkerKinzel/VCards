namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class MethodNotFoundIssue
{
    [TestMethod]
    public void MethodNotFound()
    {
        string vCard = """
            BEGIN:VCARD
            VERSION:3.0
            FN:Folker
            END:VCARD
            """;

        using TextReader stringReader = new StringReader(vCard);
        using var vcfReader = new VcfReader(stringReader);

        IEnumerable<FolkerKinzel.VCards.VCard> result = vcfReader.ReadToEnd();

        var parsedVCards = result.ToList();

    }
}
