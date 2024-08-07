﻿# FolkerKinzel.VCards
## Roadmap
### 8.0.0
- [ ] Fix the issue that with `UID` only Guid values are supported
    - [ ] Implement a `ContactID` class that holds either a `Guid`, an absolute `Uri` or a `string`. Apply different comparison rules depending on the content.
    - [ ] Let the 'IDProperty' have an instance of the `ContactID` class as value.
    - [ ] Change the `Relation` class to hold a `ContactID` instance rather than a `Guid` value.

- [ ] `VCard` properties: Don't allow `null` values in collections anymore.
- [ ] `VCard.Reference` properties: Don't allow `null` values in collections anymore.
- [ ] `VCard.Dereference` properties: Don't allow `null` values in collections anymore.
- [ ] `Vcf` methods: Don't allow `null` values in collections anymore.
- [ ] `IEnumerableExtension`: Don't allow `null` values in collections anymore.
- [ ] `NameProperty`: Allow ctors only that take a `NameBuilder` as argument.
- [ ] `AddressProperty`: Allow ctors only that take an `AddressBuilder` as argument

- [ ] Rename `FolkerKinzel.VCards.BuilderParts.NameBuilder` to `FolkerKinzel.VCards.BuilderParts.NamePropertyBuilder`
- [ ] Rename `FolkerKinzel.VCards.BuilderParts.AddressBuilder` to `FolkerKinzel.VCards.BuilderParts.AddressPropertyBuilder`
- [ ] Rename the `Opts` enum to `VcfOpts` (to separate it from JSContactOpts)
- [ ] Rename the `VCard.TimeStamp` property to `VCard.Updated` (to make its use clearer since `VCard.Created` exists).

### 7.4.0
- [ ] Implement `VCard.Clean()`.
- [ ] Implement `IEnumerable<VCard> Clean(this IEnumerable<VCard?>)`

### 7.3.0
- [ ] Implement the internal `AdrProp` enum that addresses the properties of the `Address` class.
- [ ] Let the `Address` class have a data struct `Dictionary<AdrProp, ReadOnlyCollection<string>>` (Don't use `FrozenDictionary`: It's not efficient for such small data.)
- [ ] Implement the internal `NameProp` enum that addresses the properties of the `Name` class.
- Let the `Name` class have a data struct `Dictionary<NameProp, ReadOnlyCollection<string>>` (Don't use `FrozenDictionary`: It's not efficient for such small data.)
- [ ] Implement `FolkerKinzel.VCards.AddressBuilder` class
    - Let it have a data struct `Dictionary<AdrProp, List<string>>`
    - Make it reusable implementing a `Clear` method
- [ ] Implement `FolkerKinzel.VCards.NameBuilder` class
    - Let it have a data struct `Dictionary<NameProp, List<string>>`
    - Make it reusable implementing a `Clear` method
- [ ] Implement RFC 9554:
    - [ ] Make new enum value `Opts.WriteRfc9554Extensions`
    - [ ] Implement the `Gram` enum ("animate", "common", "feminine", "inanimate", "masculine", "neuter")
    - [ ] Implement `GramConverter`
    - [ ] Implement the `GramProperty` class that has a `Gram` value as `Value`
    - [ ] Implement `readonly struct GramBuilder`
    - [ ] Implement the `Phonetic` enum ("ipa", "jyut", "piny", "script")
    - [ ] Implement `PhoneticConverter`
    - [ ] Add the values `billing` and `delivery` to the `Adr` enum
    - [ ] Update `AdrConverter`
    - [ ] Change the `Address` class
        - [ ] Add read-only property: `ReadOnlyCollection<string> Room`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Apartment`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Floor`
        - [ ] Add read-only property: `ReadOnlyCollection<string> StreetNumber`
        - [ ] Add read-only property: `ReadOnlyCollection<string> StreetName`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Building`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Block`
        - [ ] Add read-only property: `ReadOnlyCollection<string> SubDistrict`
        - [ ] Add read-only property: `ReadOnlyCollection<string> District`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Landmark`
        - [ ] Add read-only property: `ReadOnlyCollection<string> Direction`
        - [ ] Add an internal ctor to `Address` that takes an `AddressBuilder`
    - [ ] Add a new ctor to `AddressProperty` that takes an `AddressBuilder`.
    - [ ] Add an internal ctor to `Address` that takes an `AddressBuilder`
    - [ ] Change the `Name` class
        - [ ] Add read-only property: `ReadOnlyCollection<string> Surname2` (let the name be singular)
        - [ ] Add read-only property: `ReadOnlyCollection<string> Generation` (let the name be singular)
    - [ ] Add a new ctor to `NameProperty` that takes an `NameBuilder`
    - [ ] Add an internal ctor to `Name` that takes an `NameBuilder`
    - [ ] Change the `VCard` class
        - Make a new default parameter `setCreated` that defaults to `true` to the `VCard` ctor and to `VCardBuilder.Create`
        - [ ] Add property: `TimeStampProperty Created {get; set;}`
        - [ ] Add property: `IEnumerable<GramProperty?>? GramGenders {get; set;}`
        - [ ] Add property: `TextProperty Language {get; set;}`
        - [ ] Add property: `IEnumerable<TextProperty?>? Pronouns {get; set;}`
        - [ ] Add property: `IEnumerable<TextProperty?>? SocialMediaProfiles {get; set;}`
        - [ ] Redirect `X-SOCIALPROFILE` to `VCard.SocialMediaProfiles`
        - [ ] Write `X-SOCIALPROFILE` in vCard 4.0 when `Opts.WriteRfc9554Extensions` is not set.
        Use the `X-SERVICE-TYPE` parameter to preserve the `ParameterSection.ServiceType` property.
        - [ ] Add `X-SOCIALPROFILE` to the documentation of the `VCard.NonStandards` property
    - [ ] Change the `VCardBuilder` class
        - [ ] Add property: `TimeStampBuilder Created {get;}`
        - [ ] Add property: `GramBuilder GramGenders {get;}`
        - [ ] Add property: `TextSingletonBuilder Language {get;}`
        - [ ] Add property: `TextBuilder Pronouns {get;}`
        - [ ] Add property: `TextBuilder SocialMediaProfiles {get;}`
    - [ ] Change the ParameterSection class
        - [ ] Add property `string? Author` that takes an absolute Uri
        - [ ] Add property `string? AuthorName`
        - [ ] Add property `DateTimeOffset? Created`
        - [ ] Add property `bool Derived`
        - [ ] Add property `Phonetic? Phonetic`
        - [ ] Add property `string? PropID`
        - [ ] Add property `string? Script`
        - [ ] Add property `string? ServiceType`
        - [ ] Add property `string? UserName`
        - [ ] Redirect `X-SERVICE-TYPE` to the `ServiceType` property. Use the `X-SERVICE-TYPE` parameter 
        to preserve the value of `ParameterSection.ServiceType` with `VCard.Messengers` when writing vCard 4.0 and the
        `Opts.WriteRfc9554Extensions` flag is not set.

### 7.2.0
- [x] Implement RFC 8605:
    - [x] Make new enum value `Opts.WriteRfc8605Extensions`
    - [x] Change the `VCard` class
        - [ ] Add property: `IEnumerable<TextProperty?>? ContactUris {get; set;}`
    - [x] Change the `VCardBuilder` class
        - [x] Add property: `TextBuilder ContactUris {get;}`
    - [x] Change the ParameterSection class
        - [x] Add property `string? CountryCode` that takes an ISO 3166 two-character country code

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




