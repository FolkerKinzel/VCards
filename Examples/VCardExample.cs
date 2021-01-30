using System;
using System.IO;
using FolkerKinzel.VCards;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples
{
    public static class VCardExample
    {
        public static void ReadingAndWritingVCard(string directoryPath)
        {
            const string v2FileName = "VCard2.vcf";
            const string v3FileName = "VCard3.vcf";
            const string v4FileName = "VCard4.vcf";
            const string photoFileName = "Example.jpg";

            var vcard = new VCard
            {
                NameViews = new VC::NameProperty[]
                {
                    new VC::NameProperty
                    (
                        lastName: new string[] { "Müller-Risinowsky" },
                        firstName: new string[] { "Käthe" },
                        middleName: new string[] {"Alexandra", "Caroline"},
                        prefix: new string[] { "Prof.", "Dr." }
                    )
                },

                DisplayNames = new VC::TextProperty[]
                {
                    new VC.TextProperty("Käthe Müller-Risinowsky")
                },

                Organizations = new VC::OrganizationProperty[]
                {
                    new VC::OrganizationProperty
                    (
                        "Millers Company",
                        new string[] { "C#", "Webdesign" }
                    )
                },

                Titles = new VC::TextProperty[]
                {
                    new VC::TextProperty("CEO")
                },

                TimeStamp = new VC::TimeStampProperty(DateTimeOffset.UtcNow)
            };


            // Creates a small "Photo" file for demonstration purposes:
            string photoFilePath = Path.Combine(directoryPath, photoFileName);
            CreatePhoto(photoFilePath);

            vcard.Photos = new VC::DataProperty[]
            {
                new VC::DataProperty(VC::DataUrl.FromFile(photoFilePath))
            };

            var telHome = new VC::TextProperty("tel:+49-123-9876543");
            telHome.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            telHome.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Home;
            telHome.Parameters.TelephoneType = VC::Enums.TelTypes.Voice | VC::Enums.TelTypes.BBS;

            var telWork = new VC::TextProperty("tel:+49-321-1234567");
            telWork.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            telWork.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            telWork.Parameters.TelephoneType = VC::Enums.TelTypes.Cell
                                             | VC::Enums.TelTypes.Text
                                             | VC::Enums.TelTypes.Msg
                                             | VC::Enums.TelTypes.BBS
                                             | VC::Enums.TelTypes.Voice;

            vcard.PhoneNumbers = new VC::TextProperty[]
            {
                telHome, telWork
            };

            var prefMail = new VC::TextProperty("kaethe_mueller@internet.com");
            prefMail.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            prefMail.Parameters.Preference = 1;

            vcard.EmailAddresses = new VC::TextProperty[] { prefMail };

            vcard.BirthDayViews = new VC::DateTimeProperty[]
            {
                // System.DateTime has an implicit conversion to
                // System.DateTimeOffset
                new VC::DateTimeOffsetProperty(new DateTime(1984, 3, 28))
            };

            vcard.Relations = new VC::RelationProperty[]
            {
                new VC::RelationTextProperty("Paul Müller-Risinowsky",
                VC::Enums.RelationTypes.Spouse | VC::Enums.RelationTypes.CoResident | VC::Enums.RelationTypes.Colleague)
            };

            vcard.AnniversaryViews = new VC::DateTimeProperty[]
            {
                new VC::DateTimeOffsetProperty(new DateTime(2006, 7, 14))
            };

            // Save vcard as vCard 2.1:
            string v2FilePath = Path.Combine(directoryPath, v2FileName);
            vcard.Save(v2FilePath, VC::Enums.VCdVersion.V2_1);

            // Save vcard as vCard 3.0:
            // You don't need to specify the version: Version 3.0 is the default.
            string v3FilePath = Path.Combine(directoryPath, v3FileName);
            vcard.Save(v3FilePath);

            // Save vcard as vCard 4.0:
            string v4FilePath = Path.Combine(directoryPath, v4FileName);
            vcard.Save(v4FilePath, VC::Enums.VCdVersion.V4_0);

            // Load vCard:
            vcard = VCard.Load(v3FilePath)[0];

            WriteResultsToConsole(vcard);

            ///////////////////////////////////////////////////////////////

            void WriteResultsToConsole(VCard vcard)
            {
                Console.WriteLine($"{v2FileName}:");
                Console.WriteLine(File.ReadAllText(v2FilePath));
                Console.WriteLine();

                Console.WriteLine($"{v3FileName}:");
                Console.WriteLine(File.ReadAllText(v3FilePath));
                Console.WriteLine();

                Console.WriteLine($"{v4FileName}:");
                Console.WriteLine(File.ReadAllText(v4FilePath));
                Console.WriteLine();

                Console.WriteLine("Read VCard:");
                Console.WriteLine(vcard);
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
}

/*
Console Output:

VCard2.vcf:
BEGIN:VCARD
VERSION:2.1
REV:2020-05-04T22:09:25Z
FN;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:Dr. K=C3=A4the M=C3=BCller
N;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:M=C3=BCller;K=C3=A4the;;Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
TEL;HOME;VOICE;BBS:tel:+49-123-9876543
TEL;WORK;VOICE;MSG;CELL;BBS:tel:+49-321-1234567
EMAIL;TYPE=INTERNET:kaethe_mueller@internet.com
PHOTO;ENCODING=BASE64;TYPE=JPEG:L5sLGTBPAwIB

END:VCARD


VCard3.vcf:
BEGIN:VCARD
VERSION:3.0
REV:2020-05-04T22:09:25Z
FN:Dr. Käthe Müller
N:Müller;Käthe;;Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
TEL;TYPE=HOME,VOICE,BBS:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,MSG,CELL,BBS:tel:+49-321-1234567
EMAIL;TYPE=INTERNET,PREF:kaethe_mueller@internet.com
PHOTO;ENCODING=b;TYPE=JPEG:L5sLGTBPAwIB
END:VCARD


VCard4.vcf:
BEGIN:VCARD
VERSION:4.0
REV:20200504T220925Z
FN:Dr. Käthe Müller
N:Müller;Käthe;;Dr.;
TITLE:CEO
ORG:Millers Company;C#;Webdesign
TEL;TYPE=HOME,VOICE;VALUE=URI:tel:+49-123-9876543
TEL;TYPE=WORK,VOICE,CELL,TEXT;VALUE=URI:tel:+49-321-1234567
EMAIL;TYPE=WORK;PREF=1:kaethe_mueller@internet.com
PHOTO:data:image/jpeg\;base64\,L5sLGTBPAwIB
END:VCARD


Read VCard:
Version: 3.0

[DataType: Timestamp]
LastRevision: 04.05.2020 22:09:25 +00:00

DisplayNames: Dr. Käthe Müller

NameViews:
    LastName:  Müller
    FirstName: Käthe
    Prefix:    Dr.

Titles: CEO

Organizations:
    OrganizationName:    Millers Company
    OrganizationalUnits: C#; Webdesign

[PropertyClass: Home]
[TelephoneType: Voice, BBS]
PhoneNumbers: tel:+49-123-9876543

[PropertyClass: Work]
[TelephoneType: Voice, Msg, Cell, BBS]
PhoneNumbers: tel:+49-321-1234567

[EmailType: EMAIL]
[Preference: 1]
EmailAddresses: kaethe_mueller@internet.com

[Encoding: Base64]
[MediaType: image/jpeg]
Photos: data:image/jpeg;base64,L5sLGTBPAwIB
.
*/
