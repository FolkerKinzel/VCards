// Compile for .NET 7.0 or higher and FolkerKinzel.VCards 6.0.0-beta.2 or higher
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;

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
        // (See the implementation as separate example.)
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
            var vCard = new VCard
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

            vCard.Photos = VC::DataProperty.FromFile(photoFilePath);

            // This shows how to create a PropertyIDMapping, which helps to identify vCard-Properties
            // even if they are located in vCards that come from different sources (vCard 4.0 only):
            var pidMap = new VC::PropertyIDMapping(1, new Uri("http://folkerKinzel.de/file1.htm"));
            var pidMapProp = new VC::PropertyIDMappingProperty(pidMap);
            vCard.PropertyIDMappings = pidMapProp;

            var phoneHome = new VC::TextProperty("tel:+49-123-9876543");
            phoneHome.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            phoneHome.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Home;
            phoneHome.Parameters.PhoneType = VC::Enums.PhoneTypes.Voice | VC::Enums.PhoneTypes.BBS;
            phoneHome.Parameters.PropertyIDs = new VC::PropertyID(1, pidMap);

            // Phones is null here. The extension method ConcatWith would not be needed in this case:
            // phoneHome could be assigned directly. ConcatWith is only used here to show that it
            // encapsulates all the null checking and will not throw on null references.
            // (Don't forget to assign the result!)
            vCard.Phones = vCard.Phones.ConcatWith(phoneHome);

            var phoneWork = new VC::TextProperty("tel:+49-321-1234567");
            phoneWork.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            phoneWork.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            phoneWork.Parameters.PhoneType = VC::Enums.PhoneTypes.Cell
                                             | VC::Enums.PhoneTypes.Text
                                             | VC::Enums.PhoneTypes.Msg
                                             | VC::Enums.PhoneTypes.BBS
                                             | VC::Enums.PhoneTypes.Voice;
            phoneWork.Parameters.PropertyIDs = new VC::PropertyID(2, pidMap);

            vCard.Phones = vCard.Phones.ConcatWith(phoneWork);

            // Unless specified, an address label is automatically applied to the AddressProperty object.
            // Specifying the country helps to correctly format this label.
            // Applying a group name to the AddressProperty helps to automatically preserve its Label,
            // TimeZone and GeoCoordinate when writing a vCard 2.1 or vCard 3.0.
            var adrWorkProp = new VC::AddressProperty
                ("Friedrichstraße 22", "Berlin", null, "10117", "Germany", group: vCard.NewGroup());
            adrWorkProp.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            adrWorkProp.Parameters.AddressType = VC::Enums.AddressTypes.Dom | VC::Enums.AddressTypes.Intl | 
                                                 VC::Enums.AddressTypes.Postal | VC::Enums.AddressTypes.Parcel;
            adrWorkProp.Parameters.TimeZone = VC::TimeZoneID.Parse("Europe/Berlin");
            adrWorkProp.Parameters.GeoPosition = new VC::GeoCoordinate(52.51182050685474, 13.389581454284256);
            vCard.Addresses = adrWorkProp;

            var prefMail = new VC::TextProperty("kaethe_mueller@internet.com");
            prefMail.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            prefMail.Parameters.Preference = 1;

            vCard.EMails = prefMail;

            var otherMail = new VC::TextProperty("mailto:kaethe_at_home@internet.com");
            otherMail.Parameters.DataType = VC.Enums.VCdDataType.Uri;
            otherMail.Parameters.PropertyClass = VC.Enums.PropertyClassTypes.Home;

            vCard.EMails = vCard.EMails.Concat(otherMail);

            vCard.BirthDayViews = VC::DateAndOrTimeProperty.FromDate(1984, 3, 28);

            vCard.Relations = VC::RelationProperty.FromText
                (
                    "Paul Müller-Risinowsky",
                    VC::Enums.RelationTypes.Spouse
                    | VC::Enums.RelationTypes.CoResident
                    | VC::Enums.RelationTypes.Colleague
                );

            vCard.AnniversaryViews = VC::DateAndOrTimeProperty.FromDate(2006, 7, 14);
            return vCard;
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
REV:2023-11-05T17:08:00Z
FN;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Prof. Dr. K=C3=A4the Alexandra=
 Caroline M=C3=BCller-Risinowsky
