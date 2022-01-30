# FolkerKinzel.VCards 4.0.0
## Package Release Notes
- Removes the implementation of `IEquatable<VCard>` and the the overridden method `VCard.GetHashCode()`.

### Remarks:
Up to version 3.1.1, the implementation of `IEquatable<VCard>` and `VCard.GetHashCode()` 
examined only a few properties that are particularly important for identifying a vCard 
(such as `VCard.UniqueIdentifier` and `VCard.TimeStamp`). While this can be useful in 
some cases, it can also cause these methods to produce unexpected results.

On the other hand, a strict comparison of all the data in a `VCard` would in most cases be a 
pointless waste of resources. It would be better if the application would provide its own 
implementation of `IEqualityComparer<VCard>`, which fits the specific requirements
of the application. Therefore, the implementations of `VCard.Equals(VCard?)`, 
`VCard.Equals(Object?)` and `VCard.GetHashCode()` are removed in the current version: 
The `VCard` class now provides the default implementations of the `System.Object` class.

This is a breaking change that requires a new Major version. If the application does not rely 
on the specific behavior of `Equals` and `GetHashCode` from previous versions, it is strongly 
recommended to update the package to the current version.

