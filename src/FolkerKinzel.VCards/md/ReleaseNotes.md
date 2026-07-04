- Fixes issues with third-party software that treats Guid values as strings:
  - `ContactIDProperty` got a new property `ContactIDProperty.OriginalString` that preserves 
the string value found in the parsed vCard. As the default behavior, this value is preserved when serializing
the `VCard` back to a string.
- Dependency updates

&nbsp;
>**Project reference:** On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.