# Getting Started
Read here:
- [The usage of the namespaces](#the-usage-of-the-namespaces)
- [The data model explained](#the-data-model-explained)
  - [The VCardProperty class](#the-vcardproperty-class)
  - [Naming conventions](#naming-conventions)
- [Reading the project reference](#reading-the-project-reference)
- [Documents of the vCard standard](#documents-of-the-vcard-standard)

## The usage of the namespaces
```csharp
// Publish this namespace - it contains the VCard class:
using FolkerKinzel.VCards;

// It's recommended to publish also this namespace -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// These two namespaces may be published, but it's not
// recommended as they contain lots of classes and enums:
// using FolkerKinzel.VCards.Models;
// using FolkerKinzel.VCards.Models.Enums;

// Instead of publishing the two namespaces above
// better use a namespace alias:
using VC = FolkerKinzel.VCards.Models;

namespace NameSpaceAliasDemos;

public static class NameSpaceAliasDemo
{
    public static void HowToUseTheNameSpaceAlias()
        => _ = VC::RelationProperty.FromText
               (
                  "Folker", 
                   VC::Enums.RelationTypes.Contact
               );
}
```

## The data model explained

The data model used by this library is aligned to the vCard 4.0 standard (RFC6350). This means, every read vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized, they are   converted back. 

A vCard is represented by the `VCard` class.

### The VCardProperty class
The data model of the `VCard` class is built on classes, that are derived from `VCardProperty`.

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
            
(Classes that are derived from `VCardProperty` hide the generic implementation of `VCardProperty.Value` in order to return derived classes instead of `System.Object?`.)

### Naming conventions
Most properties of the `VCard` class are collections. It has to do with, that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.
            
A special feature are properties, whose name ends with "Views": These are properties, which actually is only one instance allowed, but vCard 4.0 enables you to have different versions of that single instance (e.g. in different languages). You must set the same `AltID` parameter  on each of these versions.
            
Most classes derived from `VCardProperty` implement `IEnumerable<T>` in order to be assignable to collection properties without having to be wrapped in an Array or List.

## Reading the project reference
At the [GitHub Releases page](https://github.com/FolkerKinzel/VCards/releases) there is a detailed project reference to each version of the Nuget package as CHM file in the Assets. On some systems, the content of this CHM file is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox - if it is present - in the lower right corner of the General tab in the Properties dialog.

Uppercase words, which are often found at the beginning of the documentation for a .NET property, are identifiers from the vCard standard. Digits in brackets, which can be found at the end of the documentation for a .NET property, e.g. `(2,3,4)`, describe which with vCard standard the content of the .NET property is compatible.
            
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