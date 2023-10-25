// Compile for .NET 7.0 or above and FolkerKinzel.VCards 6.0.0-beta.1 or above
using FolkerKinzel.VCards;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

public static class VCardExample
{
    private const string v2FileName = "VCard2.vcf";
    private const string v3FileName = "VCard3.vcf";
    private const string v4FileName = "VCard4.vcf";
    private const string underLine = "----------";
    private const string photoFileName = "Example.jpg";

    public static void ReadingAndWritingVCard(string directoryPath)
    {
        string v2FilePath = Path.Combine(directoryPath, v2FileName);
        string v3FilePath = Path.Combine(directoryPath, v3FileName);
        string v4FilePath = Path.Combine(directoryPath, v4FileName);

        VCard vcard = InitializeTheVCardAndFillItWithData(directoryPath, photoFileName);

        // Implements ITimeZoneIDConverter to convert IANA time zone names to UTC-Offsets.
        // (See the implementation in the example below.)
        VC::ITimeZoneIDConverter tzConverter = new TimeZoneIDConverter();

        // Save vcard as vCard 2.1:
        vcard.SaveVcf(v2FilePath, VCdVersion.V2_1, tzConverter);

        // Save vcard as vCard 3.0:
        // You don't need to specify the version: Version 3.0 is the default.
        vcard.SaveVcf(v3FilePath, tzConverter: tzConverter);

        // Save vcard as vCard 4.0. (A time zone converter is not needed):
        vcard.SaveVcf(v4FilePath, VCdVersion.V4_0);

        // Load vCard:
        vcard = VCard.LoadVcf(v3FilePath)[0];

        WriteResultsToConsole(vcard);

        ///////////////////////////////////////////////////////////////

        static VCard InitializeTheVCardAndFillItWithData(string directoryPath, string photoFileName)
        {
            var name = new VC::NameProperty
                        (
                            lastName: new string[] { "Müller-Risinowsky" },
                            firstName: new string[] { "Käthe" },
                            middleName: new string[] { "Alexandra", "Caroline" },
                            prefix: new string[] { "Prof.", "Dr." }
                        );
            var vcard = new VCard
            {
                // Although NameViews is of Type IEnumerable<NameProperty?>
                // you can assign a single NameProperty instance because NameProperty
                // (like almost all classes derived from VCardProperty) has an explicit
                // implementation of IEnumerable<T>
                NameViews = name,
                DisplayNames = new VC.TextProperty(name.ToDisplayName()),
                Organizations = new VC::OrganizationProperty
                                (
                                    "Millers Company",
                                    new string[] { "C#", "Webdesign" }
                                ),
                Titles = new VC::TextProperty("CEO"),
                TimeStamp = new VC::TimeStampProperty(DateTimeOffset.UtcNow)
            };

            // Creates a small "Photo" file for demonstration purposes:
            string photoFilePath = Path.Combine(directoryPath, photoFileName);
            CreatePhoto(photoFilePath);

            vcard.Photos = VC::DataProperty.FromFile(photoFilePath);

            // This shows how to create a PropertyIDMapping, which helps to identify vCard-Properties
            // even if they are located in vCards that come from different sources (vCard 4.0 only):
            var pidMap = new VC::PropertyIDMapping(1, new Uri("http://folkerKinzel.de/file1.htm"));
            var pidMapProp = new VC::PropertyIDMappingProperty(pidMap);
            vcard.PropertyIDMappings = pidMapProp;

            const string groupName = "gr1";
            var phoneHome = new VC::TextProperty("tel:+49-123-9876543");
            phoneHome.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            phoneHome.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Home;
            phoneHome.Parameters.PhoneType = VC::Enums.PhoneTypes.Voice | VC::Enums.PhoneTypes.BBS;
            phoneHome.Parameters.PropertyIDs = new VC::PropertyID(1, pidMap);

            var phoneWork = new VC::TextProperty("tel:+49-321-1234567");
            phoneWork.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            phoneWork.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            phoneWork.Parameters.PhoneType = VC::Enums.PhoneTypes.Cell
                                             | VC::Enums.PhoneTypes.Text
                                             | VC::Enums.PhoneTypes.Msg
                                             | VC::Enums.PhoneTypes.BBS
                                             | VC::Enums.PhoneTypes.Voice;
            phoneWork.Parameters.PropertyIDs = new VC::PropertyID(2, pidMap);


            var adrWorkProp = new VC::AddressProperty
                ("Friedrichstraße 22", "Berlin", null, "10117", "Germany", propertyGroup: groupName);
            adrWorkProp.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            adrWorkProp.Parameters.AddressType = VC::Enums.AddressTypes.Dom | VC::Enums.AddressTypes.Intl | VC::Enums.AddressTypes.Postal | VC::Enums.AddressTypes.Parcel;
            vcard.Addresses = adrWorkProp;

            var tz = VC::TimeZoneID.Parse("Europe/Berlin");
            var geo = new VC::GeoCoordinate(52.51182050685474, 13.389581454284256);

            // vCard 4.0 allows to specify the time zone and geographical
            // position in the Parameters of an AddressProperty.
            // These entries are ignored when writing older vCard versions:
            adrWorkProp.Parameters.TimeZone = tz;
            adrWorkProp.Parameters.GeoPosition = geo;

            // Make separate TZ and GEO Properties to preserve the time zone
            // and geographical Position in older vCard versions:
            vcard.TimeZones = new VC::TimeZoneProperty(tz, groupName);
            vcard.GeoCoordinates = new VC::GeoProperty(geo, groupName);

            vcard.Phones = new VC::TextProperty[]
            {
                    phoneHome, phoneWork
            };

            var prefMail = new VC::TextProperty("kaethe_mueller@internet.com");
            prefMail.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            prefMail.Parameters.Preference = 1;

            vcard.EMails = prefMail;

            vcard.BirthDayViews = VC::DateAndOrTimeProperty.FromDate(1984, 3, 28);

            vcard.Relations = VC::RelationProperty.FromText
                (
                    "Paul Müller-Risinowsky",
                    VC::Enums.RelationTypes.Spouse
                    | VC::Enums.RelationTypes.CoResident
                    | VC::Enums.RelationTypes.Colleague
                );

            vcard.AnniversaryViews = VC::DateAndOrTimeProperty.FromDate(2006, 7, 14);
            return vcard;
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
REV:2023-10-25T21:32:06Z
gr1.TZ:+01:00
gr1.GEO:52.511821;13.389581
FN;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Prof. Dr. K=C3=A4the Alexandra=
 Caroline M=C3=BCller-Risinowsky
N;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:M=C3=BCller-Risinowsky;K=C3=A4th=
e;Alexandra Caroline;Prof. Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY:1984-03-28
X-ANNIVERSARY:2006-07-14
gr1.ADR;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:;;F=
riedrichstra=C3=9Fe 22;Berlin;;10117;Germany
gr1.LABEL;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Fri=
edrichstra=C3=9Fe 22=0D=0A10117 Berlin=0D=0AGERMANY
TEL;HOME;VOICE;BBS:tel:+49-123-9876543
TEL;WORK;VOICE;MSG;CELL;BBS:tel:+49-321-1234567
EMAIL;PREF;INTERNET:kaethe_mueller@internet.com
X-SPOUSE;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Paul M=C3=BCller-Risinows=
ky
PHOTO;ENCODING=BASE64;TYPE=JPEG:
 6L49AWHlVPGmZOV4vGkrALHNIH4tMgdyR37+TTZcjSh5Xmrrw7TeFFhQ4ZcvrSj
 TGOUARW6ma4AW5H7Z

END:VCARD


VCard3.vcf:
----------
BEGIN:VCARD
VERSION:3.0
REV:2023-10-25T21:32:06Z
gr1.TZ:+01:00
gr1.GEO:52.511821;13.389581
FN:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra Caroline;Prof. Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:1984-03-28
X-ANNIVERSARY:2006-07-14
gr1.ADR;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:;;Friedrichstraße 22;Berlin;;1011
 7;Germany
gr1.LABEL;TYPE=WORK,DOM,INTL,POSTAL,PARCEL:Friedrichstraße 22\n10117 Berli
 n\nGERMANY
TEL;TYPE=HOME,VOICE,BBS:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,MSG,CELL,BBS:tel:+49-321-1234567
EMAIL;TYPE=INTERNET,PREF:kaethe_mueller@internet.com
X-SPOUSE:Paul Müller-Risinowsky
PHOTO;ENCODING=b;TYPE=JPEG:6L49AWHlVPGmZOV4vGkrALHNIH4tMgdyR37+TTZcjSh5Xmrr
 w7TeFFhQ4ZcvrSjTGOUARW6ma4AW5H7Z
END:VCARD


VCard4.vcf:
----------
BEGIN:VCARD
VERSION:4.0
REV:20231025T213206Z
gr1.TZ:Europe/Berlin
gr1.GEO:geo:52.511821,13.389581
FN:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra,Caroline;Prof.,Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:19840328
ANNIVERSARY;VALUE=DATE:20060714
gr1.ADR;TYPE=WORK;LABEL=Friedrichstraße 22\n10117 Berlin\nGERMANY;GEO="geo
 :52.511821,13.389581";TZ=Europe/Berlin:;;Friedrichstraße 22;Berlin;;10117
 ;Germany
TEL;TYPE=HOME,VOICE;VALUE=URI;PID=1.1:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,CELL,TEXT;VALUE=URI;PID=2.1:tel:+49-321-1234567
EMAIL;TYPE=WORK;PREF=1:kaethe_mueller@internet.com
RELATED;TYPE=COLLEAGUE,CO-RESIDENT,SPOUSE;VALUE=TEXT:Paul Müller-Risinowsk
 y
PHOTO:data:image/jpeg;base64,6L49AWHlVPGmZOV4vGkrALHNIH4tMgdyR37+TTZcjSh5Xm
 rrw7TeFFhQ4ZcvrSjTGOUARW6ma4AW5H7Z
CLIENTPIDMAP:1;http://folkerkinzel.de/file1.htm
END:VCARD


Read VCard:

Version: 3.0

[DataType: TimeStamp]
TimeStamp: 10/25/2023 21:32:06 +00:00

[Group: gr1]
TimeZones: +01:00

[Group: gr1]
GeoCoordinates:
    Latitude:    52.511821
    Longitude:   13.389581

DisplayNames: Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky

NameViews:
    LastName:   Müller-Risinowsky
    FirstName:  Käthe
    MiddleName: Alexandra Caroline
    Prefix:     Prof. Dr.

Titles: CEO

Organizations:
    OrganizationName:    Millers Company
    OrganizationalUnits: C#; Webdesign

[DataType: Date]
BirthDayViews: System.DateOnly: 03/28/1984

AnniversaryViews: System.DateOnly: 07/14/2006

[PropertyClass: Work]
[AddressType: Dom, Intl, Postal, Parcel]
[Label:
    Friedrichstraße 22
    10117 Berlin
    GERMANY]
[Group: gr1]
Addresses:
    Street:     Friedrichstraße 22
    Locality:   Berlin
    PostalCode: 10117
    Country:    Germany

[PropertyClass: Home]
[PhoneType: Voice, BBS]
Phones: tel:+49-123-9876543

[PropertyClass: Work]
[PhoneType: Voice, Msg, Cell, BBS]
Phones: tel:+49-321-1234567

[EMailType: EMAIL]
[Preference: 1]
EMails: kaethe_mueller@internet.com

[Relation: Spouse]
[DataType: Text]
Relations: System.String: Paul Müller-Risinowsky

[Encoding: Base64]
[MediaType: image/jpeg]
Photos: System.Byte[]: 60 Bytes
 */
