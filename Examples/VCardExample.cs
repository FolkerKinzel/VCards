using FolkerKinzel.VCards;
using VC = FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Examples
{
    static class VCardExample
    {
        public static void ReadingAndWritingVCard()
        {
            VCard vcard = new VCard
            {
                NameViews = new VC::NameProperty[]
                {
                    new VC::NameProperty
                    (
                        lastName: new string[] { "Müller" },
                        firstName: new string[] { "Käthe" },
                        prefix: new string[] { "Dr." }
                    )
                },

                DisplayNames = new VC::TextProperty[]
                {
                    new VC.TextProperty("Dr. Käthe Müller")
                },

                Organizations = new VC::OrganizationProperty[]
                {
                    new VC::OrganizationProperty
                    (
                        "Contoso",
                        new string[] { "Webdesign" }
                    )
                },


                Titles = new VC::TextProperty[]
                {
                    new VC::TextProperty("Deputy Head of Department")
                },

                LastRevision = new VC::TimestampProperty(DateTimeOffset.UtcNow)
            };


            const string photoFileName = @"..\..\KätheMüller.jpg";

            // Create little "Photo" for demonstration purposes:
            File.WriteAllBytes(photoFileName, new byte[] { 47, 155, 11, 25, 48, 79, 3, 2, 1 });

            var photoData = VC::DataUrl.FromFile(photoFileName);
            var photo = new VC::DataProperty(photoData);
            //photo.Parameters.DataType = VC::Enums.VCdDataType.Uri;

            vcard.Photos = new VC::DataProperty[] { photo };

            var telHome = new VC::TextProperty("tel:+49-123-9876543");
            telHome.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            telHome.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Home;
            telHome.Parameters.TelephoneType = VC.Enums.TelTypes.Voice | VC.Enums.TelTypes.BBS;

            var telWork = new VC::TextProperty("tel:+49-321-1234567");
            telWork.Parameters.DataType = VC::Enums.VCdDataType.Uri;
            telWork.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            telWork.Parameters.TelephoneType = VC.Enums.TelTypes.Cell | VC.Enums.TelTypes.Text | VC.Enums.TelTypes.Msg | VC.Enums.TelTypes.BBS | VC.Enums.TelTypes.Voice;

            vcard.PhoneNumbers = new VC::TextProperty[]
            {
                telHome, telWork
            };


            var prefMail = new VC::TextProperty("kaethe_mueller@contoso.com");
            prefMail.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            prefMail.Parameters.Preference = 1;

            vcard.EmailAddresses = new VC::TextProperty[] { prefMail };


            // Save vcard as vCard 2.1:
            const string v2FileName = "VCard2.vcf";
            vcard.Save(v2FileName, VC::Enums.VCdVersion.V2_1);

            Console.WriteLine($"{v2FileName}:");
            Console.WriteLine(File.ReadAllText(v2FileName));
            Console.WriteLine();

            // Save vcard as vCard 3.0:
            const string v3FileName = "VCard3.vcf";
            // You don't need to specify the version: Version 3.0 is the default.
            vcard.Save(v3FileName);

            Console.WriteLine($"{v3FileName}:");
            Console.WriteLine(File.ReadAllText(v3FileName));
            Console.WriteLine();


            // Save vcard as vCard 4.0:
            const string v4FileName = "VCard4.vcf";
            vcard.Save(v4FileName, VC::Enums.VCdVersion.V4_0);

            Console.WriteLine($"{v4FileName}:");
            Console.WriteLine(File.ReadAllText(v4FileName));
            Console.WriteLine();


            // Load vCard:
            vcard = VCard.Load(v3FileName)[0];


            Console.WriteLine("Read VCard:");
            Console.WriteLine(vcard);

        }
    }
}
