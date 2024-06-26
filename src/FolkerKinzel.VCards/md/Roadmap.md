﻿# FolkerKinzel.VCards
## Roadmap

### 7.2.0
- [ ] Implement `VCard.Clean()`.
- [ ] Implement `IEnumerable<VCard> Clean(this IEnumerable<VCard?>)`

### 7.0.0
- [x] Add generic overloads to the VCardBuilder-Parts Edit methods to pass data without having to use closures.
- [x] Implement a fluent API.

### 6.0.1
- [x] Cleanup: Remove identifiers that are marked as obsolete errors.

### 6.0.0
- [x] Higher code coverage of the unit tests.

### 6.0.0-beta.1
- [x] Dependency update.
- [x] **Breaking Change:** End .NET Framework 4.0 support.
- [x] **Breaking Change:** Remove `MultiAnsiFilter`.
- [x] **Breaking Change:** Move `VCdVersion` to namespace `FolkerKinzel.VCards`
- [x] **Breaking Change:** Rename `VCdSex` into  `Gender`
- [x] **Breaking Change:** Rename `VCdEncoding` into  `ValueEncoding`
- [x] **Breaking Change:** Rename `VCdAccess` into  `Access`
- [x] **Breaking Change:** Rename `VCdContentLocation` into  `ContentLocation`
- [x] **Breaking Change:** Rename the `Gender` class into  `GenderInfo`
- [x] **Breaking Change:** Rename the `GenderInfo.Sex` into  `GenderInfo.Gender`
- [ ] **Breaking Change:** Make `DataEncoding` an internal enum of `DataUrl`
- [ ] **Breaking Change:** Rename `DataEncoding` to `EmbeddedDataEncoding`
- [ ] **Breaking Change:** Rename `EmbeddedDataEncoding.UrlEncoded` to `EmbeddedDataEncoding.Url`
- [ ] **Breaking Change:** Make `DataUrl` internal
- [ ] **Breaking Change:** Make `DataProperty` an abstract class which is the base class of `EmbeddedBytesProperty`, `EmbeddedTextProperty` and `ReferencedContentProperty`.
- [x] **Breaking Change:** Remove `Parameters.Charset`.
- [x] Give `AnsiFilter` the functionality of `MultiAnsiFilter` with better performance.
- [ ] Use the HashCode struct to compute hash codes.
- [x] Implement `Models.ContentSizeRestriction`
- [x] Implement `Models.Enums.SizeRestriction`
- [x] Implement `static ContentSizeRestriction VCard.EmbeddedContentSize`
- [x] Implement `VcfDeserializationInfo.EmbeddedContentSize` 
- [x] Implement `VcfSerializer.EmbeddedContentSize` 


### 5.0.0-beta.3
- [x] Higher code coverage of the unit tests.
- [x] Fixes a bug that masked values in vCard 3.0 and vCard 4.0 could not be read correctly under certain circumstances.
- [x] Fixes an issue that the option `VcfOptions.WriteXExtensions` is ignored in vCard 3.0 if the option `VcfOptions.WriteImppExtension` is set.
- [x] Implement `string Name.ToDisplayName()`.
- [x] Implement `string NameProperty.ToDisplayName`.
- [x] Let vCard 4.0 and vCard 3.0 automatically generate a `FN` property from the names if no `DisplayNames` are specified.
- [x] Implement `string Address.ToLabel()`.
- [x] Implement `string AddressProperty.ToLabel()`.
- [x] Implement `string AddressProperty.AppendLabel()`.
- [x] Add an optional parameter `appendLabel = true` to the `AddressProperty` ctors.
- [x] Dependency update.
- [x] **Breaking Change:** Change the parameter order in the `AddressProperty` constructors to `street, locality, region, postalCode, country` in order to let it be more aligned to the address order used in the USA and many other parts of the world and to let its use be more intuitive.
- [x] Add to new constructors to the `AddressProperty` class, which avoid the use of the obsolete parameters `postOfficeBox` and `extendedAddress`.
- [x] Give an `Obsolete` warning when the parameters `postOfficeBox` and `extendedAddress` are used in the `AddressProperty` constructors.

### 5.0.0-beta.2       
- [x] Implement `VCard.IsEmpty()`.
- [x] Change the behavior of `AddressProperty.IsEmpty`: Return `false` if only `AddressProperty.Parameters.Label` is not `null`.
- [x] Allow to write more than one `ADR`- and `LABEL`-property into a vCard 2.1. Make this optional with `VcfOptions.AllowMultipleAdrAndLabelInVCard21` and set this option as default.
- [x] Extension methods `VCardCollectionExtension.SaveVcf(...)`, `VCardCollectionExtension.SerializeVcf(...)` and `VCardCollectionExtension.ToVcfString(...)` get an additional optional parameter: `ITimeZoneIDConverter? tzConverter = null`.
- [x] .NET 7 support.
- [x] Dependency update.

### 5.0.0-beta.1
- [x] Rename `ParameterSection.Charset` to `ParameterSection.CharSet`
- [x] Rename `VCdParam.Charset` to `VCdParam.CharSet`
- [x] `VCard.LoadVcf`, `VCard.ParseVcf` and `VCard.DeserializeVcf` should not eat too many exceptions
- [x] Add `AnsiFilter` class
- [x] Add `MultiAnsiFilter` class
- [x] Fix an issue that LABEL properties from vCard 2.1 might be assigned to the false Address
- [x] Preserve the CHARSET parameter of vCard 2.1 LABEL properties if possible

### 4.0.0
- [x] Remove `VCard: IEquatable<VCard>` and `VCard.GetHashCode()`

### 3.1.0
- [x] Cleanup: Remove `VCdDataType.Timestamp` and all other Symbols marked as Obsolete errors in 3.0.0.
- [x] Add .NET 6.0 support.
- [x] DataUrl can parse "Data" URLs, which use the Base64Url format.
- [x] Dependency update.


### 3.0.0  
- [x] Add .NET Standard 2.0 support
- [x] Add .NET Framework 4.6.1 support

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
- [x] Let Parameter.ToString view collections better.

=
- [x] DataUrl.FromText(string) should work even if the passed string is url encoded yet.
- [x] DataUrl should accept null, empty strings, empty byte arrays, empty files or whitespace
as input for the data to embed.
- [x] Fix the issue that DataUrl doesn't remove the BOM if created from an embedded text file when
retrieving the embedded text.

### 2.2.0
- [x] Implement FBURL property.

### 2.1.1
- [x] Cleanup: Remove obsolete symbols which are marked as errors.
- [x] Increase the code coverage of the unit tests.




