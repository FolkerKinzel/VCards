- **Breaking Change in `AddressProperty`:**
   - The parameter order in the `AddressProperty` constructors changed to `street, locality, region, postalCode, country` in order to let it be aligned to the address order used in the USA and many other parts of the world and to let its use be more intuitive.
   - The constructor parameter `region` is no longer optional.
   - The use of the constructor parameters `postOfficeBox` and `extendedAddress` gives now an `Obsolete` warning.
   - All `AddressProperty` constructors now by default append an auto-generated mailing label to `AddressProperty.Parameters.Label`. This behavior can be switched off with the optional parameter `appendLabel`.
- A bug was fixed that masked values in vCard 3.0 and vCard 4.0 could not be read correctly under certain circumstances.
- An issue was fixed that the option `VcfOptions.WriteXExtensions` is ignored in vCard 3.0 if the option `VcfOptions.WriteImppExtension` is set.
- When writing a vCard 4.0 and vCard 3.0 a `FN` property is automatically generated from `VCard.NameViews` if no `DisplayNames` are specified.
- **New methods**:
```csharp 
string Name.ToDisplayName();
string NameProperty.ToDisplayName();
string Address.ToLabel();
string AddressProperty.ToLabel();
string AddressProperty.AppendLabel();
```
- Dependency update.
- Higher code coverage of the unit tests.

.
>Project reference: On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox - if it is present - in the lower right corner of the General tab in the Properties dialog.