# Overview

FolkerKinzel.VCards is a full-featured .NET library for working with vCard files (*.vcf).

It allows
<ul><li><p>loading VCF files from the file system and storing them there,</p></li><li><p>

serializing VCF files to and from streams,</p></li><li><p>

and interconverting VCF files corresponding to vCard versions 2.1, 3.0, and 4.0.</p></li></ul>




Parse errors, caused by not well-formed VCF files, are silently ignored by the library: It reads as much as it can from such files.

The same is for errors caused by incompliant data when serializing the vCard: Because of the different vCard standards are not completely compliant, incompliant data is silently ignored when converting from one vCard standard to another. To minimize this kind of data loss, the library tries to preserve incompliant data using well-known x-name properties. The usage of such x-name properties can be controlled.
<p>This topic contains the following sections:</p><ul><li><a href="#the-usage-of-the-namespaces">

The usage of the namespaces</a></li><li><a href="#the-data-model">The data model</a></li><li><a href="#efficient-building-and-editing-of-vcard-objects-using-vcardbuilder">Efficient building and editing of VCard objects using VCardBuilder</a></li><li><a href="#extension-methods">Extension methods</a></li><li><a href="#parsing-and-serializing-vcf-files-using-the-vcf-class">Parsing and serializing VCF files using the Vcf class</a></li><li><a href="#the-vcard-4.0-data-synchronization-mechanism">The vCard 4.0 data synchronization mechanism</a></li><li><a href="#reading-the-project-reference">Reading the Project Reference</a></li><li><a href="#the-vcard-standard">The vCard Standard</a></li></ul>

## The usage of the namespaces

The following code example provides tips for using the namespaces of the library.


**C#**  
``` C#
// Publish this namespace - it contains the VCard class
// and the VCardBuilder class:
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
<p>This section contains the following subsections:</p><ul><li><a href="#the-vcard-class">

The VCard Class</a></li><li><a href="#the-vcardproperty-class">The VCardProperty Class</a></li><li><a href="#naming-conventions">Naming Conventions</a></li></ul>


The data model used by this library is aligned to the vCard 4.0 standard (RFC6350). This means that every vCard of version 2.1 and 3.0 is internally converted to vCard 4.0. When saved and serialized, they are converted back.


#### The VCard Class
  

A VCF file consists of one or more vCards. The content of a vCard is represented by the `VCard` class.


#### The VCardProperty Class
  

The data model of the `VCard` class is built on classes that are derived from `VCardProperty`.

`VCardProperty` exposes the following members:


**C#**  
``` C#
public abstract class VCardProperty
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual object? Value { get; protected set; }
}
```

This reflects the structure of a data row in a VCF file:

`group1.TEL;TYPE=home,voice;VALUE=uri:tel:+49-123-4567`

In this example corresponds
<ul><li><p><code>group1</code> to VCardProperty.Group,</p></li><li><p><code>

TEL;TYPE=home,voice;VALUE=uri</code> to VCardProperty.Parameters and</p></li><li><p><code>

tel:+49-123-4567</code> to VCardProperty.Value.</p></li></ul>


(Classes that are derived from `VCardProperty` hide the generic implementation of `VCardProperty.Value` in order to return derived classes instead of `System.Object?`.)


#### Naming Conventions
  

Most properties of the `VCard` class are collections. It has to do with that many properties are allowed to have more than one instance per vCard (e.g. phone numbers, e-mail addresses). Such properties are named in Plural.

A special feature are properties whose name ends with "Views": These are properties that actually is only one instance allowed, but vCard 4.0 allows to have different versions of that single instance (e.g., in different languages). The same `AltID` parameter has to be set on each instance.

Most classes derived from `VCardProperty` implement `IEnumerable&lt;T&gt;` in order to be assignable to collection properties without having to be wrapped in an Array or List.


## Efficient building and editing of VCard objects using VCardBuilder

The `VCardBuilder` class provides a fluent API for building and editing VCard objects.

The properties of the VCardBuilder class have the same names as those of the VCard class. Each of these properties gets a struct that provides methods to edit the corresponding VCard property. Each of these methods return the VCardBuilder instance so that the calls can be chained.

The `VCardBuilder.Create` method overloads initialize a VCardBuilder, which creates a new VCard instance or edits an existing one. The `VCardBuilder.VCard` property gets the VCard object that the VCardBuilder created or manipulated.


## Extension methods

The namespace `FolkerKinzel.VCards.Extensions` contains several extension methods that makes working with VCard objects more efficient and less error prone. It's recommended to publish this namespace when working with this library.

The methods are helpful in the following cases:
<ul><li><p>Most of the enums are Flags enums and most of the .NET properties with enum Types use the <code>Nullable&lt;T&gt;</code> variant of these enums. Extension methods help to savely evaluate and manipulate these nullable enum values.</p></li><li><p>

The .NET data types for date and time (such like DateOnly or DateTimeOffset) are not fully compliant with the date-time information defined by the vCard standard. Extension methods for these data types help to overcome these issues.</p></li><li><p>

Most of the properties of the VCard class are of a specialized Type of <code>IEnumerable&lt;VCardProperty?&gt;?</code>. Extension methods encapsulate most of the necessary null checking and Linq operations that are needed to retrieve the relevant data from these properties, or to store something there.</p></li><li><p>

Some operations work with collections of VCard objects (e.g., saving several VCard objects together in a common VCF file). Extension methods allow these operations to be performed directly on these collections.</p></li></ul>



## Parsing and serializing VCF files using the Vcf class

The `Vcf` class is a static class that contains a lot of methods for serializing and parsing `VCard` objects to or from VCF files.


## The vCard 4.0 data synchronization mechanism

With the vCard 4.0 standard a data synchronization mechanism using PID parameters and CLIENTPIDMAP properties has been introduced. For this to work fully automatically, only two lines of code are required:


**C#**  
``` C#
// Registering the executing application with the VCard class is a technical requirement
// when using the data synchronization mechanism introduced with vCard 4.0 (PID and
// CLIENTPIDMAP). To do this, call the static method VCard.RegisterApp with an absolute
// Uri once when the program starts. (UUID URNs are ideal for this.)
VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

