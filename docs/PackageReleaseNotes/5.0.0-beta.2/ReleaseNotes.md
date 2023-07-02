# FolkerKinzel.VCards 5.0.0-beta.2
- .NET 7 support.
- Dependency update.
- After updating the dependency FolkerKinzel.Strings to version 5.0.0 you might get a build error if you try to use the .NET Standard 2.0 part of the package to compile a .NET Core 2.x/3.0 application. (You can use the .NET Standard 2.0 part of the package for all other platforms, e.g., Xamarin or UWP apps.) This is caused by a Microsoft dependency of this library and seems to be intended by Microsoft. (Read Andrew Locks interesting blog post [Please stop lying about .NET Standard 2.0 support!](https://andrewlock.net/stop-lying-about-netstandard-2-support/)) The article describes also a solution, which you can use at own risk: Copy `<SuppressTfmSupportBuildWarnings>true</SuppressTfmSupportBuildWarnings>` to a `<PropertyGroup>` of your Visual Studio project file and you will get your .NET Core 2.x/3.0 application compiled.
- Starting with this version, the behavior of `AddressProperty.IsEmpty` has changed: It will now return `false` if only `AddressProperty.Parameters.Label` is not `null`.
- Starting with this version, the library is able to write more than one `LABEL` and `ADR` property into a vCard 2.1. Although not mentioned in the documents, this seems to be common practice. For this reason the new option is enabled by default, but you are free to disable that with vcfOptions:
```C#
var options = VcfOptions.Default.Unset(VcfOptions.AllowMultipleAdrAndLabelInVCard21));
```
- The extension methods `VCardCollectionExtension.SaveVcf(...)`, `VCardCollectionExtension.SerializeVcf(...)` and `VCardCollectionExtension.ToVcfString(...)` got an additional optional parameter: `ITimeZoneIDConverter? tzConverter = null`.
- The `VCard` class got a new instance method `IsEmpty`.

.
>Project reference: On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox - if it is present - in the lower right corner of the General tab in the Properties dialog.