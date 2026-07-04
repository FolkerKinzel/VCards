using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class UidIssueTests
{

    [TestMethod]
    public void UidIssueTest1()
    {
        const string uidStr = "d290f1ee-6c54-4b01-90e6-d701748f0851";

        var uid = Guid.Parse(uidStr);

        string uidStr1 = uid.ToString("N");
        string uidStr2 = uid.ToString("D");
        string uidStr3 = uid.ToString("B");
        string uidStr4 = uid.ToString("P");
        string uidStr5 = uid.ToString("X");
        string uidStr6 = uidStr1.ToUpperInvariant();
        string uidStr7 = uidStr2.ToUpperInvariant();
        string uidStr8 = uidStr3.ToUpperInvariant();
        string uidStr9 = uidStr4.ToUpperInvariant();
        string uidStr10 = uidStr5.ToUpperInvariant();

        var uid2 = Guid.Parse(uidStr10);


    }


    //private static string NormalizeToString(ContactID id)
    //{
    //    return id.Convert(
    //        guid => guid.ToString(),
    //        uri => uri.AbsoluteUri.ToLowerInvariant().TrimEnd('/'),
    //        str => str.Trim()
    //    );
    //}

    [TestMethod]
    public void Uid_IsChanged_On_RoundTrip()
    {
        const string input =
            "BEGIN:VCARD\r\n" +
            "VERSION:4.0\r\n" +
            "FN:Test\r\n" +
            "UID;VALUE=text:d290f1ee-6c54-4b01-90e6-d701748f0851\r\n" +
            "END:VCARD\r\n";

        VCard card = Vcf.Parse(input)[0];
        string output = card.ToVcfString(VCdVersion.V4_0);

        // I expect the same UID I read, but this fails:
        StringAssert.Contains(output,
            "UID;VALUE=text:d290f1ee-6c54-4b01-90e6-d701748f0851");
        // actual: UID:urn:uuid:d290f1ee-6c54-4b01-90e6-d701748f0851
    }
}

