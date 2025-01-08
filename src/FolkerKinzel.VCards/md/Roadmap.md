# FolkerKinzel.VCards
## Roadmap
### 8.1.0
- [ ] Implement `VCard.Clean()`.
- [ ] Implement `IEnumerable<VCard> Clean(this IEnumerable<VCard?>)`

### 8.0.1
- [ ] Remove symbols that are marked as `Obsolete` errors.

### 8.0.0-beta.1
- [ ] Write tests

### 8.0.0-alpha.1
- [x] Make .NET 9.0 version of the package

&nbsp;
- [x] Let `Vcf.DoDeserialize` return `IReadOnlyList<VCard>`

&nbsp;
- [x] Fix the issue that with `UID` only Guid values are supported
    - [x] Implement a `ContactID` class that holds either a `Guid`, an absolute `Uri` or a `string`. Apply different comparison rules depending on the content.
    - [x] Let the 'IDProperty' have an instance of the `ContactID` class as value.
    - [x] Remove `IEquatable<IDProperty>` from 'IDProperty'
    - [x] Remove the overridden `Equals` and `GetHashCode` methods from 'IDProperty'
    - [x] Remove the `==` and `!=` operator overloads from 'IDProperty'

    - [x] Let `ContactID.Create()` use `Guid.CreateVersion7` in .NET 9.0
    - [x] Update the `IDBuilder.Set` methods
    - [x] Change the `Relation` class to hold a `ContactID` instance rather than a `Guid` value.
    - [x] Update the `RelationBuilder` struct.

&nbsp;
- [x] Rename `DataPropertyValue` to `RawData`
- [x] Rename `GenderInfo` to Gender

&nbsp;
- [x] Rename `Relation.Value` to `Object`
- [x] Rename `RawData.Value` to `Object`
- [x] Rename `DateAndOrTime.Value` to `Object`

&nbsp;
- [x] Remove NameProperty.ToDisplayName()
- [x] `NameProperty`: Allow only ctors that take a `NamePropertyBuilder` as argument.

&nbsp;
- [x] Remove deprecated overloads of the `FolkerKinzel.VCards.BuilderParts.NameBuilder.Add` method
- [x] Rename `FolkerKinzel.VCards.BuilderParts.NameBuilder` to `NameViewsBuilder`

&nbsp;
- [x] Change the properties of the `Name` class to return `IReadOnlyList<string>` rather than `ReadOnlyCollection<string>`
- [x] Let `Name` internally store `string[]` rather than `ReadOnlyCollection<string>`
- [x] Remove all ctors that don't take a `NameBuilder` or `VcfRow` as argument
- [x] Remove Name.ToDisplayName()
<!--- [ ] Rename `Name.Prefixes` to `Name.Titles`
- [ ] Rename `NameBuilder.AddPrefix(string)` to `NameBuilder.AddTitle(string)`-->
- [x] Rename `Name.GivenNames` to `Name.Given`
- [x] Rename `NameBuilder.AddGivenName(string)` to `NameBuilder.AddGiven(string)`

- [x] Rename `Name.AdditionalNames` to `Name.Given2`
- [x] Rename `NameBuilder.AddAdditionalName(string)` to `NameBuilder.AddGiven2(string)`
- [x] Rename `Name.FamilyNames` to `Name.Surnames`
- [x] Rename `NameBuilder.AddFamilyName(string)` to `NameBuilder.AddSurname(string)`
<!--- [ ] Rename `Name.Suffixes` to `Name.Credentials`
- [ ] Rename `NameBuilder.AddSuffix(string)` to `NameBuilder.AddCredential(string)`-->

&nbsp;
- [x] Rename `Organization.OrganizationName` to `Name`
- [x] Rename `Organization.OrganizationalUnits` to `Units`
- [x] Change `Organization.OrgUnits` to return `IReadOnlyList<string>`

&nbsp;
- [x] `AddressProperty`: Allow only ctors that take an `Address` as argument
- [x] Remove `AddressProperty.AttachLabel()`
- [x] Remove `AddressProperty.ToLabel()`

&nbsp;
- [x] Rename `FolkerKinzel.VCards.BuilderParts.AddressBuilder` to `AddressesBuilder`
- [x] Remove deprecated overloads of the `AddressesBuilder.Add` method


&nbsp;
- [x] Change the properties of the `Address` class to return `IReadOnlyList<string>` rather than `ReadOnlyCollection<string>`
- [x] Let `Address` internally store `string[]` rather than `ReadOnlyCollection<string>`
- [x] Remove Address.ToLabel()

