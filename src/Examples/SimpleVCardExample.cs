﻿using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

public static class SimpleVCardExample
{
    public static void WritingAndReadingVCard(string filePath)
    {
        VCard vCard = VCardBuilder
                .Create()
                .NameViews.Add(NameBuilder
                    .Create()
                    .AddGivenName("Susi")
                    .AddFamilyName("Sonntag")
                              )
                .NameViews.ToDisplayNames(NameFormatter.Default)
                .GenderViews.Add(Sex.Female)
                .Phones.Add("+49-321-1234567",
                             parameters: p =>  p.PhoneType = Tel.Cell
                           )
                .EMails.Add("susi@contoso.com")
                .EMails.Add("susi@home.de")
                .EMails.SetPreferences()
                .BirthDayViews.Add(1984, 3, 28)
                .VCard;

        // Save vCard as vCard 3.0:
        // (You don't need to specify the version: Version 3.0 is the default.)
        Vcf.Save(vCard, filePath);

        // Load the VCF file. (The result is IList<VCard> because a VCF file may contain
        // many vCards.):
        IList<VCard> vCards = Vcf.Load(filePath);
        vCard = vCards[0];

        // Use Linq and/or extension methods to query the data:
        string? susisPrefMail = vCard.EMails.PrefOrNull()?.Value;

        Console.WriteLine("Susis preferred email address is {0}", susisPrefMail);

        Console.WriteLine("\nvCard:\n");
        Console.WriteLine(File.ReadAllText(filePath));
    }
}
/*
Console output:

Susis preferred email address is susi@contoso.com

vCard:

BEGIN:VCARD
VERSION:3.0
REV:2024-10-12T14:10:09Z
UID:27d7b637-b806-430a-a345-d6604406fec8
FN:Susi Sonntag
N:Sonntag;Susi;;;
X-GENDER:Female
BDAY;VALUE=DATE:1984-03-28
TEL;TYPE=CELL:+49-321-1234567
EMAIL;TYPE=INTERNET,PREF:susi@contoso.com
EMAIL;TYPE=INTERNET:susi@home.de
END:VCARD
*/