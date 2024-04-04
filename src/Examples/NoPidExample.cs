using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;

namespace Examples;

public static class NoPidExample
{
    public static void RemovePropertyIdentification()
    {
        const string vcf = """
            BEGIN:VCARD
            VERSION:4.0
            REV:20231121T200704Z
            UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
            FN;PID=1.1:John Doe
            CLIENTPIDMAP:1;http://other.com/
            END:VCARD
            """;

        VCard vCard = Vcf.Parse(vcf)[0];

        // Removes all existing PIDs and CLIENTPIDMAPs
        vCard.Sync.Reset();

        Console.WriteLine(Vcf.ToString(vCard, VCdVersion.V4_0));
    }
}

/*
Console Output:

BEGIN:VCARD
VERSION:4.0
REV:20231121T201552Z
UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
FN:John Doe
END:VCARD
*/