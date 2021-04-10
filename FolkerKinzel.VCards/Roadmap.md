# FolkerKinzel.VCards
## Roadmap

### 2.1.1
- [x] Cleanup: Remove obsolete symbols which are marked as errors.
- [x] Increase the code coverage of the unit tests.

### 2.2.0
- [ ] Implement FBURL property.

### 3.0.0  
- [ ] Rename Save to `SaveVcf`.
- [ ] Rename Serialize to `SerializeVcf`.
- [ ] Rename Load to `LoadVcf`.
- [ ] Rename Deserialize to `DeserializeVcf`.
- [ ] Rename Parse to `ParseVcf`. 
- [ ] Make the Value of `TimeZoneProperty` a String.
- [ ] Make `ParameterSection.TimeZone` a String property.
- [ ] Add an interface `ITimeZoneConverter` that allows users to inject an object which converts named time zones
(e.g. IANA names) to TimeSpan offsets when converting from vCard 4.0 to vCard 3.0.
- [ ] Move `FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate` to 
`FolkerKinzel.VCards.Models.GeoCoordinate`.
- [ ] Move `FolkerKinzel.VCards.Models.PropertyParts.PropertyIdMapping` to
`FolkerKinzel.VCards.Models.PropertyIdMapping` and **make it a class**.
- [ ] Add a constructor to the `PropertyIdMappingProperty` class, which takes a PropertyIdMapping as 
argument.
- [ ] Mark the usage of ' VCdDataType.Timestamp'  as Obsolete-**Error**.
- [ ] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.SaveVcf, VCard.SerializeVcf, VCard.ToVcfString, VCard.Reference and VCard.Dereference.
- [ ] `VCard.Reference` should copy the input data and return its result as IEnumerable&lt;VCard&gt;.
- [ ] VCard.LoadVcf, VCard.DeserializeVcf and VCard.ParseVcf should return a `ReadOnlyCollection&lt;VCard&gt;`.
- [ ] Add extension methods for IEnumerable&lt;VCard?&gt; to serialize it as VCF in a class named `VCardCollectionExtension`.
- [ ] Remove `VCardListExtension`.


### 3.0.1
- [ ] Cleanup: Remove `VCdDataType.Timestamp` and all other Symbols marked as Obsolete errors in 3.0.0.

### 4.0.0
- [ ] Replace net4.0 support by net4.5 support.
- [ ] Remove the `DataUrl` class and the `MimeType` class and replace it with something new and better like
`FolkerKinzel.URIs.DataUrlHelper`.