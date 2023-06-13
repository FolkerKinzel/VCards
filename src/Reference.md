[![GitHub](https://img.shields.io/github/license/FolkerKinzel/VCards)](https://github.com/FolkerKinzel/VCards/blob/master/LICENSE)

.NET library to read, write and convert VCF files that match the vCard standards 2.1, 3.0 and 4.0.

It allows
* to load VCF files from the file system and to save them there,
* to serialize VCF files from and to Streams and
* to convert VCF files, that match the vCard versions 2.1, 3.0 and 4.0, to each other.

FolkerKinzel.VCards is used as a dependency in [FolkerKinzel.Contacts.IO](https://www.nuget.org/packages/FolkerKinzel.Contacts.IO/) - an easy to use .NET-API to manage contact data of organizations and natural persons.

[Project Reference (English)](https://github.com/FolkerKinzel/VCards/blob/master/ProjectReference/4.0.0/FolkerKinzel.VCards.en.chm)

[Projektdokumentation (Deutsch)](https://github.com/FolkerKinzel/VCards/blob/master/ProjectReference/4.0.0/FolkerKinzel.VCards.de.chm)

> IMPORTANT: On some systems the content of the .CHM file is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox (if it is present) in the lower right corner of the General tab in the Properties dialog.


## Overview
### The Data Model Explained

The data model used by this API is aligned to the vCard 4.0 standard (RFC6350). This means, every read vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized, they are converted back. A vCard is represented by the `VCard` class.

#### The `VCardProperty` Class

The data model of the `VCard` class based on classes, that are derived from `VCardProperty`.

`VCardProperty` exposes the following members:

```csharp
public abstract class VCardProperty
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual object? Value { get; protected set; }
}
````

This reflects the structure of a data row in a VCF file:
> group1.TEL;TYPE=home,voice;VALUE=uri:tel:+49-123-4567

In this example corresponds
* `group1` to VCardProperty.Group,
* `TEL;TYPE=home,voice;VALUE=uri` to VCardProperty.Parameters and
* `tel:+49-123-4567` to VCardProperty.Value.

(Classes that are derived from `VCardProperty` hide the generic implementation of `VCardProperty.Value` in order to return derived classes instead of `System.Object?`.) 


#### Naming Conventions

Most properties of the `VCard` class are collections. It has to do with, that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.</para>
              
A special feature are properties whose name ends with "Views": These are properties, which actually is only one instance per vCard allowed, but vCard 4.0 enables you to have different versions of that single instance (e.g. in different languages). You must set the same `AltID` parameter on each of them.

#### Usage of the Namespaces
The following code example provides tips for using the namespaces of the library.

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
    public static void HowToUseTheNameSpaceAlias() =>
        _ = new VC::RelationTextProperty("Folker", VC::Enums.RelationTypes.Contact);
}
```

### How the Library Handles Data Errors

Parse errors, caused by not well-formed VCF files, are silently ignored by the library: It reads as much as it can from such files.

The same is for errors caused by incompliant data when serializing the vCard: Because of the different vCard standards are not completely compliant, incompliant data is silently ignored when converting from one vCard standard to another. To minimize this kind of data loss, the library tries to preserve incompliant data using well-known x-name properties. The usage of such x-name properties can be controlled via options (VcfOptions).