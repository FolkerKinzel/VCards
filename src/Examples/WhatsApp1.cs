using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using Mod = FolkerKinzel.VCards.Models;

namespace Examples;

internal static class WhatsAppDemo1
{
    public static void IntegrateWhatsAppNumberUsingIMPP()
    {
        const string mobilePhoneNumber = "tel:+1-234-567-89";

        // The IMPP-Extension (Instant Messaging [IM] and Presence Protocol [PP] applications)
        // is available in vCard 3.0 through RFC 4770:
        var whatsAppImpp = new Mod::TextProperty(mobilePhoneNumber);
        whatsAppImpp.Parameters.InstantMessengerType =
            Impp.Personal | Impp.Business | Impp.Mobile;

        // The vCard 4.0 standard RFC 6350 recommends to add an additional TEL entry
        // if the instant messenging device supports voice and/or video.
        // I think that's a good practice also in vCard 3.0.
        var xiamoiMobilePhone = new Mod::TextProperty(mobilePhoneNumber.Substring(4));
        xiamoiMobilePhone.Parameters.PropertyClass = PCl.Home | PCl.Work;
        xiamoiMobilePhone.Parameters.PhoneType =
            Tel.Voice | Tel.BBS | Tel.Cell | Tel.Msg | Tel.Text | Tel.Video;

        // Initialize the VCard:
        var vcard = new VCard
        {
            NameViews = new Mod::NameProperty(NameBuilder.Create()
                                                          .AddSurname("")
                                                          .AddGiven("zzMad Perla 45")
                                                          .Build()),

            DisplayNames = new Mod::TextProperty("zzMad Perla 45"),

            // Add the WhatsApp-Handle:
            Messengers = whatsAppImpp,

            // Add the mobile phone too:
            Phones = xiamoiMobilePhone
        };

        Console.WriteLine(Vcf.ToString(vcard));
    }
}
/*
Console Output:

BEGIN:VCARD
VERSION:3.0
FN:zzMad Perla 45
N:;zzMad Perla 45;;;
TEL;TYPE=HOME,WORK,VOICE,MSG,CELL,BBS,VIDEO:+1-234-567-89
IMPP;TYPE=BUSINESS,MOBILE,PERSONAL:tel:+1-234-567-89
END:VCARD
*/