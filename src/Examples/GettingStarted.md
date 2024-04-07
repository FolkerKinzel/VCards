# Getting Started
Read here:
- [The usage of the namespaces](#the-usage-of-the-namespaces)
- [The data model](#the-data-model)
  - [The VCard class](#the-vcard-class)
  - [The VCardProperty class](#the-vcardproperty-class)
  - [Naming conventions](#naming-conventions)
- [Efficient building and editing of VCard objects using VCardBuilder](#efficient-building-and-editing-of-vcard-objects-using-vcardbuilder)
- [Extension methods](#extension-methods)
- [Parsing and serializing VCF files using the Vcf class](#parsing-and-serializing-vcf-files-using-the-vcf-class)
- [The vCard 4.0 data synchronization mechanism](#the-vcard-4.0-data-synchronization-mechanism)
- [Reading the project reference](#reading-the-project-reference)
- [Documents of the vCard standard](#documents-of-the-vcard-standard)


## The usage of the namespaces
```csharp
// Publish this namespace - it contains the VCard class,
// the VCardBuilder class, and the VCF class:
using FolkerKinzel.VCards;

// It's recommended to publish also this namespace -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// This namespace contains often used enums. Decide
// yourself whether to publish this namespace or to use
// a namespace alias.
using FolkerKinzel.VCards.Enums;

// Since VCardBuilder exists, the model classes normally
// don't need to be instantiated in own code:
// using FolkerKinzel.VCards.Models;
```

## The data model
The data model used by this library is aligned to the vCard 4.0 standard (RFC 6350). This means that every vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized they are converted back. 

### The VCard class
A VCF file consists of one or more vCards. The content of a vCard is represented by the `VCard` class.

### The VCardProperty class
A vCard consists of several "properties". Accordingly the data model of the `VCard` class is built on classes that are derived from `VCardProperty`.

`VCardProperty` exposes the following members:
```csharp
public abstract class VCardProperty
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual object? Value { get; protected set; }
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
            
(Classes derived from `VCardProperty` hide the generic implementation of `VCardProperty.Value` in order to return derived classes instead of `System.Object?`.)

### Naming conventions
Most properties of the `VCard` class are collections. It has to do with that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.
            
A special feature are properties whose name ends with "Views": These are properties that actually is only one instance allowed, but vCard 4.0 allows to have different versions of that single instance (e.g., in different languages). The same `AltID` parameter has to be set on each instance.
            
Most classes derived from `VCardProperty` implement `IEnumerable<T>` in order to be assignable to collection properties without having to be wrapped in an Array or List.

## Efficient building and editing of VCard objects using VCardBuilder
The `VCardBuilder` class provides a fluent API for building and editing VCard objects.

The properties of the VCardBuilder class have the same names as those of the VCard class. Each of these 
properties gets a struct that provides methods to edit the corresponding VCard property. 
Each of these methods return the VCardBuilder instance so that the calls can be chained.

The `VCardBuilder.Create` method overloads initialize a VCardBuilder, which creates a new 
VCard instance or edits an existing one. The `VCardBuilder.VCard` property gets the VCard 
object that the VCardBuilder created or manipulated.

See an example how `VCardBuilder` can be used:
```csharp
VCard vCard = VCardBuilder
    .Create()
    .NameViews.Add(familyNames: ["Müller-Risinowsky"],
                   givenNames: ["Käthe"],
                   additionalNames: ["Alexandra", "Caroline"],
                   prefixes: ["Prof.", "Dr."],
                   displayName: (builder, prop) => builder.Add(prop.ToDisplayName())
                  )
    .GenderViews.Add(Sex.Female)
    .Organizations.Add("Millers Company", ["C#", "Webdesign"])
    .Titles.Add("CEO")
    .Photos.AddFile(photoFilePath)
    .Phones.Add("tel:+49-123-9876543",
                 parameters: p =>
                 {
                     p.DataType = Data.Uri;
                     p.PropertyClass = PCl.Home;
                     p.PhoneType = Tel.Voice | Tel.BBS;
                 }
               )
    .Phones.Add("tel:+49-321-1234567",
                 parameters: p =>
                 {
                     p.DataType = Data.Uri;
                     p.PropertyClass = PCl.Work;
                     p.PhoneType = Tel.Cell | Tel.Text | Tel.Msg | Tel.BBS | Tel.Voice;
                 }
               )
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
    .EMails.Add("mailto:kaethe_at_home@internet.com",
                 parameters: p =>
                 {
                     p.DataType = Data.Uri;
                     p.PropertyClass = PCl.Home;
                 }
               )
    .EMails.Add("kaethe_mueller@internet.com", pref: true,
                 parameters: p => p.PropertyClass = PCl.Work
               )
    .BirthDayViews.Add(1984, 3, 28)
    .Relations.Add("Paul Müller-Risinowsky", 
                   Rel.Spouse | Rel.CoResident | Rel.Colleague
                  )
    .AnniversaryViews.Add(2006, 7, 14)
    .VCard;
```
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

## Parsing and serializing VCF files using the Vcf class
The `Vcf` class is a static class that contains a lot of methods for serializing and parsing `VCard` objects to or from VCF files.

## The vCard 4.0 data synchronization mechanism
With the vCard 4.0 standard a data synchronization mechanism using PID parameters and CLIENTPIDMAP
properties has been introduced. For this to work fully automatically, only two lines of code are 
required.

[Learn more](DataSynchronization.md)

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
- [RFC 6474: vCard Format Extensions: Place of Birth, Place and Date of Death](https://tools.ietf.org/html/rfc6474)
- [RFC 6715: vCard Format Extensions: Representing vCard Extensions Defined by the Open Mobile Alliance (OMA) Converged Address Book (CAB) Group](https://tools.ietf.org/html/rfc6715])
- [RFC 6473: vCard KIND: application](https://tools.ietf.org/html/rfc6473)
- [RFC 4770: vCard Extensions for Instant Messaging (IM)](https://tools.ietf.org/html/rfc4770)
- [RFC 2739: Calendar Attributes for vCard and LDAP](https://tools.ietf.org/html/rfc2739)