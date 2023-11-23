using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

internal static class WhatsAppDemo2
{
    public static void UsingTheWhatsAppType()
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. To do this, call the static method VCard.RegisterApp with an absolute
        // Uri once when the program starts. (UUID URNs are ideal for this.) This registration
        // is used for the data synchronization mechanism introduced with vCard 4.0 (PID and
        // CLIENTPIDMAP).
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        var xiamoiMobilePhone = new VC::TextProperty("+1-234-567-89");

        xiamoiMobilePhone.Parameters.NonStandard = [ new KeyValuePair<string, string>("TYPE", "WhatsApp") ];

        // Initialize the VCard:
        var vcard = new VCard
        {
            NameViews = new VC::NameProperty[]
            {
                    new(familyName: null, givenName: "zzMad Perla 45")
            },

            DisplayNames = new VC::TextProperty[]
            {
                    new("zzMad Perla 45")
            },

            Phones = new VC::TextProperty[]
            {
                    xiamoiMobilePhone
            }
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = vcard.ToVcfString(options: VcfOptions.Default | VcfOptions.WriteNonStandardParameters);

        Console.WriteLine(vcfString);

        // Parse the VCF string:
        vcard = VCard.ParseVcf(vcfString)[0];

        // Find the WhatsApp number:
        string? whatsAppNumber = vcard.Phones?
            .FirstOrDefault(x => x?.Parameters.NonStandard?.Any(x => x.Key == "TYPE" && x.Value == "WhatsApp") ?? false)?
            .Value;

        Console.Write("The WhatsApp number is: ");
        Console.WriteLine(whatsAppNumber ?? "Don't know.");
    }
}
/*
Console Output:

BEGIN:VCARD
VERSION:3.0
FN:zzMad Perla 45
N:;zzMad Perla 45;;;
TEL;TYPE=WhatsApp:+1-234-567-89
END:VCARD

The WhatsApp number is: +1-234-567-89
*/