N;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:M=C3=BCller-Risinowsky;K=C3=A4th=
e;Alexandra Caroline;Prof. Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY:1984-03-28
X-ANNIVERSARY:2006-07-14
0.ADR;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:;;F=
riedrichstra=C3=9Fe 22;Berlin;;10117;Germany
0.LABEL;DOM;INTL;POSTAL;PARCEL;WORK;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Fri=
edrichstra=C3=9Fe 22=0D=0A10117 Berlin=0D=0AGERMANY
0.GEO:52.511821;13.389581
0.TZ:+01:00
TEL;HOME;VOICE;BBS:tel:+49-123-9876543
TEL;WORK;VOICE;MSG;CELL;BBS:tel:+49-321-1234567
EMAIL;PREF;INTERNET:kaethe_mueller@internet.com
EMAIL;INTERNET:mailto:kaethe_at_home@internet.com
X-SPOUSE;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Paul M=C3=BCller-Risinows=
ky
PHOTO;ENCODING=BASE64;TYPE=JPEG:
 5ECH58vce32AyCU9zt4UrIR6UEWNDTPgSpBN0lYawaWmMG6/voEl3VOaWdNmMZ1
 EgurNky9KcHzkFZdB

END:VCARD


VCard3.vcf:
----------
BEGIN:VCARD
VERSION:3.0
REV:2023-11-05T17:08:00Z
FN:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra Caroline;Prof. Dr.;
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
TEL;TYPE=HOME,VOICE,BBS:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,MSG,CELL,BBS:tel:+49-321-1234567
EMAIL;TYPE=INTERNET,PREF:kaethe_mueller@internet.com
EMAIL;TYPE=INTERNET:mailto:kaethe_at_home@internet.com
X-SPOUSE:Paul Müller-Risinowsky
PHOTO;ENCODING=b;TYPE=JPEG:5ECH58vce32AyCU9zt4UrIR6UEWNDTPgSpBN0lYawaWmMG6/
 voEl3VOaWdNmMZ1EgurNky9KcHzkFZdB
END:VCARD


VCard4.vcf:
----------
BEGIN:VCARD
VERSION:4.0
REV:20231105T170800Z
FN:Prof. Dr. Käthe Alexandra Caroline Müller-Risinowsky
N:Müller-Risinowsky;Käthe;Alexandra,Caroline;Prof.,Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
BDAY;VALUE=DATE:19840328
ANNIVERSARY;VALUE=DATE:20060714
0.ADR;TYPE=WORK;LABEL=Friedrichstraße 22\n10117 Berlin\nGERMANY;GEO="geo:5
 2.511821,13.389581";TZ=Europe/Berlin:;;Friedrichstraße 22;Berlin;;10117;G
 ermany
TEL;TYPE=HOME,VOICE;VALUE=URI;PID=1.1:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,CELL,TEXT;VALUE=URI;PID=2.1:tel:+49-321-1234567
EMAIL;TYPE=WORK;PREF=1:kaethe_mueller@internet.com
EMAIL;TYPE=HOME;VALUE=URI:mailto:kaethe_at_home@internet.com
RELATED;TYPE=COLLEAGUE,CO-RESIDENT,SPOUSE;VALUE=TEXT:Paul Müller-Risinowsk
 y
PHOTO:data:image/jpeg;base64,5ECH58vce32AyCU9zt4UrIR6UEWNDTPgSpBN0lYawaWmMG
 6/voEl3VOaWdNmMZ1EgurNky9KcHzkFZdB
CLIENTPIDMAP:1;http://folkerkinzel.de/file1.htm
END:VCARD


Read VCard:

Version: 3.0

[EMailType: EMAIL]
[Preference: 1]
EMails: kaethe_mueller@internet.com

[DataType: TimeStamp]
TimeStamp: 11/05/2023 17:08:00 +00:00

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

[PropertyClass: Home]
[PhoneType: Voice, BBS]
Phones: tel:+49-123-9876543

[PropertyClass: Work]
[PhoneType: Voice, Msg, Cell, BBS]
Phones: tel:+49-321-1234567

[EMailType: EMAIL]
EMails: mailto:kaethe_at_home@internet.com

[Relation: Spouse]
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