- [x] Rename `Address.ExtendedAddress` to `Address.Extended`
- [x] Rename `AddressBuilder.AddExtendedAddress` to `AddressBuilder.AddExtended`
- [x] Rename `Address.PostOfficeBox` to `Address.POBox`
- [x] Rename `AddressBuilder.AddPostOfficeBox` to `AddressBuilder.AddPOBox`

&nbsp;
- [x] Change the `StringCollectionProperty.Value` property to return `IReadOnlyList<string>` rather than `ReadOnlyCollection<string>`
and let it store as string[]

&nbsp;
- [x] Refactor `ReadOnlyCollectionConverter` to return arrays
- [x] Remove `ReadOnlyStringCollection`

&nbsp;
- [x] Rename the `VCard.TimeStamp` property to `VCard.Updated` (to make its use clearer since `VCard.Created` exists).
- [x] Rename the `VCardBuilder.TimeStamp` property to `VCardBuilder.Updated`
- [x] Rename the `Prop.TimeStamp` value to `Prop.Updated`
<!--- [ ] Rename the `Opts.UpdateTimeStamp` value in the `VCard` ctor to `Opts.UpdateUpdated`-->

&nbsp;
- [x] Rename `VCard.ID` to `VCard.ContactID`
- [x] Rename `VCardBuilder.ID` to `VCardBuilder.ContactID`
- [x] Rename the `Prop.ID` value to `Prop.ContactID`
- [x] Rename the `setID` parameter in the VCard ctor to `setContactID`
- [x] Rename the `setID` parameter in the `VCardBuilder.Create` method to `setContactID`
- [x] Rename `IDBuilder` to `ContactIDBuilder`
- [x] Rename `IDProperty` to `ContactIDProperty`

&nbsp;
- [x] Rename the `VCard.Languages` property to `SpokenLanguages`
- [x] Rename the `Prop.Languages` value to `Prop.SpokenLanguages`
- [x] Rename the `VCardBuilder.Languages` property to `VCardBuilder.SpokenLanguages`

&nbsp;
- [x] Move `ParameterSection.DefaultCalendar` to `VCard.DefaultCalendar`

&nbsp;
- [x] Change `VCard.Xmls` property to return and accept IEnumerable<TextProperty?>?
- [x] Remove XmlProperty
- [x] Remove XmlBuilder

&nbsp;
- [x] Change `ProfileProperty` to not been derived from `TextProperty`

&nbsp;
- [x] Make `TextProperty` sealed.

&nbsp;
- [x] Rename `NonStandardProperty.XName` to `Key`

&nbsp;
- [x] Rename `Opts` to `VcfOpts`
- [x] Rename `OptsExtension` to `VcfOptsExtension`

&nbsp;
- [x] Make `VCardProperty.Value` non-nullable
- [x] Make `VCardProperty.IsEmpty` abstract
- [x] Remove the `MemberNotNullWhenAttribute` from `VCardProperty.IsEmpty`
- [x] Ensure that each VCardProperty ctor has a `value` parameter.
- [x] Ensure that the corresponding VCardBuilder parameter is also named `value`

&nbsp;
- [x] Implement `GeoCoordinate? GeoCoordinate.TryCreate(double, double)` that doesn't throw any exception
- [x] Write remarks to `GeoCoordinate.IsEmpty`

&nbsp;
- [x] Rename in all BuilderParts `TData data` to `TArg arg`

&nbsp;
- [x] Check all `Empty` singletons whether they should be public

&nbsp;
- [x] Check all `Value?` operators
- [x] Check all `Value!` operators

&nbsp;
- [x] Refactor ItemOrNullIntl( ... )

### 7.4.5
- [x] Fix an issue that a "geo" URI was not parsed if this "geo" URI was masked.
- [ ] Update dependencies to contain .NET 9.0 packages.

### 7.4.4
- [x] .NET 9.0 version of the package
- [x] Change the `IDProperty(string?) ctor` to create a v7 Guid in .NET 9.0
- [x] Dependency update

### 7.4.3
- [x] Fix an issue that with `X-GENDER` and `X-WAB-GENDER` the parameters and the group name were not parsed.
- [x] Dependency updates

### 7.4.5
- [x] Fix an issue that a "geo" URI was not parsed if this "geo" URI was masked.
- [ ] Update dependencies to contain .NET 9.0 packages.

