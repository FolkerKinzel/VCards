using System;
using FolkerKinzel.VCards;

using VC = FolkerKinzel.VCards.Models;

namespace Examples
{
    internal static class WhatsAppDemo1
    {
        public static void IntegrateWhatsAppNumberUsingIMPP()
        {
            const string mobilePhoneNumber = "tel:+1-234-567-89";

            // The IMPP-Extension (Instant Messaging [IM] and Presence Protocol [PP] applications)
            // is available in vCard 3.0 through RFC 4770:
            var whatsAppImpp = new VC::TextProperty(mobilePhoneNumber);
            whatsAppImpp.Parameters.InstantMessengerType = VC::Enums.ImppTypes.Personal
                                                         | VC::Enums.ImppTypes.Business
                                                         | VC::Enums.ImppTypes.Mobile;


            // The vCard 4.0 standard RFC 6350 recommends to add an additional TEL entry
            // if the instant messenging device supports voice and/or video.
            // I think, that's a good practice also in vCard 3.0.
            var xiamoiMobilePhone = new VC::TextProperty(mobilePhoneNumber.Substring(4));
            xiamoiMobilePhone.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Home
                                                       | VC::Enums.PropertyClassTypes.Work;
            xiamoiMobilePhone.Parameters.TelephoneType = VC::Enums.TelTypes.Voice
                                                       | VC::Enums.TelTypes.BBS
                                                       | VC::Enums.TelTypes.Cell
                                                       | VC::Enums.TelTypes.Msg
                                                       | VC::Enums.TelTypes.Text
                                                       | VC::Enums.TelTypes.Video;

            // Initialize the VCard:
            var vcard = new VCard
            {
                NameViews = new VC::NameProperty[]
                {
                    new VC::NameProperty(lastName: null, firstName: "zzMad Perla 45")
                },

                DisplayNames = new VC::TextProperty[]
                {
                    new VC::TextProperty("zzMad Perla 45")
                },

                // Add the WhatsApp-Handle:
                InstantMessengerHandles = new VC::TextProperty[]
                {
                    whatsAppImpp
                },

                // Add the mobile phone too:
                PhoneNumbers = new VC::TextProperty[]
                {
                    xiamoiMobilePhone
                }
            };

            Console.WriteLine(vcard.ToVcfString());
        }
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

