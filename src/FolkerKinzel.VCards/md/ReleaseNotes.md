- Fixes issues with third-party software that treats Guid values as strings:
  - `ContactIDProperty` has gained a new property, `ContactIDProperty.OriginalString`, which preserves the string value contained in the parsed vCard. By default, this value is retained when the `VCard` is serialized back into a string.
- Improved usability:
  - Unlike in earlier versions, the return values ​​of the `ContactID.Create` methods align more closely with natural expectations, without compromising the semantic comparability of the `ContactID` instances. The new `ContactID.Comparer` property makes it clear what is used for the comparison.
  - `NameViewsBuilder.ToDisplayNames()` now has a default argument.
  - `AddressesBuilder.AttachLabels()` now has a default argument.
- Dependency updates

&nbsp;
>**Project reference:** On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.