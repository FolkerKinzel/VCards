# FolkerKinzel.VCards 5.0.0-beta.2
- .NET 7 support.
- Dependency update.
- Starting with this version, the behavior of `AddressProperty.IsEmpty` has changed: It will now return `false` if only `AddressProperty.Parameters.Label` is not `null`.
- Starting with this version, the library is able to write more than one `LABEL` and `ADR` property into a vCard 2.1. Although not mentioned in the documents, this seems to be common practice. For this reason the new option is enabled by default, but you are free to disable that with vcfOptions:
```C#
var options = VcfOptions.Default.Unset(VcfOptions.AllowMultipleAdrAndLabelInVCard21));
```
- The extension methods `VCardCollectionExtension.SaveVcf(...)`, `VCardCollectionExtension.SerializeVcf(...)` and `VCardCollectionExtension.ToVcfString(...)` got an additional optional parameter: `ITimeZoneIDConverter? tzConverter = null`.
- The `VCard` class got a new instance method `IsEmpty`.