### 7.4.4
- [x] .NET 9.0 version of the package
- [x] Change the `IDProperty(string?) ctor` to create a v7 Guid in .NET 9.0
- [x] Dependency update

### 7.4.3
- [x] Fix an issue that with `X-GENDER` and `X-WAB-GENDER` the parameters and the group name were not parsed.
- [x] Dependency updates

### 7.4.2
- [x] Fix issues with `RelationProperty` instances that contain `VCard` objects

### 7.4.1
- [x] Fix an issue that `Address.ExtendedAddress` might show some values even if any of the properties 
`Address.Building`, `Address.Floor`, `Address.Apartment`, or `Address.Room` has a value.
- [x] Fix an issue that someone might have invalid JSON warnings with `ParameterSection.JSContactPointer`.
- [x] Fix issues with address label formatting with `DefaultAddressFormatter`
- [x] Better performance of `DefaultAddressFormatter`

### 7.4.0
- [x] Implement in VCard: `IEnumerable<TextProperty?>? ABLabels {get; set;}`  (`X-ABLabel` from Apple Address Book)
       https://www.w3.org/2002/12/cal/vcard-notes.html and https://datatracker.ietf.org/doc/html/rfc9555#name-x-ablabel
- [x] Implement in `VCardBuilder`: `TextBuilder ABLabels { get; }
- [x] Add `X-ABLabel` to the documentation of the `VCard.NonStandards` property
- [x] Add `X-ABLabel` to the documentation of the `VCardBuilder.NonStandards` property

&nbsp;
- [x] Implement RFC 9555 (partially):
    - [x] Add enum value `Opts.WriteRfc9555Extensions` 
    - [x] Implement in `VCard`: `IEnumerable<TextProperty?>? JSContactProps { get; set; }` (`JSPROP`)
    - [x] Implement in `ParameterSection`: `string? ComponentOrder { get; set; }` (`JSCOMPS`)
    - [x] Implement in `ParameterSection`: `string? JSContactPointer { get; set; }` (`JSPTR`)
    - [x] Change `DefaultNameFormatter` to respect `ParameterSection.ComponentOrder`
    - [x] Change `DefaultAddressFormatter` to respect `ParameterSection.ComponentOrder`

&nbsp;
- [x] New extension method:
```csharp
IEnumerable<TSource> Items<TSource>(this IEnumerable<TSource?>?, bool discardEmptyItems = true)
        where TSource : VCardProperty
