# FolkerKinzel.VCards
## Roadmap

### 2.1.1
- [ ] Cleanup: Remove obsolete symbols that are marked as errors.

### 3.0.0  
- [ ] Move FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate to 
FolkerKinzel.VCards.Models.GeoCoordinate.
- [ ] Mark the usage of VCdDataType.Timestamp as Obsolete-Error.
- [ ] Change the parameter `List<VCard> vCardList` to `IEnumerable<VCard?> vCards` in
VCard.Save, VCard.Serialize, VCard.ToVcfString, VCard.Reference and VCard.Dereference.
- [ ] VCard.Reference should copy the input data and return its result as IEnumerable&lt;VCard&gt;.
- [ ] VCard.Load, VCard.Deserialize and VCard.Parse should return a ReadOnlyCollection&lt;VCard&gt;.
- [ ] Add extension methods for IEnumerable&lt;VCard?&gt; to serialize it as VCF in a class named VCardCollectionExtension.
- [ ] Remove VCardListExtension.
- [ ] Rename Save to SaveVcf.
- [ ] Rename Serialize to SerializeVcf.
- [ ] Rename Load to LoadVcf.
- [ ] Rename Deserialize to DeserializeVcf.
- [ ] Rename Parse to ParseVcf. 

### 3.0.1
- [ ] Cleanup: Remove VCdDataType.Timestamp and all other Symbols marked as Obsolete errors in 3.0.0.

### 4.0.0
- [ ] Replace net4.0 support through net4.5 support.
- [ ] Remove the DataUrl class and the MimeType class and replace it with something new like
FolkerKinzel.URIs.DataUrlHelper.