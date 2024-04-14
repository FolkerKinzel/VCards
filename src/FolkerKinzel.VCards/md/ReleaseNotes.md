Version 7 is a large update. Existing code won't be able to consume this update without changes. It's therefore recommended to start new projects using this version.

Some of the benefits of version 7:
- `VCardBuilder` is a **fluent API** for creating and editing VCard objects. All features of the VCard data model are now available using only one class.
- The vCard 4.0 data synchronization mechanism (PID and CLIENTPIDMAP) has been completely rewritten and now works fully automatically with only 2 lines of code.
- Shorter enum names and other refactoring lead to clear and beautiful code.
- The new static `Vcf` class separates the VCF serialization from the `VCard` class, which represents the data model.
- The functionality of `AnsiFilter` is now fully included in the deserialization methods of the `Vcf` class, e.g., in the new `LoadMany` or `DeserializeMany` methods.
- The new `VcfReader` class allows to iterate through the content of very large VCF files or very long streams.
- In order to support groups the `VCard` class got the new property `GroupIDs` and the method `NewGroup`.
- `GeoCoordinate` now is able to preserve the `Uncertainty` parameter of "geo" URIs and to compare instances for geographic equality.
- New extension methods:
```
IEnumerable<TSource> Remove<TSource>(this IEnumerable<TSource?>?, TSource?) 
        where TSource : VCardProperty;

 IEnumerable<TSource> Remove<TSource>(this IEnumerable<TSource?>?, Func<TSource, bool>) 
        where TSource : VCardProperty;
```
.
>**Project reference:** On some systems the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.