```


### 7.3.0
- [x] Refactor the `VCard` copy ctor to clone only `IEnumerable<VCardProperty?>` and `VCardProperty`
- [x] Remove the `ObsoleteAttribute` from the properties `Address.PostOfficeBox` and `Address.ExtendedAddress`

&nbsp;
- [x] Implement the internal `AdrProp` enum that addresses the properties of the `Address` class.
- [x] Let the `Address` class have a data struct `Dictionary<AdrProp, ReadOnlyCollection<string>>` (Don't use `FrozenDictionary`: It's not efficient for such small data.)
- [x] Implement `FolkerKinzel.VCards.AddressBuilder` class
    - Let it have a data struct `Dictionary<AdrProp, List<string>>`
    - Make it reusable implementing a `Clear` method
- [x] Give `AddressProperty` a ctor that takes an "AddressBuilder".
- [x] Add an internal ctor to `Address` that takes an `AddressBuilder`
- [x] Change `FolkerKinzel.VCards.BuilderParts.AddressBuilder` to have an overload for the `Add` method that takes a `FolkerKinzel.VCards.AddressBuilder`


&nbsp;
- [x] Implement the internal `NameProp` enum that addresses the properties of the `Name` class.
- [x] Let the `Name` class have a data struct `Dictionary<NameProp, ReadOnlyCollection<string>>` (Don't use `FrozenDictionary`: It's not efficient for such small data.)
- [x] Implement `FolkerKinzel.VCards.NameBuilder` class
    - Let it have a data struct `Dictionary<NameProp, List<string>>`
    - Make it reusable implementing a `Clear` method
- [x] Give `NameProperty` a ctor that takes a "NameBuilder".
- [x] Add an internal ctor to `Name` that takes an `NameBuilder`
- [x] Change `FolkerKinzel.VCards.BuilderParts.NameBuilder` to have an overload for the `Add` method that takes a `FolkerKinzel.VCards.NameBuilder`

 &nbsp;
- [x] Implement RFC 9554:
    - [x] Make new enum value `Opts.WriteRfc9554Extensions`
    - [x] Implement the `Gram` enum ("animate", "common", "feminine", "inanimate", "masculine", "neuter")
    - [x] Implement `GramConverter`
    - [x] Implement the `GramProperty` class that has a `Gram?` value as `Value`
    - [x] Update the VCard copy ctor to process `IEnumerable<GramProperty?>`
    - [x] Implement `readonly struct GramBuilder`
    - [x] Implement the `Phonetic` enum ("ipa", "jyut", "piny", "script")
    - [x] Implement `PhoneticConverter`
    - [x] Add the values `billing` and `delivery` to the `Adr` enum
    - [x] Update `AdrConverter`
    - [x] Change the `Address` class
        - [x] Add read-only property: `ReadOnlyCollection<string> Room`
        - [x] Add read-only property: `ReadOnlyCollection<string> Apartment`
        - [x] Add read-only property: `ReadOnlyCollection<string> Floor`
        - [x] Add read-only property: `ReadOnlyCollection<string> StreetNumber`
        - [x] Add read-only property: `ReadOnlyCollection<string> StreetName`
        - [x] Add read-only property: `ReadOnlyCollection<string> Building`
        - [x] Add read-only property: `ReadOnlyCollection<string> Block`
        - [x] Add read-only property: `ReadOnlyCollection<string> SubDistrict`
        - [x] Add read-only property: `ReadOnlyCollection<string> District`
        - [x] Add read-only property: `ReadOnlyCollection<string> Landmark`
        - [x] Add read-only property: `ReadOnlyCollection<string> Direction`
        - [x] Add an internal ctor to `Address` that takes an `AddressBuilder`
    - [x] Change the `Name` class
        - [x] Add read-only property: `ReadOnlyCollection<string> Surname2` (let the name be singular)
        - [x] Add read-only property: `ReadOnlyCollection<string> Generation` (let the name be singular)
    - [x] Change the `VCard` class
        - [x] Make a new default parameter `setCreated` that defaults to `true` to the `VCard` ctor and to `VCardBuilder.Create`
        - [x] Add property: `TimeStampProperty? Created {get; set;}`
        - [x] Add property: `IEnumerable<GramProperty?>? GramGenders {get; set;}`
        - [x] Add property: `TextProperty? Language {get; set;}`
        - [x] Add property: `IEnumerable<TextProperty?>? Pronouns {get; set;}`
        - [x] Add property: `IEnumerable<TextProperty?>? SocialMediaProfiles {get; set;}`
        - [x] Redirect `X-SOCIALPROFILE` to `VCard.SocialMediaProfiles`
        - [x] Write `X-SOCIALPROFILE` in vCard 4.0 to preserve `VCard.SocialMediaProfiles` when 
        `Opts.WriteRfc9554Extensions` is not set. Make this dependent on whether the `Opts.WriteXExtensions`
        flag is set.
        - [x] Add `X-SOCIALPROFILE` to the documentation of the `VCard.NonStandards` property
    - [x] Change the `VCardBuilder` class
        - [x] Add property: `TimeStampBuilder Created {get;}`
        - [x] Add property: `GramBuilder GramGenders {get;}`
        - [x] Add property: `TextSingletonBuilder Language {get;}`
        - [x] Add property: `TextBuilder Pronouns {get;}`
        - [x] Add property: `TextBuilder SocialMediaProfiles {get;}`
    - [x] Change the ParameterSection class
        - [x] Add property `Uri? Author` that takes an absolute Uri
        - [x] Add property `string? AuthorName`
        - [x] Add property `DateTimeOffset? Created`
        - [x] Add property `bool Derived`
        - [x] Add property `Phonetic? Phonetic`
        - [x] Add property `string? PropertyID`
        - [x] Add property `string? Script`
        - [x] Add property `string? ServiceType`
        - [x] Add property `string? UserName`
        - [x] Redirect `X-SERVICE-TYPE` to the `ParameterSection.ServiceType` property. 
        - [x] Use the `X-SERVICE-TYPE` parameter to preserve the value of `ParameterSection.ServiceType` with 
        `VCard.Messengers` and `VCard.SocialMediaProfiles` when writing vCard 4.0 and the 
        `Opts.WriteRfc9554Extensions` flag is not set. Make this dependent on whether the `Opts.WriteXExtensions`
        flag is set.
        

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




