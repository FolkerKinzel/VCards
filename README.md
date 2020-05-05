# FolkerKinzel.VCards
Full featured .NET-API to work with vCard files (*.vcf).

It enables you
* to load vcf files from the file system and to save them there,
* to serialize vcf files from and to Streams and
* to convert vCard files, that match the vCard-versions 2.1, 3.0 and 4.0, to each other.

```
nuget Package Manager:
PM> Install-Package FolkerKinzel.VCards -Version 1.4.2

.NET CLI:
> dotnet add package FolkerKinzel.VCards --version 1.4.2

Package Reference (Visual Studio Project File):
<PackageReference Include="FolkerKinzel.VCards" Version="1.4.2" />

Paket CLI:
paket add FolkerKinzel.VCards --version 1.4.2
```


* [Download Project Reference English](https://github.com/FolkerKinzel/VCards/blob/master/FolkerKinzel.VCards.Reference.en/Help/FolkerKinzel.VCards.en.chm)

* [Projektdokumentation (Deutsch) herunterladen](https://github.com/FolkerKinzel/VCards/blob/master/FolkerKinzel.VCards.Doku.de/Help/FolkerKinzel.VCards.de.chm)

> IMPORTANT: On some systems, the content of the CHM file is blocked. Before extracting it,
>  right click on the file, select Properties, and check the "Allow" checkbox - if it 
> is present - in the lower right corner of the General tab in the Properties dialog.


## Overview
### How errors are handled

Parse errors are silently ignored.

The same is for errors, that occur serializing the vCard: Because of the different vCard-standards 
are not completely compliant, incompliant data is silently ignored when converting from one 
vCard-Standard to another. To minimize the data loss, the API tries to preserve incompliant data 
using well-known Non-Standard-Properties. The usage of such Non-Standard-Properties can be 
controlled via options (VcfOptions).

### The data model explained

The data model used by this API is aligned to the vCard 4.0 standard (RFC6350). This means, every read
vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized, they are 
converted back.

#### Class `VCardProperty<T>`

The data model of the class `VCard` based on classes, that are derived from `VCardProperty<T>`.

`VCardProperty<T>` exposes the following data:

```csharp
public abstract class VCardProperty<T>
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual T Value { get; protected set; }
}
````

This reflects the structure of a data row in a *.vcf-file:
> group1.TEL;TYPE=home,voice;VALUE=uri:tel:+49-123-4567

In this example corresponds
* `group1` to `VCardProperty<T>.Group`,
* `TEL;TYPE=home,voice;VALUE=uri` to `VCardProperty<T>.Parameters` and
* `tel:+49-123-4567` to `VCardProperty<T>.Value`.

#### Naming Conventions

Most properties of class `VCard` are collections. It has to do with, that many properties are allowed to have more than one
instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.</para>
              
A special feature are properties whose name ends with "Views": These are properties, which actually is only one instance per vCard allowed, but
vCard 4.0 enables you to have different versions of that single instance (e.g. in different languages). You must set the same `AltID` parameter
on each of these versions.

### Example Code

**_(For better readability exception handling is ommitted in the following example.)_**

```csharp
using FolkerKinzel.VCards;
using System;
using System.IO;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples
{
    static class VCardExample
    {
        private const string v2FileName = "VCard2.vcf";
        private const string v3FileName = "VCard3.vcf";
        private const string v4FileName = "VCard4.vcf";

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
                        "Millers Company",
                        new string[] { "C#", "Webdesign" }
                    )
                },

                Titles = new VC::TextProperty[]
                {
                    new VC::TextProperty("CEO")
                },

                LastRevision = new VC::TimestampProperty(DateTimeOffset.UtcNow)
            };

            const string photoFileName = @"..\..\KätheMüller.jpg";

            // Create a little "Photo" for demonstration purposes:
            File.WriteAllBytes(photoFileName, new byte[] { 47, 155, 11, 25, 48, 79, 3, 2, 1 });

            vcard.Photos = new VC::DataProperty[] 
            {
                new VC::DataProperty(VC::DataUrl.FromFile(photoFileName))
            };

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

            var prefMail = new VC::TextProperty("kaethe_mueller@internet.com");
            prefMail.Parameters.PropertyClass = VC::Enums.PropertyClassTypes.Work;
            prefMail.Parameters.Preference = 1;

            vcard.EmailAddresses = new VC::TextProperty[] { prefMail };

            // Save vcard as vCard 2.1:
            vcard.Save(v2FileName, VC::Enums.VCdVersion.V2_1);

            // Save vcard as vCard 3.0:
            // You don't need to specify the version: Version 3.0 is the default.
            vcard.Save(v3FileName);

            // Save vcard as vCard 4.0:
            vcard.Save(v4FileName, VC::Enums.VCdVersion.V4_0);

            // Load vCard:
            vcard = VCard.Load(v3FileName)[0];

            WriteResultsToConsole(vcard);

            ///////////////////////////////////////////////////////////////

            static void WriteResultsToConsole(VCard vcard)
            {
                Console.WriteLine($"{v2FileName}:");
                Console.WriteLine(File.ReadAllText(v2FileName));
                Console.WriteLine();

                Console.WriteLine($"{v3FileName}:");
                Console.WriteLine(File.ReadAllText(v3FileName));
                Console.WriteLine();

                Console.WriteLine($"{v4FileName}:");
                Console.WriteLine(File.ReadAllText(v4FileName));
                Console.WriteLine();

                Console.WriteLine("Read VCard:");
                Console.WriteLine(vcard);
            }
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

*/

```