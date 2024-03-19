# FolkerKinzel.VCards
[![NuGet](https://img.shields.io/nuget/v/FolkerKinzel.VCards)](https://www.nuget.org/packages/FolkerKinzel.VCards/)
[![GitHub](https://img.shields.io/github/license/FolkerKinzel/VCards)](https://github.com/FolkerKinzel/VCards/blob/master/LICENSE)

## .NET library to read, write, and convert VCF files that match the vCard standards 2.1, 3.0, and 4.0

FolkerKinzel.VCards is a full featured .NET library to work with vCard files (*.vcf).</para>

It allows
- to load VCF files from the file system and to save them there,</para>
- to serialize VCF files from and to Streams and</para>
- to convert VCF files that match the vCard versions 2.1, 3.0, and 4.0 to each other.</para>

Parse errors, caused by not well-formed VCF files, are silently ignored by the library: It reads as much as it can from such files.
The same is for errors caused by incompliant data when serializing the vCard: Because of the different vCard standards are not completely compliant, incompliant data is silently ignored when converting from one vCard standard to another. To minimize this kind of data loss, the library tries to preserve incompliant data using well-known x-name properties. The usage of such x-name properties can be controlled.

[Project Reference and Release Notes](https://github.com/FolkerKinzel/VCards/releases/tag/v6.1.0)

Read the ["Getting Started" tutorial](https://github.com/FolkerKinzel/VCards/blob/master/src/Examples/GettingStarted.md) if you are new to this library!

### Code Examples
- [Getting started](src/Examples/GettingStarted.md)
- [Reading and writing of VCF files](src/Examples/VCardExample.cs)
- [Example implementation of ITimeZoneIDConverter](src/Examples/TimeZoneIDConverter.cs)
- [Automatic detection of VCF files written with ANSI encodings](src/Examples/AnsiFilterExample.cs)
- [The vCard 4.0 data synchronization](src/Examples/DataSynchronization.md)
- [Loading a VCF file from the internet](src/Examples/WebExample.cs)
- [Reading and writing group vCards](src/Examples/VCard40Example.cs)
- [Connecting vCards](src/Examples/EmbeddedVCardExample.cs)
- [Parsing a very large VCF file](src/Examples/VcfReaderExample.cs)

*_(All of the examples are in C# and with nullable reference types enabled. For the sake of 
better readability, exception handling and parameter validation has been omitted.)_*

[Version History](https://github.com/FolkerKinzel/VCards/releases)