//Write a vCard 4.0 with the option VcfOptions.SetPropertyIDs:
Vcf.Save(vCard,
         filePath,
         VCdVersion.V4_0,
         options: Opts.Default.Set(Opts.SetPropertyIDs));
```


## Reading the Project Reference

Uppercase words, which are often found at the beginning of the documentation for a .NET property, are identifiers from the vCard standard. Digits in brackets, which can be found at the end of the documentation for a .NET property, e.g. *(2,3,4)*, describe which with vCard standard the content of the .NET property is compatible.

The digits have the following meaning:
<ul><li><p><code>2</code>: vCard 2.1,</p></li><li><p><code>

3</code>: vCard 3.0</p></li><li><p><code>

4</code>: vCard 4.0</p></li></ul>



## The vCard Standard

The vCard standard is defined in the following documents:
<ul><li><p><a href="https://tools.ietf.org/html/rfc6350" target="_blank" rel="noopener noreferrer">RFC 6350 (vCard 4.0)</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc2426" target="_blank" rel="noopener noreferrer">

RFC 2426 (vCard 3.0)</a></p></li><li><p><a href="https://web.archive.org/web/20120501162958/http://www.imc.org/pdi/vcard-21.doc" target="_blank" rel="noopener noreferrer">

vCard.The Electronic Business Card.Version 2.1 (vCard 2.1)</a></p></li></ul>




Extensions of the standard describe e.g. the following documents:
<ul><li><p><a href="https://datatracker.ietf.org/doc/html/rfc9554" target="_blank" rel="noopener noreferrer">RFC 9554: vCard Format Extensions for JSContact</a></p></li><li><p><a href="https://datatracker.ietf.org/doc/html/rfc8605" target="_blank" rel="noopener noreferrer">

RFC 8605: vCard Format Extensions: ICANN Extensions for the Registration Data Access Protocol (RDAP)</a></p></li><li><p><a href="https://datatracker.ietf.org/doc/html/rfc6868" target="_blank" rel="noopener noreferrer">

RFC 6868: vCard Format Extensions: ICANN Extensions for the Registration Data Access Protocol (RDAP)</a></p></li><li><p><a href="https://datatracker.ietf.org/doc/html/rfc6715" target="_blank" rel="noopener noreferrer">

RFC 6715: vCard Format Extensions: Representing vCard Extensions Defined by the Open Mobile Alliance (OMA) Converged Address Book (CAB) Group</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc6474" target="_blank" rel="noopener noreferrer">

RFC 6474: vCard Format Extensions: Place of Birth, Place and Date of Death</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc6715" target="_blank" rel="noopener noreferrer">

RFC 6715: vCard Format Extensions: Representing vCard Extensions Defined by the Open Mobile Alliance (OMA) Converged Address Book (CAB) Group</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc6473" target="_blank" rel="noopener noreferrer">

RFC 6473: vCard KIND: application</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc4770" target="_blank" rel="noopener noreferrer">

RFC 4770: vCard Extensions for Instant Messaging (IM)</a></p></li><li><p><a href="https://tools.ietf.org/html/rfc2739" target="_blank" rel="noopener noreferrer">

RFC 2739: Calendar Attributes for vCard and LDAP</a></p></li></ul>

