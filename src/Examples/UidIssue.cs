using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;

namespace Examples;
internal static class UidIssue
{
    internal static void Test()
    {
        string vcard3 = """
            BEGIN:VCARD
            VERSION:3.0
            REV:2010-03-29T09:23:34Z
            UID:dd72824a-cfda-457f-ae74-d70c2711e532@example.org
            N:Dude;Some;Fred;;
            FN:Some dude
            NOTE:Simplified card for testing (Sogo Connector)
            NICKNAME:fred
            ROLE:Geek
            END:VCARD
            """;

        IReadOnlyList<VCard> vCards = Vcf.Parse(vcard3);

        if (vCards.Count >= 1)
        {
            VCard vCard = vCards[0];
            Console.WriteLine("Version: {0}", vCard.Version);
            Console.WriteLine("UID:     {0}", vCard.ContactID?.Value);
            Console.WriteLine("String:  {0}", vCard.ContactID?.Value.String);
            Console.WriteLine("Note:    {0}", vCard.Notes);
            Console.WriteLine();
            Console.WriteLine(vCard.ToVcfString());
        }
    }
}

/*
Console output:

Version: V3_0
UID:     String: dd72824a-cfda-457f-ae74-d70c2711e532@example.org
String:  dd72824a-cfda-457f-ae74-d70c2711e532@example.org
Note:    Simplified card for testing (Sogo Connector)

BEGIN:VCARD
VERSION:3.0
REV:2025-01-08T23:06:07Z
UID:dd72824a-cfda-457f-ae74-d70c2711e532@example.org
FN:Some dude
N:Dude;Some;Fred;;
NICKNAME:fred
ROLE:Geek
NOTE:Simplified card for testing (Sogo Connector)
END:VCARD
*/
