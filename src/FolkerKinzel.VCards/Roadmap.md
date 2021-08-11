# FolkerKinzel.VCards
## Roadmap

### 2.1.1
- [x] Cleanup: Remove obsolete symbols which are marked as errors.
- [x] Increase the code coverage of the unit tests.

### 2.2.0
- [x] Implement FBURL property.

### 3.0.0  
- [ ] Add .NET Standard 2.0 support

=

- [x] Rename VCard.Save() to `SaveVcf`.
- [x] Rename VCard.Serialize() to `SerializeVcf`.
- [x] Rename VCard.Load() to `LoadVcf`.
- [x] Rename VCard.Deserialize() to `DeserializeVcf`.
- [x] Rename VCard.Parse() to `ParseVcf`. 

=
- [ ] Make the Value of `TimeZoneProperty` a new class `TzInfo`.
- [ ] Make `ParameterSection.TimeZone` a new class `TzInfo`.
- [ ] Add an interface `ITimeZoneConverter` that allows users to inject an object which converts named time zones
(e.g. IANA names) to UTC offsets (TimeSpan) when converting from vCard 4.0 to vCard 3.0.

=
- [x] Move `FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate` to 
`FolkerKinzel.VCards.Models.GeoCoordinate`.
- [x] Remove the Property `GeoCoordinate.IsUnknown` and make sure instead that 
GeoCoordinate always points to a valid geographical position.

=
- [ ] Move `FolkerKinzel.VCards.Models.PropertyParts.PropertyIDMapping` to
`FolkerKinzel.VCards.Models.PropertyIDMapping`.
- [ ] Make `FolkerKinzel.VCards.Models.PropertyIDMapping` a class.
- [ ] Replace `Guid PropertyIdMapping.Uuid` by `Uri PropertyIDMapping.Identifier`.

=

=
- [ ] Store the value of `PropertyID` in one `byte`.
- [ ] Make `PropertyID` a class in order to have better validation.
- [ ] Give `PropertyID` only one public ctor:
```csharp
public PropertyID(int propertyNumber, PropertyIDMapping? pidMap = null);
```

=
- [x] Mark the usage of `VCdDataType.Timestamp` as Obsolete-**Error**.

=
- [ ] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.SaveVcf, VCard.SerializeVcf, VCard.ToVcfString, VCard.Reference and VCard.Dereference.
- [ ] Add a new class `VCardCollectionExtension` with extension methods for IEnumerable&lt;VCard?&gt;.
- [ ] Remove `VCardListExtension` instead.

=
- [ ] Implement `IEnumerable<VCard>` in `VCard`

=
- [ ] `VCard.Reference` should copy the input data and return its result as IEnumerable&lt;VCard&gt;.

=
- [ ] VCard.LoadVcf, VCard.DeserializeVcf and VCard.ParseVcf should return a `ReadOnlyCollection&lt;VCard&gt;`.

=
- [x] Use `System.Text.CodePagesEncodingProvider` to retrieve text encoding instances in .NET Standard and .NET 5.0.

=
- [x] DataUrl.FromText(string) should work even if the passed string is url encoded yet.
- [ ] DataUrl should accept null, empty strings, empty byte arrays, empty files or whitespace
as input for the data to embed.

### 3.0.1
- [ ] Cleanup: Remove `VCdDataType.Timestamp` and all other Symbols marked as Obsolete errors in 3.0.0.

### 3.1.0
- [ ] Implement `ICloneable` to the `VCard` class.

### 4.0.0
- [ ] End .NET Framework 4.0 support.
- [ ] Add .NET Framework 4.6.1 support
- [ ] Remove the `DataUrl` class and the `MimeType` class and replace it with something new and better like
`FolkerKinzel.Uris.DataUrl` and `FolkerKinzel.MimeTypes.MimeType`.
- [ ] Replace `FolkerKinzel.VCards.Intls.Converters.TextEncodingConverter` with `FolkerKinzel.Strings.TextEncodingConverter`