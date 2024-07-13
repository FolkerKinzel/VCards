# FolkerKinzel.VCards
[![NuGet](https://img.shields.io/nuget/v/FolkerKinzel.VCards)](https://www.nuget.org/packages/FolkerKinzel.VCards/)
[![GitHub](https://img.shields.io/github/license/FolkerKinzel/VCards)](https://github.com/FolkerKinzel/VCards/blob/master/LICENSE)

## .NET library for reading, writing, and converting VCF files that comply with vCard standards 2.1, 3.0 and 4.0

FolkerKinzel.VCards is a full-featured .NET library for working with vCard files (*.vcf).

It's performance optimized for high throughput and has a fluent API for efficient creation and editing of vCards.

The library supports [RFC 6350](https://tools.ietf.org/html/rfc6350) (vCard 4.0), 
[RFC 2426](https://tools.ietf.org/html/rfc2426) (vCard 3.0), 
[vCard 2.1](https://web.archive.org/web/20120501162958/http://www.imc.org/pdi/vcard-21.doc), 
the extensions
[RFC 8605](https://datatracker.ietf.org/doc/html/rfc8605),
[RFC 6868](https://datatracker.ietf.org/doc/html/rfc6868),
[RFC 6715](https://tools.ietf.org/html/rfc6715), 
[RFC 6474](https://tools.ietf.org/html/rfc6474),
[RFC 6473](https://tools.ietf.org/html/rfc6473), 
[RFC 4770](https://tools.ietf.org/html/rfc4770),
[RFC 2739](https://tools.ietf.org/html/rfc2739), as well as several popular non-standard vCard properties.

It allows:
- loading VCF files from the file system and saving them there,
- serializing and deserializing VCF files to and from streams,
- and interconverting VCF files corresponding to vCard versions 2.1, 3.0, and 4.0.

Read the ["Getting Started" wiki](https://github.com/FolkerKinzel/VCards/wiki) if you are new to this library, otherwise [read the project reference](https://folkerkinzel.github.io/VCards/reference/).

### Code Examples
- [Reading and writing of VCF files (simple example)](https://github.com/FolkerKinzel/VCards/wiki#simple-example)
- [Reading and writing of VCF files (advanced features)](src/Examples/VCardExample.cs)
- [Example implementation of ITimeZoneIDConverter](src/Examples/TimeZoneIDConverter.cs)
- [Automatic detection of VCF files written with ANSI encodings](src/Examples/AnsiFilterExample.cs)
- [Setting up vCard 4.0 data synchronization](https://github.com/FolkerKinzel/VCards/wiki/Setting-up-vCard-4.0-data-synchronization)
- [Loading a VCF file from the internet](src/Examples/WebExample.cs)
- [Reading and writing group vCards](src/Examples/VCard40Example.cs)
- [Connecting vCards](src/Examples/EmbeddedVCardExample.cs)
- [Parsing a very large VCF file](src/Examples/VcfReaderExample.cs)

*_(All of the examples are in C# and with nullable reference types enabled. For the sake of 
better readability, exception handling and parameter validation has been omitted.)_*

[Version History](https://github.com/FolkerKinzel/VCards/releases)