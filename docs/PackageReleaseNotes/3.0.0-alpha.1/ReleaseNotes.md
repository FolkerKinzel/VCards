# FolkerKinzel.VCards 3.0.0-alpha.1
## Package Release Notes
- .NET Standard 2.0 support and .NET Framework 4.6.1 support has been added.
- Some methods have been renamed:
  - VCard.Save to `SaveVcf`.
  - VCard.Serialize to `SerializeVcf`.
  - VCard.Load to `LoadVcf`.
  - VCard.Deserialize to `DeserializeVcf`.
  - VCard.Parse to `ParseVcf`. 
  
  This helps for clarity, because extension methods from other libraries could save/parse/serialize the 
`VCard` class from or to JSON, XML or Barcodes for example.

- An issue has been resolved that IANA time zone names could not be automatically converted into UTC-Offsets
when converting a vCard 4.0 into an earlier version. Until .NET 5.0 this is not possible to do with standard
.NET methods but the new interface `ITimeZoneIDConverter` allows to use a 3rd party library to do the job.
- In this context the new class `TimeZoneID` is introduced.
- `DataUrl` now accepts also null, empty strings, empty byte arrays, empty files or whitespace
as input for the data to embed.
- Everything that has to do with vCard 4.0 "PID matching" (`PropertyID`, `PropertyIDMapping`, `PropertyIDMappingProperty`)
has been completely rewritten.
- `FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate` has been replaced
by `FolkerKinzel.VCards.Models.GeoCoordinate`.
- Bugfixes and Refactorings.
