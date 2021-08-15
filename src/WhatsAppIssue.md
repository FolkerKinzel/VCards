Hi Kiquenet,

unfortunately, during my research on the Internet, I could not find any RFC that defines 
a WhatsApp-TYPE in a vCard file. Therefore there wasn't an easy fix for your issue like 
a new enum value `TelTypes.WhatsApp`.

Although the WhatsApp-TYPE is a "Non-Standard-Thing", you have found a real issue, because the
library supports such "Non-Standard-Things",e.g., with the `ParameterSection.NonStandardParameters` 
property. Up to version 2.0.0 of the library, there was a bug, that Non-Standard-Values of
standard parameters (like TYPE) were not recognized by the library.

Thanks to your post, the issue could be fixed now. I have published a 
preview version (2.0.2-alpha) of the package on nuget, so you don't have to wait for the
next regular release to test it. (It's a preview, because the bug fix required some changes
at the heart of the library, which should be tested more thoroughly.)

Before showing you how the WhatsApp-TYPE can be handled with the preview version, I 
would like to demonstrate, how a WhatsApp telephone number is embedded in a vCard using 
the Standard-Way:

```csharp
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
```

The following example shows how to deal with the WhatsApp-TYPE. (Make shure to use at least Version 2.0.2-alpha 
of the FolkerKinzel.VCards nuget package.):

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using FolkerKinzel.VCards;

using VC = FolkerKinzel.VCards.Models;

namespace Examples
{
    internal static class WhatsAppDemo2
    {
        public static void UsingTheWhatsAppType()
        {
            var xiamoiMobilePhone = new VC::TextProperty("+1-234-567-89");
            xiamoiMobilePhone.Parameters.NonStandardParameters = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("TYPE", "WhatsApp")
            };

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

                PhoneNumbers = new VC::TextProperty[]
                {
                    xiamoiMobilePhone
                }
            };

            // Don't forget to set VcfOptions.WriteNonStandardParameters when serializing the
            // VCard: The default ignores NonStandardParameters (and NonStandardProperties):
            string vcfString = vcard.ToVcfString(options: VcfOptions.Default | VcfOptions.WriteNonStandardParameters);

            Console.WriteLine(vcfString);

            // Parse the VCF string:
            vcard = VCard.Parse(vcfString)[0];

            // Find the WhatsApp number:
            string? whatsAppNumber = vcard.PhoneNumbers?
                .FirstOrDefault(x => x?.Parameters.NonStandardParameters?.Any(x => x.Key == "TYPE" && x.Value == "WhatsApp") ?? false)?
                .Value;

            Console.Write("The WhatsApp number is: ");
            Console.WriteLine(whatsAppNumber ?? "Don't know.");
        }
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
```

If you agree, I would close the issue now.

Best regards

Folker