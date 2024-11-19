using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models.Properties;
using Mod = FolkerKinzel.VCards.Models;

namespace Examples;

internal static class WhatsAppDemo2
{
    public static void UsingTheWhatsAppType()
    {
        var xiamoiMobilePhone = new TextProperty("+1-234-567-89");

        xiamoiMobilePhone.Parameters.NonStandard = [new KeyValuePair<string, string>("TYPE", "WhatsApp")];

        // Initialize the VCard:
        var vcard = new VCard
        {
            NameViews = new NameProperty(NameBuilder.Create()
                                                         .AddSurname("")
                                                         .AddGiven("zzMad Perla 45")
                                                         .Build()),
            DisplayNames = [new("zzMad Perla 45")],
            Phones = xiamoiMobilePhone
        };

        // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
        // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
        string vcfString = Vcf.ToString(vcard,
                                        options: VcfOpts.Default | VcfOpts.WriteNonStandardParameters);

        Console.WriteLine(vcfString);

        // Parse the VCF string:
        vcard = Vcf.Parse(vcfString)[0];

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

