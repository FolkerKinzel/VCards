# FolkerKinzel.VCards
Full featured .NET-API to work with vCard files (*.vcf).

It enables you
* to load vcf files from the file system and to save them there,
* to serialize vcf files from and to Streams and
* to convert vCard files, that match the vCard-versions 2.1, 3.0 and 4.0, to each other.

[Download Project Reference English](https://github.com/FolkerKinzel/VCards/blob/master/FolkerKinzel.VCards.Reference.en/Help/FolkerKinzel.VCards.en.chm)

[Projektdokumentation (Deutsch) herunterladen]()

## Overview
### Error Handling

Parse errors are silently ignored.

The same is for errors, that occur serializing the vCard: Because of the different vCard-standards 
are not completely compliant, incompliant data is silently ignored when converting from one 
vCard-Standard to another. To minimize the data loss, the API tries to preserve incompliant data 
using well-known Non-Standard-Properties. The usage of such Non-Standard-Properties can be 
controlled via options (VcfOptions).