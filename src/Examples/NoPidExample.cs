using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

public static class NoPidExample
{
    public static void SaveWithoutPropertyIdentification()
    {
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        const string vcf = """
            BEGIN:VCARD
            VERSION:4.0
            REV:20231121T200704Z
            UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
            FN;PID=1.1:John Doe
            CLIENTPIDMAP:1;http://other.com/
            END:VCARD
            """;

        VCard vCard = Vcf.ParseVcf(vcf)[0];

        // Removes all existing PIDs and CLIENTPIDMAPs
        vCard.Sync.Reset();

        VcfOptions options = VcfOptions.Default.Unset(VcfOptions.SetPropertyIDs);

        Console.WriteLine(vCard.ToVcfString(VCdVersion.V4_0, options: options));
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