using FolkerKinzel.VCards;
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
                .NameViews.Add(NameBuilder
                    .Create()
                    .AddPrefix("Prof.")
                    .AddPrefix("Dr.")
                    .AddGiven("Käthe")
                    .AddGiven2("Alexandra")
                    .AddGiven2("Caroline")
                    .AddSurname("Müller-Risinowsky")
                    .AddGeneration("II.")
                    .Build(),
                     parameters: p => p.Language = "de-DE",
                     group: vc => vc.NewGroup())
                .NameViews.ToDisplayNames(NameFormatter.Default)
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
                // Applying a group name to the AddressProperty helps to automatically preserve its Label,
                // TimeZone and GeoCoordinate when writing a vCard 2.1 or vCard 3.0.
                .Addresses.Add(AddressBuilder
                    .Create()
                    .AddStreetName("Friedrichstraße")
                    .AddStreetNumber("22")
                    .AddLocality("Berlin")
                    .AddPostalCode("10117")
                    .AddCountry("Germany")
                    .Build(),
                     parameters: p =>
                     {
                         p.PropertyClass = PCl.Work;
                         p.AddressType = Adr.Dom | Adr.Intl | Adr.Postal | Adr.Parcel|Adr.Billing|Adr.Delivery;
                         p.TimeZone = TimeZoneID.Parse("Europe/Berlin");
                         p.GeoPosition = new GeoCoordinate(52.51182050685474, 13.389581454284256);
                     },
                     group: vc => vc.NewGroup()
                              )
                // Specifying the country or the ParameterSection.CountryCode property helps to format automatically
                // appended address labels correctly.
                .Addresses.AttachLabels(AddressFormatter.Default)
                .EMails.Add("kaethe_mueller@internet.com", parameters: p => p.PropertyClass = PCl.Work)
                .EMails.Add("mailto:kaethe_at_home@internet.com",
                             parameters: p =>
                             {
                                 p.DataType = Data.Uri;
                                 p.PropertyClass = PCl.Home;
                             }
                           )
                .EMails.SetPreferences()
                .Messengers.Add("https://wd.me/0123456789",
                                parameters: p => p.ServiceType = "WhatsDown")
                .SocialMediaProfiles.Add("https://y.com/Semaphore",
                                          parameters: p =>
                                          {
                                              p.UserName = "Semaphore";
                                              p.ServiceType = "Y";
                                          }
                                        )
                .BirthDayViews.Add(1984, 3, 28)
                .Relations.Add("Paul Müller-Risinowsky",
                               Rel.Spouse | Rel.CoResident | Rel.Colleague
                              )
                .AnniversaryViews.Add(2006, 7, 14)
                .Notes.Add("Very experienced in Blazor.",
                            parameters: p =>
                            {
                                p.Created = DateTimeOffset.Now;
                                p.Author = new Uri("https://www.microsoft.com/");
                                p.AuthorName = "Satya Nadella";
                            }
                          )
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
REV:2024-10-12T14:01:32Z
UID:3fbf66bd-ca03-4445-8916-e7ab837a4cee
0.FN;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8;LANGUAGE=de-DE:Prof. Dr. K=C3=
=A4the Alexandra Caroline M=C3=BCller-Risinowsky II.
0.N;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8;LANGUAGE=de-DE:M=C3=BCller-Ris=
inowsky;K=C3=A4the;Alexandra Caroline;Prof. Dr.;II.
X-GENDER:Female
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY:1984-03-28
X-ANNIVERSARY:2006-07-14
1.ADR;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:=
;;22 Friedrichstra=C3=9Fe;Berlin;;10117;Germany
1.LABEL;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:=
Friedrichstra=C3=9Fe 22=0D=0A10117 Berlin=0D=0AGERMANY
1.GEO:52.511821;13.389581
1.TZ:+01:00
TEL;WORK;VOICE;MSG;CELL;BBS:tel:+49-321-1234567
TEL;HOME;VOICE;BBS:tel:+49-123-9876543
EMAIL;PREF;INTERNET:kaethe_mueller@internet.com
EMAIL;INTERNET:mailto:kaethe_at_home@internet.com
X-SOCIALPROFILE;X-SERVICE-TYPE=Y:https://y.com/Semaphore
X-SPOUSE;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Paul M=C3=BCller-Risinows=
ky
NOTE:Very experienced in Blazor.
PHOTO;ENCODING=BASE64;TYPE=JPEG:
 knlPSqqVzaKYiddMZGoqJ7NUnjVklXwbPBoXLRgno32vyBW2dHU9sVpvnEiiAx7BDe+XpLhGE2
 p8n5bH

END:VCARD


VCard3.vcf:
----------
BEGIN:VCARD
VERSION:3.0
REV:2024-10-12T14:01:32Z
UID:3fbf66bd-ca03-4445-8916-e7ab837a4cee
0.FN;LANGUAGE=de-DE:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
 II.
0.N;LANGUAGE=de-DE:Müller-Risinowsky;Käthe;Alexandra Caroline;Prof. Dr.;I
 I.
