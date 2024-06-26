﻿using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

public static class VCardExample
{
    private const string v2FileName = "VCard2.vcf";
    private const string v3FileName = "VCard3.vcf";
    private const string v4FileName = "VCard4.vcf";
    private const string underLine = "----------";
    private const string photoFileName = "Example.jpg";

    public static void WritingAndReadingVCard(string directoryPath)
    {
        // Registering the executing application with the VCard class is a technical requirement
        // when using the data synchronization mechanism introduced with vCard 4.0 (PID and
        // CLIENTPIDMAP). To do this, call the static method VCard.RegisterApp with an absolute
        // Uri once when the program starts. (UUID URNs are ideal for this.)
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        string v2FilePath = Path.Combine(directoryPath, v2FileName);
        string v3FilePath = Path.Combine(directoryPath, v3FileName);
        string v4FilePath = Path.Combine(directoryPath, v4FileName);

        VCard vCard = InitializeTheVCardAndFillItWithData(directoryPath, photoFileName);

        // Implements ITimeZoneIDConverter to convert IANA time zone names to UTC-Offsets.
        // (See the implementation as separate example.)
        ITimeZoneIDConverter tzConverter = new TimeZoneIDConverter();

        // Save vCard as vCard 2.1:
        Vcf.Save(vCard, v2FilePath, VCdVersion.V2_1, tzConverter);

        // Save vCard as vCard 3.0:
        // You don't need to specify the version: Version 3.0 is the default.
        Vcf.Save(vCard, v3FilePath, tzConverter: tzConverter);

        // Save vCard as vCard 4.0. (A time zone converter is not needed):
        Vcf.Save(vCard,
                 v4FilePath,
                 VCdVersion.V4_0,
                 options: Opts.Default.Set(Opts.SetPropertyIDs));

        // Load vCard:
        vCard = Vcf.Load(v3FilePath)[0];

        WriteResultsToConsole(vCard);

        ///////////////////////////////////////////////////////////////

        static VCard InitializeTheVCardAndFillItWithData(string directoryPath, string photoFileName)
        {
            // Creates a small "Photo" file for demonstration purposes:
            string photoFilePath = Path.Combine(directoryPath, photoFileName);
            CreatePhoto(photoFilePath);

            return VCardBuilder
                .Create()
                .NameViews.Add(familyNames: ["Müller-Risinowsky"],
                               givenNames: ["Käthe"],
                               additionalNames: ["Alexandra", "Caroline"],
                               prefixes: ["Prof.", "Dr."],
                               displayName: (displayNames, name) => displayNames.Add(name.ToDisplayName())
                              )
                .GenderViews.Add(Sex.Female)
                .Organizations.Add("Millers Company", ["C#", "Webdesign"])
                .Titles.Add("CEO")
                .Photos.AddFile(photoFilePath)
                .Phones.Add("tel:+49-321-1234567",
                             parameters: p =>
                             {
                                 p.DataType = Data.Uri;
                                 p.PropertyClass = PCl.Work;
                                 p.PhoneType = Tel.Cell | Tel.Text | Tel.Msg | Tel.BBS | Tel.Voice;
                             }
                           )
                .Phones.Add("tel:+49-123-9876543",
                             parameters: p =>
                             {
                                 p.DataType = Data.Uri;
                                 p.PropertyClass = PCl.Home;
                                 p.PhoneType = Tel.Voice | Tel.BBS;
                             }
                           )
                .Phones.SetIndexes()
                // Unless specified, an address label is automatically applied to the AddressProperty object.
                // Specifying the country helps to format this label correctly.
                // Applying a group name to the AddressProperty helps to automatically preserve its Label,
                // TimeZone and GeoCoordinate when writing a vCard 2.1 or vCard 3.0.
                .Addresses.Add("Friedrichstraße 22", "Berlin", null, "10117", "Germany",
                                parameters: p =>
                                {
                                    p.PropertyClass = PCl.Work;
                                    p.AddressType = Adr.Dom | Adr.Intl | Adr.Postal | Adr.Parcel;
                                    p.TimeZone = TimeZoneID.Parse("Europe/Berlin");
                                    p.GeoPosition = new GeoCoordinate(52.51182050685474, 13.389581454284256);
                                },
                                group: vc => vc.NewGroup()
                              )
                .EMails.Add("kaethe_mueller@internet.com", parameters: p => p.PropertyClass = PCl.Work)
                .EMails.Add("mailto:kaethe_at_home@internet.com",
                             parameters: p =>
                             {
                                 p.DataType = Data.Uri;
                                 p.PropertyClass = PCl.Home;
                             }
                           )
                .EMails.SetPreferences()
                .BirthDayViews.Add(1984, 3, 28)
                .Relations.Add("Paul Müller-Risinowsky",
                               Rel.Spouse | Rel.CoResident | Rel.Colleague
                              )
                .AnniversaryViews.Add(2006, 7, 14)
                .VCard;
        }

        void WriteResultsToConsole(VCard vcard)
        {
            WriteVCardToConsole(v2FileName, v2FilePath);
            WriteVCardToConsole(v3FileName, v3FilePath);
            WriteVCardToConsole(v4FileName, v4FilePath);

            Console.WriteLine("Read VCard:");
            Console.WriteLine();
            Console.WriteLine(vcard);

            static void WriteVCardToConsole(string fileName, string filePath)
            {
                Console.WriteLine($"{fileName}:");
                Console.WriteLine(underLine);
                Console.WriteLine(File.ReadAllText(filePath));
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Creates a small file, that simulates a photo.
    /// </summary>
    /// <param name="photoFilePath">Path to the created file.</param>
    private static void CreatePhoto(string photoFilePath)
    {
        byte[] photo = new byte[60];
        var rand = new Random();
        rand.NextBytes(photo);
        File.WriteAllBytes(photoFilePath, photo);
    }
}

/*
Console Output:

VCard2.vcf:
----------
BEGIN:VCARD
VERSION:2.1
REV:2024-05-23T16:13:19Z
UID:9abde489-7440-48e8-8438-58c2f0dae753
FN;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Prof. Dr. K=C3=A4the Alexandra =
Caroline M=C3=BCller-Risinowsky
N;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:M=C3=BCller-Risinowsky;K=C3=A4th=
e;Alexandra Caroline;Prof. Dr.;
X-GENDER:Female
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY:1984-03-28
X-ANNIVERSARY:2006-07-14
0.ADR;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:=
;;Friedrichstra=C3=9Fe 22;Berlin;;10117;Germany
0.LABEL;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:=
Friedrichstra=C3=9Fe 22=0D=0A10117 Berlin=0D=0AGERMANY
0.GEO:52.511821;13.389581
0.TZ:+01:00
TEL;WORK;VOICE;MSG;CELL;BBS:tel:+49-321-1234567
TEL;HOME;VOICE;BBS:tel:+49-123-9876543
EMAIL;PREF;INTERNET:kaethe_mueller@internet.com
EMAIL;INTERNET:mailto:kaethe_at_home@internet.com
X-SPOUSE;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Paul M=C3=BCller-Risinows=
ky
PHOTO;ENCODING=BASE64;TYPE=JPEG:
 ER4xGDkvK9SnOWrpamw3JBSXCUpjnWWXTPtgVTNNBTDziBmGQvi/u9yRK1YdXqmIJ2H97VnBT2
 VtsNAn

END:VCARD


VCard3.vcf:
----------
BEGIN:VCARD
VERSION:3.0
REV:2024-05-23T16:13:20Z
UID:9abde489-7440-48e8-8438-58c2f0dae753
FN:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra Caroline;Prof. Dr.;
X-GENDER:Female
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:1984-03-28
X-ANNIVERSARY:2006-07-14
0.ADR;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:;;Friedrichstraße 22;Berlin;;10117;
 Germany
0.LABEL;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:Friedrichstraße 22\n10117 Berlin\
 nGERMANY
0.GEO:52.511821;13.389581
0.TZ:+01:00
TEL;TYPE=WORK,VOICE,MSG,CELL,BBS:tel:+49-321-1234567
TEL;TYPE=HOME,VOICE,BBS:tel:+49-123-9876543
EMAIL;TYPE=INTERNET,PREF:kaethe_mueller@internet.com
EMAIL;TYPE=INTERNET:mailto:kaethe_at_home@internet.com
X-SPOUSE:Paul Müller-Risinowsky
PHOTO;ENCODING=b;TYPE=JPEG:ER4xGDkvK9SnOWrpamw3JBSXCUpjnWWXTPtgVTNNBTDziBmG
 Qvi/u9yRK1YdXqmIJ2H97VnBT2VtsNAn
END:VCARD


VCard4.vcf:
----------
BEGIN:VCARD
VERSION:4.0
REV:20240523T161320Z
UID:urn:uuid:9abde489-7440-48e8-8438-58c2f0dae753
FN;PID=1.1:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra,Caroline;Prof.,Dr.;
GENDER:F
TITLE;PID=1.1:CEO
ORG;PID=1.1:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:19840328
ANNIVERSARY;VALUE=DATE:20060714
0.ADR;TYPE=WORK;LABEL=Friedrichstraße 22\n10117 Berlin\nGERMANY;GEO="geo:5
 2.511821,13.389581";TZ=Europe/Berlin;PID=1.1:;;Friedrichstraße 22;Berlin;
 ;10117;Germany
TEL;TYPE=WORK,VOICE,CELL,TEXT;VALUE=URI;PID=1.1;INDEX=1:tel:+49-321-1234567
TEL;TYPE=HOME,VOICE;VALUE=URI;PID=2.1;INDEX=2:tel:+49-123-9876543
EMAIL;TYPE=WORK;PREF=1;PID=1.1:kaethe_mueller@internet.com
EMAIL;TYPE=HOME;PREF=2;VALUE=URI;PID=2.1:mailto:kaethe_at_home@internet.com
RELATED;TYPE=COLLEAGUE,CO-RESIDENT,SPOUSE;VALUE=TEXT;PID=1.1:Paul Müller-R
 isinowsky
PHOTO;PID=1.1:data:image/jpeg;base64,ER4xGDkvK9SnOWrpamw3JBSXCUpjnWWXTPtgVT
 NNBTDziBmGQvi/u9yRK1YdXqmIJ2H97VnBT2VtsNAn
CLIENTPIDMAP:1;urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556
END:VCARD


Read VCard:

Version: 3.0

[DataType: TimeStamp]
TimeStamp: 05/23/2024 16:13:20 +00:00

ID: 9abde489-7440-48e8-8438-58c2f0dae753

DisplayNames: Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky

NameViews:
    FamilyNames:     Müller-Risinowsky
    GivenNames:      Käthe
    AdditionalNames: Alexandra Caroline
    Prefixes:        Prof. Dr.

GenderViews: Female

Titles: CEO

Organizations:
    OrganizationName:    Millers Company
    OrganizationalUnits: C#; Webdesign

[DataType: Date]
BirthDayViews: System.DateOnly: 03/28/1984

AnniversaryViews: System.DateOnly: 07/14/2006

[PropertyClass: Work]
[PhoneType: Voice, Msg, Cell, BBS]
Phones: tel:+49-321-1234567

[PropertyClass: Home]
[PhoneType: Voice, BBS]
Phones: tel:+49-123-9876543

[EMailType: INTERNET]
[Preference: 1]
EMails: kaethe_mueller@internet.com

[EMailType: INTERNET]
EMails: mailto:kaethe_at_home@internet.com

[RelationType: Spouse]
[DataType: Text]
Relations: System.String: Paul Müller-Risinowsky

[Encoding: Base64]
[MediaType: image/jpeg]
Photos: System.Byte[]: 60 Bytes

[Group: 0]
TimeZones: +01:00

[Group: 0]
GeoCoordinates:
    Latitude:    52.511821
    Longitude:   13.389581

[PropertyClass: Work]
[AddressType: Dom, Intl, Postal, Parcel]
[Label:
    Friedrichstraße 22
    10117 Berlin
    GERMANY]
[GeoPosition:
    Latitude:    52.511821
    Longitude:   13.389581]
[TimeZone: +01:00]
[Group: 0]
Addresses:
    Street:     Friedrichstraße 22
    Locality:   Berlin
    PostalCode: 10117
    Country:    Germany
 */