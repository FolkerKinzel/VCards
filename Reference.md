# FolkerKinzel.VCards
.NET library to read, write and convert VCF files that match the vCard standards 2.1, 3.0 and 4.0.

It enables you
* to load VCF files from the file system and to save them there,
* to serialize VCF files from and to Streams and
* to convert VCF files, that match the vCard-versions 2.1, 3.0 and 4.0, to each other.

FolkerKinzel.VCards is used as a dependency in [FolkerKinzel.Contacts.IO](https://www.nuget.org/packages/FolkerKinzel.Contacts.IO/) - an easy to use .NET-API to manage contact data of organizations and natural persons.

[Download Project Reference English](https://github.com/FolkerKinzel/VCards/blob/master/FolkerKinzel.VCards.Reference.en/Help/FolkerKinzel.VCards.en.chm)

[Projektdokumentation (Deutsch) herunterladen](https://github.com/FolkerKinzel/VCards/blob/master/FolkerKinzel.VCards.Doku.de/Help/FolkerKinzel.VCards.de.chm)

> IMPORTANT: On some systems, the content of the chm file is blocked. Before extracting it,  right click on the file, select Properties, and check the "Allow" checkbox (if it is present) in the lower right corner of the General tab in the Properties dialog.


## Overview
### The Data model explained

The data model used by this API is aligned to the vCard 4.0 standard (RFC6350). This means, every read vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized, they are converted back.

#### Class VCardProperty<T>

The data model of the class `VCard` based on classes, that are derived from VCardProperty<T>.

VCardProperty<T> exposes the following data:

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
* `group1` to VCardProperty<T>.Group,
* `TEL;TYPE=home,voice;VALUE=uri` to VCardProperty<T>.Parameters and
* `tel:+49-123-4567` to VCardProperty<T>.Value.

#### Naming Conventions

Most properties of class `VCard` are collections. It has to do with, that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.</para>
              
A special feature are properties whose name ends with "Views": These are properties, which actually is only one instance per vCard allowed, but vCard 4.0 enables you to have different versions of that single instance (e.g. in different languages). You must set the same `AltID` parameter on each of these versions.

### How the library handles data errors

Parse errors, caused by not well-formed VCF files, are silently ignored by the library: It reads as much as it can from such files.

The same is for errors caused by incompliant data when serializing the vCard: Because of the different vCard standards are not completely compliant, incompliant data is silently ignored when converting from one vCard standard to another. To minimize this kind of data loss, the API tries to preserve incompliant data using well-known x-name properties. The usage of such x-name properties can be controlled via options (VcfOptions).