X-GENDER:Female
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:1984-03-28
X-ANNIVERSARY:2006-07-14
1.ADR;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:;;22 Friedrichstraße;Berlin;;10117;
 Germany
1.LABEL;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:Friedrichstraße 22\n10117 Berlin\
 nGERMANY
1.GEO:52.511821;13.389581
1.TZ:+01:00
TEL;TYPE=WORK,VOICE,MSG,CELL,BBS:tel:+49-321-1234567
TEL;TYPE=HOME,VOICE,BBS:tel:+49-123-9876543
EMAIL;TYPE=INTERNET,PREF:kaethe_mueller@internet.com
EMAIL;TYPE=INTERNET:mailto:kaethe_at_home@internet.com
X-SOCIALPROFILE;X-SERVICE-TYPE=Y:https://y.com/Semaphore
IMPP;X-SERVICE-TYPE=WhatsDown:https://wd.me/0123456789
X-SPOUSE:Paul Müller-Risinowsky
NOTE:Very experienced in Blazor.
PHOTO;ENCODING=b;TYPE=JPEG:knlPSqqVzaKYiddMZGoqJ7NUnjVklXwbPBoXLRgno32vyBW2
 dHU9sVpvnEiiAx7BDe+XpLhGE2p8n5bH
END:VCARD


VCard4.vcf:
----------
BEGIN:VCARD
VERSION:4.0
CREATED;VALUE=TIMESTAMP:20241012T140132Z
REV:20241012T140132Z
UID:urn:uuid:3fbf66bd-ca03-4445-8916-e7ab837a4cee
0.FN;DERIVED=TRUE;LANGUAGE=de-DE;PID=1.1:Prof. Dr. Käthe Alexandra Carolin
 e Müller-Risinowsky II.
0.N;LANGUAGE=de-DE;PID=1.1:Müller-Risinowsky;Käthe;Alexandra,Caroline;Pro
 f.,Dr.;II.;;II.
GENDER;PID=1.1:F
TITLE;PID=1.1:CEO
ORG;PID=1.1:Millers Company;C#;Webdesign
BDAY;VALUE=DATE;PID=1.1:19840328
ANNIVERSARY;VALUE=DATE;PID=1.1:20060714
1.ADR;TYPE=WORK,billing,delivery;LABEL=Friedrichstraße 22\n10117 Berlin\nG
 ERMANY;GEO="geo:52.511821,13.389581";TZ=Europe/Berlin;PID=1.1:;;22,Friedri
 chstraße;Berlin;;10117;Germany;;;;22;Friedrichstraße;;;;;;
TEL;TYPE=WORK,VOICE,CELL,TEXT;VALUE=URI;PID=1.1;INDEX=1:tel:+49-321-1234567
TEL;TYPE=HOME,VOICE;VALUE=URI;PID=2.1;INDEX=2:tel:+49-123-9876543
EMAIL;TYPE=WORK;PREF=1;PID=1.1:kaethe_mueller@internet.com
EMAIL;TYPE=HOME;PREF=2;VALUE=URI;PID=2.1:mailto:kaethe_at_home@internet.com
SOCIALPROFILE;SERVICE-TYPE=Y;USERNAME=Semaphore;PID=1.1:https://y.com/Semap
 hore
IMPP;SERVICE-TYPE=WhatsDown;PID=1.1:https://wd.me/0123456789
RELATED;TYPE=colleague,co-resident,spouse;VALUE=TEXT;PID=1.1:Paul Müller-R
 isinowsky
NOTE;PID=1.1;AUTHOR-NAME=Satya Nadella;AUTHOR="https://www.microsoft.com/";
 CREATED=20241012T140132Z:Very experienced in Blazor.
PHOTO;PID=1.1:data:image/jpeg;base64,knlPSqqVzaKYiddMZGoqJ7NUnjVklXwbPBoXLR
 gno32vyBW2dHU9sVpvnEiiAx7BDe+XpLhGE2p8n5bH
CLIENTPIDMAP:1;urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556
END:VCARD


Read VCard:

Version: 3.0

[DataType: TimeStamp]
TimeStamp: 10/12/2024 14:01:32 +00:00

ID: 3fbf66bd-ca03-4445-8916-e7ab837a4cee

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

[ServiceType: Y]
SocialMediaProfiles: https://y.com/Semaphore

[ServiceType: WhatsDown]
Messengers: https://wd.me/0123456789

[RelationType: Spouse]
[DataType: Text]
Relations: System.String: Paul Müller-Risinowsky

Notes: Very experienced in Blazor.

[Encoding: Base64]
[MediaType: image/jpeg]
Photos: System.Byte[]: 60 Bytes

[Language: de-DE]
[Group: 0]
DisplayNames: Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky II.

[Language: de-DE]
[Group: 0]
NameViews:
    FamilyNames:     Müller-Risinowsky
    GivenNames:      Käthe
    AdditionalNames: Alexandra Caroline
    Prefixes:        Prof. Dr.
    Suffixes:        II.

[Group: 1]
TimeZones: +01:00

[Group: 1]
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
[Group: 1]
Addresses:
    Street:     22 Friedrichstraße
    Locality:   Berlin
    PostalCode: 10117
    Country:    Germany
 */