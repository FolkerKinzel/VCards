# Getting Started
Read here:
- [The usage of the namespaces](#the-usage-of-the-namespaces)
- [The data model](#the-data-model)
  - [The VCard class](#the-vcard-class)
  - [The VCardProperty class](#the-vcardproperty-class)
  - [Naming conventions](#naming-conventions)
- [Efficient building and editing of VCard instances using Fluent APIs](#efficient-building-and-editing-of-vcard-instances-using-fluent-apis)
- [Parsing and serializing VCF files using the Vcf class](#parsing-and-serializing-vcf-files-using-the-vcf-class)
- [Extension methods](#extension-methods)
- [The vCard 4.0 data synchronization mechanism](#the-vcard-40-data-synchronization)
- [Handling of incompliant data](#handling-of-incompliant-data)
- [Reading the project reference](#reading-the-project-reference)
- [Documents of the vCard standard](#documents-of-the-vcard-standard)

## Simple example
```csharp
public static void WritingAndReadingVCard(string filePath)
{
    VCard vCard = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddGiven("Susi")
                .AddSurname("Sonntag")
                .Build()
                          )
            .NameViews.ToDisplayNames(NameFormatter.Default)
            .GenderViews.Add(Sex.Female)
            .Phones.Add("+49-321-1234567",
                         parameters: p => p.PhoneType = Tel.Cell
                       )
            .EMails.Add("susi@contoso.com")
            .EMails.Add("susi@home.de")
            .EMails.SetPreferences()
            .BirthDayViews.Add(1984, 3, 28)
            .VCard;

    // Save vCard as vCard 3.0:
    // (You don't need to specify the version: Version 3.0 is the default.)
    Vcf.Save(vCard, filePath);

    // Load the VCF file. (The result is IReadOnlyList<VCard> because a VCF file may contain
    // many vCards.):
    IReadOnlyList<VCard> vCards = Vcf.Load(filePath);
    vCard = vCards[0];

    // Use Linq and/or extension methods to query the data:
    string? susisPrefMail = vCard.EMails.PrefOrNull()?.Value;

    Console.WriteLine("Susis preferred email address is {0}", susisPrefMail);

    Console.WriteLine("\nvCard:\n");
    Console.WriteLine(File.ReadAllText(filePath));
}
```
The VCF file the method creates is:
```
BEGIN:VCARD
VERSION:3.0
REV:2024-12-01T14:37:03Z
UID:019382a7-515c-7486-a956-107bc79c1525
FN:Susi Sonntag
N:Sonntag;Susi;;;
X-GENDER:Female
BDAY;VALUE=DATE:1984-03-28
TEL;TYPE=CELL:+49-321-1234567
EMAIL;TYPE=INTERNET,PREF:susi@contoso.com
EMAIL;TYPE=INTERNET:susi@home.de
END:VCARD
```

## The usage of the namespaces
```csharp
// Publish this namespace - it contains commonly used classes such as
// the VCard class, the VCardBuilder class, the NameBuilder class, and the
// AddressBuilder class:
using FolkerKinzel.VCards;

// It's recommended to publish this namespace too -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// This namespace contains often used enums. Decide
// yourself whether to publish this namespace or to use
// a namespace alias:
using FolkerKinzel.VCards.Enums;

// This namespace contains the model classes such as GeoCoordinate or
// TimeZoneID:
using FolkerKinzel.VCards.Models;

// Contains the implementations of VCardProperty. If you use VCardBuilder to
// create and manipulate VCard objects, you usually do not need to publish this
// namespace:
//using FolkerKinzel.VCards.Models.Properties;
```

## The data model
The data model used by this library is aligned to the vCard 4.0 standard (RFC 6350). This means that every vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized they are converted back. 

### The VCard class
A VCF file consists of one or more vCards. The content of a vCard is represented by the `VCard` class.

### The VCardProperty class
A vCard consists of several "properties". Accordingly the data model of the `VCard` class is built on 
classes that are derived from the abstract `VCardProperty` class.

`VCardProperty` exposes the following members:
```csharp
public abstract class VCardProperty
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual object Value { get; protected set; }
}
```
This reflects the structure of a data row in a VCF file:
```
    group1.TEL;TYPE=home,voice;VALUE=uri:tel:+49-123-4567
```        
In this example corresponds
- `group1` to VCardProperty.Group,
- `TEL;TYPE=home,voice;VALUE=uri` to VCardProperty.Parameters and
- `tel:+49-123-4567` to VCardProperty.Value.
            
(Classes derived from `VCardProperty` hide the generic implementation of `VCardProperty.Value` in order 
to return derived classes instead of `System.Object`.)

### Naming conventions
Most properties of the `VCard` class are collections. It has to do with that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.
            
A special feature are properties whose name ends with "Views": These are properties that actually is only one instance allowed, but vCard 4.0 allows to have different versions of that single instance (e.g., in different languages). The same `AltID` parameter has to be set on each instance.
            
Most classes derived from `VCardProperty` implement `IEnumerable<T>` in order to be assignable to collection properties without having to be wrapped in an Array or List.

Use Linq and the built-in [extension methods](#extension-methods) when querying the data.

## Efficient building and editing of VCard instances using Fluent APIs
The VCard data model consists of numerous classes. Some of them encapsulate a lot of properties. 

Fortunately, there are Fluent APIs like `VCardBuilder`, `AddressBuilder` and `NameBuilder` that handle 
all the complicated operations in the background.

[Learn more](https://github.com/FolkerKinzel/VCards/wiki/The-VCardBuilder-class) about these Fluent APIs.

## Parsing and serializing VCF files using the Vcf class
The `Vcf` class is a static class that contains a lot of methods for serializing and parsing `VCard` objects to or from VCF files.

## Extension methods
The namespace `FolkerKinzel.VCards.Extensions` contains several extension methods that makes working with VCard objects 
more efficient and less error prone. It's recommended to publish this namespace when working with this
library. 

The methods are helpful in the following cases:
- Most of the enums are Flags enums and most of the .NET properties with enum Types use the `Nullable<T>` variant of these 
enums. Extension methods help to savely evaluate and manipulate these nullable enum values.
- The .NET data types for date and time (such like DateOnly or DateTimeOffset) are not fully compliant with the date-time
information defined by the vCard standard. Extension methods for these data types help to overcome these issues.
- Most of the properties of the VCard class are of a specialized Type of `IEnumerable<VCardProperty?>?`. Extension methods
encapsulate most of the necessary null checking and Linq operations that are needed to retrieve the relevant data from these 
properties, or to store something there.
- Some operations work with collections of VCard objects (e.g., saving several VCard objects together in a common VCF file). 
Extension methods allow these operations to be performed directly on these collections.

## The vCard 4.0 data synchronization
With the vCard 4.0 standard a data synchronization mechanism using PID parameters and CLIENTPIDMAP
properties has been introduced. For this to work fully automatically, only two lines of code are 
required.

[Learn more](https://github.com/FolkerKinzel/VCards/wiki/Setting-up-vCard-4.0-data-synchronization)

## Handling of incompliant data
Parse errors, caused by not well-formed VCF files, are silently ignored by the library: It reads as much as it can from such files.

When converting from a higher vCard standard to a lower one, not all data is compliant. To minimize 
data loss, the library tries to preserve incompliant data using well-known x-name properties. 
The usage of such x-name properties can be controlled with the `VcfOpts` enum.

## Reading the project reference
At the [GitHub Releases page](https://github.com/FolkerKinzel/VCards/releases) there is a detailed project reference to each version of the Nuget package as CHM file in the Assets. On some systems the content of this CHM file is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox - if it is present - in the lower right corner of the General tab in the Properties dialog.

Uppercase words, which are often found at the beginning of the documentation for a .NET property, are identifiers from the vCard standard. Digits in brackets that can be found at the end of the documentation for a .NET property, e.g., `(2,3,4)`, describe which vCard standard the content of the .NET property is compatible with.
            
The digits have the following meaning:
- `2`: vCard 2.1,
- `3`: vCard 3.0
- `4`: vCard 4.0

## Documents of the vCard standard
The vCard standard is defined in the following documents:
- [RFC 6350 (vCard 4.0)](https://tools.ietf.org/html/rfc6350)
- [RFC 2426 (vCard 3.0)](https://tools.ietf.org/html/rfc2426)
- [vCard.The Electronic Business Card.Version 2.1 (vCard 2.1)](https://web.archive.org/web/20120501162958/http://www.imc.org/pdi/vcard-21.doc)

Extensions of the standard describe, e.g., the following documents:
- [RFC 9555: JSContact: Converting from and to vCard](https://datatracker.ietf.org/doc/html/rfc9555)
- [RFC 9554: vCard Format Extensions for JSContact](https://datatracker.ietf.org/doc/html/rfc9554)
- [RFC 8605: vCard Format Extensions: ICANN Extensions for the Registration Data Access Protocol (RDAP)](https://datatracker.ietf.org/doc/html/rfc8605)
- [RFC 6474: vCard Format Extensions: Place of Birth, Place and Date of Death](https://tools.ietf.org/html/rfc6474)
- [RFC 6715: vCard Format Extensions: Representing vCard Extensions Defined by the Open Mobile Alliance (OMA) Converged Address Book (CAB) Group](https://tools.ietf.org/html/rfc6715)
- [RFC 6473: vCard KIND: application](https://tools.ietf.org/html/rfc6473)
- [RFC 4770: vCard Extensions for Instant Messaging (IM)](https://tools.ietf.org/html/rfc4770)
- [RFC 2739: Calendar Attributes for vCard and LDAP](https://tools.ietf.org/html/rfc2739)