# FolkerKinzel.VCards
## Roadmap

### 2.1.1
- [x] Cleanup: Remove obsolete symbols which are marked as errors.
- [x] Increase the code coverage of the unit tests.

### 2.2.0
- [x] Implement FBURL property.

### 3.0.0  
- [ ] Add .NET Standard 2.0 support
- [ ] Add .NET Framework 4.6.1 support

=

- [x] Rename VCard.Save() to `SaveVcf`.
- [x] Rename VCard.Serialize() to `SerializeVcf`.
- [x] Rename VCard.Load() to `LoadVcf`.
- [x] Rename VCard.Deserialize() to `DeserializeVcf`.
- [x] Rename VCard.Parse() to `ParseVcf`. 

=
- [x] Make the Value of `TimeZoneProperty` a new class `TimeZoneID`.
- [x] Make `ParameterSection.TimeZone` a new class `TimeZoneID`.
- [x] Add an interface `ITimeZoneIDConverter` that allows users to inject an object which 
converts named time zones (e.g. IANA names) to UTC offsets (TimeSpan) when converting from 
vCard 4.0 to vCard 3.0. Make an option for this conversion.


=
- [x] Move `FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate` to 
`FolkerKinzel.VCards.Models.GeoCoordinate`.
- [x] Remove the Property `GeoCoordinate.IsUnknown` and make sure instead that 
GeoCoordinate always points to a valid geographical position.



=
- [x] Move `FolkerKinzel.VCards.Models.PropertyParts.PropertyIDMapping` to
`FolkerKinzel.VCards.Models.PropertyIDMapping`.
- [x] Replace in `PropertyIDMapping.Parse` the method char.IsDigit(char) 
with an extension method `char.IsDecimalDigit()`
- [x] Make `FolkerKinzel.VCards.Models.PropertyIDMapping` a class.
- [x] Rename `PropertyIdMapping.MappingNumber` to `PropertyIdMapping.ID`
- [x] Replace `Guid PropertyIdMapping.Uuid` by `Uri PropertyIDMapping.Mapping`.
- [x] Remove `IEquatable<PropertyIDMapping`.

=

- [x] `PropertyIDMappingProperty`: Ctor, which takes `PropertyIDMapping` as 
argument.

=
- [x] Store the value of `PropertyID` in one `byte`.
- [x] Replace in `PropertyID.ParseInto` the method char.IsDigit(char) 
with an extension method `char.IsDecimalDigit()`.
- [x] Make `PropertyID` a class in order to have better validation.
- [x] Rename `PropertyID.PropertyNumber` to `PropertyID.ID`.
- [x] Rename `PropertyID.MappingNumber` to `PropertyID.Mapping`.
- [x] Make an explicit implementation of `IEnumerable<PropertyID>` in `PropertyID`.
- [x] Give `PropertyID` only one public ctor:
```csharp
public PropertyID(int id, PropertyIDMapping? mapping = null);
```

=
- [x] Mark the usage of `VCdDataType.Timestamp` as Obsolete-**Error**.

=
- [x] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.SaveVcf.
- [x] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.SerializeVcf.
- [x] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.ToVcfString.
- [x] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.Reference.
- [x] Let VCard.Reference return `IEnumerable<VCard>`.
- [x] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.Dereference.
- [x] Let VCard.Dereference return `IEnumerable<VCard>`.
- [x] Add a new class `VCardCollectionExtension` with extension methods for IEnumerable&lt;VCard?&gt;.
Remove `VCardListExtension` instead.

=
- [x] Implement `IEnumerable<VCard>` in `VCard`.
- [x] Implement `IEquatable<VCard>` in `VCard`.
- [x] Implement `ICloneable` in `VCard`.
- [x] Implement `ICloneable` in `ParameterSection`.
- [x] Implement `ICloneable` in `VCardProperty`.

=
- [x] Let VCard.LoadVcf should return `IList&lt;VCard&gt;`.
- [x] Let VCard.DeserializeVcf return `IList&lt;VCard&gt;`.
- [x] Let VCard.ParseVcf return `IList&lt;VCard&gt;`.

=
- [x] Use `System.Text.CodePagesEncodingProvider` to retrieve text encoding instances in .NET Standard and .NET 5.0.

=
- [x] DataUrl.FromText(string) should work even if the passed string is url encoded yet.
- [x] DataUrl should accept null, empty strings, empty byte arrays, empty files or whitespace
as input for the data to embed.

### 3.0.1
- [ ] Cleanup: Remove `VCdDataType.Timestamp` and all other Symbols marked as Obsolete errors in 3.0.0.



### 4.0.0
- [ ] End .NET Framework 4.0 support.
- [ ] Use the HashCode struct to compute hash codes.
- [ ] Remove the `DataUrl` class and the `MimeType` classes and replace it with something new and better like
`FolkerKinzel.Uris.DataUrl` and `FolkerKinzel.MimeTypes.MimeType`.
- [ ] Replace `FolkerKinzel.VCards.Intls.Converters.TextEncodingConverter` with `FolkerKinzel.Strings.TextEncodingConverter`